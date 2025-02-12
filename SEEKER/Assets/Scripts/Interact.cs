using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public bool hasCollectedItem1 = false;
    public bool hasCollectedItem2 = false;
    public bool hasCollectedItem3 = false;
    public bool hasCollectedItem4 = false;

    public GameObject interactableObject; // Assign this in the Inspector
    private bool canUseObject = true; // Prevents spamming activation
    public float interactionCooldown = 2f; // Delay before it can be used again

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if "E" is pressed
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);

            if (Physics.Raycast(ray, out hit, 10f)) // Raycast with 10f max distance
            {
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
            }

            // Check if the interactable object can be activated
            if (canUseObject && (hasCollectedItem1 || hasCollectedItem2 || hasCollectedItem3 || hasCollectedItem4))
            {
                StartCoroutine(ActivateObject());
            }
        }
    }

    private IEnumerator ActivateObject()
    {
        canUseObject = false; // Disable interaction temporarily

        if (interactableObject != null)
        {
            interactableObject.SetActive(true); // Enable the object
            Animator anim = interactableObject.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetTrigger("activate"); // Play animation
            }
        }

        yield return new WaitForSeconds(interactionCooldown); // Wait before allowing another interaction
        canUseObject = true; // Enable interaction again
    }
}

