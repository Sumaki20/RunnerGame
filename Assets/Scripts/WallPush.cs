using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPush : MonoBehaviour
{
    [SerializeField] float range = 0.3f;
    PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray rayLeft = new Ray(transform.position, -transform.right);
        Ray rayRight = new Ray(transform.position, transform.right);

        if (Physics.Raycast(rayRight, out hit, range))
        {
            playerMovement.rb.AddForce(Vector3.right * 1600f);
        }
        else if (Physics.Raycast(rayLeft, out hit, range))
        {
            playerMovement.rb.AddForce(-Vector3.right * 1600f);
        }
    }
}
