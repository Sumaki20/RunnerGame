using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu inst;
    
    [Header("WhereAnim")]
    [SerializeField] GameObject upAnim;
    [SerializeField] GameObject settingAnim;
    [SerializeField] GameObject htpAnim;
    //[SerializeField] GameObject allAnim;
    [Header("Animation")]
    Animator buttonAnimator;
    Animator htpAnimator;
    Animator buttonStart;
    Animator buttonHTP;
    Animator upButton;
    Animator buttonSetting;
    Animator buttonQuit;
    Animator UpGradeAnimator;

    [Header("AudioSetting")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Slider soundSlider;
    public Slider sfxSlider;
    public Toggle toggleMute;
    //[SerializeField] private Slider musicSlider;
    [Header("Audio")]
    [SerializeField] AudioSource buttonSound;
    [SerializeField] AudioSource pressButtonSound;
    [Header("Panel")]
    [SerializeField] GameObject boardHTP;
    [SerializeField] GameObject boardUpgrade;
    [SerializeField] GameObject boardSetting;
    

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
        //buttonQuit = GameObject.Find("Quit").GetComponent<Animator>();
        buttonSetting = GameObject.Find("Setting").GetComponent<Animator>();
        buttonHTP = GameObject.Find("HowToPlay").GetComponent<Animator>();
        upButton = GameObject.Find("Upgrade").GetComponent<Animator>();
        buttonStart = GameObject.Find("Start").GetComponent<Animator>();
        buttonAnimator = settingAnim.GetComponent<Animator>();
        htpAnimator = htpAnim.GetComponent<Animator>();
        UpGradeAnimator = upAnim.GetComponent<Animator>();
        toggleMute.isOn = false;
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
            SetSound();
            SetSFX();
        }
        
    }
    private void Update()
    {
        
    }
    #region AnimMainUI
    public void StartButton()
    {
        bool isStart = buttonStart.GetBool("Start");
        if (isStart == true)
        {
            buttonStart.SetBool("Start", false);
        }
        else
        {
            buttonStart.SetBool("Start", true);
        }
    }
    public void UpButton()
    {
        bool isStart = upButton.GetBool("UpButton");
        if (isStart == true)
        {
            upButton.SetBool("UpButton", false);
        }
        else
        {
            upButton.SetBool("UpButton", true);
        }
    }
    public void HTPButton()
    {
        bool isStart = buttonHTP.GetBool("HTP");
        if (isStart == true)
        {
            buttonHTP.SetBool("HTP", false);
        }
        else
        {
            buttonHTP.SetBool("HTP", true);
        }
    }
    public void SettingButton()
    {
        bool isStart = buttonSetting.GetBool("Setting");
        if (isStart == true)
        {
            buttonSetting.SetBool("Setting", false);
        }
        else
        {
            buttonSetting.SetBool("Setting", true);
        }
    }
    public void QuitButton()
    {
        bool isStart = buttonQuit.GetBool("Quit");
        if (isStart == true)
        {
            buttonQuit.SetBool("Quit", false);
        }
        else
        {
            buttonQuit.SetBool("Quit", true);
        }
    }
    #endregion
    #region FunctionButton
    public void PressSound()
    {
        pressButtonSound.Play();
    }
    public void PlayGame()
    {
        StartCoroutine(SoundCoroutine());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void UpgradeOn()
    {
        StartCoroutine(SoundCoroutine());
        boardUpgrade.SetActive(true);
        UpGradeAnimator.SetTrigger("OpenUpGrade");
    }
    public void UpgradeOff()
    {
        boardUpgrade.SetActive(false);
    }
    public void HowToPlayOff()
    {
        boardHTP.SetActive(false);
    }
    
    public void HowToPlayOn()
    {
        StartCoroutine(SoundCoroutine());
        boardHTP.SetActive(true);
        htpAnimator.SetTrigger("OpenHTP");
    }
    public void SettingMenuOff()
    {

        boardSetting.SetActive(false);
    }
    public void SettingMenuOn()
    {
        
        StartCoroutine(SoundCoroutine());
        boardSetting.SetActive(true);
        buttonAnimator.SetTrigger("OpenSetting");
    }
    public void QuitGame ()
    {
        StartCoroutine(SoundCoroutine());
        Debug.Log("QUIT");
        Application.Quit();
    }
    IEnumerator SoundCoroutine()
    {
        buttonSound.Play();
        yield return new WaitForSeconds(0.75f);
    }
    #endregion
    #region SettingFunction
    public void SetVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetSound()
    {
        float volume = soundSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFX()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        soundSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetVolume();
        SetSound();
        SetSFX();
    }
    public void ToggleVolume(bool isOn)
    {

        if (isOn)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
        
    }
    #endregion
}
