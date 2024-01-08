using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] float turnSpeed = 45f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // Check that the object we collided with is the player
        if (other.gameObject.name != "Player")
        {
            return;
        }
        // Add to the player's score
        playerMovement.PickCoin();

        // Destroy this coin object
        Destroy(gameObject);
    }
    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}