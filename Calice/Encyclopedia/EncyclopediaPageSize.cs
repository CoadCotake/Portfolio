using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EncyclopediaPageSize : MonoBehaviour
{
    public RectTransform content;

    int Height;

    public void SetContentHeight()  //드래그 영역 넓이 설정
    {
        Height = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)        //활성화 되어 있는 것만 체크
            {
                Height += (int)transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            }
        }


        content.sizeDelta = new Vector2(content.sizeDelta.x, Height + transform.GetChild(0).GetComponent<EncyclopediaPage>().SumSlotHeight);
        content.anchoredPosition = new Vector3(0, 0, 0);
    }

}
