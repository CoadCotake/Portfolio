using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class BTSlot1
{
    [JsonProperty("R1_Item")]
    string R1_Item;
    public List<ItemDisplayData> ItemData
    {
        get
        {
            return BTSlot1_Helper.StringListToProcessedData(R1_Item);
        }
    }

    [JsonProperty("R3_PreItem")]
    string R3_PreItem;
    public List<ItemDisplayData> Pre_ItemData
    {
        get
        {
            return BTSlot1_Helper.StringListToProcessedData(R3_PreItem);
        }
    }

    [JsonProperty("R2_Gold")]
    string R2_Gold;
    public List<ItemDisplayData> GoldData
    {
        get
        {
            return BTSlot1_Helper.StringListToProcessedData(R2_Gold);
        }
    }

    [JsonProperty("R4_PreGold")]
    string R4_PreGold;
    public List<ItemDisplayData> Pre_GoldData
    {
        get
        {
            return BTSlot1_Helper.StringListToProcessedData(R4_PreGold);
        }
    }

    public class BTSlot_SaveData
    {
        // 저장할 값
        public List<bool> RewardCheck = new List<bool>();  //0 - 아이템, 1 - 골드, 2 - 프리미엄 아이템, 3 - 프리미엄 골드
        public bool Requirement;
        public BTSlot_SaveData()
        {
            Requirement = false;
            for (int i = 0; i < 4; i++)
            {
                RewardCheck.Add(false);
            }
        }
    }


    private BTSlot_SaveData _saveData = new BTSlot_SaveData();

    public BTSlot_SaveData SaveData
    {
        get
        {
            return _saveData;
        }
        set
        {
            _saveData = value ?? new BTSlot_SaveData();
        }
    }

}