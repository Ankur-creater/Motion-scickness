using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 5f; // Base movement speed
    public float acceleration = 0.1f; // Speed increases over time
    public Transform pillarPrefab; // Assign the pillar prefab in the inspector
    public float spawnDistance = 10f; // Distance between spawned pillars
    public int maxPillars = 10; // Number of active pillars in scene
    public Camera playerCamera; // Assign the main camera
    public GameObject ssqPanel; // Assign the SSQ Panel in Inspector

    private List<Transform> pillars = new List<Transform>(); // Keep track of spawned pillars
    private Vector3 startPosition; // Store the starting position
    private Quaternion startRotation; // Store the starting rotation
    private bool isRunning = true; // Controls movement

    private float lastSpawnZ;

    private float bobbingAmount = 0.1f; // Head bobbing intensity
    private float bobbingSpeed = 2f; // Head bobbing speed
    private float tiltAmount = 1f; // Camera tilt intensity

    void Start()
    {
        // Store the starting position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        lastSpawnZ = transform.position.z;

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (ssqPanel != null)
            ssqPanel.SetActive(false); // Hide SSQ panel initially

        for (int i = 0; i < maxPillars; i++)
        {
            SpawnPillar(lastSpawnZ + (i * spawnDistance));
        }

        StartCoroutine(StopAndShowSurvey());
    }

    void Update()
    {
        if (!isRunning) return; // Stop movement when SSQ starts

        // Increase speed over time
        speed += acceleration * Time.deltaTime;

        // Move forward
        transform.position += transform.forward * speed * Time.deltaTime;

        // Spawn new pillars
        if (transform.position.z >= lastSpawnZ - spawnDistance)
        {
            lastSpawnZ += spawnDistance;
            SpawnPillar(lastSpawnZ);
        }

        // Apply motion sickness effects
        ApplyHeadBobbing();
        ApplyCameraTilt();
        AdjustFOV();
    }

    void SpawnPillar(float zPosition)
    {
        if (pillarPrefab == null)
        {
            Debug.LogWarning("Pillar prefab is not assigned!");
            return;
        }

        Transform newPillar = Instantiate(pillarPrefab, new Vector3(0, 0, zPosition), Quaternion.identity);
        pillars.Add(newPillar);

        // Maintain max pillars
        if (pillars.Count > maxPillars)
        {
            Destroy(pillars[0].gameObject);
            pillars.RemoveAt(0);
        }
    }

    void ApplyHeadBobbing()
    {
        float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        playerCamera.transform.localPosition = new Vector3(0, bobbingOffset, 0);
    }

    void ApplyCameraTilt()
    {
        float tiltOffset = Mathf.Sin(Time.time * bobbingSpeed) * tiltAmount;
        playerCamera.transform.localRotation = Quaternion.Euler(tiltOffset, 0, 0);
    }

    void AdjustFOV()
    {
        playerCamera.fieldOfView = Mathf.Lerp(60, 80, Mathf.PingPong(Time.time * 0.5f, 1));
    }

    IEnumerator StopAndShowSurvey()
    {
        yield return new WaitForSeconds(60); // Wait for 1 minute

        isRunning = false; // Stop movement

        // Reset player position and rotation
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Stop physics movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true; // Prevent movement
        }

        // Show the SSQ panel in front of the player
        if (ssqPanel != null)
        {
            ssqPanel.SetActive(true);
            ssqPanel.transform.position = transform.position + transform.forward * 2f; // Place 2m ahead
            ssqPanel.transform.LookAt(playerCamera.transform); // Face the player
        }
    }
}
