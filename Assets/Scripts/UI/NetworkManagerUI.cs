using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.Serialization;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private TMP_InputField lobbyInputField;
    [SerializeField] private TextMeshProUGUI lobbyTitle;
    [SerializeField] private TextMeshProUGUI lobbyIdText;
    [SerializeField] private Button joinAsClientButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI connectionErrorText;

    private void Awake()
    {
        clientBtn.onClick.AddListener((ShowClientPanel));
        hostBtn.onClick.AddListener((StartHost));
        joinAsClientButton.onClick.AddListener(JoinAsClient);
        backButton.onClick.AddListener((GoBack));
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.OnClientConnectedCallback += CheckForSessionStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback += (ulong u) => { OnPlayerDisconnected(); };
    }

    private void JoinAsClient()
    {
        try
        {
            GamingServicesManager.Instance.JoinAsClient(lobbyInputField.text);
        }
        catch (Exception e)
        {
            connectionErrorText.gameObject.SetActive(true);
            Console.WriteLine(e);
            throw;
        }
    }

    private void ShowClientPanel()
    {
        //Panel setup
        lobbyTitle.SetText("Enter lobby id");
        lobbyInputField.gameObject.SetActive(true);
        joinAsClientButton.gameObject.SetActive(true);
        lobbyIdText.gameObject.SetActive(false);
        connectionErrorText.gameObject.SetActive(false);

        UIManager.Instance.ShowNetworkPanel();
    }

    private async void StartHost()
    {
        //Panel setup
        lobbyTitle.SetText("Waiting for players...");
        lobbyInputField.gameObject.SetActive(false);
        joinAsClientButton.gameObject.SetActive(false);
        lobbyIdText.gameObject.SetActive(true);
        lobbyIdText.SetText("");
        connectionErrorText.gameObject.SetActive(false);

        UIManager.Instance.ShowNetworkPanel();
        lobbyIdText.text = await GamingServicesManager.Instance.StartHost();
    }

    private void GoBack()
    {
        if (lobbyIdText.gameObject.activeSelf)
        {
            GamingServicesManager.Instance.StopHost();
        }

        UIManager.Instance.ShowStartPanel();
    }

    private void CheckForSessionStarted(ulong numberUlong)
    {
        if (!IsServer) return;
        if (NetworkManager.Singleton.ConnectedClients.Count < 2) return;

        UIManager.Instance.ShowGamePanel();
        ShowGamePanelClientRpc();
    }

    [ClientRpc]
    private void ShowGamePanelClientRpc()
    {
        UIManager.Instance.ShowGamePanel();
    }

    private void OnPlayerDisconnected()
    {
        UIManager.Instance.ShowNetworkPanel();
        GameManager.Instance.RestartGame();
        OnPlayerDisconnectedClientRpc();
    }

    [ClientRpc]
    private void OnPlayerDisconnectedClientRpc()
    {
        UIManager.Instance.ShowNetworkPanel();
        GameManager.Instance.RestartGame();
    }
}