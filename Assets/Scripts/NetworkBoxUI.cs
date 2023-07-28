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

    private NetworkVariable<FixedString32Bytes> _boxTextNetworkVariable =
        new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.OnGameRestart += ResetText;
        boxButton.onClick.AddListener(UpdateBoxTextNetworkVariable);
        _boxTextNetworkVariable.OnValueChanged += (value, newValue) =>
        {
            boxText.text = newValue.Value;
            UpdatePlayerTurnState();
            GameManager.Instance.CheckGameEnd();
            isBeingCheck = false;
        };
    }

    private void UpdateBoxTextNetworkVariable()
    {
        if (boxText.text != "") return;
        if (isBeingCheck) return;

        isBeingCheck = true;
        var playerTurn = TurnManager.GetPlayerTurn();

        if (IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Server))
        {
            ChangeBoxTextNetworkVariable("X");
        }
        else if (IsClient && !IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Client))
        {
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

    private void ChangeBoxTextNetworkVariable(string value)
    {
        _boxTextNetworkVariable.Value = value;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeBoxTextServerRpc(string text)
    {
        ChangeBoxTextNetworkVariable(text);
    }

    private void ResetText()
    {
        boxText.text = "";
    }
}