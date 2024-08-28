using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelPage : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI FrontText;
    public TextMeshProUGUI BackText;
    public Button Upgrade;
    public TextMeshProUGUI NeedText;
    public GameObject GaugeContent;
    public GameObject GaugePrefab;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    [SerializeField] string insstring;
    [SerializeField] SkillData1 Insdata;
    [SerializeField] int Insindex;
    [SerializeField] GameObject Slot;
    [SerializeField] List<GaugeImage> UpgradeImageSlot=new List<GaugeImage>();
    public GameObject ins_obj;

    public void DisplayLevelPage()
    {
        FrontText.text = "";
        BackText.text = "";

        TitleText.text = Insdata.Name;
        UpdateBodyText(Insdata, Insindex);
        if (Insdata.EndLevel > SkillPointManager.ins.GetSkillPoint(Insindex, Insdata.GetSkilltype1()).PlusNumber + 1)
        {
            BackText.text += "+" + (SkillPointManager.ins.GetSkillPoint(Insindex, Insdata.GetSkilltype1()).PlusNumber + 1) + " " + Insdata.Name;
        }
        NeedText.text = "강화시 ㅁㅁㅁ 필요";

        //막대 단계 표시하기
        DisplayGauge();
    }

    public void SettingLevelPage(SkillData1 data, int index, GameObject slot = null)
    {
        Insdata = data;
        Insindex = index;
        Slot = slot;
    }

    public void DisplayGauge()
    {
        for(int i =0;i< SkillPointManager.ins.GetSkillPoint(Insindex, Insdata.GetSkilltype1()).PlusNumber;i++)
        {
            UpgradeImageSlot[i].Display();
        }
    }

    public void SettingGauge()
    {
        ResetGauge();

        for (int i = 0; i < Insdata.EndLevel; i++)
        {
            ins_obj = Instantiate(GaugePrefab, GaugeContent.transform);
            UpgradeImageSlot.Add(ins_obj.GetComponent<GaugeImage>());
        }
    }

    public void ResetGauge()
    {
        if (UpgradeImageSlot.Count != 0)
        {
            for (int i = UpgradeImageSlot.Count - 1; i >= 0; i--)
            {
                Destroy(UpgradeImageSlot[i].gameObject);
            }
            UpgradeImageSlot.Clear();
        }
    }

    public void UpdateBodyText(SkillData1 data, int index)
    {
        SkillPointManager.ins.GetSkillPoint(index, data.GetSkilltype1()).UpdateStat(data, index, false);
        SkillPointManager.ins.GetSkillPoint(index, data.GetSkilltype1()).UpdateStat(data, index, true);

        FrontText.text = SkillPointManager.ins.GetSkillPoint(index, data.GetSkilltype1()).StatText(false) + "\n\n" + "+" + SkillPointManager.ins.GetSkillPoint(index, data.GetSkilltype1()).PlusNumber + " " + data.Name;
        BackText.text = SkillPointManager.ins.GetSkillPoint(index, data.GetSkilltype1()).StatText(true) + "\n\n";
    }

    public void UpgradeLevel()
    {
        //if()  확률 계산식

        //강화에 성공할 경우
        switch (Insdata.GetSkilltype1())
        {
            case SkillType1.Race:
                //적용
                SkillPointManager.ins.SkillUp_Race(Insindex);
                //슬롯 리프레쉬
                SkillPointManager.ins.RaceRefresh(Slot);
                break;
            case SkillType1.Skill:
                // 없음
                break;
            case SkillType1.Job:
                //적용
                SkillPointManager.ins.SkillUp_job(Insindex);
                //슬롯 리프레쉬
                SkillPointManager.ins.Refresh_job(Slot);
                break;
            case SkillType1.Weapon:
                //적용
                SkillPointManager.ins.SkillUp_weapon(Insindex);
                //슬롯 리프레쉬
                SkillPointManager.ins.Refresh_weapon(Slot);
                break;
            default:
                break;
        }



        //리프레쉬
        DisplayLevelPage();
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
