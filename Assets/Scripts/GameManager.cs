using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    GameData saveData = new GameData();

    public int score;
    public int coin;
    public int potion;
    public int manaPotion;
    public int combo;

    [Header("StatXP")]
    public int level = 1;
    public float currentXp;
    public float requiredXp;

    private float lerpTimer;
    private float delayTimer;
    [Header("UI XP")]
    public Slider sliderXp;
    public Slider backSliderXp;
    [SerializeField] Text levelUI;
    [SerializeField] private ParticleSystem vfxLevelUp;
    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    Animator tryAgainAnima;
    Animator comboAnim;
    bool checkPause = true;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    [SerializeField] private AudioSource buttonSound;
    [SerializeField] public AudioSource soundBGM;
    [SerializeField] Text potionText;
    [SerializeField] Text manaPotionText;
    [SerializeField] Text scoreText;
    [SerializeField] Text coinText;
    [SerializeField] Text comboText;
    [SerializeField] Text sumScore;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject dashUI;
    [SerializeField] GameObject animUI;
    [SerializeField] GameObject comboUI;

    [SerializeField] PlayerMovement playerMovement;


    private void Awake()
    {
        inst = this;
        /*if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/

    }
    private void Start()
    {
        LoadPlayer();
        requiredXp = CalculateRequiredXp();
        sliderXp.value = currentXp / requiredXp;
        backSliderXp.value = currentXp / requiredXp;
        comboAnim = comboUI.GetComponent<Animator>();
        tryAgainAnima = animUI.GetComponent<Animator>();
        checkPause = false;
    }

    void Update()
    {
        //addCoin();
        scoreText.text = "SCORE: " + score;
        sumScore.text = score.ToString();
        coinText.text = coin.ToString();
        potionText.text = potion.ToString();
        manaPotionText.text = manaPotion.ToString();
        comboText.text = "Combo " + combo;
        levelUI.text = "Lv." + level;
        UpdateXpUI();
        if (Input.GetKeyDown(KeyCode.C))
        {
            GainExperienceFlatRate(20);
        }
        if (currentXp >= requiredXp)
        {
            LevelUp();
        }

        if (!checkPause)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }
    /*public void addCoin()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            coin++;
            SavePlayer();
            LoadPlayer();
            Debug.Log("coin " + saveData.coin);
        }
    }*/
    public void SavePlayer()
    {
        saveData.coin = coin;
        saveData.potion = potion;
        saveData.manaPotion = manaPotion;
        saveData.level = level;
        saveData.currentXp = currentXp;

        SaveSystem.instance.SaveGame(saveData);
        //Debug.Log("coin" + saveData.coin + "potion " + saveData.potion);
        Debug.Log("save level " + saveData.level + "save currentXp " + saveData.currentXp);
    }
    public void LoadPlayer()
    {
        saveData = SaveSystem.instance.LoadGame();
        coin = saveData.coin;
        potion = saveData.potion;
        manaPotion = saveData.manaPotion;
        level = saveData.level;
        currentXp = saveData.currentXp;
        //Debug.Log("coin" +  saveData.coin + "potion " + saveData.potion);
        Debug.Log("load level " + saveData.level + "load currentXp " + saveData.currentXp);
    }
    public void UpdateXpUI()
    {
        float xpFraction = currentXp / requiredXp;
        float FXP = sliderXp.value;
        if (FXP < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backSliderXp.value = xpFraction;
            if (delayTimer > 1)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer;
                sliderXp.value = Mathf.Lerp(FXP, backSliderXp.value, percentComplete);
            }
        }
    }
    public void GainExperienceFlatRate(float xpGained)
    {
        currentXp += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
        ///SavePlayer();
    }
    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp *= xpGained * multiplier;
        }
        else
        {
            currentXp += xpGained;
        }
        lerpTimer = 0f;
        delayTimer = 0f;
    }
    public void LevelUp()
    {
        level++;
        sliderXp.value = 0f;
        backSliderXp.value = 0f;
        currentXp = Mathf.RoundToInt(currentXp - requiredXp);
        requiredXp = CalculateRequiredXp();
        vfxLevelUp.Play();
    }
    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
    public void TryAginButton()
    {
        bool isStart = tryAgainAnima.GetBool("TryAgin");
        if (isStart == true)
        {
            tryAgainAnima.SetBool("TryAgin", false);
        }
        else
        {
            tryAgainAnima.SetBool("TryAgin", true);
        }
    }
    public void OpenPauseMenu()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        checkPause = true;
        soundBGM.Pause();
    }
    public void ClosePauseMenu()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        checkPause = false;
        soundBGM.Play();
    }
    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        checkPause = false;
    }
    public void TitleManu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void IncrementScore()
    {
        score++;
        coin++;

        // Increase the player's speed
        //playerMovement.speed += playerMovement.speedIncreasePerPoint * score;
        //return playerMovement.speed;
    }
    public void ComboSystem()
    {
        combo++;
        comboAnim.SetTrigger("Combo");
    }
    public void EnemyScore()
    {
        score += 5;
        coin += 2;
    }

    public void ScoreBoard()
    {
        Time.timeScale = 0f;
        scoreBoard.SetActive(true);
        SavePlayer();
    }

    public void DashUI()
    {
        dashUI.SetActive(false);
    }

    public void Restart()
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(RestartCoroutine());
    }
    IEnumerator RestartCoroutine()
    {
        buttonSound.Play();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}