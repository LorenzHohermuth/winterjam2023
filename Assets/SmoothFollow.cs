using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; 
    public float smoothTime = 0.3f;
    public Vector3 offset = new Vector3(0, 5, -10);

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
