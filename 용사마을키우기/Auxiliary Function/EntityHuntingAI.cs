using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EntityHuntingAI : MonoBehaviour
{
    public Entity targetEntity;
    Entity entity;
    MoveToTarget moveToTarget;
    public bool isAttackCool = false;
    Animator animator;

    [Header("탐색범위 지정")]
    [SerializeField] float SearchRange;

    [Header("적 탐색")]
    [SerializeField] List<GameObject> EnemyList;

    [Header("범위포함 적")]
    [SerializeField] List<GameObject> EnemyRangeList;

    float MinDis;   //최소거리
    GameObject MinDis_Enemy;
    GameObject parent;
    bool EnemyCheck;

    //GameMapPage entity.myMapPage;

    private void Start()
    {
        entity = GetComponent<Entity>();
        moveToTarget = GetComponent<MoveToTarget>();
        animator = GetComponentInChildren<Animator>();
        parent = transform.parent.gameObject;
        EnemyCheck = true;
        StartCoroutine(MyAI());
    }
    IEnumerator MyAI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (targetEntity != null)
            {
                //타겟이 있을 때
                moveToTarget.targetPosition = targetEntity.transform.position;
                if (isAttackCool == false)
                {
                    if (Vector3.Distance(targetEntity.transform.position, transform.position) < 0.5f)
                    {
                        animator.Play(Chance.chance(50) ? "Attack1" : (Chance.chance(50) ? "Attack2" : "Attack3"), 0);
                        isAttackCool = true;

                        StartCoroutine(DelayedAttack(0.2f));

                        StartCoroutine(WaitAttackCool());
                    }
                }
            }
            else if (EnemyCheck == true) //적이 존재하는 경우
            {
                // 움직이는 중이 아니면 탐색
                if (moveToTarget.IsMoving() == false)
                    SearchEnemy();
            }
            else if (entity.myMapPage.EnemyList.Count >= 0)
            {
                EnemyCheck = true;
            }
            else
            {
                EnemyCheck = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, SearchRange);
    }

    public void SearchEnemy()   //적 찾기
    {
        /*
        EnemyList.Clear();  // 리스트 초기화

        for (int i = 0; i < parent.transform.childCount; i++)   //적 탐색
        {
            if (parent.transform.GetChild(i).gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyList.Add(parent.transform.GetChild(i).gameObject);
            }
        }
        */

        if (entity.myMapPage.EnemyList.Count != 0)  // 적이 있을 경우
        {
            Range_Dis_Search();
        }
        else        //적이 더이상 없다고 판단 탐색 종료
        {
            EnemyCheck = false;
            //Debug.Log("적이 더이상 존재하지 않음");
        }
    }

    public void Range_Dis_Search()      //범위안 랜덤 탐색
    {
        EnemyRangeList.Clear();
        entity.myMapPage.SearchEnemy();

        for (int i = 0; i < entity.myMapPage.EnemyList.Count; i++)
        {
            if (Vector2.Distance(transform.position, entity.myMapPage.EnemyList[i].transform.position) < SearchRange)
            {
                EnemyRangeList.Add(entity.myMapPage.EnemyList[i]);
            }
        }

        if (EnemyRangeList.Count == 0)  //적이 있으나 근처에 탐색이 안될때
        {
            Min_Dis_Search();
        }
        else
        {
            targetEntity = EnemyRangeList[Random.Range(0, EnemyRangeList.Count)].GetComponent<Entity>();
        }
    }

    public void Min_Dis_Search()      //최소거리 적 탐색
    {
        if (entity.myMapPage.EnemyList.Count > 0)
        {
            MinDis = Vector2.Distance(transform.position, entity.myMapPage.EnemyList[0].transform.position);
            MinDis_Enemy = entity.myMapPage.EnemyList[0];

            for (int i = 1; i < entity.myMapPage.EnemyList.Count; i++)
            {
                if (Vector2.Distance(transform.position, entity.myMapPage.EnemyList[i].transform.position) < MinDis)
                {
                    MinDis = Vector2.Distance(transform.position, entity.myMapPage.EnemyList[i].transform.position);
                    MinDis_Enemy = entity.myMapPage.EnemyList[i];
                }
            }
            targetEntity = MinDis_Enemy.GetComponent<Entity>();
        }
    }

    IEnumerator DelayedAttack(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("스테미나 감소???");
        if (targetEntity != null)
        {
            targetEntity.GetComponent<Entity>().DoDmg(gameObject, 10);
            this.gameObject.GetComponent<Entity>().GetStamina().DoUse(100);
            Debug.Log("스테미나 감소");
        }
        yield return null;
    }
    IEnumerator WaitAttackCool()
    {
        yield return new WaitForSeconds(0.5f);
        isAttackCool = false;
        yield return null;
    }


}
