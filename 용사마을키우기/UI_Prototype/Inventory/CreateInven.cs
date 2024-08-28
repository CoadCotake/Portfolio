using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum InvenType
{
    Inventory,
    Chest,
};

public class CreateInven : MonoBehaviour
{

    [Header("인벤 종류 설정하기")]
    public InvenType mytype;

    [Header("Content 위치 직접지정")]
    public Transform invenContent;

    private void OnEnable()
    {
        Inventory.main.CreateInven(invenContent,mytype);
        StartCoroutine("Refresh");
    }

    IEnumerator Refresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Inventory.main.CreateInven(invenContent, mytype);
        }
    }
}
