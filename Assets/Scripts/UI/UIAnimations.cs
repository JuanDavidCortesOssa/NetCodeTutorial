using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIAnimations : MonoBehaviour
{
    [Header("DoTweenAnimations")] 
    [SerializeField] private Ease hoverEase;
    [SerializeField] private AnimationCurve buttonClickAnimationCurve;
    
    
    //General
    private RectTransform _rectTransform;
    private Vector3 InitialScale;

    //Twiners
    private Tween _hoverTween;
    private Tween _clickTween;
    
    //Triggers
    private Button _button;
    private EventTrigger _eventTrigger;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _eventTrigger = GetComponent<EventTrigger>();

        AddListeners();
    }

    private void AddListeners()
    {
        _button.onClick.AddListener(OnPointerClick);
        AddEventTriggerListener(_eventTrigger, EventTriggerType.PointerEnter, data => OnPointerEnterAnimation());
        AddEventTriggerListener(_eventTrigger, EventTriggerType.PointerExit, data => OnPointerExitAnimation());
    }

    private static void AddEventTriggerListener(EventTrigger trigger,
        EventTriggerType eventType,
        System.Action<BaseEventData> callback)
    {
        var entry = new EventTrigger.Entry
        {
            eventID = eventType,
            callback = new EventTrigger.TriggerEvent()
        };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }

    public void OnPointerEnterAnimation()
    {
        _hoverTween = _rectTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.25f).SetEase(hoverEase);
    }

    public void OnPointerExitAnimation()
    {
        _hoverTween = _rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetEase(hoverEase);
    }

    public void OnPointerClick()
    {
        if (_clickTween.IsActive() && _clickTween.IsPlaying()) return;
        _clickTween = _rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetEase(buttonClickAnimationCurve);
    }
}