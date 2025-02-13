using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public bool hasCollectedItem1 = false;
    public bool hasCollectedItem2 = false;
    public bool hasCollectedItem3 = false;
    public bool hasCollectedItem4 = false;

    public LayerMask ignoredLayers; // Set this in the inspector to ignore walls, etc.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if "E" is pressed
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.green);

            // Use bitwise NOT (~) to exclude specific layers and allow the ray to hit everything else
            if (Physics.Raycast(ray, out hit, 10f, ~ignoredLayers, QueryTriggerInteraction.Collide))
            {
                // Collectibles
                if (hit.collider.CompareTag("Collectable1"))
                {
                    hasCollectedItem1 = true;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable2"))
                {
                    hasCollectedItem2 = true;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable3"))
                {
                    hasCollectedItem3 = true;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("destroy");
                }
                if (hit.collider.CompareTag("Collectable4"))
                {
                    hasCollectedItem4 = true;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("destroy");
                }

                // Puzzle Objects (only activate if the corresponding collectible was obtained)
                if (hit.collider.CompareTag("Puzzle1"))
                {
                    hasCollectedItem1 = false;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("up");
                }
                if (hit.collider.CompareTag("Puzzle2"))
                {
                    hasCollectedItem2 = false;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("up");
                }
                if (hit.collider.CompareTag("Puzzle3"))
                {
                    hasCollectedItem3 = false;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("up");
                }
                if (hit.collider.CompareTag("Puzzle4"))
                {
                    hasCollectedItem4 = false;
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("up");
                }
            }
        }
    }
}
