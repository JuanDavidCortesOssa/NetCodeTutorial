using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private NetworkVariable<TextStruct> myTextNetworkVariable = new NetworkVariable<TextStruct>(
        new TextStruct() { text = "" }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner
    );

    public struct TextStruct : INetworkSerializable
    {
        public FixedString32Bytes text;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref text);
        }
    }

    public override void OnNetworkSpawn()
    {
        myTextNetworkVariable.OnValueChanged += (TextStruct previousValue, TextStruct newValue) =>
        {
            _textMeshProUGUI.text = newValue.text.ToString();
            Debug.Log(newValue.text.ToString());
            //UpdateTextServerRpc(newValue.text.ToString());
        };
    }

    [ClientRpc]
    private void UpdateTextClientRpc(string text)
    {
        _textMeshProUGUI.text = text;
        Debug.Log(text);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateTextServerRpc(string text)
    {
        UpdateNetworkVariable(text);
    }

    private void UpdateNetworkVariable(string newText)
    {
        myTextNetworkVariable.Value = new TextStruct()
        {
            text = newText
        };
    }

    private void Update()
    {
        //if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (IsOwner)
            {
                Debug.Log("T");
                UpdateNetworkVariable("X");
            }
            else
            {
                Debug.Log("T");
                UpdateTextServerRpc("O");
            }

            //myTextNetworkVariable.Value = IsServer ? new TextStruct { text = "X" } : new TextStruct { text = "O" };
        }
    }
}