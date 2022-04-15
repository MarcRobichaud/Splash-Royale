using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button hostButton;
    public Button connectButton;
    public Button quitButton;

    public Camera p1Cam;
    public Camera p2Cam;

    private PlayerStats p1;
    private PlayerStats p2;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (hostButton != null)
            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                
                TurnOnCamera(p2Cam, false);
                TurnOnCamera(p1Cam, true);
            });
        
        if (connectButton != null)
            connectButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient(); 
                
                TurnOnCamera(p1Cam, false);
                TurnOnCamera(p2Cam, true);
            });
        
        if (quitButton != null)
            quitButton.onClick.AddListener(Application.Quit);
    }

    private void TurnOnCamera(Camera cam, bool on)
    {
        cam.enabled = on;
    }
}
