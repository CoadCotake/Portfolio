 using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattlePassSlot : MonoBehaviour
{
    [Header("출석날짜와 이름")]
    [SerializeField] int Num;
    [SerializeField] TextMeshProUGUI Title;

    [Header("슬롯 위치")]
    [SerializeField] GameObject ItemPlace;
    [SerializeField] List<GameObject> ItemPlaceList;

    [SerializeField] GameObject GoldPlace;
    [SerializeField] List<GameObject> GoldPlaceList;

    [SerializeField] GameObject Pre_ItemPlace;
    [SerializeField] List<GameObject> Pre_ItemPlaceList;

    [SerializeField] GameObject Pre_GoldPlace;
    [SerializeField] List<GameObject> Pre_GoldPlaceList;

    [Header("후속처리 이미지")]
    [SerializeField] List<GameObject> EndImage;
    [SerializeField] List<GameObject> LockImage;

    GameObject ins;

    public void SlotSetting(int n, BTSlot1 bt)
    {
        Debug.Log("슬롯넘버" + n);
        Num = n;
        Title.text = Num+1 + "일차";

        //슬롯 세팅
        for (int i = 0; i < ItemPlaceList.Count; i++)     //아이템 슬롯들
        {
            if (i < bt.ItemData.Count)
            {
                ItemPlaceList[i].GetComponent<SimpleItemDisplay>().SetItem(bt.ItemData[i]);
                ItemPlaceList[i].SetActive(true);
            }
            else
            {
                ItemPlaceList[i].SetActive(false);
            }
        }

        for (int i = 0; i < Pre_ItemPlaceList.Count; i++) //프리미엄 아이템 슬롯들
        {
            if (i < bt.Pre_ItemData.Count)
            {
                Pre_ItemPlaceList[i].GetComponent<SimpleItemDisplay>().SetItem(bt.Pre_ItemData[i]);
                Pre_ItemPlaceList[i].SetActive(true);
            }
            else
            {
                Pre_ItemPlaceList[i].SetActive(false);
            }
        }

        for (int i = 0; i < GoldPlaceList.Count; i++) //골드 슬롯들
        {
            if (i < bt.GoldData.Count)
            {
                GoldPlaceList[i].GetComponent<SimpleItemDisplay>().SetItem(bt.GoldData[i]);
                GoldPlaceList[i].SetActive(true);
            }
            else
            {
                GoldPlaceList[i].SetActive(false);
            }
        }

        for (int i = 0; i < Pre_GoldPlaceList.Count; i++) //프리미엄 골드 슬롯들
        {
            if (i < bt.Pre_GoldData.Count)
            {
                Pre_GoldPlaceList[i].GetComponent<SimpleItemDisplay>().SetItem(bt.Pre_GoldData[i]);
                Pre_GoldPlaceList[i].SetActive(true);
            }
            else
            {
                Pre_GoldPlaceList[i].SetActive(false);
            }
        }

        //획득 세팅
        GetSetting();

        //프리미엄 세팅
        PremiumSetting();
    }

    public void GetSetting(int n=-1)    //값을 넣고 함수를 부를경우 해당 슬롯의 이미지만 바꾸기
    {
        if(n==-1)   //디폴트는 전체 바꾸기
        {
            for (int i = 0; i < 4; i++)
            {
                if(BattlePassManager.instance.GetDataCount(Num,i)==0)   //가져올 데이터가 없다면
                {
                    EndImage[i].SetActive(true);
                    continue;
                }

                if (BattlePassManager.instance.GetRequirement(Num))  //요구사항에 만족하였는가?
                {
                    Debug.Log("넘버" + Num + "요구사항" + BattlePassManager.instance.GetRequirement(Num));
                    if (BattlePassManager.instance.GetReward(Num, i))    //해당 슬롯에 보상 받았나 데이터 받기 , 받았다면?
                    {
                        EndImage[i].SetActive(true);
                    }
                    else
                    {
                        EndImage[i].SetActive(false);
                    }
                }
                else
                {
                    EndImage[i].SetActive(true);
                }
            }
        }
        else        //특정 자리만 바꾸기
        {
            if (BattlePassManager.instance.GetRequirement(Num))  //요구사항에 만족하였는가?
            {
                if (BattlePassManager.instance.GetReward(Num, n))    //해당 슬롯에 보상 받았나 데이터 받기 , 받았다면?
                {
                    EndImage[n].SetActive(true);
                }
                else
                {
                    EndImage[n].SetActive(false);
                }
            }
            else
            {
                EndImage[n].SetActive(true);
            }
        }
    }

    public void PremiumSetting()        //자물쇠 이미지 세팅
    {
        foreach (var img in LockImage)
        {
            if (!BattlePassManager.instance.GetPremium())   //프리미엄이 아니면
            {
                img.SetActive(true);
            }
            else
            {
                img.SetActive(false);
            }
        }
    }

    public void ButtonClick(int rewardnumber)       //rewardnumber는 버튼에 따로 설정해야함, 0 - 아이템, 1 - 골드, 2 - 프리미엄 아이템, 3 - 프리미엄 골드
    {
        Debug.Log("버튼 클릭" + rewardnumber + "슬롯");
        BattlePassManager.instance.OpenRewardPanel(Num, rewardnumber);     //획득 패널 열기
    }
}
