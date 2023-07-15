using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public enum PlayerTurn
    {
        Server,
        Client
    }

    [SerializeField] private TextMeshProUGUI playerTurnText;
    private static GameManager _instance;

    public NetworkVariable<PlayerTurn> playerTurnNetworkVariable = new NetworkVariable<PlayerTurn>(PlayerTurn.Server,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        _instance = this;
        ChangePlayerTurnText(PlayerTurn.Server);
        playerTurnNetworkVariable.OnValueChanged += (value, newValue) =>
        {
            ChangePlayerTurnText(playerTurnNetworkVariable.Value);
        };
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerTurnServerRpc(PlayerTurn playerTurn)
    {
        playerTurnNetworkVariable.Value = playerTurn;
    }

    private void ChangePlayerTurnText(PlayerTurn playerTurn)
    {
        if ((IsServer && playerTurn == PlayerTurn.Server) || (!IsServer && IsClient && playerTurn == PlayerTurn.Client))
        {
            playerTurnText.text = "Your turn";
        }
        else
        {
            playerTurnText.text = "Opponent turn";
        }
    }

    public static void ChangePlayerTurnNetworkVariable(PlayerTurn playerTurn)
    {
        _instance.ChangePlayerTurnServerRpc(playerTurn);
        _instance.ChangePlayerTurnText(playerTurn);
    }

    public static PlayerTurn GetPlayerTurn()
    {
        return _instance.playerTurnNetworkVariable.Value;
    }
}