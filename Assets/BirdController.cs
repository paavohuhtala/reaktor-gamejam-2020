using UnityEngine;

public class BirdController : MonoBehaviour
{
    AudioSource flapAudio;
    AudioSource pickupFruitAudio;
    private readonly Vector3 Left = new Vector3(0, 0, 1);
    private readonly Vector3 Right = new Vector3(0, 0, -1);


    public float HorizontalSpeed = 200.0f;
    public float HorizontalTurnMultiplier = 3.0f;
    public float FlapForce = 75.0f;
    public float FlapCooldown = 0.3f;
    public float GlideToFlapTransition = 0.2f;

    public float AntigravityMultiplier = -0.7f;
    public float GlideMaxVerticalVelocity = 2.5f;

    public GameManager manager;

    private float timeOfLastFlap = 0.0f;
    private float timeOfLastGlide = 0.0f;
    public Transform bodyTransform;

    private void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        flapAudio = audioSources[0];
        pickupFruitAudio = audioSources[1];
    }

    public void FixedUpdate()
    {
        if (manager.state == GameState.Gameover) return;

        var body = GetComponent<Rigidbody>();

        var timeSinceLastFlap = Time.timeSinceLevelLoad - timeOfLastFlap;
        var timeSinceLastGlide = Time.timeSinceLevelLoad - timeOfLastGlide;

        if (Input.GetKey(KeyCode.D))
        {
            if (body.velocity.y < 10)
            {
                // FlapForce "reloads" linearly during FlapCooldown
                var FlapForceFactor = Mathf.Min(1, (timeSinceLastFlap / FlapCooldown));
                body.AddForce(Vector3.up * FlapForce * FlapForceFactor);
                if (timeSinceLastFlap > 0.1f)
                {
                    flapAudio.Play();
                }
                timeOfLastFlap = Time.timeSinceLevelLoad;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            timeOfLastGlide = Time.timeSinceLevelLoad;
            body.AddForce(Physics.gravity * AntigravityMultiplier, ForceMode.Force);
            body.velocity = new Vector3(0, Mathf.Min(body.velocity.y, GlideMaxVerticalVelocity), body.velocity.z);
        }

        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bodyTransform.rotation = Quaternion.Euler(-90,180,0);
            if (body.velocity.z < 0) // Moving left
            {
                direction = Left * HorizontalTurnMultiplier;
            }
            else
            {
                direction += Left;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            bodyTransform.rotation = Quaternion.Euler(-90,0,0);
            if (body.velocity.z > 0) // Moving left
            {
                direction = Right * HorizontalTurnMultiplier;
            }
            else
            {
                direction += Right;
            }
        } 


        if (direction.magnitude > 0.001)
        {
            body.AddForce(direction * HorizontalSpeed, ForceMode.Force);
        }
    }

//
//    void OnCollisionEnter(Collision collision)
//    {
//        if(collision.gameObject.tag == "Fruit")
//        {
//            Destroy(collision.gameObject);
//        }
//    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Fruit")
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            manager.AddScore(10);

            pickupFruitAudio.Play();
        }
    }
}
