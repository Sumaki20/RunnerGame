using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTest : MonoBehaviour
{
    public int level = 1;
    private int coin = 100000;
    private int price = 500;
    public int timeup;
    // Start is called before the first frame update
    void Start()
    {
        //CountTimes();
        Physics.IgnoreLayerCollision(3, 2,false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            buyHpStat();
        }
            if (Input.GetKeyDown(KeyCode.B)) 
        {
            buyItem();
            Debug.Log("coin =" + coin);
            Debug.Log("price =" + price);
            Debug.Log("timeup =" + timeup);
        }
    }
    private void buyItem()
    {
        if (coin > price)
        {
            timeup++;
            price = (int)(price * 1.15f);
            //Debug.Log(price);

        }
    }
    public void CountTimes()
    {
        for (int i = 0; i < timeup; i++)
        {
            price = (int)(price * 1.07f);
        }
    }
    public int[] levelRequired = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 };
    public void buyHpStat()
    {
        if (timeup < levelRequired[level - 1])
        {
            Debug.Log("Can Upgrade");
            
        }
        else
        {
            Debug.Log("Can not Upgrade");
        }
    }
    
}
