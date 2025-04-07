using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFOV : MonoBehaviour
{
    public float baseFOV = 60f;
    public float maxFOV = 90f;
    public float speedMultiplier = 2f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float speed = GetComponentInParent<PlayerMovement1>().speed;
        cam.fieldOfView = Mathf.Lerp(baseFOV, maxFOV, speed / speedMultiplier);
    }
}

