using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int level = 1;
    public float currentXp = 0;
    public float requiredXp = 0;

    /*private float lerpTimer;
    private float delayTimer;*/
    [Header("UI")]
    public Slider sliderXp;
    public Slider backSliderXp;
    [SerializeField] Text levelUI;
    [Header("Multipliers")]
    [Range(1f,300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    private void Awake()
    {
        //LoadPlayer();
    }
    void Start()
    {
        requiredXp = CalculateRequiredXp();
        sliderXp.maxValue = requiredXp;
        sliderXp.value = currentXp;
        backSliderXp.maxValue = requiredXp;
        backSliderXp.value = currentXp;

    }

    // Update is called once per frame
    void Update()
    {
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
    }
    private float lerpTimer;
    private float delayTimer = 2f;
    public void UpdateXpUI()
    {
        float xpFraction = backSliderXp.value = currentXp;
        float FXP = sliderXp.value;
        if (FXP < xpFraction) 
        {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / delayTimer;
            sliderXp.value = Mathf.Lerp(FXP, xpFraction, percentComplete);
            /*delayTimer += Time.deltaTime;
            if (delayTimer > 1)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer;
                sliderXp.value = Mathf.Lerp(xpFraction, FXP, percentComplete);
            }*/
        }
    }
    public void GainExperienceFlatRate(float xpGained)
    {
        currentXp += xpGained;
        lerpTimer = 0f;
        delayTimer = 0f;
        backSliderXp.value = currentXp;
        //sliderXp.value = currentXp;
        //SavePlayer();
        //GameManager.inst.SavePlayer();
    }
    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < level)
        {
            float multiplier = 1+ (level - passedLevel) * 0.1f;
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
        sliderXp.maxValue = requiredXp;
        backSliderXp.maxValue = requiredXp;
    }
    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle =1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }
}
