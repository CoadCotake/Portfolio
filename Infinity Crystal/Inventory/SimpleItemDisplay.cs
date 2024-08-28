using DTT.Utils.Extensions;
using PWCommon5;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleItemDisplay : MonoBehaviour, TooltipClickSetting
{
    public bool TooltipSlot = false;        //툴팁 띄우는 슬롯인가?
    public bool OnlySee = false;            //보기전용 인가?

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI gradeText;
    public ItemDisplayData currentItem;
    public GameObject stateIcon;

    public void SetItem(ItemDisplayData item)
    {
        currentItem = item;
        UpdateItemDisplay();
    }

    private void UpdateItemDisplay()
    {
        try
        {
            if (iconImage != null && currentItem != null)
            {
                if (currentItem.isGold)
                {
                    gradeText.text = $"{currentItem.count}";
                    iconImage.sprite = Resources.Load<Sprite>($"#02.Sprites/Items/{"Loot_129"}");
                    iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1.0f); // 아이템이 있으면 투명도를 1로 설정
                }
                else
                {
                    iconImage.sprite = currentItem.itemV2.ItemData1.GetSprite();
                    iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 1.0f); // 아이템이 있으면 투명도를 1로 설정
                    UpdateGradeText();
                }
            }
            else
            {
                iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 0.0f); // 아이템이 없으면 투명도를 0으로 설정
                gradeText.text = ""; // 아이템이 없을 때 등급 텍스트를 비움
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void UpdateGradeText()
    {
        if (gradeText != null && currentItem != null)
        {
            if (currentItem.itemV2.ItemData1.Tier == 0)
            {
                gradeText.text = "";
            }
            else if (currentItem.itemV2.ItemData1.Grade.IsNullOrEmpty() || currentItem.itemV2.ItemData1.Grade == "Non")
            {
                string tier = currentItem.itemV2.ItemData1.Tier.ToString();
                string coloredGrade = ItemV2.Utils.GetColoredGrade(currentItem.itemV2.ItemData1);

                gradeText.text = $"{tier}T";
            }
            else
            {
                string tier = currentItem.itemV2.ItemData1.Tier.ToString();
                string coloredGrade = ItemV2.Utils.GetColoredGrade(currentItem.itemV2.ItemData1);

                gradeText.text = $"{tier}T/{coloredGrade}"; // 예: "3T/<color=#FF0000>A+</color>"
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipScript.Instance.ToolTipOnPointerEnter(eventData, TooltipSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TooltipScript.Instance.ToolTipOnPointerClick(eventData, currentItem.itemV2,TooltipSlot, OnlySee);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipScript.Instance.ToolTipOnPointerExit(eventData, TooltipSlot);
    }
}
