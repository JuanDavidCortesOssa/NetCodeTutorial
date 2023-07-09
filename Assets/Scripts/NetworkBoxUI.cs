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
        boxButton.onClick.AddListener(UpdateBoxTextNetworkVariable);
        _boxTextNetworkVariable.OnValueChanged += (value, newValue) => { boxText.text = newValue.Value; };
    }


    private void UpdateBoxTextNetworkVariable()
    {
        var playerTurn = GameManager.GetPlayerTurn();

        if (IsServer && playerTurn.Equals(GameManager.PlayerTurn.Server))
        {
            ChangeBoxTextNetworkVariable("X");
            GameManager.ChangePlayerTurnNetworkVariable(GameManager.PlayerTurn.Client);
        }
        else if (IsClient && !IsServer && playerTurn.Equals(GameManager.PlayerTurn.Client))
        {
            ChangeBoxTextServerRpc("O");
            GameManager.ChangePlayerTurnNetworkVariable(GameManager.PlayerTurn.Server);
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
}