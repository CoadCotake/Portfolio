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

    [Header("��۰� ��۾� �ؽ�Ʈ �ֱ�")]
    public List<TooggleTex> ToogleList = new List<TooggleTex>();

    private void Start()
    { 
        for (int i = 0; i < ToogleList.Count; i++)      //ó�� ����
        {
            ToogleList[i].SetNotClick();
        }

        ToggleClick(0);     //���� �� ù ������ ���� ��Ű��
    }

    public void Refresh()
    {
        for (int i = 0; i < ToogleList.Count; i++)
        {
            ToogleList[i].SetNotClick();
        }

        //ù ������ ����
        ToogleList[0].toggle.isOn = true;
        ToggleClick(0);     
    }

    public void ToggleClick(int index)      //���� ȿ��
    {
        if (ToogleList[index].toggle.isOn)
        {
            for (int i = 0; i < ToogleList.Count; i++)      //��ü ��Ӱ� �����
            {
                ToogleList[i].SetNotClick();
            }

            ToogleList[index].SetClick();   //Ŭ���� ��۸� �̹��� ����

            Encyclopedia.instance.SettingEncylopediaType(ToogleList[index].type);   //Ŭ���� �Ϳ� ���� ������ ����
            Encyclopedia.instance.RemoveCheckSlot();     //������ ���� üũ ����
        }
    }
}
