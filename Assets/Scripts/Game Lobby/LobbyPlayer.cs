using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {


    //private static readonly List<Color> Colors = new List<Color>(){Color.magenta, Color.black, Color.cyan, Color.blue, Color.green, Color.yellow};

    //private static List<Color> _colorsInUse;
    //public static List<Color> ColorsInUse { get { return _colorsInUse ?? (_colorsInUse = new List<Color>()); }} 

    //private static List<Color> _availableColors = new List<Color>();

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

        //if(MultiplayerManager.Instance!= null)
        //    MultiplayerManager.Instance.OnPlayerCountChange(1); // gross

        _lobbyPlayers.AddPlayer(this);

        SetupOtherPlayer();

        OnMyName(PlayerName);
        //OnMyColor(MultiplayerManager.Instance.FirstAvailablePlayerColor());
        //MultiplayerManager.Instance.SetFirstAvailablePlayerColor(this);
        //UpdateAvailableColors();
        OnMyColor(PlayerColor);
        OnMyReady(PlayerReady);
    }

    private void SetupOtherPlayer() {
        if (isLocalPlayer) return;

        Debug.Log("Setup other player.");

        NameInput.interactable = false;
        ReadyCheck.interactable = false;
        ColorChoicesDropdown.interactable = false;
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

        ColorChoicesDropdown.options.Clear();
        foreach (var c in MultiplayerManager.Instance.Colors)
            ColorChoicesDropdown.options.Add(new Dropdown.OptionData() { text = c.GetName() });

        CmdColorChanged(MultiplayerManager.Instance.FirstAvailablePlayerColor());

        ReadyCheck.isOn = false;
        PlayerReady = false;

        if (PlayerName == "")
            CmdNameChanged("Player " + (_lobbyPlayers.PlayerListContentTransform.childCount-1));

        OnMyReady(PlayerReady);


        //MultiplayerManager.Instance.UpdateAvailableColors(this, );
        //OnMyColor(MultiplayerManager.Instance.FirstAvailablePlayerColor());

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

        if (PlayerReady) SendReadyToBeginMessage();
        else SendNotReadyToBeginMessage();

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

public static class Extensions {
    public static string GetName(this Color color) {
        if (color == Color.black) return "Black";
        if (color == Color.blue) return "Blue";
        if (color == Color.white) return "White";
        if (color == Color.cyan) return "Cyan";
        if (color == Color.magenta) return "Magenta";
        if (color == Color.green) return "Green";
        return color == Color.yellow ? "Yellow" : "Unknown";
    }
}
