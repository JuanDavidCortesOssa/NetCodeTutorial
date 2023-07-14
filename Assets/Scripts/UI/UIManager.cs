using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform startButtonsTransform, boardTransform;
    [SerializeField] private List<Button> buttons;

    private void Start()
    {
        startButtonsTransform.DOAnchorPos(Vector2.zero, 0.25f);

        foreach (var button in buttons)
        {
            button.onClick.AddListener(ShowBoard);
        }
    }

    private void ShowBoard()
    {
        startButtonsTransform.DOAnchorPos(new Vector2(-1200f, 0), 0.25f);
        boardTransform.DOAnchorPos(Vector2.zero, 0.25f).SetDelay(0.25f);
    }
}