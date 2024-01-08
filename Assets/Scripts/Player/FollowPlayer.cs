using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Camera cameraA;
    [SerializeField] bool dynamicFOV;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    // Update is called once per frame
    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void Update()
    {
        Vector3 targetPos = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        targetPos.x = 0;
        transform.position = smoothedPosition;
    }

    public void DynamicFOV()
    {
        if (Input.GetKey(KeyCode.W) && dynamicFOV == true)
        {
            cameraA.fieldOfView = Mathf.Lerp(cameraA.fieldOfView, 70, 10f * Time.deltaTime);

        }
        else
        {
            cameraA.fieldOfView = Mathf.Lerp(cameraA.fieldOfView, 60, 10f * Time.deltaTime);
        }
    }
}
