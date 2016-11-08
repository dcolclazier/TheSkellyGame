using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayer : NetworkLobbyPlayer {


    private static readonly List<Color> Colors = new List<Color>(){Color.magenta, Color.black, Color.cyan, Color.blue, Color.green, Color.yellow};

    private static List<Color> _colorsInUse;
    public static List<Color> ColorsInUse { get { return _colorsInUse ?? (_colorsInUse = new List<Color>()); }} 

    private static List<Color> _availableColors = new List<Color>();

    [SyncVar(hook = "OnMyName")] public string PlayerName = "";

    [SyncVar(hook = "OnMyColor")] public Color PlayerColor = Color.white;

    [SyncVar(hook = "OnMyReady")] public bool PlayerReady = false;

    public InputField NameInput;
    public Toggle ReadyCheck;
    public Dropdown ColorChoicesDropdown;
    
    public Image DropDownImage;

    public override void OnClientEnterLobby() {
        base.OnClientEnterLobby();

        if(MultiplayerManager.Instance!=null)
            MultiplayerManager.Instance.OnPlayerCountChange(1); // gross

        LobbyPlayerList.Instance.AddPlayer(this);

        SetupOtherPlayer();

        OnMyName(PlayerName);
        OnMyColor(PlayerColor);
        OnMyReady(PlayerReady);
    }

    private void SetupOtherPlayer() {
        if (isLocalPlayer) return;

        NameInput.interactable = false;
        ReadyCheck.interactable = false;
        ColorChoicesDropdown.interactable = false;

    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        SetupLocalPlayer();
    }
    private void SetupLocalPlayer() {
        NameInput.interactable = true;
        ReadyCheck.interactable = true;
        ColorChoicesDropdown.interactable = true;

        //must choose color?
        ReadyCheck.isOn = false;
        PlayerReady = false;

        UpdateAvailableColors();
        Debug.Log("First available color: " + _availableColors.First().GetName());
        CmdColorChanged(_availableColors.First());

        Debug.Log("Index of player color: " + Colors.IndexOf(PlayerColor));
        Debug.Log("Player Color: " + PlayerColor.GetName());
        OnColorChanged(Colors.IndexOf(PlayerColor));

        if (PlayerName == "")
            CmdNameChanged("Player " + (LobbyPlayerList.Instance.PlayerListContentTransform.childCount-1));

        OnMyReady(PlayerReady);

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
        CmdColorChanged(Colors[colorIndex]);
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

        if (newColor == Color.white) {
            UpdateAvailableColors();
            newColor = _availableColors.First();
        }

        ColorsInUse.Remove(PlayerColor);

        Debug.Log("Player color has been changed from " + PlayerColor.GetName() + " to " + newColor.GetName());
        PlayerColor = newColor;
        DropDownImage.color = PlayerColor;
        ColorChoicesDropdown.GetComponentInChildren<Text>().text = PlayerColor.GetName();
        ColorChoicesDropdown.itemText.text = PlayerColor.GetName();

        ColorsInUse.Add(PlayerColor);
    }

    public void UpdateAvailableColors() {

        //_availableColors = Colors.ToList();
        _availableColors = Colors.Where(c => ColorsInUse.All(c2 => c2 != c)).ToList();
        ColorChoicesDropdown.options.Clear();
        foreach (var color in Colors) {
            ColorChoicesDropdown.options.Add(new Dropdown.OptionData() { text = color.GetName()});
        }
    }

    public void OnPlayerListChanged(int playerIndex) {


    }

    [Command]
    public void CmdNameChanged(string playerName) {
        PlayerName = playerName;
    }

    [Command]
    public void CmdColorChanged(Color color) {

        OnMyColor(color);
        UpdateAvailableColors();

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
