using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SkillPointSlot_Scroll : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public Button PlusButton;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;
    public TextMeshProUGUI LimitText;
    public TextMeshProUGUI NextLevelText;
    public TextMeshProUGUI SkillLevelText;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    [SerializeField] string insstring;
    [SerializeField] SkillData1 Insdata;
    [SerializeField] int Insindex;
    

    public void DisplaySlot(SkillData1 data, int index)
    {
        Insdata = data;
        Insindex = index;

        TextRefresh();
    }

    public void TextRefresh()
    {
        TitleText.text = Insdata.Name;
        UpdateBodyText(Insdata, Insindex);
        SkillLevelText.text = SkillPointManager.ins.GetSkillPoint(Insindex, Insdata.GetSkilltype1()).PlusNumber.ToString();
    }


    public void UpdateBodyText(SkillData1 data, int index)
    {
        SkillPointManager.ins.GetSkillPoint(index, Insdata.GetSkilltype1()).UpdateStat(data, index, false);
        SkillPointManager.ins.GetSkillPoint(index, Insdata.GetSkilltype1()).UpdateStat(data, index, true);

        BodyText.text = SkillPointManager.ins.GetSkillPoint(index, Insdata.GetSkilltype1()).StatText(false);
        NextLevelText.text = "Next Lv " + SkillPointManager.ins.GetSkillPoint(index, Insdata.GetSkilltype1()).StatText(true);
    }

    public void Refresh()
    {
        DisplaySlot(Insdata, Insindex);
    }

    public void UpButton()
    {
        //강화 페이지 열기
        SkillPointManager.ins.LevelUpPageButton(Insdata, Insindex,this.gameObject);

        /*SkillPointManager.ins.SkillUp(Insindex);
        Refresh();*/
    }
}
