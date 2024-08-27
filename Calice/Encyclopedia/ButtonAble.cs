using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAble : MonoBehaviour
{
    public void Able()
    {
        this.gameObject.SetActive(true);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
