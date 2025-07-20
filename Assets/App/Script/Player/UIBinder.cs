using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIBinder : MonoBehaviour
{
   
    public PlayerController PlayerController;
    public CanvasGroup gameOverPanel;
    public TMP_Text scoreText;
    public Button restartButton;
    public Button homeButton;

    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.BindUI(this);
        }
       
    }   

}
