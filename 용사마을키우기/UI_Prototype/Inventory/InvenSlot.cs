using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InvenSlot : MonoBehaviour
{
    public ItemData itemData;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI textUI;
    private void OnEnable()
    {
        itemImage.sprite = itemData.GetMySprite();
        textUI.text = itemData.count.ToString();
    }

    public void Refresh()
    {
        itemImage.sprite = itemData.GetMySprite();
        textUI.text = itemData.count.ToString();
    }
}
