using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button serverBtn;

    private void Awake()
    {
        // clientBtn.onClick.AddListener((() => { NetworkManager.Singleton.StartClient(); }));
        // hostBtn.onClick.AddListener((() => { NetworkManager.Singleton.StartHost(); }));
        // serverBtn.onClick.AddListener((() => { NetworkManager.Singleton.StartServer(); }));
    }
}