using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ManageShop : MonoBehaviour
{
    //public static ManageShop inst;
    GameData saveData = new GameData();

    [Header("Stat")]
    private int[] levelRequired = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 };
    public int level;
    public int coin;
    [SerializeField] Text coinText;
    public int potion;
    public int manaPotion;

    [Header("HpStat")]
    private int maxHealth;
    public int hpCostUp = 500;
    public int timeUpHP;
    [SerializeField] Text amountHPText;
    [SerializeField] Text priceStatText;
    [SerializeField] Text healthNeedlevel;
    [SerializeField] GameObject notEnough;

    [Header("Switch")]
    [SerializeField] GameObject switchWinUpgrade;
    [SerializeField] GameObject switchUpgrade;
    [SerializeField] GameObject switchWinShop;
    [SerializeField] GameObject switchShop;


    private void Awake()
    {
    }
    void Start()
    {
        SaveSystem.instance.CreateGame(saveData);
        LoadPlayer();
        CheckBuystat();
        CountTimes();
    }

    
    void Update()
    {
        addCoin();
        coinText.text = coin.ToString();
        priceStatText.text = hpCostUp + " G";
        amountHPText.text = maxHealth + " HP";
        healthNeedlevel.text = "Need level " + (level + 1);
    }
    public void addCoin()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            coin += 1000;
            SavePlayer();
            Debug.Log("coin " + saveData.coin);
        }
    }
    public void buyPotionHP()
    {
        if (coin >= 100)
        {
            coin -= 100;
            potion++;
            SavePlayer();
            LoadPlayer();
        }
    }
    public void buyPotionMP()
    {
        if (coin >= 100)
        {
            coin -= 100;
            manaPotion++;
            SavePlayer();
            LoadPlayer();
        }
    }
    public void SavePlayer()
    {
        saveData.coin = coin;
        saveData.potion = potion;
        saveData.manaPotion = manaPotion;
        saveData.maxHealth = maxHealth;
        saveData.timeUpHP = timeUpHP;
        SaveSystem.instance.SaveGame(saveData);
    }
    public void LoadPlayer()
    {
        saveData = SaveSystem.instance.LoadGame();
        coin = saveData.coin;
        potion = saveData.potion;
        manaPotion = saveData.manaPotion;
        level = saveData.level;
        maxHealth = saveData.maxHealth;
        timeUpHP = saveData.timeUpHP;
        Debug.Log("coin " + saveData.coin + "potion " + saveData.potion + "level " + saveData.level + "MaxHealth " + saveData.maxHealth);
    }
    public void SwitchUpgrade()
    {
        StartCoroutine(SoundButton());
        switchWinUpgrade.SetActive(true);
        switchUpgrade.SetActive(false);
        switchWinShop.SetActive(false);
        switchShop.SetActive(true);
    }
    public void SwitchShop()
    {
        StartCoroutine(SoundButton());
        switchWinUpgrade.SetActive(false);
        switchUpgrade.SetActive(true);
        switchWinShop.SetActive(true);
        switchShop.SetActive(false);
    }
    public void buyHpStat()
    {
        if (coin > hpCostUp)
        {
            coin -= hpCostUp;
            maxHealth += 5;
            timeUpHP++;
            hpCostUp = (int)(hpCostUp * 1.20f);
            SavePlayer();
            CheckBuystat();
        }
    }
    public void CheckBuystat()
    {
        if (level >= 10)
        {
            notEnough.SetActive(true);
            healthNeedlevel.text = "Max level ";
        }
        if (timeUpHP < levelRequired[level - 1])
        {
            notEnough.SetActive(false);
            Debug.Log("Can Upgrade");
        }
        else
        {
            notEnough.SetActive(true);
            Debug.Log("Can not Upgrade");
        }
    }
    
    public void CountTimes()
    {
        for (int i = 0; i < timeUpHP; i++)
        {
            hpCostUp = (int)(hpCostUp * 1.20f);
        }
    }
    private IEnumerator SoundButton()
    {
        //buttonSound.Play();
        yield return new WaitForSeconds(0.75f);
    }
}
