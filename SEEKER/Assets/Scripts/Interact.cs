using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public bool hasCollectedItem1 = false;
    public bool hasCollectedItem2 = false;
    public bool hasCollectedItem3 = false;
    public bool hasCollectedItem4 = false;
    public bool hasSpawned = false;
    public bool hasCollectedAll = false;

    public GameObject player;
    public GameObject enemy;

    public LayerMask ignoredLayers; // Set this in the inspector to ignore walls, etc.

    private Canvas currentCanvas = null; // Store the currently active canvas

    void Start()
    {
    }

    void Update()
    {
        Camera activeCamera = GetActiveCamera(); // Get the currently active camera
        if (activeCamera == null) return; // If no active camera, exit

        // First Raycast - For the interactable mechanics
        if (Input.GetKeyDown(KeyCode.E)) // Check if "E" is pressed
        {
            RaycastHit hit;
            Ray ray = new Ray(activeCamera.transform.position, activeCamera.transform.forward);
            Debug.DrawRay(activeCamera.transform.position, activeCamera.transform.forward * 5f, Color.green);

            if (Physics.Raycast(ray, out hit, 5f, ~ignoredLayers, QueryTriggerInteraction.Collide))
            {
                Animator animator = hit.collider.gameObject.GetComponent<Animator>();

                // Collectibles
                if (hit.collider.CompareTag("Collectable1"))
                {
                    hasCollectedItem1 = true;
                    animator?.SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable2"))
                {
                    hasCollectedItem2 = true;
                    animator?.SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable3"))
                {
                    hasCollectedItem3 = true;
                    animator?.SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable4"))
                {
                    hasCollectedItem4 = true;
                    animator?.SetTrigger("destroy");
                }

                // Puzzle Objects
                if (hit.collider.CompareTag("Puzzle1") && hasCollectedItem1)
                {
                    animator?.SetTrigger("rot");
                }
                if (hit.collider.CompareTag("Puzzle2") && hasCollectedItem2)
                {
                    animator?.SetTrigger("rot");
                }
                if (hit.collider.CompareTag("Puzzle3") && hasCollectedItem3)
                {
                    animator?.SetTrigger("rot");
                }
                if (hit.collider.CompareTag("Puzzle4") && hasCollectedItem4)
                {
                    animator?.SetTrigger("rot");
                }

                // Hide Mechanic
                if (hit.collider.CompareTag("Hide"))
                {
                    if (animator != null)
                    {
                        bool isOpen = animator.GetBool("Open");
                        if (!animator.GetBool("playered"))
                        {
                            animator.SetBool("playered", true);
                        }
                        animator.SetBool("Open", !isOpen); // Toggle Open state
                    }
                }
            }
        }

        // Second Raycast - To enable canvases tagged "Interact"
        EnableCanvasOnHit();

        if (!hasSpawned)
        {
            // Check if 2 or more items are collected and spawn the enemy
            CheckAndSpawnEnemy();
        }
    }

    // This raycast enables canvases on objects that have a Canvas component and are tagged "Interact"
    private void EnableCanvasOnHit()
    {
        Camera activeCamera = GetActiveCamera(); // Get the currently active camera
        if (activeCamera == null) return; // If no active camera, exit

        RaycastHit hit;
        Ray ray = new Ray(activeCamera.transform.position, activeCamera.transform.forward);
        Debug.DrawRay(activeCamera.transform.position, activeCamera.transform.forward * 5f, Color.green);
        Debug.Log("test");
        if (Physics.Raycast(ray, out hit, 5f, ~ignoredLayers, QueryTriggerInteraction.Collide))
        {
            Debug.Log(hit);
            Canvas canvas = hit.collider.GetComponentInChildren<Canvas>(); // Check if the object has a Canvas component
            if (canvas != null && canvas.CompareTag("Interact")) // Only enable canvas if it has the "Interact" tag
            {
                Debug.Log("CanvasFound");
                canvas.gameObject.SetActive(true); // Enable the canvas if it is tagged "Interact"
            }
        }
        else
        {
            // If raycast does not hit anything, disable the currently active canvas
            DisableCurrentCanvas();
        }
    }

    void DisableCurrentCanvas()
    {
        if (currentCanvas != null)
        {
            currentCanvas.gameObject.SetActive(false); // Disable the currently active canvas
            currentCanvas = null; // Reset the reference
        }
    }

    void CheckAndSpawnEnemy()
    {
        int collectedCount = 0;

        if (hasCollectedItem1) collectedCount++;
        if (hasCollectedItem2) collectedCount++;
        if (hasCollectedItem3) collectedCount++;
        if (hasCollectedItem4) collectedCount++;

        if (collectedCount >= 2 && enemy != null && !enemy.activeInHierarchy && !hasSpawned)
        {
            hasSpawned = true;
            enemy.SetActive(true);
            Debug.Log("Enemy Spawned!");
        }
    }

    // Function to find the currently active camera
    private Camera GetActiveCamera()
    {
        Camera[] cameras = Camera.allCameras;
        foreach (Camera cam in cameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam;
            }
        }
        return null; // No active camera found
    }
}
