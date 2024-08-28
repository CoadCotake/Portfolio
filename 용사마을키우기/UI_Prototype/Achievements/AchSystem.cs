using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AchData
{
    [Header("업적코드")]
    public string AchCode;
    [Header("업적 진행도")]
    public int Ach_Num;
    [Header("클리어여부")]
    public bool isCleard;
    [Header("보상여부")]
    public bool isRewarded;


    public AchData(string name=null,int num=0,bool isc = false, bool isr = false)
    {
        AchCode = name;
        Ach_Num = num;
        isCleard = isc;
        isRewarded = isr;
        
    }
}

public class AchSystem : MonoBehaviour
{
    static public AchSystem m_achsystem;
    GameObject Slot;

    [SerializeField]
    [Header("업적리스트")]
    public List<AchData> AchList = new List<AchData>();

    [Header("!! AchContent 위치 직접 지정해주기 !!")]
    public Transform AchContent;

    private void Awake()
    {
        m_achsystem = this;
        Slot = Resources.Load<GameObject>("Prefabs/AchSlot");
        LoadAchData();
    }

    private void OnEnable()
    {
        CreateAch();
        //StartCoroutine("Refresh");
    }

    public void LoadAchData()
    {
        Ach_ResourceManager.LoadData();
        foreach (var item in Ach_ResourceManager.AchDatas)
        {
            AchData Ach_Refernce = new AchData();
            Ach_Refernce.AchCode = item.Key;
            AchList.Add(Ach_Refernce);
        }
        
    }

    public void CreateAch()
    {
        foreach (AchSlot achSlot in AchContent.GetComponentsInChildren<AchSlot>())      //슬롯 전부 삭제 시키고
        {
            
            Destroy(achSlot.gameObject);
            
        }


        foreach (AchData achData in AchList)    //전부 생성한다. 1. 보상 받지않은 업적
        {
            if (achData.isRewarded == false)
            {
                GameObject obj = Instantiate(Slot, AchContent);
                obj.GetComponent<AchSlot>().achData = achData;
                obj.GetComponent<AchSlot>().Refresh();        //아이템 적용시키기
                obj.SetActive(true);
            }
        }

        foreach (AchData achData in AchList)    //전부 생성한다. 2. 보상 받은 업적
        {
            if (achData.isRewarded == true)
            {
                GameObject obj = Instantiate(Slot, AchContent);
                obj.GetComponent<AchSlot>().achData = achData;
                obj.GetComponent<AchSlot>().Refresh();        //아이템 적용시키기
                obj.SetActive(true);
            }
        }



        Slot.SetActive(false);
    }
}
