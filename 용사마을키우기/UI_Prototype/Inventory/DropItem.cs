using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    [SerializeField] ItemData dropItem;

    Ray ray;
    RaycastHit hit;
    Vector3 MouseDownPos;

    public float ItemLifeTime;
    public float ItemSpeed;
    public float Item_DropChangeTime;

    Vector3 con_positon;
    SpriteRenderer Spr;
    float time;
    private void Start()
    {
        Invoke("ItemDie", ItemLifeTime);
        StartCoroutine("ChangeNum");
        con_positon = new Vector3(0, ItemSpeed, 0);
        Spr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        transform.Translate(con_positon);
    }

    private void Update()
    {

        //if (Input.touchCount > 0) 터치
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 클릭");
            MouseDownPos = Input.mousePosition;
            MouseEvent();
        }
    }

    public void MouseEvent()
    {
        // 마우스 클릭 위치를 카메라 스크린 월드포인트로 변경합니다.
        Vector2 pos = Camera.main.ScreenToWorldPoint(MouseDownPos);

        // Raycast함수를 통해 부딪치는 collider를 hit에 리턴받습니다.
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null)
        {
            // 어떤 오브젝트인지 로그를 찍습니다.
            Debug.Log(hit.collider.name);

            if (hit.transform.gameObject.tag == "DropItem")
            {
                Inventory.main.TakeItem(dropItem);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void ChangeItem(string Un,int C)
    {
        dropItem.uniqueName = Un;
        dropItem.count = C;
        this.GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>("ItemSprites/" + Un);
    }

    IEnumerator ChangeNum()
    {
        while (true)
        {
            yield return new WaitForSeconds(Item_DropChangeTime);
            ItemSpeed *= -1;
            con_positon = new Vector3(0, ItemSpeed, 0);
        } 
    }

    public void ItemDie()
    {
        Debug.Log("바로 실행됨");
        Inventory.main.TakeItem(dropItem);
        ItemChaseManger.m_ICM.OneSpawn(dropItem.count, Resources.Load<Sprite>("ItemSprites/" + dropItem.uniqueName), this.transform, ItemChaseManger.m_ICM.SetTarget("Chest"));
        Destroy(this.gameObject);
    }

    public void FadeOut()
    {
        if (time < ItemLifeTime)
        {
            Spr.color = new Color(1, 1, 1, 1f - time / ItemLifeTime);
        }
        time += Time.deltaTime;
    }
}
