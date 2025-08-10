
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MenuScreen, IInjectable
{
    [Header ("Local References")]
    [SerializeField] Button startButton;
    [SerializeField] Button exitGameButton;
  
    void OnEnable()
    {
        GameEvents.OnRegisterInjectables += RegisterInjectable;
    }
    void OnDisable()
    {
        GameEvents.OnRegisterInjectables -= RegisterInjectable;
    }
    public void RegisterInjectable()
    {
        InterfaceDependencyInjector.Instance.RegisterInjectable(this);
    }
    public void Initialize(IInjectable[] _injectedElements)
    {

        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        if (exitGameButton != null)
        {
            exitGameButton.onClick.AddListener(OnExitGameButtonClicked);
        }

    }

    private void OnExitGameButtonClicked()
    {
       Application.Quit();
    }

    private void OnStartButtonClicked()
    {
        GameEvents.PostOnGameStart();
    }

}

