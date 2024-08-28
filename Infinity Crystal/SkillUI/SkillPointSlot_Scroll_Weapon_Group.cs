using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot_Scroll_Weapon_Group : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public GameObject EndText;
    public TextMeshProUGUI LevelText;
    public GameObject Content;
    public GameObject SlotPrefab;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    public List<SkillPointSlot_Scroll_Weapon> SkillSlotScroll2List = new List<SkillPointSlot_Scroll_Weapon>();


    public void ResetText()
    {
        //EndText.SetActive(false);
        LevelText.text = "";
    }

    public void SetText(string level, string type1, string type2,int num)
    {
        LevelText.text =(num+1)+"차\n"+"<Lv "+ level+">";

        SetGroupSize();
    }

    public void EndSlot()
    {
        EndText.SetActive(true);
    }

    public void SetGroupSize()
    {
        gameObject.GetRectTransform().sizeDelta = new Vector2(this.gameObject.GetRectTransform().sizeDelta.x, 110 * ((SkillSlotScroll2List.Count + 2) / 3));
    }

    public GameObject ins_obj;
    public void AddList(SkillData1 skilldata1, int index)
    {
        ins_obj = Instantiate(SlotPrefab, Content.transform);
        SkillSlotScroll2List.Add(ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon>());
        ins_obj.GetComponent<SkillPointSlot_Scroll_Weapon>().DisplaySlot(skilldata1, index);

    }

    public void Refresh()
    {
        for (int i = 0; i < SkillSlotScroll2List.Count; i++)
        {
            SkillSlotScroll2List[i].Refresh();
        }
    }
}
