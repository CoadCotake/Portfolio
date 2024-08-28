using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot_Scroll2_Group : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public GameObject EndText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI Type1Text;
    public TextMeshProUGUI Type2Text;
    public GameObject Content;
    public GameObject SlotPrefab;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    public List<SkillPointSlot_Scroll2> SkillSlotScroll2List = new List<SkillPointSlot_Scroll2>();


    public void ResetText()
    {
        //EndText.SetActive(false);
        LevelText.text = "";
        Type1Text.text = "";
        Type2Text.text = "";
    }

    public void SetText(string level, string type1, string type2)
    {
        LevelText.text = level;
        Type1Text.text = type1;
        Type2Text.text = type2;

        SetGroupSize();
    }

    public void EndSlot()
    {
        EndText.SetActive(true);
    }

    public void SetGroupSize()
    {
        gameObject.GetRectTransform().sizeDelta = new Vector2(this.gameObject.GetRectTransform().sizeDelta.x, 240 * ((SkillSlotScroll2List.Count + 2) / 3));
    }

    public GameObject ins_obj;
    public void AddList(SkillData1 skilldata1, int index)
    {
        ins_obj = Instantiate(SlotPrefab, Content.transform);
        SkillSlotScroll2List.Add(ins_obj.GetComponent<SkillPointSlot_Scroll2>());
        ins_obj.GetComponent<SkillPointSlot_Scroll2>().DisplaySlot(skilldata1, index);
    }

    public void Refresh()
    {
        for (int i = 0; i < SkillSlotScroll2List.Count; i++)
        {
            SkillSlotScroll2List[i].Refresh();
        }
    }
}
