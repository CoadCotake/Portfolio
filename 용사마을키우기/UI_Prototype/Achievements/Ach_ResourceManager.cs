using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class AchDataReference
{
    // 업적 원본 데이터
    public string uniqueName;
    public string Content;
    public string Reward;
    public string Sprtie_Name;
    public int Reward_Num;
    public int Goal;
    public Sprite Sprtie;
}

public static class Ach_ResourceManager
{
    // 아이템 원본 데이터 관리 목적 static 스크립트
    public static string LANGUAGE = "KR";

    [SerializeField]
    public static Dictionary<string, AchDataReference> AchDatas = null;


    public static Dictionary<string, AchDataReference> GetItemDatas()
    {
        _LoadItemDataReferences();
        // 모든 아이템 Reference
        return AchDatas;
    }
    public static AchDataReference GetItemData(string uniqueName)
    {
        _LoadItemDataReferences();
        try
        {
            // 특정 아이템 Reference
            return AchDatas[uniqueName];
        }
        catch (System.Exception)
        {
            Debug.LogError("아이템 데이터 없음!");
            return null;
        }
    }
    public static void LoadData()
    {
        _LoadItemDataReferences();
    }
    static Dictionary<string, Dictionary<string, string>> GetSheetData(string sheetName)
    {
        string json = Resources.Load<TextAsset>("ResourceManager/용사마을키우기_업적포함").text;
        return JsonConvert.DeserializeObject<Dictionary<string,
                    Dictionary<string, Dictionary<string, string>>
                >>(json)["업적"];
    }
    static void _LoadItemDataReferences()
    {
        if (AchDatas == null)
        {
            AchDatas = new Dictionary<string, AchDataReference>();
            // 아이템 Reference 데이터 로드
            Dictionary<string, Dictionary<string, string>> parsedData = GetSheetData("업적");
            foreach (var item in parsedData)
            {
                try
                {
                    AchDataReference reference = new AchDataReference();
                    try
                    {
                        // AchDataReference 클래스로 변환
                        reference.uniqueName = item.Key;
                        reference.Content = item.Value["Content"];
                        reference.Goal = int.Parse(item.Value["Goal"]);
                        reference.Sprtie_Name = item.Value["SprtieName"];
                        reference.Reward = item.Value["Reward"];
                        reference.Reward_Num = int.Parse(item.Value["Reward_Num"]);
                        

                        try
                        {
                            Debug.Log("스프라이트 이름"+reference.Sprtie_Name);
                            reference.Sprtie = Resources.Load<Sprite>("Sprites/" + reference.Sprtie_Name);
                        }
                        catch (System.Exception)
                        {
                            Debug.LogError("스프라이트 찾기 실패");
                        }
                    }
                    catch (System.Exception)
                    {
                        Debug.LogError("파싱 실패");
                    }
                    AchDatas.Add(item.Key, reference);
                }
                catch (System.Exception)
                {
                    Debug.LogError("Add 실패");
                }
            }
        }
    }
}
