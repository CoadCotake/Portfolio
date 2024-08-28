using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapSettings : MonoBehaviour
{
    [Header("적 탐색")]
    public List<GameObject> EnemyList;

    public static GameMapSettings main;
    public Transform entityTransform;
    public GameObject myCameraController;
    public Transform myCameraTarget;
    public Vector2 LT;
    public Vector2 RB;

    public void SetLookingMap()
    {
        main.myCameraController.SetActive(false);
        myCameraController.SetActive(true);
        main = this;

        //CameraSlider.main.cameraTarget = myCameraTarget;
        //CameraSlider.main.OnSliderValueChanged();
        //CameraSlider.main.Refresh();
    }
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        StartCoroutine(CheckEnemy());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.z) + LT, new Vector2(transform.position.x, transform.position.z) + RB);
    }
    public Vector2 GetRandomGroundPosition()
    {
        return new Vector2(transform.position.x, transform.position.z) + new Vector2(Random.Range(LT.x, RB.x), Random.Range(LT.y, RB.y));
    }

    public void SearchEnemy()   //적 찾기
    {

        EnemyList.Clear();  // 리스트 초기화

        for (int i = 0; i < entityTransform.childCount; i++)   //적 탐색
        {
            if (entityTransform.GetChild(i).gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyList.Add(entityTransform.GetChild(i).gameObject);
            }
        }

    }
    IEnumerator CheckEnemy()
    {
        while(true)
        {
            SearchEnemy();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
