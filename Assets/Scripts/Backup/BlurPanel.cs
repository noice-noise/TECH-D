using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/Blur Panel")]
public class BlurPanel : Image
{
    public bool animate;
    public float time = 0.5f;
    public float delay = 0f;

    CanvasGroup canvasBlur;

    // protected override void Reset()
    // {
    //     color = Color.black * 0.1f;
    // }

    protected override void Awake()
    {
        canvasBlur = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        if (Application.isPlaying)
        {
            material.SetFloat("_Size", 0);
            canvasBlur.alpha = 0;
            canvasBlur.DOFade(1, time).OnComplete(UpdateBlur);
        }
    }

    private void UpdateBlur()
    {
        material.SetFloat("_Size", 1);
        canvasBlur.alpha = 1;
        // color = Color.black * 1f;
    }
}
