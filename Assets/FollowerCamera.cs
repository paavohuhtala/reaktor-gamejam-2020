using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    public Transform Target;
    public float Distance = -25f;

    private Vector3 velocity;

    void Update()
    {
        var desiredPosition = Target.position + new Vector3(Distance, 2, 0);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
    }
}
