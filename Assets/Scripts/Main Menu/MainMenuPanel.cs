using UnityEngine;

public class MainMenuPanel : MonoBehaviour {


	public void OnClickSinglePlayer()
    {

    }

    public void OnClickMultiPlayer()
    {
        PanelManager.Instance.SwitchPanel(PanelManager.Instance.MultiplayerPanel);
    }
    public void OnClickSettings()
    {
        PanelManager.Instance.SwitchPanel(PanelManager.Instance.SettingsPanel);
    }
    public void OnClickExit()
    {

    }

}
