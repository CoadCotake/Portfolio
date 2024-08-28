using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToWorldPosition : MonoBehaviour
{
    public GameObject targetObject;
    public Camera camera;
    public Vector2 worldPivot = new Vector2(0, 0);

    // Update is called once per frame
    void FixedUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        transform.position
            = camera.WorldToScreenPoint(targetObject.transform.position + (Vector3)worldPivot);
    }
}
