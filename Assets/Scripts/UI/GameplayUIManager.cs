using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameplayUIManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private TextMeshProUGUI playerStatusValue;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] Image panelImage;

    [SerializeField] Color detectedColor = Color.red;
    [SerializeField] Color safeColor = Color.green;



    private void OnEnable()
    {
        GameEvents.OnPlayerDetected += Handle_PlayerDetectedState;
        GameEvents.OnUpdateGameUI += UpdateGameUI;
    }

   

    private void OnDisable()
    {
        GameEvents.OnPlayerDetected -= Handle_PlayerDetectedState;
        GameEvents.OnUpdateGameUI -= UpdateGameUI;
    }
    private void Handle_PlayerDetectedState(bool isDetected)
    {
        if(isDetected)
        {
            panelImage.color = detectedColor;
        }
        else
        {
            panelImage.color = safeColor;
        }
        UpdateGameUI();
    }

    private void UpdateGameUI()
    {
        playerStatusValue.text = GameManager.Instance.Player.PlayerStatus.ToString();
        scoreValue.text = $"{GameManager.Instance.CollectedTheftObjectsCount}/{GameManager.Instance.TotalGameObjectsCount}";      
        timerValue.text = $"{Mathf.FloorToInt(GameManager.Instance.CurrentGameTime / 60)}:{Mathf.FloorToInt(GameManager.Instance.CurrentGameTime % 60):00}";
    }

}
