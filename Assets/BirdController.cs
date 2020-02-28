using UnityEngine;

public class BirdController : MonoBehaviour
{
    private readonly Vector3 Left = new Vector3(0, 0, 1);
    private readonly Vector3 Right = new Vector3(0, 0, -1);

    public float HorizontalSpeed = 10.0f;
    public float FlapForce = 200.0f;
    public float FlapSpaceDelay = 0.5f;
    public float FlapCooldown = 0.5f;

    public float AntigravityMultiplier = -0.7f;
    public float GlideMaxVerticalVelocity = 2.5f;

    private float timeOfLastFlap = 0.0f;

    private void Start()
    {
        
    }

    public void FixedUpdate()
    {
        var body = GetComponent<Rigidbody>();

        var timeSinceLastFlap = Time.timeSinceLevelLoad - timeOfLastFlap;

        if (timeSinceLastFlap > FlapCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector3.up * FlapForce);
            timeOfLastFlap = Time.timeSinceLevelLoad;
        }

        if (timeSinceLastFlap > FlapSpaceDelay && Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Physics.gravity * AntigravityMultiplier, ForceMode.Force);
            body.velocity = new Vector3(0, Mathf.Min(body.velocity.y, GlideMaxVerticalVelocity), body.velocity.z);
        }

        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Right;
        }

        if (direction.magnitude > 0.001)
        {
            body.AddForce(direction * HorizontalSpeed, ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Fruit")
        {
            Destroy(collision.gameObject);
        }
    }
}
