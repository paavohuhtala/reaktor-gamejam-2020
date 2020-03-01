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

    public float PoopToFruitPercentage = 0.15f;

    public float AntigravityMultiplier = -0.7f;
    public float GlideMaxVerticalVelocity = 2.5f;

    public GameManager manager;

    private float timeOfLastFlap = 0.0f;
    private float timeOfLastGlide = 0.0f;

    public Transform bodyTransform;
    public Vector3 originalBodyScale;

    public Transform wingTransform;

    public float wingUpAngle = 40;
    public float wingDefaultAngle = 0;
    public float wingDownAngle = -45;

    public AudioClip EatSound;

    public GameObject PoopPrefab;

    private void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        flapAudio = audioSources[0];
        pickupFruitAudio = audioSources[1];
        originalBodyScale = bodyTransform.localScale;
    }

    public void Update()
    {
        var angle = Mathf.Sin(Time.timeSinceLevelLoad * 10f) * 30f;
        wingTransform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }

    public void FixedUpdate()
    {
        if (manager.state == GameState.Gameover) return;

        var body = GetComponent<Rigidbody>();

        var timeSinceLastFlap = Time.timeSinceLevelLoad - timeOfLastFlap;
        var timeSinceLastGlide = Time.timeSinceLevelLoad - timeOfLastGlide;

        if (Input.GetKey(KeyCode.Space))
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

        if (Input.GetKey(KeyCode.G))
        {
            timeOfLastGlide = Time.timeSinceLevelLoad;
            body.AddForce(Physics.gravity * AntigravityMultiplier, ForceMode.Force);
            body.velocity = new Vector3(0, Mathf.Min(body.velocity.y, GlideMaxVerticalVelocity), body.velocity.z);
        }

        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bodyTransform.localScale = new Vector3(originalBodyScale.x, -originalBodyScale.y, originalBodyScale.z);
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
            bodyTransform.localScale = originalBodyScale;
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

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Fruit")
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            manager.AddScore(10);
            // pickupFruitAudio.Play();
            pickupFruitAudio.pitch = Random.Range(0.85f, 1.25f);
            pickupFruitAudio.PlayOneShot(EatSound);

            
            if (Random.Range(0.0f, 1.0f) < PoopToFruitPercentage) 
            {
                var body = GetComponent<Rigidbody>();
                var poopPosition3d = new Vector3(body.position.x, body.position.y - 1.3f, body.position.z);
                GameObject poop = (GameObject)Instantiate(PoopPrefab, poopPosition3d, PoopPrefab.transform.rotation);
                poop.GetComponent<Rigidbody>().velocity = body.velocity;
            }
        }
    }
}
