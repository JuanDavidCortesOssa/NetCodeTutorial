using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

[Serializable]
public struct UIPanel
{
    public RectTransform Transform;
    public Vector2 OutPosition;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIPanel networkManagerTransform, boardTransform, winPanelTransform;
    [SerializeField] private float transitionTime;
    [SerializeField] private List<Button> buttons;

    private UIPanel currentPanel;

    private void Start()
    {
        AddListeners();
        currentPanel = networkManagerTransform;
        networkManagerTransform.Transform.DOAnchorPos(Vector2.zero, transitionTime);
    }

    private void AddListeners()
    {
        GameManager.Instance.OnGameDraw += () => { ShowUIPanel(winPanelTransform); };

        GameManager.Instance.OnPlayerWin += (TurnManager.PlayerTurn playerTurn) => { ShowUIPanel(winPanelTransform); };

        foreach (var button in buttons)
        {
            button.onClick.AddListener((() => { ShowUIPanel(boardTransform); }));
        }
    }

    private void ShowUIPanel(UIPanel newPanel)
    {
        currentPanel.Transform.DOAnchorPos(currentPanel.OutPosition, transitionTime);
        newPanel.Transform.DOAnchorPos(Vector2.zero, transitionTime).SetDelay(transitionTime);
        currentPanel = newPanel;
    }
}