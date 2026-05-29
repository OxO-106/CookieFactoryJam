using TMPro;
using UnityEngine;

public class PlayerCookies : MonoBehaviour
{
    public static PlayerCookies instance;

    public int cookies = 0;

    public TextMeshProUGUI cookieCounter;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCookie(int amount)
    {
        cookies += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        cookieCounter.text = "Cookies: " + cookies;
    }
}