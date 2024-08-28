using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chase : MonoBehaviour
{
    public ChaseImage Ci = new ChaseImage();

    public bool isForEntity = false;

    bool ChaseCheck = false;    //추격 체크
    bool WaitCheck = false;     //기다림 체크
    float Sqrlen;               //충돌 되었는가 체크
    Vector2 MoveDir;            //움직임 벡터
    float time = 0;               //시간 체크
    float waittime = 0;
    Vector3 m_scale;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartMove");
        Ci.ChaseSpeed = Random.Range(Ci.ChaseSpeed - 0.01f, Ci.ChaseSpeed + 0.01f);
        Vector3 asd = this.transform.position - Ci.Target.transform.position;
        Sqrlen = asd.sqrMagnitude;
        waittime = Ci.WaitDelay;
        m_scale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (ChaseCheck)
        {
            if (WaitCheck == true)  //기다림 딜레이가 끝날경우
            {
                if (Sqrlen > Ci.TargetRange * Ci.TargetRange)
                {
                    Vector3 targetPos = Ci.Target.transform.position;
                    if (isForEntity)
                        targetPos = Camera.main.WorldToScreenPoint(targetPos);
                    transform.position = Vector3.Lerp(transform.position, targetPos, Ci.ChaseSpeed * Time.deltaTime * 100.0f);
                    Vector3 asd = this.transform.position - targetPos;
                    Sqrlen = asd.sqrMagnitude;
                }
                else
                {
                    ItemDestroy();
                }
            }
        }
        else
        {
            if (Ci.SpwanScaleChange)
            {
                m_scale.x *= Ci.ScaleNum;
                m_scale.y *= Ci.ScaleNum;
                transform.localScale = m_scale;
            }
            transform.Translate(MoveDir.x*(1-(time/Ci.Movetime)), MoveDir.y * (1 - (time / Ci.Movetime)), 0);
        }
    }

    IEnumerator StartMove()
    {

        while (time < Ci.Movetime)
        {
            //Debug.Log("코루틴 실행중");
            time+=0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        ChaseCheck = true;
        yield return new WaitForSeconds(Ci.WaitDelay);
        WaitCheck = true;
    }

    public void SetTarget(GameObject m_Target, Vector2 movedir)
    {
        MoveDir = movedir;
        Ci.Target = m_Target;
    }

    public void ItemDestroy()   //아이템 삭제되는 시점
    {
        //Debug.Log("도착 안녕 ");

        if (Ci.Target.tag == "Gold" || Ci.Target.tag == "Stamina" || Ci.Target.tag == "Cristal" || Ci.Target.tag == "Exp")
        {
            Ci.Target.GetComponentInParent<BounceAnim>().BounceStart();
        }

        Destroy(this.gameObject);
    }
}