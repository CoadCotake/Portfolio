using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class ItemDataReference
{
    // 아이템 원본 데이터
    public string uniqueName;
    public string upper;
    public string lower;
    public string name;
    public string text;
    public int price;
    public int level;
    public string cate;
    //public Vector2 shape;
    public Sprite sprite;
    public string[] materials;
}
public static class ResourceManager
{
    // 아이템 원본 데이터 관리 목적 static 스크립트
    public static string LANGUAGE = "KR";
    public static Dictionary<string, ItemDataReference> itemDatas = null;


    public static Dictionary<string, ItemDataReference> GetItemDatas()
    {
        _LoadItemDataReferences();
        // 모든 아이템 Reference
        return itemDatas;
    }
    public static ItemDataReference GetItemData(string uniqueName)
    {
        _LoadItemDataReferences();
        try
        {
            // 특정 아이템 Reference
            return itemDatas[uniqueName];
        }
        catch (System.Exception)
        {
            Debug.LogError(uniqueName + "아이템 데이터 없음!");
            return null;
        }
    }
    public static bool GetItemExists(string uniqueName)
    {
        _LoadItemDataReferences();
        try
        {
            // 특정 아이템 Reference
            return itemDatas.ContainsKey(uniqueName);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
    static Dictionary<string, Dictionary<string, string>> GetSheetData(string sheetName)
    {
        string json = Resources.Load<TextAsset>("ResourceManager/용사마을키우기").text;
        return JsonConvert.DeserializeObject<Dictionary<string,
                    Dictionary<string, Dictionary<string, string>>
                >>(json)["아이템"];
    }
    static void _LoadItemDataReferences()
    {
        if (itemDatas == null)
        {
            itemDatas = new Dictionary<string, ItemDataReference>();
            // 아이템 Reference 데이터 로드
            Dictionary<string, Dictionary<string, string>> parsedData = GetSheetData("아이템");
            foreach (var item in parsedData)
            {
                try
                {
                    ItemDataReference reference = new ItemDataReference();
                    try
                    {
                        // ItemDataReference 클래스로 변환
                        reference.uniqueName = item.Key;

                        // 위아래 찾기, 넣기
                        string removedName = item.Key.Remove(item.Key.Length-1, 1);
                        int lastNumber = int.Parse(item.Key.Remove(0, item.Key.Length-1));
                        string upperName = removedName + (lastNumber + 1);
                        reference.upper = (parsedData.ContainsKey(upperName)) ? upperName : null;
                        string lowerName = removedName + (lastNumber - 1);
                        reference.lower = (parsedData.ContainsKey(lowerName)) ? lowerName : null;

                        reference.name = item.Value["name_" + LANGUAGE];
                        reference.level = int.Parse(item.Value["level"]);
                        reference.price = int.Parse(item.Value["price"]);
                        reference.cate = item.Value["cate"];
                        reference.text = item.Value["text_" + LANGUAGE];

                        string materialsStr = item.Value["mat"];
                        if (materialsStr != null)
                        {
                            string[] materials = materialsStr.Split(" ");
                            reference.materials = materials;
                        }
                        //string[] shape = item.Value["shape"].Split("x");
                        //reference.shape = new Vector2(float.Parse(shape[0]), float.Parse(shape[1]));
                        try
                        {
                            reference.sprite = Resources.Load<Sprite>("ItemSprites/" + reference.uniqueName);
                        }
                        catch (System.Exception)
                        {
                            Debug.LogError("스프라이트 찾기 실패");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e);
                        Debug.LogError("파싱 실패");
                    }
                    itemDatas.Add(item.Key, reference);
                }
                catch (System.Exception)
                {
                    Debug.LogError("Add 실패");
                }
            }
        }
    }
}   