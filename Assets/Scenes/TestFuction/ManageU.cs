using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageU : MonoBehaviour
{
    GameData saveData = new GameData();

    public int coin;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            coin++;
            //saveData.AddScore(1);
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            coin--;
            //saveData.AddScore(-1);
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayer();
            //SaveSystem.instance.SaveGame(saveData);
            Debug.Log("Saved data.");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayer();
            //saveData = SaveSystem.instance.LoadGame();
            Debug.Log("Loaded data.");
            PrintScore();
        }
    }
    public void SavePlayer()
    {
        saveData.coin = coin;
        SaveSystem.instance.SaveGame(saveData);
        Debug.Log("coin" + saveData.coin + "potion " + saveData.potion);
    }
    public void LoadPlayer()
    {
        saveData = SaveSystem.instance.LoadGame();
        coin = saveData.coin;
        Debug.Log("coin" + saveData.coin + "potion " + saveData.potion);
    }
    void PrintScore()
    {
        Debug.Log("The current score is " + saveData.coin);
    }
}
