using DTT.Utils.Extensions;
using Newtonsoft.Json;
using PsychoticLab;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using TMPro;
using UIWidgets.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ChStatCalculator;
using Debug = UnityEngine.Debug;

public enum SkillType1
{
    Race, Skill, Job, Weapon
}

public enum SkillType2
{
    All,Passive, Active
}


[System.Serializable]
public class SkillPoint 
{
    public string SkillCode;    //스킬 코드
    public SkillType1 PageType; //스텟 종류 (종족특화, 기술, 전직, 무기)
    public SkillType2 PageType2;
    public int PlusNumber;      //찍은 횟수
    public bool Max;    //찍은 스텟이 최대치 인가

    //디폴트 스텟 (기본)
    public List<Skilleffect> effectdefault = new List<Skilleffect>();

    //현재 적용중인 스텟
    public List<Skilleffect> skilleffects = new List<Skilleffect>();

    //레벨 업 하면 적용되는 스텟
    public List<Skilleffect> skillLevelUpeffects = new List<Skilleffect>();

    public SkillPoint DeepCopy()
    {
        SkillPoint newSkillPoint = new SkillPoint(null,null);

        // 값 복사
        newSkillPoint.SkillCode = this.SkillCode;
        newSkillPoint.PageType = this.PageType;
        newSkillPoint.PageType2 = this.PageType2;
        newSkillPoint.PlusNumber = this.PlusNumber;
        newSkillPoint.Max = this.Max;

        // List<Skilleffect> 깊은 복사
        newSkillPoint.effectdefault = new List<Skilleffect>(this.effectdefault.Select(e => e.DeepCopy()));
        newSkillPoint.skilleffects = new List<Skilleffect>(this.skilleffects.Select(e => e.DeepCopy()));
        newSkillPoint.skillLevelUpeffects = new List<Skilleffect>(this.skillLevelUpeffects.Select(e => e.DeepCopy()));

        return newSkillPoint;
    }

    public SkillPoint(string pagetype,string pagetype2,string code="",List<Skilleffect> sk=null, int num = 0)
    {
        switch (pagetype)
        {
            case "종족 특화":
                PageType = SkillType1.Race;
                break;
            case "일반 기술":
                PageType = SkillType1.Skill;
                break;
            case "전직 기술":
                PageType = SkillType1.Job;
                break;
            case "무기 특성":
                PageType = SkillType1.Weapon;
                break;
            default:
                break;
        }

        switch (pagetype2)
        {
            case "행동 기술":
                PageType2 = SkillType2.Active;
                break;
            case "지속 기술":
                PageType2 = SkillType2.Passive;
                break;
            default:
                PageType2 = SkillType2.All;
                break;
        }

        SkillCode = code;
        PlusNumber = num;

        if(sk!=null)
        {
            foreach (var item in sk)
            {
                effectdefault.Add(item.DeepCopy());
                skilleffects.Add(item.DeepCopy());
                skillLevelUpeffects.Add(item.DeepCopy());
            }
        }
    }

    bool Check;
    string inssting;


    public void ResetStat_()
    {
        //초기화
        foreach (var item in skilleffects)
        {
            item.effect_value = 0;
        }

        //기본 값 넣기
        for (int i = 0; i < effectdefault.Count; i++)
        {
            skilleffects[i].effect_value = effectdefault[i].effect_value;
        }
        
    }
    public void ResetStat_levelup()
    {
        //초기화
        foreach (var item in skillLevelUpeffects)
        {
            item.effect_value = 0;
        }

        //기본 값 넣기
        for (int i = 0; i < effectdefault.Count; i++)
        {
            skillLevelUpeffects[i].effect_value = effectdefault[i].effect_value;
        }
    }

    public string GetLvUpEffectValueChar(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (value.Contains("%"))
            return "%";
        return "";
    }

    public string StatText(bool next)
    {
        inssting = null;

        if (next)
        {
            if (Max)
            {
                return "Max";
            }
            else
            {
                foreach (var item in skillLevelUpeffects)
                {
                    if (item.effect != null)
                    {
                        inssting += item.effect + " +" + item.effect_value + item.effect_type + " ";
                    }
                }

                return inssting;
            }
        }
        else
        {
            foreach (var item in skilleffects)
            {
                if (item.effect != null)
                {
                    Debug.LogError(item.effect + " 테스트");
                    inssting += item.effect + " +" + item.effect_value + item.effect_type + " ";
                }
            }

            return inssting;
        }
    }

    public void UpdateStat(SkillData1 data, int index, bool next)
    {
        //값들 초기화
        Check = false;
        Max = false;        

        if (next)
        {
            if (data.EndLevel >= SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber + 1)
            {
                //스텟 합산 초기화
                ResetStat_levelup();

                foreach (var item in data.skillleveleffects)
                {
                    Check = false;

                    for (int i = 0; i < skillLevelUpeffects.Count; i++)
                    {
                        if(item.effect == skillLevelUpeffects[i].effect)
                        {
                            skillLevelUpeffects[i].effect_value += item.effect_value * (SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber + 1);
                            Check = true;
                        }
                    }

                    if(!Check)  //만약 중복된 값이 없으면 리스트에 새로 추가
                    {
                        skillLevelUpeffects.Add(new Skilleffect(item.effect,item.effect_value * (SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber + 1),item.effect_type));
                    }
                }
            }
        }
        else
        {
            if (SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber != 0)
            {
                if (data.EndLevel >= SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber)
                {
                    //스텟 합산 초기화
                    ResetStat_();

                    foreach (var item in data.skilleffects)
                    {
                        Check = false;

                        for (int i = 0; i < skilleffects.Count; i++)
                        {
                            if (item.effect == skilleffects[i].effect)
                            {
                                skilleffects[i].effect_value += item.effect_value * (SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber);
                                Check = true;
                            }
                        }

                        if (!Check)  //만약 중복된 값이 없으면 리스트에 새로 추가
                        {
                            skilleffects.Add(new Skilleffect(item.effect, item.effect_value * (SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber),item.effect_type));
                        }
                    }
                }
            }
        }

        if (data.EndLevel <= SkillPointManager.ins.GetSkillPoint(index, PageType).PlusNumber)
        {
            Max = true;
        }

    }

}


public class SkillPointManager : MonoBehaviour
{
    public static SkillPointManager ins;

    [Header("---------------------------------------종족 페이지---------------------------------------")]
    #region 종족 페이지 변수들
    [Header("직접 넣어야 하는것")]
    public GameObject RacePage;
    public GameObject RaceSkillPrefab;
    public GameObject RacePageContent;

    [Header("변수들")]
    [SerializeField] List<SkillPointSlot_Scroll> SkillPointSlotList_race = new List<SkillPointSlot_Scroll>();  //스킬 슬롯 리스트
    [SerializeField] List<SkillData1> SkillDatas_race_ins = new List<SkillData1>();      //임시 사용용 리스트
    [SerializeField] List<SkillPoint> SkillPointlist_race_ins = new List<SkillPoint>();  //임시 사용용 리스트
    #endregion

    [Header("---------------------------------------전직 페이지---------------------------------------")]
    #region 전직 페이지 변수들
    [Header("직접 넣어야 하는것")]
    public GameObject JobSkillPrefab;
    public GameObject JobPageContent;
    public GameObject JobLevelPageButton;
    public Transform JobLevelPageButtonsPosion;

    [Header("변수들")]
    [SerializeField] List<SkillPointSlot_Scroll2_Group> SkillPointSlotList_job = new List<SkillPointSlot_Scroll2_Group>();  //스킬 슬롯 리스트
    [SerializeField] List<SkillData1> SkillDatas_job_ins = new List<SkillData1>();      //임시 사용용 리스트
    [SerializeField] List<SkillPoint> SkillPointlist_job_ins = new List<SkillPoint>();  //임시 사용용 리스트

    [SerializeField] Dictionary<int, Dictionary<string, List<SkillData1>>> JobLevelSkills = new Dictionary<int, Dictionary<string, List<SkillData1>>>();  //레벨별 패시브, 엑티브 분배한 딕셔너리
    [SerializeField] List<int> JobLevelKey; //레벨 키값 모음 슬롯 만들 때 사용
    public int JobLevelButtonIndex; //레벨 버튼 인덱스 번호, 버튼 클릭후 재배열할 때 사용
    #endregion


    [Header("---------------------------------------무기 페이지---------------------------------------")]
    #region 무기 페이지 변수들
    [Header("직접 넣어야 하는것")]
    public GameObject WeaponSkillPrefab;
    public GameObject WeaponPageContent;
    public GameObject WeaponButtonContent;
    public GameObject WeaponButtonPrefab;

    [Header("변수들")]
    [SerializeField] List<SkillPointSlot_Scroll_Weapon_Group> SkillPointSlotList_weapon = new List<SkillPointSlot_Scroll_Weapon_Group>();  //스킬 슬롯 리스트
    [SerializeField] List<SkillPointSlot_Button_Weapon> SkillButtonSlotList_weapon = new List<SkillPointSlot_Button_Weapon>();  //버튼 슬롯 리스트
    [SerializeField] List<SkillData1> SkillDatas_weapon_ins = new List<SkillData1>();      //임시 사용용 리스트
    [SerializeField] List<SkillPoint> SkillPointlist_weapon_ins = new List<SkillPoint>();  //임시 사용용 리스트

    [SerializeField] Dictionary<int, Dictionary<string, List<SkillData1>>> WeaponLevelSkills = new Dictionary<int, Dictionary<string, List<SkillData1>>>();  //레벨별로 카테고리 분류한 리스트
    [SerializeField] List<int> WeaponLevelKey; //레벨 키값 모음 슬롯 만들 때 사용
    [SerializeField] List<string> WeaponTypeKey; //레벨 키값 모음 슬롯 만들 때 사용
    public string Nowtype;  //현재 보고있는 카테고리
    #endregion


    [Header("---------------------------------------기술 페이지---------------------------------------")]
    #region 기술 페이지 변수들
    [Header("직접 넣어야 하는것")]
    public GameObject SkillPage;
    public GameObject ViewPointContant;
    public Button UpButton;
    public Button DownButton;
    public TextMeshProUGUI PageText;
    public SkillPointSlotTooltip SkillTooltip;


    [Header("변수들")]
    [SerializeField] List<SkillPointSlot> SkillPointSlotList_skill = new List<SkillPointSlot>();  //스킬 슬롯 리스트
    [SerializeField] SkillType2 type=SkillType2.All;

    

    [SerializeField] List<SkillData1> SkillDatas_ins = new List<SkillData1>();      //임시 사용용 리스트 (패시브, 액티브 나눌 때 사용하는거)
    [SerializeField] List<SkillPoint> SkillPointlist_ins = new List<SkillPoint>();  //임시 사용용 리스트

    #endregion

    [Header("---------------------------------------전체 변수들---------------------------------------")]
    #region 전체 변수들
    public GameObject SkillLevelUpPage;
    [SerializeField] List<SkillData1> AllSkillDatas = new List<SkillData1>();       //전체적인 데이터 모음 (시작할 때 전체적인 스킬 데이터를 받음)
    [SerializeField] List<SkillPoint> SkillPointlist = new List<SkillPoint>();      //스킬 찍은 수 리스트
    
    //주소 값이 전달된 리스트들 값이 변경되면 원본 값도 함께 변경됨.
    //스킬
    [SerializeField] List<SkillData1> SkillDatas_skill = new List<SkillData1>();      //스킬 데이터 (데이터를 타입에 맞게 가공해 넣음)
    [SerializeField] List<SkillPoint> SkillPointlist_skill = new List<SkillPoint>();  //스킬 찍은 수 리스트

    //종
    [SerializeField] List<SkillData1> SkillDatas_race = new List<SkillData1>();      //스킬 데이터 (데이터를 타입에 맞게 가공해 넣음)
    [SerializeField] List<SkillPoint> SkillPointlist_race = new List<SkillPoint>();  //스킬 찍은 수 리스트

    //직업
    [SerializeField] List<SkillData1> SkillDatas_job = new List<SkillData1>();      //스킬 데이터 (데이터를 타입에 맞게 가공해 넣음)
    [SerializeField] List<SkillPoint> SkillPointlist_job = new List<SkillPoint>();  //스킬 찍은 수 리스트

    //무기
    [SerializeField] List<SkillData1> SkillDatas_weapon = new List<SkillData1>();      //스킬 데이터 (데이터를 타입에 맞게 가공해 넣음)
    [SerializeField] List<SkillPoint> SkillPointlist_weapon = new List<SkillPoint>();  //스킬 찍은 수 리스트
    #endregion

    //임시 사용용 변수. 잠시 주소값을 담는 변수들
    [SerializeField] GameObject ins_obj;
    int innum;
    int inlevel;
    string instype;
    public GameObject insbutton;
    public GameObject button;

    public SkillPoint GetSkillPoint(int i, SkillType1 type=SkillType1.Skill)
    {
        button.SetActive(true); //나중에 빼기 임시임

        switch (type)
        {
            case SkillType1.Race:
                return SkillPointlist_race_ins[i];
            case SkillType1.Skill:
                return SkillPointlist_ins[i];
            case SkillType1.Job:
                return SkillPointlist_job_ins[i];
            case SkillType1.Weapon:
                return SkillPointlist_weapon_ins[i];
            default:
                return null;
        }
        
    }

    [SerializeField] int NowPage;
    [SerializeField] int MaxPage;

    public bool buttonclick;

    public void Cancle()        //스텟 찍은것 취소
    {
        instancegetpoint();

        DistributeData();

        //종 리프레쉬
        DataSetting_race();
        RaceAllRefresh();

        //스킬 리프레쉬
        DataSetting();
        SettingSkillSlot();

        //직업 리프레쉬
        DataSetting_job();
        JobAllRefresh();

        //무기 리프레쉬
        DataSetting_weapon();
        WeaponAllRefresh();

        button.SetActive(false);
    }

    public void DataStatsSetUpload()      //스텟 찍은값 확정 짓기 세이브 데이터 값 교체
    {
        Debug.Log("SkillPointManager.DataStatsSetUpload");

        SaveData.SkillPointlist.Clear();
        for (int i = 0; i < SkillPointlist.Count; i++)
        {
            SaveData.SkillPointlist.Add(SkillPointlist[i].DeepCopy());          //스킬 데이터 덮어씌우기
        }

        //Entity.myMainEntity.RefreshChStat_01();  //총 스텟 리프래쉬
        ChStatCalculator.CalculateAndApplyToMyEntity();

        button.SetActive(false);
    }

    //스킬 찍은 리스트 전체 값 받기
    public void instancegetpoint()
    {
        if (SkillPointlist.Count == 0)
        {
            for (int i = 0; i < SaveData.SkillPointlist.Count; i++)
            {
                SkillPointlist.Add(SaveData.SkillPointlist[i].DeepCopy());
            }
        }
        else
        {
            for (int i = 0; i < SaveData.SkillPointlist.Count; i++)
            {
                SkillPointlist[i]=SaveData.SkillPointlist[i].DeepCopy();
            }
        }
    }

    //스킬 데이터 리스트 전체 값 받기
    public void instanceget()
    {
        AllSkillDatas = JsonDBManager.Instance.GetAllSkillData();
        #region 임시 값 넣는거 현재는 사용 x
        /*
        Ins_SkillDatas.Add(new SkillData1("한손 검 숙달", "한손 검 착용 시 물리 피해와 명중 증가","지속 기술", 99f, "물리 피해", 4.0f, "물리 명중", 0.3f, "물리 피해", 4.0f, "물리 명중", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("한손 둔기 숙달", "한손 둔기 착용 시 물리 피해와 HP 증가", "지속 기술", 99f, "물리 피해", 4.4f, "HP", 5.0f, "물리 피해", 4.4f, "HP", 5.0f));
        Ins_SkillDatas.Add(new SkillData1("한손 창 숙달", "한손 창 착용 시 물리 피해와 치명타 피해 증가", "지속 기술", 99f, "물리 피해", 3.6f, "물리 치명타 피해", 1.5f, "물리 피해", 3.6f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("단검 숙달", "단검 착용 시 물리 피해와 치명타 확률 증가", "지속 기술", 99f, "물리 피해", 1.8f, "물리 치명타 확률", 0.25f, "물리 피해", 1.8f, "물리 치명타 확률", 0.25f));
        Ins_SkillDatas.Add(new SkillData1("한손 지팡이 숙달", "한손 지팡이 착용 시 마법 피해와 마법 속도 증가", "지속 기술", 99f, "마법 피해", 6.2f, "마법 속도", 0.2f, "마법 피해", 6.2f, "마법 속도", 0.2f));
        Ins_SkillDatas.Add(new SkillData1("양손 지팡이 숙달", "양손 지팡이 착용 시 마법 피해와 MP 회복 증가", "지속 기술", 99f, "마법 피해", 8.0f, "MP 회복", 0.4f, "마법 피해", 8.0f, "MP 회복", 0.4f));
        Ins_SkillDatas.Add(new SkillData1("활 숙달", "활 착용 시 물리 피해와 치명타 피해 증가", "지속 기술", 99f, "물리 피해", 7.2f, "물리 치명타 피해", 1.5f, "물리 피해", 7.2f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("양손 검 숙달", "양손 검 착용 시 물리 피해와 명중 증가", "지속 기술", 99f, "물리 피해", 5.6f, "물리 명중", 0.3f, "물리 피해", 5.6f, "물리 명중", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("양손 둔기 숙달", "양손 둔기 착용 시 물리 피해와 HP 증가", "행동 기술", 99f, "물리 피해", 6.0f, "HP", 5.0f, "물리 피해", 6.0f, "HP", 5.0f));
        Ins_SkillDatas.Add(new SkillData1("양손 창 숙달", "양손 창 착용 시 물리 피해와 치명타 피해 증가", "행동 기술", 99f, "물리 피해", 5.2f, "물리 치명타 피해", 1.5f, "물리 피해", 5.2f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("석궁 숙달", "석궁 착용 시 물리 피해와 치명타 피해 증가", "행동 기술", 99f, "물리 피해", 3.0f, "물리 치명타 피해", 1.5f, "물리 피해", 3.0f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("방패 숙달", "방패 착용 시 물리 & 마법 방어와 방패 방어율 증가", "행동 기술", 99f, "물리 방어", 6.0f, "마법 방어", 6.0f, "물리 방어", 6.0f, "마법 방어", 6.0f, "방패 방어율", 0.5f));
        Ins_SkillDatas.Add(new SkillData1("중갑 & 판금 숙달", "중갑 & 판금 착용 시 방어력과 HP & HP 회복 증가", "행동 기술", 99f, "물리 방어", 16.0f, "마법 방어", 16.0f, "물리 방어", 16.0f, "마법 방어", 16.0f, "HP", 5.0f, "HP 회복", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("경갑 숙달", "경갑 착용 시 방어력과 회피, 치명타 확률, HP & MP 증가", "행동 기술", 99f, "물리 방어", 18.0f, "마법 방어", 18.0f, "물리 방어", 18.0f, "마법 방어", 18.0f, "물리 치명타 확률", 0.3f, "물리 치명타 저항률", 0.3f, "HP 회복", 3.0f, "MP", 2.0f));
        Ins_SkillDatas.Add(new SkillData1("로브 숙달", "로브 착용 시 방어력과 마법 속도, MP & MP 회복 증가", "행동 기술", 99f, "물리 방어", 9.0f, "마법 방어", 20.0f, "물리 방어", 9.0f, "마법 방어", 20.0f, "마법 속도", 0.3f, "MP", 5.0f, "MP 회복", 0.15f));
        Ins_SkillDatas.Add(new SkillData1("집중", "공격 & 마법 속도 증가", "지속 기술", 99f, "공격 속도", 0.5f, "마법 속도", 0.5f, "공격 속도", 0.5f, "마법 속도", 0.5f));
        Ins_SkillDatas.Add(new SkillData1("활력", "HP & MP & Stamina 증가", "지속 기술", 99f, "HP", 25.0f, "MP", 8.0f, "HP", 25.0f, "MP", 8.0f, "Stamina", 1.0f));
        Ins_SkillDatas.Add(new SkillData1("능숙한 활", "활 속도와 활 사용 거리 증가", "지속 기술", 99f, "활 속도", 0.5f, "활 사거리", 0.1f, "활 속도", 0.5f, "활 사거리", 0.1f));
        Ins_SkillDatas.Add(new SkillData1("약점 간파", "물리 & 마법 치명타 피해 증가", "지속 기술", 99f, "물리 치명타 피해 증가", 0.20f, "마법 치명타 피해 증가", 0.20f, "물리 치명타 피해 증가", 0.20f, "마법 치명타 피해 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("명중", "모든 명중과 상태이상 확률 증가", "지속 기술", 99f, "물리 명중", 0.6f, "마법 명중", 0.6f, "물리 명중", 0.6f, "마법 명중", 0.6f, "상태이상 확률 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("회피", "모든 회피와 상태이상 저항 증가", "행동 기술", 99f, "물리 회피", 0.6f, "마법 회피", 0.6f, "물리 회피", 0.6f, "마법 회피", 0.6f, "상태이상 저항 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("약점 숨기기", "받는 치명타 피해와 상태 이상에 대한 효과를 감소 #방패 착용 시 방패 방어율에 따라 추가 치명타 피해 감소 및 모든 상태 이상 저항 확률 증가 <비공식 : 방패방어율 1에 추가 치명타 피해 -0.1% 감소 / 모든 상태 이상 저항 +0.1% 증가>", "지속 기술", 99f, "물리 치명타 피해 감소", 0.25f, "마법 치명타 피해 감소", 0.25f, "물리 치명타 피해 감소", 0.25f, "마법 치명타 피해 감소", 0.25f, "상태이상 저항 증가", 0.25f));
        Ins_SkillDatas.Add(new SkillData1("공격 대상 증가", "양손무기 착용 시 물리 피해와 공격 대상 & 공격 범위 증가 #기술 점수 9점 단위로 증가", "지속 기술", 11f, "공격 대상 증가", 1.0f, "공격 대상 증가", 1.0f, "물리 피해", 15.0f, "피해 거리", 0.1f));
        Ins_SkillDatas.Add(new SkillData1("재집 숙련", "재료 채집 시 추가 획득 #Shard 종족 추가 획득 확률 증가", "행동 기술", 99f, "재료 채집 시 추가 획득 활률", 0.03f, "Shard 종족 추가 획득", 0.02f, "재료 채집 시 추가 획득 활률", 0.005f, "Shard 종족 추가 획득", 0.0055f));
        Ins_SkillDatas.Add(new SkillData1("한손 검 숙달", "한손 검 착용 시 물리 피해와 명중 증가", "지속 기술", 99f, "물리 피해", 4.0f, "물리 명중", 0.3f, "물리 피해", 4.0f, "물리 명중", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("한손 둔기 숙달", "한손 둔기 착용 시 물리 피해와 HP 증가", "지속 기술", 99f, "물리 피해", 4.4f, "HP", 5.0f, "물리 피해", 4.4f, "HP", 5.0f));
        Ins_SkillDatas.Add(new SkillData1("한손 창 숙달", "한손 창 착용 시 물리 피해와 치명타 피해 증가", "지속 기술", 99f, "물리 피해", 3.6f, "물리 치명타 피해", 1.5f, "물리 피해", 3.6f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("단검 숙달", "단검 착용 시 물리 피해와 치명타 확률 증가", "지속 기술", 99f, "물리 피해", 1.8f, "물리 치명타 확률", 0.25f, "물리 피해", 1.8f, "물리 치명타 확률", 0.25f));
        Ins_SkillDatas.Add(new SkillData1("한손 지팡이 숙달", "한손 지팡이 착용 시 마법 피해와 마법 속도 증가", "지속 기술", 99f, "마법 피해", 6.2f, "마법 속도", 0.2f, "마법 피해", 6.2f, "마법 속도", 0.2f));
        Ins_SkillDatas.Add(new SkillData1("양손 지팡이 숙달", "양손 지팡이 착용 시 마법 피해와 MP 회복 증가", "지속 기술", 99f, "마법 피해", 8.0f, "MP 회복", 0.4f, "마법 피해", 8.0f, "MP 회복", 0.4f));
        Ins_SkillDatas.Add(new SkillData1("활 숙달", "활 착용 시 물리 피해와 치명타 피해 증가", "지속 기술", 99f, "물리 피해", 7.2f, "물리 치명타 피해", 1.5f, "물리 피해", 7.2f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("양손 검 숙달", "양손 검 착용 시 물리 피해와 명중 증가", "지속 기술", 99f, "물리 피해", 5.6f, "물리 명중", 0.3f, "물리 피해", 5.6f, "물리 명중", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("양손 둔기 숙달", "양손 둔기 착용 시 물리 피해와 HP 증가", "행동 기술", 99f, "물리 피해", 6.0f, "HP", 5.0f, "물리 피해", 6.0f, "HP", 5.0f));
        Ins_SkillDatas.Add(new SkillData1("양손 창 숙달", "양손 창 착용 시 물리 피해와 치명타 피해 증가", "행동 기술", 99f, "물리 피해", 5.2f, "물리 치명타 피해", 1.5f, "물리 피해", 5.2f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("석궁 숙달", "석궁 착용 시 물리 피해와 치명타 피해 증가", "행동 기술", 99f, "물리 피해", 3.0f, "물리 치명타 피해", 1.5f, "물리 피해", 3.0f, "물리 치명타 피해", 1.5f));
        Ins_SkillDatas.Add(new SkillData1("방패 숙달", "방패 착용 시 물리 & 마법 방어와 방패 방어율 증가", "행동 기술", 99f, "물리 방어", 6.0f, "마법 방어", 6.0f, "물리 방어", 6.0f, "마법 방어", 6.0f, "방패 방어율", 0.5f));
        Ins_SkillDatas.Add(new SkillData1("중갑 & 판금 숙달", "중갑 & 판금 착용 시 방어력과 HP & HP 회복 증가", "행동 기술", 99f, "물리 방어", 16.0f, "마법 방어", 16.0f, "물리 방어", 16.0f, "마법 방어", 16.0f, "HP", 5.0f, "HP 회복", 0.3f));
        Ins_SkillDatas.Add(new SkillData1("경갑 숙달", "경갑 착용 시 방어력과 회피, 치명타 확률, HP & MP 증가", "행동 기술", 99f, "물리 방어", 18.0f, "마법 방어", 18.0f, "물리 방어", 18.0f, "마법 방어", 18.0f, "물리 치명타 확률", 0.3f, "물리 치명타 저항률", 0.3f, "HP 회복", 3.0f, "MP", 2.0f));
        Ins_SkillDatas.Add(new SkillData1("로브 숙달", "로브 착용 시 방어력과 마법 속도, MP & MP 회복 증가", "행동 기술", 99f, "물리 방어", 9.0f, "마법 방어", 20.0f, "물리 방어", 9.0f, "마법 방어", 20.0f, "마법 속도", 0.3f, "MP", 5.0f, "MP 회복", 0.15f));
        Ins_SkillDatas.Add(new SkillData1("집중", "공격 & 마법 속도 증가", "지속 기술", 99f, "공격 속도", 0.5f, "마법 속도", 0.5f, "공격 속도", 0.5f, "마법 속도", 0.5f));
        Ins_SkillDatas.Add(new SkillData1("활력", "HP & MP & Stamina 증가", "지속 기술", 99f, "HP", 25.0f, "MP", 8.0f, "HP", 25.0f, "MP", 8.0f, "Stamina", 1.0f));
        Ins_SkillDatas.Add(new SkillData1("능숙한 활", "활 속도와 활 사용 거리 증가", "지속 기술", 99f, "활 속도", 0.5f, "활 사거리", 0.1f, "활 속도", 0.5f, "활 사거리", 0.1f));
        Ins_SkillDatas.Add(new SkillData1("약점 간파", "물리 & 마법 치명타 피해 증가", "지속 기술", 99f, "물리 치명타 피해 증가", 0.20f, "마법 치명타 피해 증가", 0.20f, "물리 치명타 피해 증가", 0.20f, "마법 치명타 피해 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("명중", "모든 명중과 상태이상 확률 증가", "지속 기술", 99f, "물리 명중", 0.6f, "마법 명중", 0.6f, "물리 명중", 0.6f, "마법 명중", 0.6f, "상태이상 확률 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("회피", "모든 회피와 상태이상 저항 증가", "행동 기술", 99f, "물리 회피", 0.6f, "마법 회피", 0.6f, "물리 회피", 0.6f, "마법 회피", 0.6f, "상태이상 저항 증가", 0.20f));
        Ins_SkillDatas.Add(new SkillData1("약점 숨기기", "받는 치명타 피해와 상태 이상에 대한 효과를 감소 #방패 착용 시 방패 방어율에 따라 추가 치명타 피해 감소 및 모든 상태 이상 저항 확률 증가 <비공식 : 방패방어율 1에 추가 치명타 피해 -0.1% 감소 / 모든 상태 이상 저항 +0.1% 증가>", "지속 기술", 99f, "물리 치명타 피해 감소", 0.25f, "마법 치명타 피해 감소", 0.25f, "물리 치명타 피해 감소", 0.25f, "마법 치명타 피해 감소", 0.25f, "상태이상 저항 증가", 0.25f));
        Ins_SkillDatas.Add(new SkillData1("공격 대상 증가", "양손무기 착용 시 물리 피해와 공격 대상 & 공격 범위 증가 #기술 점수 9점 단위로 증가", "지속 기술", 11f, "공격 대상 증가", 1.0f, "공격 대상 증가", 1.0f, "물리 피해", 15.0f, "피해 거리", 0.1f));
        Ins_SkillDatas.Add(new SkillData1("재집 숙련", "재료 채집 시 추가 획득 #Shard 종족 추가 획득 확률 증가", "행동 기술", 99f, "재료 채집 시 추가 획득 활률", 0.03f, "Shard 종족 추가 획득", 0.02f, "재료 채집 시 추가 획득 활률", 0.005f, "Shard 종족 추가 획득", 0.0055f));
    */
        #endregion
    }


    private void Awake()
    {
        if(ins==null)
            ins = this;
        
        //값 받기
        instanceget();
        instancegetpoint();

        //데이터 분할
        DistributeData();

        //툴팁 숨기기
        SkillTooltip.gameObject.SetActive(false);

        //스킬 슬롯 리스트에 넣기
        foreach (var item in ViewPointContant.transform.GetComponentsInChildren<SkillPointSlot>())
        {
            SkillPointSlotList_skill.Add(item);
        }

        //종 셋팅
        DataSetting_race();
        SettingSkillSlot_race();

        //스킬 셋팅
        DataSetting();
        SettingSkillSlot();

        //직업 셋팅
        DataSetting_job();
        SettingSkillSlot_job();
        JobLevelButtonSet();

        //무기 셋팅
        DataSetting_weapon();
        SettingSkillSlot_weapon();
        ButtonSetting_weapon();

        button.SetActive(false); //나중에 빼기 임시임
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !buttonclick)  //빈공간 클릭시 끄기 기능     (설명: 여기가 먼저 실행 후 클릭 이벤트가 실행됨 즉 껐다가 다시키는 것)
        {
            SkillTooltip.gameObject.SetActive(false);
        }
    }

    //데이터 분배
    public void DistributeData()
    {
        SkillDatas_race.Clear();
        SkillPointlist_race.Clear();

        SkillDatas_skill.Clear();
        SkillPointlist_skill.Clear();

        SkillDatas_job.Clear();
        SkillPointlist_job.Clear();

        SkillDatas_weapon.Clear();
        SkillPointlist_weapon.Clear();

        for (int i = 0; i < AllSkillDatas.Count; i++)
        {
            if (AllSkillDatas[i].Skilltype1=="종족 특화")
            {
                SkillDatas_race.Add(AllSkillDatas[i]);
                SkillPointlist_race.Add(SkillPointlist[i]);
            }
            else if (AllSkillDatas[i].Skilltype1 == "일반 기술")
            {
                SkillDatas_skill.Add(AllSkillDatas[i]);
                SkillPointlist_skill.Add(SkillPointlist[i]);
            }
            else if (AllSkillDatas[i].Skilltype1 == "전직 기술")
            {
                SkillDatas_job.Add(AllSkillDatas[i]);
                SkillPointlist_job.Add(SkillPointlist[i]);
            }
            else if (AllSkillDatas[i].Skilltype1 == "무기 특성")
            {
                SkillDatas_weapon.Add(AllSkillDatas[i]);
                SkillPointlist_weapon.Add(SkillPointlist[i]);
            }
        }
    }
    [VisibleEnum(typeof(Race))]    
    public void RefreshAll_race(int race)       //종족 필터링 버튼
    {
        DistributeData();

        //툴팁 숨기기
        SkillTooltip.gameObject.SetActive(false);

        //종 셋팅
        DataSetting_race((Race)race);
        SettingSkillSlot_race();

        //스킬 셋팅
        DataSetting((Race)race);
        SettingSkillSlot();

        //직업 셋팅
        DataSetting_job((Race)race);
        SettingSkillSlot_job();

        //무기 셋팅
        DataSetting_weapon((Race)race);
        SettingSkillSlot_weapon(Nowtype);

        button.SetActive(false); //나중에 빼기 임시임
    }

    public void LevelUpPageButton(SkillData1 data, int index,GameObject slot)
    {
        SkillLevelUpPage.SetActive(true);        
        SkillLevelUpPage.GetComponent<SkillLevelPage>().SettingLevelPage(data, index, slot);
        SkillLevelUpPage.GetComponent<SkillLevelPage>().SettingGauge();
        SkillLevelUpPage.GetComponent<SkillLevelPage>().DisplayLevelPage();
        /*
        switch (data.GetSkilltype1())
        {
            case SkillType1.Race:
                SkillLevelUpPage.SetActive(true);
                SkillLevelUpPage.GetComponent<SkillLevelPage>().SettingGauge();
                SkillLevelUpPage.GetComponent<SkillLevelPage>().DisplayLevelPage(data, index, slot);
                break;
            case SkillType1.Skill:
                // 없음
                break;
            case SkillType1.Job:
                SkillLevelUpPage.SetActive(true);
                SkillLevelUpPage.GetComponent<SkillLevelPage>().SettingGauge();
                SkillLevelUpPage.GetComponent<SkillLevelPage>().DisplayLevelPage(data, index,slot);
                break;
            case SkillType1.Weapon:
                SkillLevelUpPage.SetActive(true);
                SkillLevelUpPage.GetComponent<SkillLevelPage>().SettingGauge();
                SkillLevelUpPage.GetComponent<SkillLevelPage>().DisplayLevelPage(data, index, slot);
                break;
            default:
                break;
        }
        */
    }
    #region 무기 페이지 함수들

    public void DataSetting_weapon(Race race=Race.Null)
    {

        SkillDatas_weapon_ins.Clear();
        SkillPointlist_weapon_ins.Clear();

        //데이터 값 받기
        if (race == Race.Null)
        {
            SkillDatas_weapon_ins = SkillDatas_weapon.ToList();
            SkillPointlist_weapon_ins = SkillPointlist_weapon.ToList();
        }
        else
        {
            for (int i = 0; i < SkillDatas_weapon.Count; i++)
            {
                if (SkillDatas_weapon[i].Race.Contains(race.ToString()) || SkillDatas_weapon[i].Race == "All")
                {
                    SkillDatas_weapon_ins.Add(SkillDatas_weapon[i]);
                    SkillPointlist_weapon_ins.Add(SkillPointlist_weapon[i]);
                }
            }
        }


        innum = 0;

        WeaponLevelSkills.Clear();
        WeaponLevelKey.Clear();
        WeaponTypeKey.Clear();

        //레벨별로 카테고리 키값 넣음 ex) 1레벨 - 한손검리스트, 두손검 리스트 이렇게 들어감
        foreach (var skill in SkillDatas_weapon_ins)
        {
            inlevel = skill.StartLevel;
            instype = skill.WCate;


            if(instype.IsNullOrEmpty() == false)
            {
                string[] types = instype.Split(",");
                if(instype == "전체")
                {
                    types = "한손 검,한손 둔기,한손 창,양손 검,양손 둔기,양손 창,방패,한손 지팡이,양손 지팡이,활,석궁,단검".Split(",");
                }
                if (instype == "근접")
                {
                    types = "한손 검,한손 둔기,한손 창,양손 검,양손 둔기,양손 창,방패,한손 지팡이,양손 지팡이,단검".Split(",");
                }
                if (instype == "근접,활,석궁")
                {
                    types = "한손 검,한손 둔기,한손 창,양손 검,양손 둔기,양손 창,방패,한손 지팡이,양손 지팡이,단검,활,석궁".Split(",");
                }

                // 레벨을 키로 사용하여 딕셔너리에 추가
                if (!WeaponLevelSkills.ContainsKey(inlevel))
                {
                    WeaponLevelSkills[inlevel] = new Dictionary<string, List<SkillData1>>();
                    WeaponLevelKey.Add(inlevel);
                }

                foreach (var item in types)
                {
                    // 타입을 키로 사용하여 딕셔너리에 추가
                    if (!WeaponLevelSkills[inlevel].ContainsKey(item))
                    {
                        WeaponLevelSkills[inlevel][item] = new List<SkillData1>();

                        if (!WeaponTypeKey.Contains(item))
                        {
                            WeaponTypeKey.Add(item);
                        }
                    }
                    // 스킬을 해당 레벨 및 타입에 추가
                    WeaponLevelSkills[inlevel][item].Add(skill);
                }

            }
            skill.ForeignKey = innum;       //스킬 찍은 데이터 연결 가능하도록 키 추가
            innum++;
        }

        WeaponLevelKey.Sort();
    }

    public void ButtonSetting_weapon()
    {
        for(int i= SkillButtonSlotList_weapon.Count-1;i>=0;i--)
        {
            Destroy(SkillButtonSlotList_weapon[i]);
        }

        SkillButtonSlotList_weapon.Clear();

        for (int i=0;i< WeaponTypeKey.Count;i++)
        {
            ins_obj= Instantiate(WeaponButtonPrefab, WeaponButtonContent.transform);
            ins_obj.GetComponent<SkillPointSlot_Button_Weapon>().DisplayButton(WeaponTypeKey[i]);
            SkillButtonSlotList_weapon.Add(ins_obj.GetComponent<SkillPointSlot_Button_Weapon>());
        }
    }

    public void DisplaySkillPointSlot_weapon(string type=null)
    {
        ResetWeaponSlot();

        if (type == null)
        {
            type = WeaponTypeKey[0];            
        }

        Nowtype = type;

        for (int i = 0; i < WeaponLevelSkills.Count; i++)      //레벨 묶음당 타입별 묶음 슬롯 그룹
        {
            if (WeaponLevelSkills[WeaponLevelKey[i]].ContainsKey(type))
            {
                ins_obj = Instantiate(WeaponSkillPrefab, WeaponPageContent.transform);
                ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon_Group>().ResetText();
                SkillPointSlotList_weapon.Add(ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon_Group>());
                
                //타입 슬롯 그룹
                for (int ii = 0; ii < WeaponLevelSkills[WeaponLevelKey[i]][type].Count; ii++)
                {
                    ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon_Group>().AddList(WeaponLevelSkills[WeaponLevelKey[i]][type][ii], WeaponLevelSkills[WeaponLevelKey[i]][type][ii].ForeignKey);
                }
                ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon_Group>().SetText(WeaponLevelSkills[WeaponLevelKey[i]][type][0].StartLevel.ToString(), WeaponLevelSkills[WeaponLevelKey[i]][type][0].Race, WeaponLevelSkills[WeaponLevelKey[i]][type][0].Skilltype2, i);
            }
        }

    }

    public void WeaponAllRefresh()
    {
        for (int i = 0; i < SkillPointSlotList_weapon.Count; i++)
        {
            SkillPointSlotList_weapon[i].Refresh();
        }
    }

    public void ResetWeaponSlot()
    {
        if (SkillPointSlotList_weapon.Count != 0)
        {
            for(int i = SkillPointSlotList_weapon.Count-1; i>=0; i--)
            {
                Destroy(SkillPointSlotList_weapon[i].gameObject);
            }
            SkillPointSlotList_weapon.Clear();
        }
    }
    public void Refresh_weapon(GameObject slot)
    {
        slot.GetComponent<SkillPointSlot_Scroll_Weapon>().TextRefresh();
    }

    public void SkillUp_weapon(int index)
    {
        if (SkillPointlist_weapon_ins[index].PlusNumber < SkillDatas_weapon_ins[index].EndLevel)
        {
            //스킬 포인트 감소

            //해당 스킬 증가
            SkillPointlist_weapon_ins[index].PlusNumber++;
        }

    }
    public void SettingSkillSlot_weapon(string type=null)
    {
        //슬롯 세팅
        DisplaySkillPointSlot_weapon(type);
    }

    #endregion

    #region 직업 페이지 함수들

    public void JobLevelButtonSet()
    {
        for (int i = 0; i < JobLevelSkills.Count; i++)
        {
            insbutton = Instantiate(JobLevelPageButton, JobLevelPageButtonsPosion);
            insbutton.GetComponent<JobLevelPageButton>().SetButton("["+(i+1) + "차 전직]", i);
        }
    }

    public void JobResetRefresh(int i)
    {
        JobLevelButtonIndex = i;

        for(int n = SkillPointSlotList_job.Count-1;n>=0;n--)
        {
            Destroy(SkillPointSlotList_job[n].gameObject);
        }

        SkillPointSlotList_job.Clear();

        ins_obj = Instantiate(JobSkillPrefab, JobPageContent.transform);
        ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().ResetText();
        SkillPointSlotList_job.Add(ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>());


        //지속 기술 슬롯 그룹
        for (int ii = 0; ii < JobLevelSkills[JobLevelKey[i]]["지속 기술"].Count; ii++)
        {
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().AddList(JobLevelSkills[JobLevelKey[i]]["지속 기술"][ii], JobLevelSkills[JobLevelKey[i]]["지속 기술"][ii].ForeignKey);
        }
        ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().SetText(JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].StartLevel.ToString(), JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].Race, JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].Skilltype2);


        //행동 기술 슬롯 그룹
        ins_obj = Instantiate(JobSkillPrefab, JobPageContent.transform);
        ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().ResetText();
        SkillPointSlotList_job.Add(ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>());

        for (int ii = 0; ii < JobLevelSkills[JobLevelKey[i]]["행동 기술"].Count; ii++)
        {
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().AddList(JobLevelSkills[JobLevelKey[i]]["행동 기술"][ii], JobLevelSkills[JobLevelKey[i]]["행동 기술"][ii].ForeignKey);
        }
        ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().SetText("", "", JobLevelSkills[JobLevelKey[i]]["행동 기술"][0].Skilltype2);
    }

    public void DataSetting_job(Race race=Race.Null)
    {

        SkillDatas_job_ins.Clear();
        SkillPointlist_job_ins.Clear();

        //데이터 값 받기
        if (race == Race.Null)
        {
            SkillDatas_job_ins = SkillDatas_job.ToList();
            SkillPointlist_job_ins = SkillPointlist_job.ToList();
        }
        else
        {
            for (int i = 0; i < SkillDatas_job.Count; i++)
            {
                if (SkillDatas_job[i].Race.Contains(race.ToString()) || SkillDatas_job[i].Race == "All")
                {
                    SkillDatas_job_ins.Add(SkillDatas_job[i]);
                    SkillPointlist_job_ins.Add(SkillPointlist_job[i]);
                }
            }
        }


        innum = 0;

        JobLevelSkills.Clear();
        JobLevelKey.Clear();
        
        //레벨당 패시브, 액티브 나누기
        foreach (var skill in SkillDatas_job_ins)
        {
            inlevel = skill.StartLevel;
            instype = skill.Skilltype2;

            // 레벨을 키로 사용하여 딕셔너리에 추가
            if (!JobLevelSkills.ContainsKey(inlevel))
            {
                JobLevelSkills[inlevel] = new Dictionary<string, List<SkillData1>>();
                JobLevelKey.Add(inlevel);
            }

            // 타입을 키로 사용하여 딕셔너리에 추가
            if (!JobLevelSkills[inlevel].ContainsKey(instype))
            {
                JobLevelSkills[inlevel][instype] = new List<SkillData1>();
            }

            // 스킬을 해당 레벨 및 타입에 추가
            JobLevelSkills[inlevel][instype].Add(skill);
            skill.ForeignKey = innum;       //스킬 찍은 데이터 연결 가능하도록 키 추가
            innum++;
        }
        Debug.Log("딕셔너리 갯수확인" + JobLevelSkills.Count);
        JobLevelKey.Sort();
        JobLevelButtonIndex = -1;
    }

    public void JobAllRefresh()
    {
        for (int i = 0; i < SkillPointSlotList_job.Count; i++)
        {
            SkillPointSlotList_job[i].Refresh();
        }
    }

    public void DisplaySkillPointSlot_job()
    {
        if(SkillPointSlotList_job.Count != 0)
        {
            for (int i = SkillPointSlotList_job.Count-1; i >= 0; i--)
            {
                Destroy(SkillPointSlotList_job[i].gameObject);
            }
            SkillPointSlotList_job.Clear();
        }

        for (int i = 0; i < JobLevelSkills.Count; i++)      //레벨 묶음당 지속, 행동 슬롯 묶음 총 2개
        {
            ins_obj = Instantiate(JobSkillPrefab, JobPageContent.transform);
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().ResetText();
            SkillPointSlotList_job.Add(ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>());
            

            //지속 기술 슬롯 그룹
            for (int ii = 0; ii < JobLevelSkills[JobLevelKey[i]]["지속 기술"].Count; ii++)
            {
                ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().AddList(JobLevelSkills[JobLevelKey[i]]["지속 기술"][ii], JobLevelSkills[JobLevelKey[i]]["지속 기술"][ii].ForeignKey);
            }
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().SetText(JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].StartLevel.ToString(), JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].Race, JobLevelSkills[JobLevelKey[i]]["지속 기술"][0].Skilltype2);


            //행동 기술 슬롯 그룹
            ins_obj = Instantiate(JobSkillPrefab, JobPageContent.transform);
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().ResetText();
            SkillPointSlotList_job.Add(ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>());

            for (int ii = 0; ii < JobLevelSkills[JobLevelKey[i]]["행동 기술"].Count; ii++)
            {
                ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().AddList(JobLevelSkills[JobLevelKey[i]]["행동 기술"][ii], JobLevelSkills[JobLevelKey[i]]["행동 기술"][ii].ForeignKey);
            }
            ins_obj.GetComponent<SkillPointSlot_Scroll2_Group>().SetText("", "", JobLevelSkills[JobLevelKey[i]]["행동 기술"][0].Skilltype2);
        }

    }

    public void Refresh_job(GameObject slot)
    {
        slot.GetComponent<SkillPointSlot_Scroll2>().TextRefresh();
    }

    public void SkillUp_job(int index)
    {
        if (SkillPointlist_job_ins[index].PlusNumber < SkillDatas_job_ins[index].EndLevel)
        {
            //스킬 포인트 감소

            //해당 스킬 증가
            SkillPointlist_job_ins[index].PlusNumber++;
        }

    }
    public void SettingSkillSlot_job()
    {
        //슬롯 세팅
        DisplaySkillPointSlot_job();
    }

    #endregion

    #region 종 페이지 함수들
    public void DataSetting_race(Race race=Race.Null)
    {

        SkillDatas_race_ins.Clear();
        SkillPointlist_race_ins.Clear();

        //데이터 값 받기
        if (race == Race.Null)
        {
            SkillDatas_race_ins = SkillDatas_race.ToList();
            SkillPointlist_race_ins = SkillPointlist_race.ToList();
        }
        else
        {
            for (int i = 0; i < SkillDatas_race.Count; i++)
            {
                if (SkillDatas_race[i].Race.Contains(race.ToString()) || SkillDatas_race[i].Race == "All")
                {
                    SkillDatas_race_ins.Add(SkillDatas_race[i]);
                    SkillPointlist_race_ins.Add(SkillPointlist_race[i]);
                }
            }
        }

    }

    public void DisplaySkillPointSlot_race()
    {
        if (SkillPointSlotList_race.Count != 0) 
        {
            for (int i = SkillPointSlotList_race.Count-1; i >= 0 ; i--)
            {
                Destroy(SkillPointSlotList_race[i].gameObject);
            }
            SkillPointSlotList_race.Clear();
        }

        for (int i = 0; i < SkillDatas_race_ins.Count; i++)
        {
            ins_obj = Instantiate(RaceSkillPrefab, RacePageContent.transform);
            SkillPointSlotList_race.Add(ins_obj.GetComponent<SkillPointSlot_Scroll>());
            ins_obj.GetComponent<SkillPointSlot_Scroll>().DisplaySlot(SkillDatas_race_ins[i], i);
        }

    }

    public void RaceAllRefresh()
    {
        for (int i = 0; i < SkillPointSlotList_race.Count; i++)
        {
            SkillPointSlotList_race[i].Refresh();
        }
    }


    public void RaceRefresh(GameObject slot)
    {
        slot.GetComponent<SkillPointSlot_Scroll>().TextRefresh();
    }

    public void SkillUp_Race(int index)
    {
        if (SkillPointlist_race_ins[index].PlusNumber < SkillDatas_race_ins[index].EndLevel)
        {
            //스킬 포인트 감소

            //해당 스킬 증가
            SkillPointlist_race_ins[index].PlusNumber++;
        }

    }
    public void SettingSkillSlot_race()
    {
        //슬롯 세팅
        DisplaySkillPointSlot_race();
    }

    #endregion

    #region 스킬 페이지 함수들
    public void SettingSkillSlot()
    {
        //페이지 셋팅
        NowPage = 1;
        MaxPage = (SkillDatas_ins.Count / SkillPointSlotList_skill.Count)+1;
        UpdatePageText();

        //슬롯 세팅
        DisplaySkillPointSlot();
    }

    public void SetType(SkillType2 type)
    {
        this.type = type;
        DataSetting();
        SettingSkillSlot();
    }

    public void DataSetting(Race race=Race.Null)
    {
        SkillDatas_ins.Clear();
        SkillPointlist_ins.Clear();

        switch (type)
        {
            case SkillType2.All:
                //데이터 값 받기
                if (race == Race.Null)
                {
                    SkillDatas_ins = SkillDatas_skill.ToList();
                    SkillPointlist_ins = SkillPointlist_skill.ToList();
                }
                else
                {
                    for (int i = 0; i < SkillDatas_skill.Count; i++)
                    {
                        if (SkillDatas_skill[i].Skilltype2 == "지속 기술" && (SkillDatas_skill[i].Race.Contains(race.ToString()) || SkillDatas_skill[i].Race == "All"))
                        {
                            SkillDatas_ins.Add(SkillDatas_skill[i]);
                            SkillPointlist_ins.Add(SkillPointlist_skill[i]);
                        }
                    }
                }
                break;
            case SkillType2.Passive:
                if (race == Race.Null)
                {
                    for (int i = 0; i < SkillDatas_skill.Count; i++)
                    {
                        if (SkillDatas_skill[i].Skilltype2 == "지속 기술")
                        {
                            SkillDatas_ins.Add(SkillDatas_skill[i]);
                            SkillPointlist_ins.Add(SkillPointlist_skill[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SkillDatas_skill.Count; i++)
                    {
                        if (SkillDatas_skill[i].Skilltype2 == "지속 기술" && (SkillDatas_skill[i].Race.Contains(race.ToString()) || SkillDatas_skill[i].Race == "All"))
                        {
                            SkillDatas_ins.Add(SkillDatas_skill[i]);
                            SkillPointlist_ins.Add(SkillPointlist_skill[i]);
                        }
                    }
                }
                break;
            case SkillType2.Active:
                if (race == Race.Null)
                {
                    for (int i = 0; i < SkillDatas_skill.Count; i++)
                    {
                        if (SkillDatas_skill[i].Skilltype2 == "행동 기술")
                        {
                            SkillDatas_ins.Add(SkillDatas_skill[i]);
                            SkillPointlist_ins.Add(SkillPointlist_skill[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SkillDatas_skill.Count; i++)
                    {
                        if (SkillDatas_skill[i].Skilltype2 == "행동 기술" && (SkillDatas_skill[i].Race.Contains(race.ToString()) || SkillDatas_skill[i].Race == "All"))
                        {
                            SkillDatas_ins.Add(SkillDatas_skill[i]);
                            SkillPointlist_ins.Add(SkillPointlist_skill[i]);
                        }
                    }
                }
                break;
            default:                
                break;
        }
    }

    public void DisplaySkillPointSlot()
    {
        for(int i=0;i< SkillPointSlotList_skill.Count;i++)
        {
            if (NowPage == MaxPage && (i+ SkillPointSlotList_skill.Count*(NowPage-1))> SkillDatas_ins.Count-1)
            {
                SkillPointSlotList_skill[i].gameObject.SetActive(false);                
            }
            else
            {
                SkillPointSlotList_skill[i].DisplaySlot(SkillDatas_ins[(NowPage - 1) * SkillPointSlotList_skill.Count + i], (NowPage - 1) * SkillPointSlotList_skill.Count + i);
                SkillPointSlotList_skill[i].gameObject.SetActive(true);
            }
        }
    }

    public void SkillUp(int index)
    {
        if (SkillPointlist_ins[index].PlusNumber < SkillDatas_ins[index].EndLevel)
        {
            //스킬 포인트 감소

            //해당 스킬 증가
            SkillPointlist_ins[index].PlusNumber++;
        }
    }

    public void UpdatePageText()
    {
        PageText.text = NowPage + "/" + MaxPage;
    }


    
    #region 버튼
    public void UpPageButton()
    {
        if (NowPage < MaxPage)
        {
            NowPage++;
            UpdatePageText();
            DisplaySkillPointSlot();
        }
    }

    public void DownPageButton()
    {
        if (NowPage > 1)
        {
            NowPage--;
            UpdatePageText();
            DisplaySkillPointSlot();
        }
    }

    public void AllSkillButton()
    {
        SkillPage.SetActive(true);
        SetType(SkillType2.All);        
    }

    public void PassiveSkillButton()
    {
        SkillPage.SetActive(true);
        SetType(SkillType2.Passive);     
    }

    public void ActiveSkillButton()
    {
        SkillPage.SetActive(true);
        SetType(SkillType2.Active);        
    }
    #endregion
    #endregion
}
