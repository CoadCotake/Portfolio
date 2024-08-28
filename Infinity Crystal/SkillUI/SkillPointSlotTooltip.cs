using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using Unity.VisualScripting.FullSerializer;

public class SkillPointSlotTooltip : MonoBehaviour
{
    [Header("직접 넣어야 하는것")]
    public Image Image;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI NowSkillLevelText;
    public TextMeshProUGUI MaxSkillLevelText;
    public TextMeshProUGUI BodyText;
    public TextMeshProUGUI TypeText;

    string insstring;

    public void OnSkillTooltip(SkillData1 data, int index)
    {
        Image.sprite = data.GetSkillIcon();

        if (Image.sprite == null)
        {
            Image.enabled = false;
        }
        else
        {
            Image.enabled = true;
        }

        TitleText.text = data.Name;
        NowSkillLevelText.text = " Level"+SkillPointManager.ins.GetSkillPoint(index).PlusNumber.ToString();
        MaxSkillLevelText.text = "[Max "+ data.EndLevel.ToString()+ "]";
        UpdateBodyText(data,index);
        TypeText.text = data.Skilltype2;

        gameObject.SetActive(true);
    }

    public void UpdateBodyText(SkillData1 data, int index)
    {
        SkillPointManager.ins.GetSkillPoint(index).UpdateStat(data, index, false);
        SkillPointManager.ins.GetSkillPoint(index).UpdateStat(data, index, true);

        BodyText.text = SkillPointManager.ins.GetSkillPoint(index).StatText(false) + "\n\n Next Lv:\n" + SkillPointManager.ins.GetSkillPoint(index).StatText(true);
    }
}
