using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChStatCalculator;

public enum ChStats
{
    Strength,
    Agile,
    Defence,
    Vitality,
    Magic,
    Wisdom,
    MDefence,
    Recovery
}

public class ChStatManager : MonoBehaviour
{
    public static ChStatManager Instance;

    [SerializeField] ChStatSlot[] ChStatSlots;
    [SerializeField] int LeftNum = 1;        //횟수 남은 수

    List<float> statnum = new List<float>();  //계산한 스텟 임시저장
    List<string> statname = new List<string>(); //스텟이름 임시저장

    public bool StatChangingCheck = false;     //스텟의 변화가 보이면 True
    ChStatCalculator.StatLevels statlevelins = new ChStatCalculator.StatLevels();   //스텟 레벨 임시
    

    [Header("플레이어 레벨")]
    [SerializeField] int Level = 0;

    [Header("버튼")]
    [SerializeField] Button Back;
    [SerializeField] Button DetailStatBack;
    [SerializeField] Button StatClear;
    [SerializeField] Button StatChangingSetButton;
    [SerializeField] Button StatChangingCancleButton;

    [Header("능력치")]
    [SerializeField] TextMeshProUGUI TotalStat;

    public UI_Ch_Base UI_Ch_Base;

    public GameObject StatSaveTextWindow;

    private void Awake()
    {
        Instance = this;

        //오브젝트 넣기
        ChStatSlots = this.transform.GetComponentsInChildren<ChStatSlot>();
        TotalStat = this.transform.Find("TotalStat").GetComponent<TextMeshProUGUI>();
        Back = this.transform.Find("Back").GetComponent<Button>();
        DetailStatBack = this.transform.Find("DetailStatBack").GetComponent<Button>();
        StatClear = this.transform.Find("StatClear").GetComponent<Button>();
        StatChangingSetButton = this.transform.Find("StatSet").GetComponent<Button>();
        StatChangingCancleButton = this.transform.Find("StatCancle").GetComponent<Button>();
        StatSaveTextWindow = this.transform.Find("StatChangingCheckWindow").gameObject;

        ChangingButtonActive(false);
        StatChangingCheck = false;
        SettingChStatManager();
        StatSaveWindowActive(false);
    }

    private void Start()
    {
        statlevelins.SetValue(SaveData.StatLevels);  //찍은 스텟들 받아두기, 캔슬때 사용
    }


    public void SettingChStatManager()
    {
        //스텟 슬롯 번호 설정
        for (int i = 0; i < ChStatSlots.Length; i++)    
        {
            ChStatSlots[i].Statnum = i;
        }

        Level = (int)Entity.myMainEntity.currentStat.GetStat(Stat_.Level);
        LeftNum = SaveData.LeftNum;

        UpdateTotalStatText();
        UpdateDisplayStatTexts();
    }

    public void UpdateDisplayStatTexts()
    {
        Strength(SaveData.StatLevels.Strength);
        ChStatSlots[0].DisplayStat("Strength " + SaveData.StatLevels.Strength, statname.ToList(), statnum.ToList());

        Agile(SaveData.StatLevels.Agile);
        ChStatSlots[1].DisplayStat("Agile " + SaveData.StatLevels.Agile, statname.ToList(), statnum.ToList());

        Defence(SaveData.StatLevels.Defence);
        ChStatSlots[2].DisplayStat("Defence " + SaveData.StatLevels.Defence, statname.ToList(), statnum.ToList());

        Vitality(SaveData.StatLevels.Vitality);
        ChStatSlots[3].DisplayStat("Vitality " + SaveData.StatLevels.Vitality, statname.ToList(), statnum.ToList());

        Magic(SaveData.StatLevels.Magic);
        ChStatSlots[4].DisplayStat("Magic " + SaveData.StatLevels.Magic, statname.ToList(), statnum.ToList());

        Wisdom(SaveData.StatLevels.Wisdom);
        ChStatSlots[5].DisplayStat("Wisdom " + SaveData.StatLevels.Wisdom, statname.ToList(), statnum.ToList());

        MDefence(SaveData.StatLevels.MDefence);
        ChStatSlots[6].DisplayStat("MDefence " + SaveData.StatLevels.MDefence, statname.ToList(), statnum.ToList());

        Recovery(SaveData.StatLevels.Recovery);
        ChStatSlots[7].DisplayStat("Recovery " + SaveData.StatLevels.Recovery, statname.ToList(), statnum.ToList());

        //새로고침
        ChStatCalculator.CalculateAndApplyToMyEntity();
        //Entity.myMainEntity.RefreshChStat_01();
    }

    public void UpdateTotalStatText()
    {
        Level = (int)Entity.myMainEntity.currentStat.GetStat(Stat_.Level);
        TotalStat.text = "성장 능력치 "+LeftNum + " / " + Level * 5;
    }

    public void StatChanging()      //스텟을 건들기 시작했을 때
    {
        if (!StatChangingCheck)
        {
            ChangingButtonActive(true);
            StatChangingCheck=true;
        }
    }

    public void ChangingButtonActive(bool Active)
    {
        if(Active)
        {
            StatChangingSetButton.gameObject.SetActive(true);
            StatChangingCancleButton.gameObject.SetActive(true);
        }
        else
        {
            StatChangingSetButton.gameObject.SetActive(false);
            StatChangingCancleButton.gameObject.SetActive(false);
        }
    }

    #region 버튼 함수
    public void StatChangeSet()
    {
        // 횟수 저장
        SaveData.LeftNum = LeftNum;

        //스텟 찍기 전 갱신
        statlevelins.SetValue(SaveData.StatLevels);

        //버튼 및 체인지 값 변경
        ChangingButtonActive(false);
        StatChangingCheck = false;
    }

    public void StatChangeCancle()
    {
        //횟수 찍기 전으로 초기화
        LeftNum = SaveData.LeftNum;

        //찍기 전으로 스텟 초기화
        SaveData.StatLevels.SetValue(statlevelins);     //찍기 전 스텟

        //새로고침
        ChStatCalculator.CalculateAndApplyToMyEntity();
        UpdateDisplayStatTexts();
        UpdateTotalStatText();
        UI_Ch_Base.StatRefresh();

        //버튼 및 체인지 값 변경
        ChangingButtonActive(false);
        StatChangingCheck = false;
    }

    public void StatSaveWindowActive(bool actvie)
    {
        if (actvie)
        {
            StatSaveTextWindow.SetActive(true);
        }
        else
        {
            StatSaveTextWindow.SetActive(false);
        }
    }

    public void StatSaveWindowSetButton()
    {
        StatChangeSet();
        StatSaveWindowActive(false);
    }

    public void StatSaveWindowCancleButton()
    {
        StatChangeCancle();
        StatSaveWindowActive(false);
    }

    public void StatReset()
    {
        // 만약에 수정 크리스탈을 가지고 있지 않을 때
        /*if( ==0)
        {
            return;
        }
        else
        {
            크리스탈--;
        }
        */
        Debug.Log("능력치 초기화");
        
        //스텟 기본 값으로 변경
        SaveData.StatLevels.Reset(SaveData.StatBaseLevels);
        statlevelins.SetValue(SaveData.StatLevels);
       

        //Level=데이터 값;
        LeftNum = Level * 5;
        SaveData.LeftNum = LeftNum;

        //새로고침
        ChStatCalculator.CalculateAndApplyToMyEntity();

        UpdateDisplayStatTexts();
        UpdateTotalStatText();
        UI_Ch_Base.StatRefresh();
    }

    public void UpDownStat(int num, bool up)
    {
        if (up && LeftNum == 0)
        {
            return;
        }

        ChStats chStats = (ChStats)num;
        if (up)
        {
            IncreaseStat(chStats);

            //스텟에 변화가 있다면
            StatChanging();
        }
        else
        {
            if(DecreaseStat(chStats))
            {
                //스텟에 변화가 있다면
                StatChanging();
            }
        }

        //새로고침
        ChStatCalculator.CalculateAndApplyToMyEntity();

        UpdateTotalStatText();
        UpdateDisplayStatTexts();
        UI_Ch_Base.StatRefresh();
    }

    public void BackButton()
    {
        if (!StatChangingCheck)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            StatSaveWindowActive(true);
        }
    }

    public void DetailButton()
    {

    }

    private void IncreaseStat(ChStats chStats)
    {
        switch (chStats)
        {
            case ChStats.Strength:
                SaveData.StatLevels.Strength++;
                LeftNum--;
                break;

            case ChStats.Agile:
                SaveData.StatLevels.Agile++;
                LeftNum--;
                break;

            case ChStats.Defence:
                SaveData.StatLevels.Defence++;
                LeftNum--;
                break;

            case ChStats.Vitality:
                SaveData.StatLevels.Vitality++;
                LeftNum--;
                break;

            case ChStats.Magic:
                SaveData.StatLevels.Magic++;
                LeftNum--;
                break;

            case ChStats.Wisdom:
                SaveData.StatLevels.Wisdom++;
                LeftNum--;
                break;

            case ChStats.MDefence:
                SaveData.StatLevels.MDefence++;
                LeftNum--;
                break;

            case ChStats.Recovery:
                SaveData.StatLevels.Recovery++;
                LeftNum--;
                break;
        }
    }

    private bool DecreaseStat(ChStats chStats)
    {
        switch (chStats)
        {
            case ChStats.Strength:
                if (SaveData.StatLevels.Strength > 0 && SaveData.StatLevels.Strength > statlevelins.Strength)
                {
                    SaveData.StatLevels.Strength--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Agile:
                if (SaveData.StatLevels.Agile > 0 && SaveData.StatLevels.Agile > statlevelins.Agile)
                {
                    SaveData.StatLevels.Agile--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Defence:
                if (SaveData.StatLevels.Defence > 0 && SaveData.StatLevels.Defence > statlevelins.Defence)
                {
                    SaveData.StatLevels.Defence--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Vitality:
                if (SaveData.StatLevels.Vitality > 0 && SaveData.StatLevels.Vitality > statlevelins.Vitality)
                {
                    SaveData.StatLevels.Vitality--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Magic:
                if (SaveData.StatLevels.Magic > 0 && SaveData.StatLevels.Magic > statlevelins.Magic)
                {
                    SaveData.StatLevels.Magic--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Wisdom:
                if (SaveData.StatLevels.Wisdom > 0 && SaveData.StatLevels.Wisdom > statlevelins.Wisdom)
                {
                    SaveData.StatLevels.Wisdom--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.MDefence:
                if (SaveData.StatLevels.MDefence > 0 && SaveData.StatLevels.MDefence > statlevelins.MDefence)
                {
                    SaveData.StatLevels.MDefence--;
                    LeftNum++;
                    return true;
                }
                break;

            case ChStats.Recovery:
                if (SaveData.StatLevels.Recovery > 0 && SaveData.StatLevels.Recovery > statlevelins.Recovery)
                {
                    SaveData.StatLevels.Recovery--;
                    LeftNum++;
                    return true;
                }
                break;
        }
        return false;
    }
    #endregion

    #region 스텟 계산
    private void Strength(int num)  //0번
    {
        if(num< 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        //리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("물리 피해");
        statnum.Add(SaveData.StatNumAll.Strength.PhysicalDamage);

        statname.Add("물리 치명타피해");
        statnum.Add(SaveData.StatNumAll.Strength.CriticalDamage);
    }

    private void Agile(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        //리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("물리 명중");
        statnum.Add(SaveData.StatNumAll.Agile.PhysicalHit);

        statname.Add("공격속도");
        statnum.Add(SaveData.StatNumAll.Agile.AttackSpeed);

        statname.Add("물리 회피");
        statnum.Add(SaveData.StatNumAll.Agile.PhysicalDodge);

        statname.Add("물리 치명타확률");
        statnum.Add(SaveData.StatNumAll.Agile.CriticalChance);
    }

    private void Defence(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("물리 방어");
        statnum.Add(SaveData.StatNumAll.Defence.PhysicalDefence);

        statname.Add("물리 치명타저항률");
        statnum.Add(SaveData.StatNumAll.Defence.CriticalResistance);

        statname.Add("물리 치명타피해 감소");
        statnum.Add(SaveData.StatNumAll.Defence.CriticalDamageReduction);
    }

    private void Vitality(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("Hp");
        statnum.Add(SaveData.StatNumAll.Vitality.HP);

        statname.Add("Mp");
        statnum.Add(SaveData.StatNumAll.Vitality.MP);

        statname.Add("이동속도");
        statnum.Add(SaveData.StatNumAll.Vitality.MovementSpeed);

        statname.Add("스테미나");
        statnum.Add(SaveData.StatNumAll.Vitality.Stamina);
    }

    private void Magic(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("마법 피해");
        statnum.Add(SaveData.StatNumAll.Magic.MagicDamage);

        statname.Add("마법 치명타 피해");
        statnum.Add(SaveData.StatNumAll.Magic.CriticalMagicDamage);
    }

    private void Wisdom(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("마법 명중");
        statnum.Add(SaveData.StatNumAll.Wisdom.MagicHit);

        statname.Add("마법 속도");
        statnum.Add(SaveData.StatNumAll.Wisdom.MagicSpeed);

        statname.Add("마법 회피");
        statnum.Add(SaveData.StatNumAll.Wisdom.MagicDodge);

        statname.Add("마법 치명타확률");
        statnum.Add(SaveData.StatNumAll.Wisdom.MagicCriticalChance);
    }

    private void MDefence(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("마법 방어");
        statnum.Add(SaveData.StatNumAll.MDefence.MagicDefence);

        statname.Add("마법 치명타저항률");
        statnum.Add(SaveData.StatNumAll.MDefence.MagicCriticalResistance);

        statname.Add("마법 치명타피해 감소");
        statnum.Add(SaveData.StatNumAll.MDefence.MagicCriticalDamageReduction);
    }

    private void Recovery(int num)
    {
        if (num < 0)
        {
            Debug.LogError("마이너스 스텟값을 받음");
            return;
        }

        // 리스트 초기화
        statnum.Clear();
        statname.Clear();

        statname.Add("Hp 회복");
        statnum.Add(SaveData.StatNumAll.Recovery.HPRecovery);

        statname.Add("Mp 회복");
        statnum.Add(SaveData.StatNumAll.Recovery.MPRecovery);

        statname.Add("Stamina 회복");
        statnum.Add(SaveData.StatNumAll.Recovery.StaminaRecovery);
    }

    #endregion
}
