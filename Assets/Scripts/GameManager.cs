using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score;

    public static GameManager inst;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    [SerializeField] private AudioSource buttonSound;
    [SerializeField] Text scoreText;
    [SerializeField] Text sumScore;
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject dashUI;

    [SerializeField] PlayerMovement playerMovement;

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    public void IncrementScore()
    {
        score++;

        // Increase the player's speed
        playerMovement.speed += playerMovement.speedIncreasePerPoint;
    }
    public void EnemyScore()
    {
        score += 10;

    }

    public void ScoreBoard()
    {
        scoreBoard.SetActive(true);
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
    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        scoreText.text = "SCORE: " + score;
        sumScore.text = score.ToString();

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }
}