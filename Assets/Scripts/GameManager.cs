using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;

    public override void OnNetworkSpawn()
    {
        instance = this;
    }

    public enum PlayerTurn
    {
        Server,
        Client
    }

    public NetworkVariable<PlayerTurn> playerTurnNetworkVariable = new NetworkVariable<PlayerTurn>(PlayerTurn.Server,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerTurnServerRpc(PlayerTurn playerTurn)
    {
        playerTurnNetworkVariable.Value = playerTurn;
    }

    public static void ChangePlayerTurnNetworkVariable(PlayerTurn playerTurn)
    {
        instance.ChangePlayerTurnServerRpc(playerTurn);
    }

    public static PlayerTurn GetPlayerTurn()
    {
        return instance.playerTurnNetworkVariable.Value;
    }
}