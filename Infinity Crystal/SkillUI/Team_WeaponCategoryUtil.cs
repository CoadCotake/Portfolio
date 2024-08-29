using System;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponCategoryUtil
{
    private static readonly Dictionary<string, List<string>> specialCategories = new Dictionary<string, List<string>>()
    {
        { "근거리", new List<string> { "한손 검", "단검", "한손 둔기", "한손 창" } },
        { "원거리", new List<string> { "활", "석궁" } },
        { "전체", new List<string> { "한손 검", "단검", "한손 둔기", "한손 창", "활", "석궁", "양손 검", "양손 둔기", "양손 창", "양손 지팡이", "한손 지팡이" } }
    };

    public static bool ContainsCategory(string originalData, string category)
    {
        // 원본 데이터나 카테고리가 null이거나 빈 문자열인 경우 false를 반환
        if (string.IsNullOrEmpty(originalData) || string.IsNullOrEmpty(category))
        {
            return false;
        }

        if (category.Equals("전체", StringComparison.OrdinalIgnoreCase) || originalData.Contains("전체"))
        {
            return true;
        }

        if (specialCategories.ContainsKey(category))
        {
            foreach (var item in specialCategories[category])
            {
                if (originalData.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            string[] dataItems = originalData.Split(',');
            foreach (var item in dataItems)
            {
                if (item.Trim().Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
