using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderDelivery : MonoBehaviour
{
    [Header("Order Settings")]
    public int goalCount = 12;

    [Header("Timer")]
    public float totalTime = 120f;
    private float timeRemaining;
    private bool timerRunning = true;
    private bool orderComplete = false;
    private bool timeUp = false;

    [Header("UI — auto-created if left empty")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI goalText;

    [Header("Feedback Timing")]
    public float messageDuration = 2.5f;
    private float messageTimer = 0f;

    private Canvas canvas;

    private void Start()
    {
        timeRemaining = totalTime;

        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("OrderCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        if (timerText == null)
            timerText = CreateUIText("TimerText", new Vector2(0.5f, 1f), new Vector2(0f, -40f), 28, TextAlignmentOptions.Center);

        if (goalText == null)
            goalText = CreateUIText("GoalText", new Vector2(0.5f, 1f), new Vector2(0f, -80f), 24, TextAlignmentOptions.Center);

        if (feedbackText == null)
            feedbackText = CreateUIText("FeedbackText", new Vector2(0.5f, 1f), new Vector2(0f, -120f), 24, TextAlignmentOptions.Center);

        feedbackText.text = "";
        goalText.text = $"Order: {goalCount} cookies";

        // Move CookieCounter to top-right
        GameObject cookieCounter = GameObject.Find("CookieCounter");
        if (cookieCounter != null)
        {
            RectTransform rt = cookieCounter.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(1f, 1f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.anchoredPosition = new Vector2(-120f, -40f);
            }
        }
    }

    private TextMeshProUGUI CreateUIText(string name, Vector2 anchor, Vector2 offset, int fontSize, TextAlignmentOptions alignment)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(canvas.transform, false);

        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = offset;
        rect.sizeDelta = new Vector2(400f, 40f);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.color = Color.white;

        return tmp;
    }

    private void Update()
    {
        if (orderComplete || timeUp) return;

        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerRunning = false;
                OnTimeUp();
                return;
            }
        }

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
                feedbackText.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (orderComplete || timeUp) return;
        if (!other.CompareTag("Player")) return;

        int collected = PlayerCookies.instance.cookies;

        if (collected >= goalCount)
        {
            PlayerCookies.instance.cookies -= goalCount;
            OrderSuccess();
        }
        else
        {
            ShowFeedback("We need more cookies!");
        }
    }

    private void OrderSuccess()
    {
        orderComplete = true;
        timerRunning = false;

        ShowFeedback("Order Complete!");
        goalText.text = "";

        Time.timeScale = 0f;
    }

    private void OnTimeUp()
    {
        timeUp = true;
        timerText.text = "Time: 00:00";

        ShowFeedback("Time's Up!");

        Time.timeScale = 0f;
    }

    private void ShowFeedback(string msg)
    {
        feedbackText.text = msg;
        messageTimer = messageDuration;
    }
}