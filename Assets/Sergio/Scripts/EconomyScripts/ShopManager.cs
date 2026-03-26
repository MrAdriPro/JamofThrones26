using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{   
    public static ShopManager shopInstance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI costText;

    public float actualCoins = 0f;
    public float upgradeCost = 100f;
    public float costMult = 2.5f;

    public int tekLevel = 0;

    [SerializeField] PlayerModelController playerModel;


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
        UpdateText();
    }

    public void GetCoin(float coins)
    {
        actualCoins += coins;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = "Coins: " + actualCoins.ToString("F0"); 
        costText.text = upgradeCost.ToString("F0");
    }

    public void BuyUpgrade()
    {
        if(upgradeCost <= actualCoins) 
        {
            playerModel.SwapModel();
            // towerModel.UpgradeAll();

            actualCoins -= upgradeCost;
            upgradeCost *= costMult;

            tekLevel++;
        }

        UpdateText();
    }
}
