using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class ShopManager : MonoBehaviour
{
    public static ShopManager shopInstance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("Carga del Botón")]
    [SerializeField] private Image fillImage; 
    [SerializeField] private float upgradeDuration = 2.0f; 
    private float currentFillTimer = 0f;
    private bool isPressing = false;

    [Header("Economía")]
    public float actualCoins = 0f;
    public float upgradeCost = 100f;
    public float costMult = 2.5f;

    public int tekLevel = 0;

    [SerializeField] PlayerModelController playerModel;

    private void Awake()
    {
        if (shopInstance == null) shopInstance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateText();
        if (fillImage != null) fillImage.fillAmount = 0;
    }

    void Update()
    {
        if (isPressing && playerModel.CanEvolve && actualCoins >= upgradeCost)
        {
            currentFillTimer += Time.deltaTime;
            fillImage.fillAmount = currentFillTimer / upgradeDuration;

            if (currentFillTimer >= upgradeDuration)
            {
                BuyUpgrade();
                ResetFill(); 
            }
        }
        else
        {
            if (currentFillTimer > 0)
            {
                currentFillTimer -= Time.deltaTime * 2; 
                fillImage.fillAmount = currentFillTimer / upgradeDuration;
            }
        }
    }

    public void OnPointerDown() { isPressing = true; }
    public void OnPointerUp() { isPressing = false; }

    private void ResetFill()
    {
        isPressing = false;
        currentFillTimer = 0f;
        fillImage.fillAmount = 0f;
    }

    public void BuyUpgrade()
    {
        playerModel.SwapModel();
        towerModel.UpgradeModels();

        actualCoins -= upgradeCost;
        upgradeCost *= costMult;

        UpdateText();

        if (!playerModel.CanEvolve)
        {
            costText.text = "MAX";
            fillImage.fillAmount = 0; 
        }
    }

    void UpdateText()
    {
        coinText.text = "Coins: " + actualCoins.ToString("F0");
        costText.text = upgradeCost.ToString("F0");
    }

    public void GetCoin(float coins)
    {
        actualCoins += coins;
        UpdateText();
    }
    public bool TrySpendMoney(float amount)
    {
        if (actualCoins >= amount)
        {
<<<<<<< HEAD
            playerModel.SwapModel();
            // towerModel.UpgradeAll();

            actualCoins -= upgradeCost;
            upgradeCost *= costMult;

            tekLevel++;
=======
            actualCoins -= amount;
            UpdateText();
            return true;
>>>>>>> origin/newMain
        }
        return false;
    }
}