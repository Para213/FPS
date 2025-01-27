using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject floatingScorePrefab; 
    [SerializeField] private Transform floatingScoreSpawnPoint; 

    private int currentScore = 0; 

    private void Start()
    {
        UpdateScoreText(); 
    }

    public void AddScore(int points)
    {
        currentScore += points; 
        UpdateScoreText(); 
        ShowFloatingScore(points); 
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{currentScore}"; 
    }

    private void ShowFloatingScore(int points)
    {
        GameObject floatingScoreInstance = Instantiate(floatingScorePrefab, floatingScoreSpawnPoint.position, Quaternion.identity, floatingScoreSpawnPoint); 
        TextMeshProUGUI floatingScoreText = floatingScoreInstance.GetComponent<TextMeshProUGUI>(); 
        floatingScoreText.text = $"+{points}"; 
        Destroy(floatingScoreInstance, 2f); 
    }
}
