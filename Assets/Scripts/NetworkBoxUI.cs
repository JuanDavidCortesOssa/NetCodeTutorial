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
            GameManager.Instance.CheckGameEnd();
        };
    }

    private void UpdateBoxTextNetworkVariable()
    {
        var playerTurn = TurnManager.GetPlayerTurn();

        if (IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Server))
        {
            ChangeBoxTextNetworkVariable("X");
            TurnManager.ChangePlayerTurnNetworkVariable(TurnManager.PlayerTurn.Client);
        }
        else if (IsClient && !IsServer && playerTurn.Equals(TurnManager.PlayerTurn.Client))
        {
            ChangeBoxTextServerRpc("O");
            TurnManager.ChangePlayerTurnNetworkVariable(TurnManager.PlayerTurn.Server);
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