using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine.Serialization;

[Serializable]
public struct UIPanel
{
    public RectTransform Transform;
    public Vector2 OutPosition;
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIPanel startPanel, boardTransform, winPanelTransform, networkPanel;
    [SerializeField] private float transitionTime;

    private UIPanel currentPanel;

    private void Start()
    {
        AddListeners();
        currentPanel = startPanel;
        startPanel.Transform.DOAnchorPos(Vector2.zero, transitionTime);
    }

    private void AddListeners()
    {
        GameManager.Instance.OnGameDraw += () => { ShowUIPanel(winPanelTransform); };

        GameManager.Instance.OnPlayerWin += (TurnManager.PlayerTurn playerTurn) => { ShowUIPanel(winPanelTransform); };
    }

    private void ShowUIPanel(UIPanel newPanel)
    {
        currentPanel.Transform.DOAnchorPos(currentPanel.OutPosition, transitionTime);
        newPanel.Transform.DOAnchorPos(Vector2.zero, transitionTime).SetDelay(transitionTime);
        currentPanel = newPanel;
    }

    public void ShowNetworkPanel()
    {
        ShowUIPanel(networkPanel);
    }

    public void ShowStartPanel()
    {
        ShowUIPanel(startPanel);
    }

    public void ShowGamePanel()
    {
        ShowUIPanel(boardTransform);
    }
}