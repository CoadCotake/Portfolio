using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainUI : MonoBehaviour
{
    public Text title;
    public Text subtitle;
    public GameObject parent;
    public GameObject prefab;
    public GameObject Eq_parent;
    public GameObject Eq_prefab;

    GameObject ins;
    ItemDataReference item;
    string code;
    string name;
    int count = 20;

    

    private void OnEnable()
    {
        code = SelectedIcon.explainItem.uniqueName;
        name = ResourceManager.GetItemData(code).name;
        title.text = name;
        subtitle.text = "(레벨 " + ResourceManager.GetItemData(code).level+")";
        ExplainStart();
        ExplainStart_Eq();
    }

    private void Awake()
    {
        
    }

    public void ExplainStart()
    {
        count = 10;
        for (int i = 1; i < count; i++)
        {
            if (ResourceManager.GetItemExists(name + i.ToString()))
            {
                item = ResourceManager.GetItemData(name + i.ToString());

                ins = Instantiate(prefab, parent.transform);
                Debug.Log("설명 아이템 생성");
                if (name+i==code)
                {
                    ins.GetComponent<explain_icon>().icon_image.color = Color.green;
                    ins.GetComponent<explain_icon>().level_image.color = Color.green;
                }
                ins.GetComponent<explain_icon>().sprite_image.sprite = item.sprite;
                ins.GetComponent<explain_icon>().level_text.text = i.ToString();
                count++;
            }
            else
            {
                Debug.Log("아이템이 없어요");
                ins.GetComponent<explain_icon>().arrow_image.SetActive(false);
                break;
            }
                
        }
            
            
        
    }

    public void ExplainStart_Eq()
    {
        foreach (var item in ResourceManager.itemDatas)
        {
            Debug.Log("이놈의 이름은" + item.Value.name);

            if(item.Value.materials!=null)
            {
                Debug.Log("오 이놈은 조합법을 가지고 있어" + item.Value.name);
                foreach (var it in item.Value.materials)
                {
                    Debug.Log("조합법 가진놈 " + it + "그리고" + code);
                    if(it==code)
                    {
                        ins = Instantiate(Eq_prefab, Eq_parent.transform);
                        ins.GetComponent<explain_icon>().sprite_image.sprite = item.Value.sprite;
                        ins.GetComponent<explain_icon>().Code = item.Value.uniqueName;
                        break;
                    }
                }
            }
        }
    }
}
