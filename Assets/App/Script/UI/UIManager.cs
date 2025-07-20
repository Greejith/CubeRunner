using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public PlayerController PlayerController;
    [Header("Main Menu Buttons")]
    public CanvasGroup mainMenuPanel;
    public Button startButton;
    public Button quitButton;

    [Header("Gameover Menu")]
    public CanvasGroup gameOverPanel;
    public TMP_Text scoreText;
    public Button restartButton;
    public Button homeButton;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    void OnEnable()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        if (homeButton != null)
            homeButton.onClick.AddListener(OnHomeClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartGame);

    }

    void OnDisable()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.RemoveListener(OnQuitClicked);

        if (homeButton != null)
            homeButton.onClick.RemoveListener(OnHomeClicked);

        if (restartButton != null)
            restartButton.onClick.RemoveListener(OnRestartGame);
    }

    void OnStartClicked()
    {
        SceneManager.LoadScene("MainScene");

    }

    public void BindUI(UIBinder binder)
    {
       
        homeButton = binder.homeButton;
        restartButton = binder.restartButton;
        gameOverPanel = binder.gameOverPanel;
        PlayerController = binder.PlayerController;
        scoreText = binder.scoreText;
        PlayerController.isGameStarted = true;

        ResetScore();
    }

    void OnQuitClicked()
    {
        Application.Quit();
    }

  

    public void PlayerHit()
    {

        PlayerController.isGameStarted = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.alpha = 1;
            gameOverPanel.interactable = true;
            gameOverPanel.blocksRaycasts = true;
        }
        UpdateScoreDisplay();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreDisplay();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
    public void OnHomeClicked()
    {
        SceneManager.LoadScene("MainMenu");


    }
    public void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetScore();
        PlayerController.isGameStarted = true;
    }   
}
