using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
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
}
