using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coin = 0;
    public int potion = 0;
    public int manaPotion = 0;

    public int level = 1;
    public float currentXp;

    public int maxHealth = 100;
    public int timeUpHP = 0;
    public void buyPotion(int points, int Item)
    {
        coin += points;
        potion += Item;
    }
    public void buyManaPotion(int points, int Item)
    {
        coin += points;
        manaPotion += Item;
    }
    public void AddScore(int points)
    {
        coin += points;
    }
}