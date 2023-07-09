using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<myCustomData> randomNumber = new NetworkVariable<myCustomData>(new myCustomData()
        {
            _int = 12, _bool = true
        }, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public struct myCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (myCustomData previousValue, myCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " random number: " + randomNumber.Value._int);
        };
    }

    private void Update()
    {
        if (!IsOwner) return;

        #region PlayerMovement

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.N))
        {
            randomNumber.Value = new myCustomData { _int = (int)OwnerClientId };
        }

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     Debug.Log("T");
        // }

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = +1f;
        }

        const float moveSpeed = 1f;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        #endregion
    }
}