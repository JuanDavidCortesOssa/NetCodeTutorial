using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("DoTweenAnimations")] [SerializeField]
    private Ease hoverEase;

    [SerializeField] private AnimationCurve buttonClickAnimationCurve;
    [SerializeField] private float hoverSize;

    [FormerlySerializedAs("hoverTime")] [SerializeField]
    private float hoverDuration;

    [SerializeField] private float clickSize;

    [FormerlySerializedAs("clickTime")] [SerializeField]
    private float onClickDuration;

    //General
    private RectTransform _rectTransform;
    private Vector3 _initialScale;

    //Twiners
    private Tween _hoverTween;
    private Tween _clickTween;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialScale = _rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverTween = _rectTransform.DOScale(_initialScale * hoverSize, hoverDuration).SetEase(hoverEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hoverTween = _rectTransform.DOScale(_initialScale, hoverDuration).SetEase(hoverEase);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickTween.IsActive() && _clickTween.IsPlaying()) return;
        _clickTween = _rectTransform.DOScale(_initialScale * clickSize, onClickDuration)
            .SetEase(buttonClickAnimationCurve);
    }
}