using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public Button hostButton;
    public Button connectButton;
    public Button quitButton;
    public Button test;

    public TMP_Text yourID;
    public TMP_Text hisID;

    public Camera p1Cam;
    public Camera p2Cam;

    [SerializeField]
    private NetworkVariable<PlayerStats> hostStats = new NetworkVariable<PlayerStats>();
    [SerializeField]
    private NetworkVariable<PlayerStats> clientStats = new NetworkVariable<PlayerStats>();

    private PlayerStats HostStats => hostStats.Value;
    private PlayerStats ClientStats => clientStats.Value;

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

        if (test != null)
            test.onClick.AddListener( () =>
            {
                var temp = HostStats;
                temp.Lives -= 1;
                hostStats.Value = temp;
            });
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
        NetworkManager.Singleton.StartClient(); 
                
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
    }

    private void TurnOnCamera(Camera cam, bool on)
    {
        cam.enabled = on;
    }

    public void DisplayStats()
    {
        if (clientStats == null || hostStats == null) return;
        
        if (IsHost)
        {
            DisplayStat(HostStats.ToString(), ClientStats.ToString());
        }
        else
        {
            DisplayStat(ClientStats.ToString(), HostStats.ToString());
        }
    }

    private void DisplayStat(string you, string him)
    {
        yourID.text = you;
        hisID.text = him;
    }
    
    public void InitPlayerStats(ulong id, bool isHost)
    {
        PlayerStats stats = new PlayerStats(id);

        if (isHost)
            hostStats.Value = stats;
        else
            clientStats.Value = stats;
    }
}
