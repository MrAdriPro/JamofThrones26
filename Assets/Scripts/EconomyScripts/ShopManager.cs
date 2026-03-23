using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{   
    public static ShopManager shopInstance;
    [SerializeField] private TextMeshProUGUI coinText;
    public float actualCoins = 0f;

    private void Awake()
    {
        if (shopInstance == null)
        {
            shopInstance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateCoinText();
    }

    public void GetCoin(float coins)
    {
        actualCoins += coins;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        coinText.text = "Coins: " + actualCoins.ToString("F0"); 
    }
}
