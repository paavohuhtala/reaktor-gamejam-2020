using UnityEngine;

public class BirdSplash : MonoBehaviour
{
    public Transform wingTransform;
    public Transform birdTransform;

    private Vector3 birdOriginalPosition;

    private void Start()
    {
        birdOriginalPosition = birdTransform.position;
    }

    public void Update()
    {
        var angle = Mathf.Sin(Time.timeSinceLevelLoad * 10f) * 30f;
        wingTransform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));

        birdTransform.position = birdOriginalPosition + new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad * 2f) * 2.3f, 0);
    }
}
