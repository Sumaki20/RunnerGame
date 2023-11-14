using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    public static MainMenu inst;
    [SerializeField] GameObject settingAnim;
    [SerializeField] GameObject htpAnim;

    Animator buttonAnimator;
    Animator htpAnimator;
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Toggle toggleMute;

    [SerializeField] AudioSource buttonSound;
    [SerializeField] GameObject boardHTP;
    [SerializeField] GameObject boardSetting;

    private void Start()
    {
        buttonAnimator = settingAnim.GetComponent<Animator>();
        htpAnimator = htpAnim.GetComponent<Animator>();
        toggleMute.isOn = false;
    }
    public void PlayGame()
    {
        StartCoroutine(SoundCoroutine());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
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

    private void Awake()
    {
        inst = this;
    }
}
