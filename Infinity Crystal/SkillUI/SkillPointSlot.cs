using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot : MonoBehaviour
{
    [Header("직접 넣어야 하는것")]
    public Image Image;
    public Button PlusButton;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;
    public TextMeshProUGUI SkillLevelText;


    SkillData1 Insdata;
    int Insindex;
    [SerializeField]string insstring;

    public void DisplaySlot(SkillData1 data,int index)
    {
        Insdata = data;
        Insindex = index;
        
        Image.sprite = data.GetSkillIcon();

        if (Image.sprite==null)
        {
            Image.enabled=false;
        }
        else
        {
            Image.enabled = true;
        }

        TitleText.text = data.Name;
        UpdateBodyText(data,index);
        SkillLevelText.text = "Lv "+SkillPointManager.ins.GetSkillPoint(index).PlusNumber.ToString();
    }

    public void UpdateBodyText(SkillData1 data,int index)
    {
        SkillPointManager.ins.GetSkillPoint(index).UpdateStat(data, index,false);
        BodyText.text = SkillPointManager.ins.GetSkillPoint(index).StatText(false);
    }

    public void Refresh()
    {
        DisplaySlot(Insdata, Insindex);
    }

    public void OnToolTip()
    {
        SkillPointManager.ins.SkillTooltip.OnSkillTooltip(Insdata, Insindex);
    }

    public void UpButton()
    {
        SkillPointManager.ins.SkillUp(Insindex);
        Refresh();
    }
}
