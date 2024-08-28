using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    [Header("적용시킬 효과")]
    [SerializeField] Material flashMaterial;
    [Header("원본 메테리얼")]
    [SerializeField] Material originalMaterial;

    [Tooltip("효과 지속시간")]
    [SerializeField] float duration;

    [Tooltip("스프라이트 모음")]
    [SerializeField] public SpriteRenderer[] AllSprite;

    Coroutine flashRoutine;

    void Start()
    {
        AllSprite = gameObject.GetComponentsInChildren<SpriteRenderer>(true);
    }

    public void FlashApply()
    {
        foreach (var item in AllSprite)
        {
            item.material = flashMaterial;
        }
    }

    public void FlashUnapply()
    {
        foreach (var item in AllSprite)
        {
            item.material = originalMaterial;
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        FlashApply();

        yield return new WaitForSeconds(duration);

        FlashUnapply();

        flashRoutine = null;
    }
}
