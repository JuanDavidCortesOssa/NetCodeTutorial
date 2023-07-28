using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private Button restartButton;
    private GameManager _gameManager;

    public override void OnNetworkSpawn()
    {
        _gameManager = GameManager.Instance;
        AddListeners();
    }

    private void AddListeners()
    {
        _gameManager.OnPlayerWin += OnGameWin;
        _gameManager.OnGameDraw += OnGameDraw;
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnGameWin(TurnManager.PlayerTurn playerWinner)
    {
        if (IsServer)
        {
            winText.SetText(playerWinner == TurnManager.PlayerTurn.Server ? "Winner" : "Looser");
        }
        else
        {
            winText.SetText(playerWinner == TurnManager.PlayerTurn.Server ? "Looser" : "Winner");
        }
    }

    private void OnGameDraw()
    {
        winText.SetText("Draw");
    }

    private void RestartGame()
    {
        if (IsServer)
        {
            RestartGameClientRpc();
        }
        else
        {
            RestartGameServerRpc();
        }

        GameManager.Instance.RestartGame();
        UIManager.Instance.ShowGamePanel();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RestartGameServerRpc()
    {
        GameManager.Instance.RestartGame();
        UIManager.Instance.ShowGamePanel();
    }

    [ClientRpc]
    private void RestartGameClientRpc()
    {
        GameManager.Instance.RestartGame();
        UIManager.Instance.ShowGamePanel();
    }
}