using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot_Button_Weapon : MonoBehaviour
{
    [Header("---------------------------------------직접 넣어야 하는것---------------------------------------")]
    public TextMeshProUGUI TypeText;
    public Toggle toggle;
    [Header("---------------------------------------값 확인용 (변경x)---------------------------------------")]
    [SerializeField] string Type;

    public void DisplayButton(string type)
    {
        toggle.group = this.transform.parent.GetComponent<ToggleGroup>();
        Type = type;

        TextRefresh();
    }

    public void TextRefresh()
    {
        TypeText.text = Type;
    }

    public void ButtonUp()
    {
        SkillPointManager.ins.DisplaySkillPointSlot_weapon(Type);
        SkillPointManager.ins.WeaponPageContent.GetRectTransform().position = new Vector3(SkillPointManager.ins.WeaponPageContent.GetRectTransform().position.x, 0, SkillPointManager.ins.WeaponPageContent.GetRectTransform().position.z);
    }
}
