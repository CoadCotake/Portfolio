using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.Animation;
using UnityEngine;
//using UnityEngine.UIElements;
//using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class Bubble
{
    public GameObject Target;
    public string Name_text;
    public string Text;

    public string Offset_name;
    public Vector3 Offset;



    public Bubble(GameObject target = null,string name_text =null, string text=null,string offset_name=null)
    {
        Target = target;
        Name_text = name_text;
        Text = text;
        Offset_name = offset_name;
    }

    public void Setbubble(GameObject target,string name_text,string text,string off_name=null)
    {
        Target = target;
        Name_text = name_text;
        Text = text;
        Offset_name = off_name;
        SetOffset(off_name);
    }

    public void Setbubble(Bubble bu)
    {
        Target = bu.Target;
        Name_text = bu.Name_text;
        Text = bu.Text;
        SetOffset(bu.Offset_name);
    }

    public void SetOffset(string name)
    {
        if(name=="몬스터")
        {
            Offset = new Vector3(-20,20);
        }
        else if(name=="사람")
        {
            Debug.Log("사람");
            Offset = new Vector3(-20,80);
        }
        else
        {
            Offset = new Vector3(-20,10);
        }
    }
}


public class BubbleManager : MonoBehaviour
{
    public static BubbleManager m_Bm;

    public GameObject Prefab_Bubble;
    public GameObject _ins;

    public Camera camera;
    public GameObject canvus;

    [Header("테스트")]
    public GameObject TestObject;
    public GameObject TestObject2;
    public GameObject TestObject3;
    public GameObject TestObject4;

    private void Awake()
    {
        m_Bm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateBubble(TestObject, "몬스터1", "안녕 너의 이름은 뭐니 나는 김영인이ㅏㄹ고 하는드ㅁㅇㄴㄹㄴㅁㄻㄴㅇ ㄹㅇㄴㅁㄹㄴㅁㄹㅇㄴㅁㄹㄴㅇㅁㄹㄴㅁㄹㄴㅁ ㄻㄴㄹ ㄴㅁㄻㄴㄹㄴㄹㅇㄴㅁㄹ ㅇㄴㅁㄹ ㄴㅁ ㅁㅇㄻㄴㄹ ㅁㄴㅇㄹ ㅁㄴㅇㄹ ㄴㅁ ㄻㄴㅇㄹ ㅇㅁㄴㄹ ?","몬스터");
        //CreateBubble(TestObject2, "대장장이", "안녕 너의 이름은 뭐니 나는 김영인이ㅏㄹ고 하는드ㅔㄴㅁㅇㅁㄴㅇㄴㅁㅇㄴㅁㅇㄴㅁㅇㅁㄴㅇㅁㄴㅇㅁㄴㅇㅁㄴㄴㅇ2?");
        //CreateBubble(TestObject3, "용사", "안녕 너의 이름은 뭐니 나는 김영인이ㅏㄹ고 하는드ㅔ3?","사람");
        //CreateBubble(TestObject4, "몬스터2", "안녕 너의1?","몬스터");
    }


    public GameObject CreateBubble(GameObject Target, string name = null, string text = null,string Offset_name=null)
    {
        _ins = Instantiate(Prefab_Bubble, canvus.transform);
        _ins.GetComponent<Bubbleprefab>().bubble.Setbubble(Target, name, text,Offset_name);
        _ins.GetComponent<Bubbleprefab>().SetElement(camera);

        return _ins;
    }
}
