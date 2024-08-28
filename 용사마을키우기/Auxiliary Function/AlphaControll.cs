using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class AlphaControll : MonoBehaviour
{
    MaterialFlash Maf;
    Color fadecolor;
    float start;
    float end;

    [Header("동작시간설정")]
    public float FadeTime = 2f; // Fade효과 재생시간
    
    [Header("시간체크")]
    [SerializeField] float time = 0f;

    [Header("동작체크")]
    [SerializeField]bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        Maf = this.gameObject.GetComponent<MaterialFlash>();

        foreach (var item in Maf.AllSprite)
        {
            fadecolor = item.color;
            fadecolor.a = 0;
            item.color = fadecolor;
        }

        InStartFadeAnim();
    }

    public void OutStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        start = 1f;
        end = 0f;

        StartCoroutine(FadeOutRoutine());    //코루틴 실행
    }

    public void InStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        start = 0f;
        end = 1f;

        StartCoroutine(FadeInRoutine());

    }

    private IEnumerator FadeInRoutine()
    {
        if (Maf.AllSprite.Length == 0)
        {
        }
        else
        {
            isPlaying = true;
            fadecolor = Maf.AllSprite[0].color;

            time = 0f;

            while (fadecolor.a < 1f)
            {
                //Debug.Log("반복중");
                //color.a = Mathf.Lerp(start, end, time);
                foreach (var item in Maf.AllSprite)
                {
                    fadecolor = item.color;
                    //Debug.Log("알파 색" + Mathf.Lerp(start, end, time));
                    fadecolor.a = Mathf.Lerp(start, end, time);
                    item.color = fadecolor;
                }
                time += Time.deltaTime / FadeTime;

                yield return null;
            }

            isPlaying = false;
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        if (Maf.AllSprite.Length == 0)
        {
        }
        else
        {
            //Debug.Log("페이드 아웃 시작");
            isPlaying = true;
            fadecolor = Maf.AllSprite[0].color;

            time = 0f;

            while (fadecolor.a > 0f)
            {
                //Debug.Log("반복중");
                //color.a = Mathf.Lerp(start, end, time);
                foreach (var item in Maf.AllSprite)
                {
                    fadecolor = item.color;
                    //Debug.Log("알파 색" + Mathf.Lerp(start, end, time));
                    fadecolor.a = Mathf.Lerp(start, end, time);
                    item.color = fadecolor;
                }
                time += Time.deltaTime / FadeTime;

                yield return null;
            }

            isPlaying = false;
        }
    }
}
