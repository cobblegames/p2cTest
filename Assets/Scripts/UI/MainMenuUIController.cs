using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitGameButton;


    private void Start()
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
        gameObject.SetActive(false);
    }

  
}

