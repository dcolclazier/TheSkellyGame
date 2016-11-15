using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {


    [SyncVar(hook = "OnMyName")] public string PlayerName = "";

    [SyncVar(hook = "OnMyColor")] public Color PlayerColor = Color.white;

    [SyncVar(hook = "OnMyReady")] public bool PlayerReady = false;

    public InputField NameInput;
    public Toggle ReadyCheck;
    public Dropdown ColorChoicesDropdown;
    
    public Image DropDownImage;

    private readonly LobbyPlayerList _lobbyPlayers = LobbyPlayerList.Instance;

    public override void OnClientEnterLobby() {
        base.OnClientEnterLobby();

        _lobbyPlayers.AddPlayer(this);

        SetupOtherPlayer();

        OnMyName(PlayerName);
        OnMyColor(PlayerColor);
        OnMyReady(PlayerReady);
    }

    private void SetupOtherPlayer() {
        if (isLocalPlayer) return;

        Debug.Log("Setup other player.");

        NameInput.interactable = false;
        ReadyCheck.interactable = false;
        ColorChoicesDropdown.interactable = false;
        ColorChoicesDropdown.options.Clear();
        foreach (var c in MultiplayerManager.Instance.Colors)
            ColorChoicesDropdown.options.Add(new Dropdown.OptionData() { text = c.GetName() });
        GetComponent<Image>().color = Color.gray;

    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("On Start authority");
        SetupLocalPlayer();
    }
    private void SetupLocalPlayer() {

        Debug.Log("Setup local player.");
        NameInput.interactable = true;
        ReadyCheck.interactable = true;
        ColorChoicesDropdown.interactable = true;
        GetComponent<Image>().color = Color.white;

        CmdColorChanged(MultiplayerManager.Instance.FirstAvailablePlayerColor());

        if (PlayerName == "")
            CmdNameChanged("Player " + (_lobbyPlayers.PlayerListContentTransform.childCount-1));

        ReadyCheck.isOn = false;
        PlayerReady = false;
        OnMyReady(PlayerReady);

        NameInput.Select();
        NameInput.onEndEdit.RemoveAllListeners();
        NameInput.onEndEdit.AddListener(OnNameChanged);

        ColorChoicesDropdown.onValueChanged.RemoveAllListeners();
        ColorChoicesDropdown.onValueChanged.AddListener(OnColorChanged);

        ReadyCheck.onValueChanged.RemoveAllListeners();
        ReadyCheck.onValueChanged.AddListener(OnReadyClicked);
    }

    private void OnReadyClicked(bool ready) {
        CmdReadyChanged(ready);
    }

    private void OnColorChanged(int colorIndex) {
        CmdColorChanged(MultiplayerManager.Instance.Colors[colorIndex]);
    }
    

    private void OnNameChanged(string newName) {
        CmdNameChanged(newName);
    }

    public void OnMyName(string newName) {

        PlayerName = newName;
        NameInput.text = PlayerName;
    }
    

    public void OnMyReady(bool ready) {

        PlayerReady = ready;
        ReadyCheck.isOn = PlayerReady;

        if (PlayerReady && isLocalPlayer) SendReadyToBeginMessage();
        else if(isLocalPlayer) SendNotReadyToBeginMessage();

    }

    public void OnMyColor(Color newColor) {

        MultiplayerManager.Instance.UpdateAvailableColors(this,newColor);
        PlayerColor = newColor;
        DropDownImage.color = newColor;
        ColorChoicesDropdown.GetComponentInChildren<Text>().text = newColor.GetName();
        ColorChoicesDropdown.value = MultiplayerManager.Instance.Colors.IndexOf(newColor);
        
    }

    [Command]
    public void CmdNameChanged(string playerName) {
        PlayerName = playerName;
    }

    [Command]
    public void CmdColorChanged(Color color)
    {
        PlayerColor = color;
    }

    [Command]
    public void CmdReadyChanged(bool ready) {       
        PlayerReady = ready;
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        MultiplayerManager.Instance.CountdownPanel.UIText.text = "Match Starting in " + countdown;
        MultiplayerManager.Instance.CountdownPanel.gameObject.SetActive(countdown != 0);
    }
}