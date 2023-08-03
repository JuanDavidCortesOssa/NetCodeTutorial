using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkBoxUI : NetworkBehaviour
{
    [SerializeField] private Button boxButton;
    [SerializeField] private TextMeshProUGUI boxText;
    private bool isBeingCheck = false;

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.OnGameRestart += ResetText;
        boxButton.onClick.AddListener(UpdateBoxTextNetworkVariable);
    }

    private void UpdateBoxTextNetworkVariable()
    {
        Debug.Log("BoxText: " + boxText.text);
        if (boxText.text != "") return;
        if (isBeingCheck) return;

        isBeingCheck = true;
        var playerTurn = TurnManager.GetPlayerTurn();

        if (IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Server))
        {
            ChangeBoxText("X");
            ChangeBoxTextClientRpc("X");
        }
        else if (IsClient && !IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Client))
        {
            ChangeBoxText("O");
            ChangeBoxTextServerRpc("O");
        }
    }

    private void UpdatePlayerTurnState()
    {
        switch (boxText.text)
        {
            case "O":
                TurnManager.ChangePlayerTurnNetworkVariable(TurnManager.PlayerTurn.Server);
                break;
            case "X":
                TurnManager.ChangePlayerTurnNetworkVariable(TurnManager.PlayerTurn.Client);
                break;
        }
    }

    private void ChangeBoxText(string text)
    {
        boxText.SetText(text);
        UpdatePlayerTurnState();
        GameManager.Instance.CheckGameEnd();
        isBeingCheck = false;
    }

    [ClientRpc]
    private void ChangeBoxTextClientRpc(string text)
    {
        ChangeBoxText(text);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeBoxTextServerRpc(string text)
    {
        ChangeBoxText(text);
    }

    private void ResetText()
    {
        boxText.text = "";
    }
}