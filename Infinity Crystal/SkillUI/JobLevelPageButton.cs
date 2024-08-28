using DTT.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobLevelPageButton : MonoBehaviour
{
    [Header("직접 넣어야하는 것")]
    public TextMeshProUGUI text;
    [Header("보기용")]
    [SerializeField] int index; 

    public void SetButton(string t,int i)
    {
        text.text = t;
        index = i;
    }

    public void ClickButton()
    {

        SkillPointManager.ins.JobResetRefresh(index);
        SkillPointManager.ins.JobPageContent.GetRectTransform().position = new Vector3(SkillPointManager.ins.JobPageContent.GetRectTransform().position.x, 0, SkillPointManager.ins.JobPageContent.GetRectTransform().position.z);
    }
}
