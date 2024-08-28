using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePassManager : MonoBehaviour
{
    public static BattlePassManager instance;

    [Header("리스트")]
    [SerializeField] List<BTSlot1> DataLists = new List<BTSlot1>();
    [SerializeField] BattlePassSlot[] SlotLists;

    [Header("패널")]
    [SerializeField] GameObject RewardPanel;
    [SerializeField] GameObject BattlePassPanel;

    [Header("페이지 변수")]
    [SerializeField] int NowPage = 0;
    [SerializeField] int TotalPage = 0;
    [SerializeField] int SlotCountInPage = 0;

    [Header("별도 구매 변수")]
    bool Premium = false;
    bool RemoveAD = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()      //테스트
    {
        DataLists = JsonDBManager.Instance.GetAllBTSlot();
        DataLists[3].SaveData.Requirement = true;
        Premium = true;
        SettingStart();

        
    }


    #region 배틀패스 관련 함수
    public void SettingStart()  //패스 열 때 데이터 로드 및 설정
    {
        RewardPanel = GameObject.Find("RewardPanel");
        RewardPanel.SetActive(false);

        BattlePassPanel = GameObject.Find("UI_배틀패스");
        SlotLists = BattlePassPanel.transform.GetComponentsInChildren<BattlePassSlot>();
        SlotCountInPage = SlotLists.Length;
        TotalPage = (DataLists.Count-1) / SlotCountInPage;

        //BTSlot 데이터 불러오기
        //BTSlot에 

        RefreshPass();
    }

    public void RefreshPass(int num=-1)       //패스 새로고침
    {
        if (num == -1)  //전체고침
        {
            for (int i = 0; i < SlotCountInPage; i++)
            {
                if (NowPage == TotalPage && (i+(SlotCountInPage*NowPage)) > DataLists.Count-1)   //끝 페이지고 더이상 불러올 데이터가 없다면
                {
                    Debug.Log("이거 왜 안나옴"+i+"/"+ ((DataLists.Count % SlotCountInPage) - 1));
                    SlotLists[i].gameObject.SetActive(false);
                }
                else
                {
                    //슬롯마다 데이터 넣고 모습 변화시키기
                    SlotLists[i].SlotSetting((NowPage * SlotCountInPage) + i, DataLists[(NowPage * SlotCountInPage) + i]);
                    SlotLists[i].gameObject.SetActive(true);
                }
            }
        }
        else    //일부 고침
        {
            SlotLists[num%SlotCountInPage].SlotSetting(num, DataLists[num]);
        }
    }
    #endregion

    public void SetRequirement(int i)   //요구 충족 될 경우 데이터 변경 (현재 요구 = 출석)
    {
        DataLists[i].SaveData.Requirement = true;
        RefreshPass();
    }

    #region 아이템 획득
    public void GetItem(int slotnumber, int rewardnumber)      //아이템 획득
    {
        //아이템 획득하고 데이터 처리하기


        //해당 보상 획득했으니 저장
        DataLists[slotnumber].SaveData.RewardCheck[rewardnumber] = true;

        //획득 패널 닫기
        CloseRewardPanel();

        //새로고침
        RefreshPass();

    }

    public void GetItemStart(int slotnumber, int rewardnumber)      //아이템 획득 시작
    {
        Debug.Log("획득시작"+slotnumber+"/"+rewardnumber+"/"+ DataLists[slotnumber].SaveData.Requirement+"/"+DataLists[slotnumber].SaveData.RewardCheck[rewardnumber]);
        if (DataLists[slotnumber].SaveData.Requirement && !DataLists[slotnumber].SaveData.RewardCheck[rewardnumber])  //요구가 충족되면 , 해당 보상을 받지 않았다면
        {
            Debug.Log("획득시작2");
            StartCoroutine(GetItem_co(slotnumber, rewardnumber));
        }
    }
    public IEnumerator GetItem_co(int slotnumber, int rewardnumber)      //아이템 획득 
    {
        if (rewardnumber == 0 || rewardnumber == 1)     //보상 품목이 아이템 or 골드일 경우 (프리미엄이 아닐경우)
        {
            if (!RemoveAD)   //광고 제거가 없다면
            {
                yield return StartCoroutine(LoadAd());
            }
        }

        //보상획득
        GetItem(slotnumber, rewardnumber);
        yield return null;
    }

    public bool GetPremium()
    {
        return Premium;
    }
    public int GetDataCount(int slotnumber, int rewardnumber)
    {
        switch (rewardnumber)       //아이템 아이콘 보여주기 세팅
        {
            case 0:     //아이템
                return DataLists[slotnumber].ItemData.Count;
            case 1:     //골드
                return DataLists[slotnumber].GoldData.Count;
            case 2:     //프리미엄 아이템
                return DataLists[slotnumber].Pre_ItemData.Count;
            case 3:     //프리미엄 골드
                return DataLists[slotnumber].Pre_GoldData.Count;
            default:
                Debug.LogError("데이터 개수 값을 제대로 받지 못함");
                return -1;
        }
    }

    public bool GetReward(int slotnumber, int rewardnumber)
    {
        return DataLists[slotnumber].SaveData.RewardCheck[rewardnumber];
    }

    public bool GetRequirement(int slotnumber)
    {
        return DataLists[slotnumber].SaveData.Requirement;
    }

    public IEnumerator LoadAd()        //광고 불러오기
    {
        //광고 재생 코드
        yield return null;
    }
    #endregion

    #region 패널 함수
    public void OpenRewardPanel(int slotnumber, int rewardnumber)
    {
        switch (rewardnumber)       //아이템 아이콘 보여주기 세팅
        {
            case 0:     //아이템
                RewardPanel.GetComponent<RewardPanel>().SettingReward(slotnumber, rewardnumber, DataLists[slotnumber].ItemData);
                break;
            case 1:     //골드
                RewardPanel.GetComponent<RewardPanel>().SettingReward(slotnumber, rewardnumber, DataLists[slotnumber].GoldData);
                break;
            case 2:     //프리미엄 아이템
                RewardPanel.GetComponent<RewardPanel>().SettingReward(slotnumber, rewardnumber, DataLists[slotnumber].Pre_ItemData);
                break;
            case 3:     //프리미엄 골드
                RewardPanel.GetComponent<RewardPanel>().SettingReward(slotnumber, rewardnumber, DataLists[slotnumber].Pre_GoldData);
                break;
            default:
                Debug.LogError("보상 세팅 중 데이터 값을 제대로 받지 못함");
                break;
        }

        RewardPanel.SetActive(true);
    }

    public void CloseRewardPanel()
    {
        RewardPanel.SetActive(false);
    }

    public void OpenBattlePass()
    {
        BattlePassPanel.SetActive(true);
    }

    public void CloseBattlePass()
    {
        BattlePassPanel.SetActive(false);
    }
    #endregion
    #region 페이지 함수
    public void UpPage()
    {
        if (NowPage < TotalPage)
        {
            NowPage++;
            RefreshPass();
        }
    }

    public void DownPage()
    {
        if (NowPage > 0)
        {
            NowPage--;
            RefreshPass();
        }
    }
    #endregion
  
}



