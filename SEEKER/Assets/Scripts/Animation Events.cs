using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Puzzle2 puzzle; // Reference to Puzzle2 script

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

    public void rotated1()
    {
        if (puzzle != null) puzzle.Rotated1 = true;
    }

    public void Unrotated1()
    {
        if (puzzle != null) puzzle.Rotated1 = false;
    }

    public void rotated2()
    {
        if (puzzle != null) puzzle.Rotated2 = true;
    }

    public void Unrotated2()
    {
        if (puzzle != null) puzzle.Rotated2 = false;
    }

    public void rotated3()
    {
        if (puzzle != null) puzzle.Rotated3 = true;
    }

    public void Unrotated3()
    {
        if (puzzle != null) puzzle.Rotated3 = false;
    }

    public void rotated4()
    {
        if (puzzle != null) puzzle.Rotated4 = true;
    }

    public void Unrotated4()
    {
        if (puzzle != null) puzzle.Rotated4 = false;
    }
}
