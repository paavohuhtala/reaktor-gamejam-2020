using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public Transform centerOfWorld;
    public float startAngle;
    public float endAngle;
    public float distance;

    public GameManager manager;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.state == GameState.Running)
        {
            float relativeTime = manager.currentTimeOfDay / manager.lengthOfDay;
            var axis = new Vector3(1, 0, 0);
            var totalAngle = endAngle - startAngle;

            var anglePerSecond = totalAngle / manager.lengthOfDay;

            transform.RotateAround(centerOfWorld.position, axis, -anglePerSecond * Time.deltaTime);
        }
    }
}
