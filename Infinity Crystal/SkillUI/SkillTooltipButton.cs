using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltipButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    SkillPointSlot skillPointSlot;
    private void Start()
    {
        skillPointSlot = this.transform.parent.GetComponent<SkillPointSlot>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("스킬 UI 클릭업");
        SkillPointManager.ins.buttonclick = true;
        skillPointSlot.OnToolTip();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("스킬 UI 클릭다운");
        SkillPointManager.ins.buttonclick = false;
    }
}
