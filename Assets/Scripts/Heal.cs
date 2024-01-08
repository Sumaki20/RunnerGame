using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] float turnSpeed = 45f;
    public int healing;
    //Health healthBar;
    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check that the object we collided with is the player
        if (other.gameObject.name != "Player")
        {
            return;
        }
        playerMovement.Healing(healing);
        
        // Destroy this coin object
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}
