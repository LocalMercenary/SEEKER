using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public bool hasCollectedItem1 = false;
    public bool hasCollectedItem2 = false;
    public bool hasCollectedItem3 = false;
    public bool hasCollectedItem4 = false;

    public GameObject player;
    public GameObject enemy;


    public LayerMask ignoredLayers; // Set this in the inspector to ignore walls, etc.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if "E" is pressed
        {
            Camera activeCamera = GetActiveCamera(); // Get the currently active camera
            if (activeCamera == null) return; // If no active camera, exit

            RaycastHit hit;
            Ray ray = new Ray(activeCamera.transform.position, activeCamera.transform.forward);
            Debug.DrawRay(activeCamera.transform.position, activeCamera.transform.forward * 10f, Color.green);

            if (Physics.Raycast(ray, out hit, 10f, ~ignoredLayers, QueryTriggerInteraction.Collide))
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

        // Check if 2 or more items are collected and spawn the enemy
        CheckAndSpawnEnemy();
    }
    void CheckAndSpawnEnemy()
    {
        int collectedCount = 0;

        if (hasCollectedItem1) collectedCount++;
        if (hasCollectedItem2) collectedCount++;
        if (hasCollectedItem3) collectedCount++;
        if (hasCollectedItem4) collectedCount++;

        if (collectedCount >= 2 && enemy != null && !enemy.activeInHierarchy)
        {
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
