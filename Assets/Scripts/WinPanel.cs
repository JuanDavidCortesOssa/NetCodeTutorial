using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class WinPanel : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.OnPlayerWin += OnGameWin;
        GameManager.Instance.OnGameDraw += OnGameDraw;
    }

    private void OnGameWin(TurnManager.PlayerTurn playerTurn)
    {
        if (IsServer)
        {
            winText.SetText("Winner");
        }
        else
        {
            winText.SetText("Looser ZZZ");
        }
    }

    private void OnGameDraw()
    {
        winText.SetText("Draw");
    }
}