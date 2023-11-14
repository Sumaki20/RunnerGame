using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WallPush : MonoBehaviour
{
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

        if (Physics.Raycast(rayRight, out hit, 0.2f))
        {
            playerMovement.rb.AddForce(Vector3.right * 1600f);
        }
        if (Physics.Raycast(rayLeft, out hit, 0.2f))
        {
            playerMovement.rb.AddForce(-Vector3.right * 1600f);
        }
    }
}
