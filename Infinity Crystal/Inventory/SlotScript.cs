using TMPro;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

public enum SlotCategory
{
    head, cloak, shoulder, arm, gloves, upperbody, belt, lowerbody, shoes,
    weapon, assistant, earring, bracelet, ring, necklace, charm, item, potion
}

public class SlotScript : MonoBehaviour
{
    private ItemV2 _slotItem;

    public ItemV2 SlotItem {
        set
        {
            UpdateItem(value);
        }
        get
        {
            return _slotItem;
        }
    }

    void UpdateItem(ItemV2 item)
    {
        _slotItem = item;
        if (iconImage != null)
        {
            Sprite spr = null;
            if(item != null)
                spr = item.ItemData1.GetSprite();
            iconImage.sprite = spr;

            if (iconImage.sprite != null)
            {
                Color color = iconImage.color;
                color.a = 1.0f;
                iconImage.color = color;
            }
            else
            {
                Color color = iconImage.color;
                color.a = 0.0f;
                iconImage.color = color;
            }
        }
    }

    public void OnClickItemSlot()
    {
        Debug.Log("버튼 클릭");
        if (_slotItem != null)
        {
            TooltipScript.Instance.DisplayTooltip(this);
            TooltipScript.Instance.tooltipPosition(new Vector2(transform.position.x, transform.position.y));
        }
    }

    #region Slot Editor Settings

    [Header("Slot Settings")]
    public SlotCategory slotType;

    public Image edgeImage;
    public Image overlayImage;
    public Image backImage;
    public Image iconImage;
    public Image equippedImage;

    public Button slotButton;
    public TextMeshProUGUI slotGrade;

    private void OnEnable()
    {
        equippedImage.gameObject.SetActive(false);
        slotGrade.text = "";
        UpdateSlotBackImage();
        slotButton.onClick.AddListener(OnClickItemSlot);
    }

    [ContextMenu("Update Slot Image")]
    public void UpdateSlotBackImage()
    {
        if (backImage != null)
        {
            string path = "#04.Icons/" + slotType.ToString();  // 예: "Slots/head"
            Sprite slotSprite = Resources.Load<Sprite>(path);
            if (slotSprite != null)
            {
                backImage.sprite = slotSprite;
                Color color = backImage.color;
                color.a = 0.5f;
                backImage.color = color;
                Debug.LogWarning("image found at " + path);
            }
            else
            {
                Color color = backImage.color;
                color.a = 0.0f;
                backImage.color = color;
                Debug.LogWarning("No image found at " + path);
            }

            if (_slotItem!=null)
            {
                slotGrade.text = _slotItem.ItemData1.Tier + "T/" + ItemV2.Utils.GetColoredGrade(_slotItem.ItemData1);

                // 슬롯 색깔
                string inputText = _slotItem.ItemData1.Grade;
                string colorCode = "#FFFFFF"; // Default color (black)
                if (inputText.Contains("A+"))
                {
                    colorCode = "#FFC000";
                }
                else if (inputText.Contains("S+"))
                {
                    colorCode = "#FF5100";
                }
                else if (inputText.Contains("S"))
                {
                    colorCode = "#FFC000";
                }
                Color newColor;
                if (ColorUtility.TryParseHtmlString(colorCode, out newColor))
                {
                    // Keep the original alpha for edgeImage
                    newColor.a = edgeImage.color.a;
                    edgeImage.color = newColor;

                    // Keep the original alpha for overlayImage
                    newColor.a = overlayImage.color.a;
                    overlayImage.color = newColor;
                }

                // 장비중인지
                equippedImage.gameObject.SetActive(DataManager.Instance.IsItemUsed(_slotItem));
            }
        }
    }

    #endregion
}
