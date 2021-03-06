using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public Button hostButton;
    public Button connectButton;
    public Button quitButton;

    public TMP_Text yourID;
    public TMP_Text hisID;

    public TMP_Text gameOver;

    public Camera p1Cam;
    public Camera p2Cam;

    public TMP_InputField inputField;
    
    public UNetTransport transport;
    
    private void Awake()
    {
        Init();
    }
    
    private void Update()
    {
        DisplayStats();
    }

    private void Init()
    {
        if (hostButton != null)
            hostButton.onClick.AddListener(InitHost);
        
        if (connectButton != null)
            connectButton.onClick.AddListener(InitClient);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(Application.Quit);
    }

    private void InitHost()
    {
        NetworkManager.Singleton.StartHost();
                
        TurnOnCamera(p2Cam, false);
        TurnOnCamera(p1Cam, true);
        TurnOffButtons();
    }

    private void InitClient()
    {
        if (inputField.text != "")
            transport.ConnectAddress = inputField.text;

        NetworkManager.StartClient();
        
        TurnOnCamera(p1Cam, false);
        TurnOnCamera(p2Cam, true);
        TurnOffButtons();
    }

    private void TurnOffButtons()
    {
        if (hostButton != null)
            hostButton.gameObject.SetActive(false);

        if (connectButton != null)
            connectButton.gameObject.SetActive(false);
        
        if (inputField != null)
            inputField.gameObject.SetActive(false);
    }

    private void TurnOnCamera(Camera cam, bool on)
    {
        cam.enabled = on;
    }

    private void DisplayStats()
    {
        if (IsHost)
        {
            DisplayStat(GameManager.Instance.HostStats.ToString(), GameManager.Instance.ClientStats.ToString());
        }
        else
        {
            DisplayStat(GameManager.Instance.ClientStats.ToString(), GameManager.Instance.HostStats.ToString());
        }
    }

    private void DisplayStat(string you, string him)
    {
        yourID.text = you;
        hisID.text = him;
    }
}
