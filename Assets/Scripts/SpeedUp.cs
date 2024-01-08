using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] float turnSpeed = 45f;
    public float driveSpeed = 15f;
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
        //playerMovement.DriveSpeed(driveSpeed);
        
        // Destroy this coin object
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }
}
