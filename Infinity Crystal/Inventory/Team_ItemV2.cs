using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemV2
{
    public string itemUniqueID;
    public int count = 0;
    public ItemMetaData metaData;

    public string Hash
    {
        get
        {
            return this.itemUniqueID + JsonConvert.SerializeObject(this.metaData);
        }
    }

    public ItemData1 ItemData1
    {
        get
        {
            return JsonDBManager.Instance.GetGameItem(itemUniqueID);
        }
    }

    [JsonConstructor]
    public ItemV2(string itemUniqueID, ItemMetaData itemMetaData)
    {
        this.itemUniqueID = itemUniqueID;
        this.metaData = itemMetaData;
        if (itemMetaData == null)
        {
            this.metaData = new ItemMetaData();
        }
    }

    public ItemV2(string itemUniqueID)
    {
        this.itemUniqueID = itemUniqueID;
        this.metaData = new ItemMetaData();
    }


    public Stat Stat
    {
        get
        {
            Stat stat = new Stat();

            stat.SetStat(Stat_.Attack_Defence, ItemData1.AttackDefense);
            stat.SetStat(Stat_.Spell_Defence, ItemData1.MagicDefense);
            stat.SetStat(Stat_.Attack_Speed, ItemData1.AttackMagicSpeed);

            stat.SetStat(Stat_.Attack_Strength, ItemData1.MinDamage); // 임시

            stat.SetStat(Stat_.Attacks_Hit_the_target, ItemData1.Accuracy); // 임시

            return stat;
        }
    }

    /// <summary>
    /// 강화 등의 특수 데이터 (해시에 반영됨)
    /// </summary>
    public class ItemMetaData
    {
        public string test1 = "";
    }

    public static class Utils
    {
        public static string GetColoredGrade(ItemData1 itemData1)
        {
            string inputText = itemData1.Grade;
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

            return $"<color={colorCode}>{inputText}</color>";
        }
    }
}
