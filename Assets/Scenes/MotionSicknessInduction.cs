using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSicknessInduction : MonoBehaviour
{
    public float positionIntensity = 0.5f; // Higher values make movement more erratic
    public float rotationIntensity = 5f;   // Affects the rotational sickness
    public float fovChangeSpeed = 1f;      // Speed of FOV oscillations
    public float fovIntensity = 15f;       // How much the FOV changes
    public float movementSpeed = 3f;       // Forward movement speed

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Camera cam;
    private float baseFOV;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
        cam = Camera.main;
        baseFOV = cam.fieldOfView;
    }

    void Update()
    {
        // Erratic positional movement (introducing randomness)
        float shakeX = Mathf.PerlinNoise(Time.time * 2f, 0) * positionIntensity - positionIntensity / 2;
        float shakeY = Mathf.PerlinNoise(0, Time.time * 2f) * positionIntensity - positionIntensity / 2;
        float shakeZ = Mathf.Sin(Time.time * 1.5f) * positionIntensity; // Slight forward-backward movement

        // Unpredictable rotation (tilting the camera)
        float rotX = Mathf.Sin(Time.time * 2f) * rotationIntensity;
        float rotY = Mathf.Cos(Time.time * 1.7f) * rotationIntensity;
        float rotZ = Mathf.Sin(Time.time * 2.3f) * rotationIntensity;

        // FOV distortion effect
        cam.fieldOfView = baseFOV + Mathf.Sin(Time.time * fovChangeSpeed) * fovIntensity;

        // Apply transformations
        transform.localPosition = originalPos + new Vector3(shakeX, shakeY, shakeZ);
        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);

        // Automatic forward movement
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
}
