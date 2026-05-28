using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShantiMvpLoop : MonoBehaviour
{
    private enum CookieShape
    {
        None,
        Circle,
        Star
    }

    [Header("UI Text")]
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text messageText;

    [Header("Buttons")]
    [SerializeField] private Button circleButton;
    [SerializeField] private Button starButton;
    [SerializeField] private Button completeOrderButton;
    [SerializeField] private Button restartButton;

    [Header("Game Settings")]
    [SerializeField] private int targetScore = 3;
    [SerializeField] private float startTime = 60f;

    private CookieShape currentOrder;
    private CookieShape selectedShape;

    private int score;
    private float timeLeft;
    private bool gameOver;

    private void Awake()
    {
        if (circleButton != null)
        {
            circleButton.onClick.RemoveAllListeners();
            circleButton.onClick.AddListener(PickCircle);
        }

        if (starButton != null)
        {
            starButton.onClick.RemoveAllListeners();
            starButton.onClick.AddListener(PickStar);
        }

        if (completeOrderButton != null)
        {
            completeOrderButton.onClick.RemoveAllListeners();
            completeOrderButton.onClick.AddListener(CompleteOrder);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    private void Start()
    {
        RestartGame();
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            LoseGame();
        }

        UpdateUI();
    }

    public void PickCircle()
    {
        if (gameOver)
        {
            return;
        }

        selectedShape = CookieShape.Circle;
        SetMessage("Selected: Circle Cookie");
        UpdateUI();
    }

    public void PickStar()
    {
        if (gameOver)
        {
            return;
        }

        selectedShape = CookieShape.Star;
        SetMessage("Selected: Star Cookie");
        UpdateUI();
    }

    public void CompleteOrder()
    {
        if (gameOver)
        {
            return;
        }

        if (selectedShape == CookieShape.None)
        {
            SetMessage("Pick Circle or Star first.");
            UpdateUI();
            return;
        }

        if (selectedShape != currentOrder)
        {
            SetMessage("Wrong cookie. Customer wanted: " + currentOrder + " Cookie");
            selectedShape = CookieShape.None;
            UpdateUI();
            return;
        }

        score++;
        selectedShape = CookieShape.None;

        if (score >= targetScore)
        {
            WinGame();
            return;
        }

        GenerateOrder();
        SetMessage("Correct order! New customer order started.");
        UpdateUI();
    }

    public void RestartGame()
    {
        score = 0;
        timeLeft = startTime;
        gameOver = false;
        selectedShape = CookieShape.None;

        GenerateOrder();
        SetButtons(true);
        SetMessage("Pick the cookie shape that matches the customer order.");
        UpdateUI();
    }

    private void GenerateOrder()
    {
        int choice = Random.Range(0, 2);

        if (choice == 0)
        {
            currentOrder = CookieShape.Circle;
        }
        else
        {
            currentOrder = CookieShape.Star;
        }
    }

    private void WinGame()
    {
        gameOver = true;
        SetMessage("You Win! All customer orders completed.");
        SetButtons(false);
        UpdateUI();
    }

    private void LoseGame()
    {
        gameOver = true;
        SetMessage("Game Over. Time ran out.");
        SetButtons(false);
        UpdateUI();
    }

    private void SetButtons(bool active)
    {
        if (circleButton != null)
        {
            circleButton.interactable = active;
        }

        if (starButton != null)
        {
            starButton.interactable = active;
        }

        if (completeOrderButton != null)
        {
            completeOrderButton.interactable = active;
        }

        if (restartButton != null)
        {
            restartButton.interactable = true;
        }
    }

    private void SetMessage(string text)
    {
        if (messageText != null)
        {
            messageText.text = text;
        }
    }

    private void UpdateUI()
    {
        if (stateText != null)
        {
            stateText.text = "Selected: " + selectedShape;
        }

        if (orderText != null)
        {
            orderText.text = "Order: " + currentOrder + " Cookie";
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + " / " + targetScore;
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
        }
    }
}