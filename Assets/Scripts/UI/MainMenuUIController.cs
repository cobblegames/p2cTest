
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MenuScreen, IInjectable
{
    [Header ("Local References")]
    [SerializeField] MenuElement startButton;
    [SerializeField] MenuElement exitGameButton;
  
    void OnEnable()
    {
    
    }
    void OnDisable()
    {
    
    }
   
    public void Initialize(IInjectable[] _injectedElements)
    {

        if (startButton != null)
        {
         
           
            Debug.Log("Start Button Initialized");
        }
        if (exitGameButton != null)
        {
           
            Debug.Log("Exit Game Button Initialized");
        }

    }

    private void OnExitGameButtonClicked()
    {
       Application.Quit();
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Starting Game...");
        GameEvents.PostOnGameStart();
    }

}

