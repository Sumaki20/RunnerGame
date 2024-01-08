using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageEnemy : MonoBehaviour
{
    public static ManageEnemy inst;

    [Header("Common Enemy")]
    public int[] commonHP = { 10,15,20,25,30};

    private void Awake()
    {
        inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
