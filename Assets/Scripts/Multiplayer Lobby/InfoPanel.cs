using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    public Text infoText;
    public Text buttonText;
    public Button panelButton;

    public void Display(string info, string buttoninfo, UnityEngine.Events.UnityAction buttonDelegate)
    {
        infoText.text = info;
        buttonText.text = buttoninfo;

        panelButton.onClick.RemoveAllListeners();

        if (buttonDelegate != null)
            panelButton.onClick.AddListener(buttonDelegate);

        panelButton.onClick.AddListener(() => { gameObject.SetActive(false); });

        gameObject.SetActive(true);

    }
}
