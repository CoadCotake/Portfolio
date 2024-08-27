using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtons : MonoBehaviour
{
    [System.Serializable]
    public class TooggleTex
    {
        public EncyclopediaType type;
        public Toggle toggle;
        public Image image;
        public TextMeshProUGUI text;

        [SerializeField]
        Color ClickColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        [SerializeField]
        Color NotClickColor = new Color(140f / 255f, 140f / 255f, 140f / 255f);

        public void SetClick()
        {
            image.color = ClickColor;
            text.color = ClickColor;
        }

        public void SetNotClick()
        {
            image.color = NotClickColor;
            text.color = NotClickColor;
        }
    }

    [Header("토글과 토글안 텍스트 넣기")]
    public List<TooggleTex> ToogleList = new List<TooggleTex>();

    private void Start()
    { 
        for (int i = 0; i < ToogleList.Count; i++)      //처음 세팅
        {
            ToogleList[i].SetNotClick();
        }

        ToggleClick(0);     //시작 시 첫 페이지 세팅 시키기
    }

    public void Refresh()
    {
        for (int i = 0; i < ToogleList.Count; i++)
        {
            ToogleList[i].SetNotClick();
        }

        //첫 페이지 세팅
        ToogleList[0].toggle.isOn = true;
        ToggleClick(0);     
    }

    public void ToggleClick(int index)      //누름 효과
    {
        if (ToogleList[index].toggle.isOn)
        {
            for (int i = 0; i < ToogleList.Count; i++)      //전체 어둡게 만들고
            {
                ToogleList[i].SetNotClick();
            }

            ToogleList[index].SetClick();   //클릭한 토글만 이미지 변경

            Encyclopedia.instance.SettingEncylopediaType(ToogleList[index].type);   //클릭한 것에 따른 페이지 변경
            Encyclopedia.instance.RemoveCheckSlot();     //선택한 슬롯 체크 해제
        }
    }
}
