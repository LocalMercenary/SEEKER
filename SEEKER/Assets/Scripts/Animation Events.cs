using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Puzzle2 puzzle; // Reference to Puzzle2 script
    public GameObject player; // Assign the player in the Inspector
    public List<GameObject> objectsToHide; // Assign other objects that need to be hidden

    void Start()
    {
        puzzle = FindObjectOfType<Puzzle2>(); // Find Puzzle2 in the scene
        if (puzzle == null)
        {
            Debug.LogError("Puzzle2 script not found in the scene!");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void HideObjects()
    {
        // Hide player if assigned
        if (player != null)
        {
            player.SetActive(false);
        }

        // Hide all other assigned objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void UnhideObjects()
    {
        // Unhide player if assigned
        if (player != null)
        {
            player.SetActive(true);
        }

        // Unhide all other assigned objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    public void rotated1() { if (puzzle != null) puzzle.Rotated1 = true; }
    public void Unrotated1() { if (puzzle != null) puzzle.Rotated1 = false; }
    public void rotated2() { if (puzzle != null) puzzle.Rotated2 = true; }
    public void Unrotated2() { if (puzzle != null) puzzle.Rotated2 = false; }
    public void rotated3() { if (puzzle != null) puzzle.Rotated3 = true; }
    public void Unrotated3() { if (puzzle != null) puzzle.Rotated3 = false; }
    public void rotated4() { if (puzzle != null) puzzle.Rotated4 = true; }
    public void Unrotated4() { if (puzzle != null) puzzle.Rotated4 = false; }
}
