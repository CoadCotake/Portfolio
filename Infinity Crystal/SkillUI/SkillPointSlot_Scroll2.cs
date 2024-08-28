using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot_Scroll2 : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public Button PlusButton;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;
    public TextMeshProUGUI UseText;
    public TextMeshProUGUI NextLevelText;
    public TextMeshProUGUI SkillLevelText;
    public Image image;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    [SerializeField] string insstring;
    [SerializeField] SkillData1 Insdata;
    [SerializeField] int Insindex;
    

    public void DisplaySlot(SkillData1 data, int index)
    {
        Insdata = data;
        Insindex = index;

        //스프라이트 설정
        image.sprite = data.GetSkillIcon();

        if (image.sprite == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }

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
