using TMPro;
using UIWidgets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface TooltipClickSetting : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)      //클릭 전 이벤트 - 예를 들어 버튼 위에 아이콘을 클릭할 때 여기서 처리해주면 됨 
    {
        //TooltipScript.Instance.ToolTipOnPointerEnter(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)  //클릭 이벤트 - 툴팁 처리
    {
        //TooltipScript.Instance.ToolTipOnPointerClick(eventData);  
    }


    public void OnPointerExit(PointerEventData eventData)   //클릭 후 이벤트 - 후처리
    {
        //TooltipScript.Instance.ToolTipOnPointerExit(eventData);
    }
}

public class TooltipScript : MonoBehaviour, IPointerClickHandler
{
    public static TooltipScript Instance;
    public TextMeshProUGUI tooltipText; // Assign a UI Text component to this variable in the inspector

    public Button equipButton;
    public Button unequipButton;
    public Button useButton;

    SlotScript currentSlot;

    string inputText;
    string colorCode;
    string tooltipString;

    public int width;
    public int height;
    GameObject ClickObject;

    public bool buttonclick;
    Vector3 pos;

    private void Awake()
    {
        Instance = this;
        Instance.gameObject.SetActive(false);
        width = (int)GetComponent<RectTransform>().rect.width;
        height = (int)GetComponent<RectTransform>().rect.height;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0) &&!buttonclick)  //빈공간 클릭시 끄기 기능     (설명: 여기가 먼저 실행 후 클릭 이벤트가 실행됨 즉 껐다가 다시키는 것)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void DisplayTooltip(SlotScript slot,bool OnlySee=false)
    {
        currentSlot = slot;
        ItemV2 itemV2 = slot.SlotItem;

        Instance.gameObject.SetActive(true);
        UpdateText(itemV2.ItemData1);
        if (!OnlySee)
        {
            UpdateButtons(itemV2, itemV2.ItemData1.GetItemType());
        }
        else
        {
            CloseButton();
        }
    }

    public void DisplayTooltip(ItemV2 Item, bool OnlySee = false)
    {
        ItemV2 itemV2 = Item;

        Instance.gameObject.SetActive(true);
        UpdateText(itemV2.ItemData1);
        if (!OnlySee)
        {
            UpdateButtons(itemV2, itemV2.ItemData1.GetItemType());
        }
        else
        {
            CloseButton();
        }
    }

    void CloseButton()
    {
        equipButton.gameObject.SetActive(false); // Activate equip button only for equippable items
        unequipButton.gameObject.SetActive(false); // A condition for unequip can be added here
        useButton.gameObject.SetActive(false); // Activate use button only for potions, for example
    }

    private void UpdateButtons(ItemV2 itemV2, int i = 0)
    {
        if (i == (int)Item_Type.장비) //장비
        {
            equipButton.gameObject.SetActive(!DataManager.Instance.IsItemUsed(itemV2) && itemV2.ItemData1.SlotCategory != SlotCategory.item); // Activate equip button only for equippable items
            unequipButton.gameObject.SetActive(DataManager.Instance.IsItemUsed(itemV2)); // A condition for unequip can be added here            

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => EquipItem(itemV2));

            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(() => UnequipItem(itemV2));
        }
        else if (i == (int)Item_Type.소비)    //소비
        {
            useButton.gameObject.SetActive(itemV2.ItemData1.SlotCategory == SlotCategory.potion); // Activate use button only for potions, for example

            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(() => UseItem(itemV2));
        }        
        else    //기타
        {

        }
    }

    void RefreshEvents()
    {
        if (currentSlot != null)
        {
            currentSlot.UpdateSlotBackImage();
            EquipmentManager.Instance.Refresh();
        }
    }

    void EquipItem(ItemV2 itemV2)
    {
        Debug.Log("장비 장착");
        // Logic to equip item
        Debug.Log($"Equipping {itemV2.ItemData1.UniqueID}");

        if (EquipmentManager.Instance != null)
        {
            EquipmentManager.Instance.EquipItem(itemV2);
            Instance.gameObject.SetActive(false);
            RefreshEvents();
        }
    }

    void UnequipItem(ItemV2 itemV2)
    {
        // Logic to unequip item
        Debug.Log($"Unequipping {itemV2.ItemData1.UniqueID}");

        if (EquipmentManager.Instance != null)
        {
            EquipmentManager.Instance.UnequipItem(itemV2);
            Instance.gameObject.SetActive(false);
            RefreshEvents();
        }
    }

    void UseItem(ItemV2 itemData)
    {
        // Logic to use item
        Debug.Log($"Using {itemData.ItemData1.UniqueID}");
        RefreshEvents();
    }

    void UpdateText(ItemData1 itemData)
    {
        //초기화
        inputText = itemData.Grade;
        colorCode = "#FFFFFF"; // Default color (black)
        tooltipString = "";

        //아이템 종류에 따라 텍스트 표기 다르게 실행
        if (itemData.GetItemType()== (int)Item_Type.장비)     //장비일 경우
        {
            EquipmentUpdateText(itemData);
        }
        else if(itemData.GetItemType() == (int)Item_Type.소비)    //소비일 경우
        {
            ConsumableUpdateText(itemData);
        }
        else    //기타, 재료 템일 경우
        {
            EtcUpdateText(itemData);
        }

        //텍스트 적용
        tooltipText.text = tooltipString;
    }

    void EquipmentUpdateText(ItemData1 itemData)
    {

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
        else
        {
            colorCode = "#FFFF00";
        }

        // Item Type and Set Name
        tooltipString += "구분 " + itemData.SubType1;
        if (!string.IsNullOrEmpty(itemData.SetName))
            tooltipString += MakeColored(colorCode, " [ " + itemData.SetName + " - Set Item ]")
                + MakeColored("#7030A0", " 세트효과") + "\n";
        else
            tooltipString += "\n";

        // Tier and Grade
        tooltipString += "등급 " + MakeColored("#FFFF00", itemData.Tier.ToString())
            + " Tier / " + MakeColored(colorCode, itemData.Grade) + " Grade\n";

        // Effects
        string setEffectString = "";
        if (!string.IsNullOrEmpty(itemData.Effect1))
            setEffectString += itemData.Effect1 + " / ";
        if (!string.IsNullOrEmpty(itemData.Effect2))
            setEffectString += itemData.Effect2;
        tooltipString += MakeColored(colorCode, setEffectString) + "\n";

        // Durability
        tooltipString += "내구도 " + itemData.Durability + "\n";

        // Damage
        if (itemData.MinDamage > 0 || itemData.MaxDamage > 0)
            tooltipString += "물리 피해 " + itemData.MinDamage + " ~ " + itemData.MaxDamage + "\n";

        // Attack Speed
        if (itemData.AttackMagicSpeed > 0)
            tooltipString += "공격속도 " + itemData.AttackMagicSpeed + "\n";

        // Accuracy
        if (itemData.Accuracy > 0)
            tooltipString += "명중 " + itemData.Accuracy + "\n";

        // Attack Defense
        if (itemData.AttackDefense > 0)
            tooltipString += "물리 방어 " + itemData.AttackDefense + "\n";

        // Weight
        if (itemData.ItemWeight > 0)
            tooltipString += "무게 " + itemData.ItemWeight;

        
    }
    void ConsumableUpdateText(ItemData1 itemData)
    {
        if (inputText.Contains("1"))
        {
            colorCode = "#FFC000";
        }
        else if (inputText.Contains("2"))
        {
            colorCode = "#FF5100";
        }
        else if (inputText.Contains("3"))
        {
            colorCode = "#FFC000";
        }
        else
        {
            colorCode = "#FFFF00";
        }

        tooltipString += "이름 " + itemData.SetName;

        // Item Type and Set Name
        tooltipString += "구분 " + itemData.SubType1 + "\n";

        // Tier and Grade
        tooltipString += "등급 " + MakeColored("#FFFF00", itemData.Tier.ToString())
            + " Tier / " + MakeColored(colorCode, itemData.Grade) + " Grade\n";

        tooltipString += "효과" + "\n";

        tooltipString += "효과 수치" + "\n";

        tooltipString += "아이템 무게" + "\n";

    }

    void EtcUpdateText(ItemData1 itemData)
    {
        if (inputText.Contains("1"))
        {
            colorCode = "#FFC000";
        }
        else if (inputText.Contains("2"))
        {
            colorCode = "#FF5100";
        }
        else if (inputText.Contains("3"))
        {
            colorCode = "#FFC000";
        }
        else
        {
            colorCode = "#FFFF00";
        }

        tooltipString += "이름 " + itemData.SetName +"\n";

        // Item Type and Set Name
        tooltipString += "구분 " + itemData.SubType1+"\n";
       
        // Tier and Grade
        tooltipString += "등급 " + MakeColored("#FFFF00", itemData.Tier.ToString())
            + " Tier / " + MakeColored(colorCode, itemData.Grade) + " Grade\n";

        tooltipString += "효과" + "\n";

        tooltipString += "효과 수치" + "\n";

        tooltipString += "아이템 무게" + "\n";
        
    }

    string MakeColored(string colorCode, string input)
    {
        return $"<color={colorCode}>{input}</color>";
    }

    public void tooltipPosition(Vector2 position)
    {
        //가로 조정
        if (position.x < Screen.width / 2) //클릭 위치가 왼쪽일 경우 마우스 오른쪽에 출력
        {
            gameObject.transform.position = new Vector2(position.x + (width / 2 + 30), position.y);
        }
        else    //반대의 경우
        {
            gameObject.transform.position = new Vector2(position.x - (width / 2 + 30), position.y);
        }

        //세로 조정
        if (position.y < Screen.height / 2) //아래에 있을 경우
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, position.y + (height / 2 + 15));
        }
        else    //위에 있을 경우
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, position.y - (height / 2 + 15));
        }


    }

    #region 슬롯에 넣을 함수
    public void ToolTipOnPointerEnter(PointerEventData eventData, bool TooltipSlot = true)      //클릭 전 이벤트 - 예를 들어 버튼 위에 아이콘을 클릭할 때 여기서 처리해주면 됨 
    {
        if (TooltipSlot)    //툴팁 슬롯일때만 실행
        {

        }
    }

    public void ToolTipOnPointerClick(PointerEventData eventData, ItemV2 v2, bool TooltipSlot = true,bool OnlySee=false)  //클릭 이벤트 - 툴팁 처리
    {
        if (TooltipSlot)    //툴팁 슬롯일때만 실행
        {
            if (ClickObject && ClickObject != eventData.pointerPress)   //클릭한 오브젝트와 이름이 다르면
            {
                tooltipPosition(eventData.position);
                DisplayTooltip(v2, OnlySee);
            }
            else
            {
                if (gameObject.activeSelf)    //켜져있을 때
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    tooltipPosition(eventData.position);
                    gameObject.SetActive(true);
                    DisplayTooltip(v2,OnlySee);
                }
            }

            ClickObject = eventData.pointerPress;
        }
    }

    public void ToolTipOnPointerClick(PointerEventData eventData, SlotScript slot, bool TooltipSlot = true, bool OnlySee = false)  //클릭 이벤트 - 툴팁 처리
    {
        if (TooltipSlot)    //툴팁 슬롯일때만 실행
        {
            if (ClickObject && ClickObject != eventData.pointerPress)   //클릭한 오브젝트와 이름이 다르면
            {
                tooltipPosition(eventData.position);
                DisplayTooltip(slot, OnlySee);
            }
            else
            {
                if (gameObject.activeSelf)    //켜져있을 때
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    tooltipPosition(eventData.position);
                    gameObject.SetActive(true);
                    DisplayTooltip(slot, OnlySee);
                }
            }

            ClickObject = eventData.pointerPress;
        }
    }

    public void ToolTipOnPointerExit(PointerEventData eventData, bool TooltipSlot = true)   //클릭 후 이벤트 - 후처리
    {
        if (TooltipSlot)    //툴팁 슬롯일때만 실행
        {

        }
    }

    #endregion
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("툴팁 클릭됨");
        if(ClickObject!=eventData.pointerPress)
        {
            gameObject.SetActive(false);
        }
    }
}
