using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject sceneGame;
    [SerializeField] private GameObject sceneMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text textBestScore;
    [SerializeField] private Text textCurrentScore;

    private int currentScore = 0;
    private int bestScore = 0;

    private void Awake()
    {
        GameLogic.addScore += AddScore;
        GameLogic.restartGame += RestartGame;
        GameLogic.loadSaveGame += LoadFromSaves;
        GameLogic.onGameOver += OnGameOver; 
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        textBestScore.text = bestScore.ToString();
        textCurrentScore.text = currentScore.ToString(); 
    }

    private void OnDestroy()
    {
        GameLogic.addScore -= AddScore;
        GameLogic.restartGame -= RestartGame;
        GameLogic.loadSaveGame -= LoadFromSaves;
        GameLogic.onGameOver -= OnGameOver;
    }

    private void AddScore()
    {
        currentScore++;
        textCurrentScore.text = currentScore.ToString();
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        if (bestScore < currentScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            bestScore = currentScore;
            textBestScore.text = bestScore.ToString();
        }
    }

    private void RestartGame()
    {
        currentScore = 0;
        PlayerPrefs.SetInt("CurrentScore", 0);
        textCurrentScore.text = currentScore.ToString();
    }

    private void LoadFromSaves()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0); ;
        textCurrentScore.text = currentScore.ToString();
    }

    public void OnBackButtonClick()
    {
        gameOver.SetActive(false);
        sceneMenu.SetActive(true);
        sceneGame.SetActive(false);
        GameLogic.onSoundClick.Invoke();
    }

    public void OnRestartGameClick()
    { 
        gameOver.SetActive(false);
        GameLogic.restartGame.Invoke();
        GameLogic.onSoundClick.Invoke();
    }

    public void OnGameOver()
    {
        GameLogic.restartGame.Invoke();
        gameOver.SetActive(true);
    }
     
}
