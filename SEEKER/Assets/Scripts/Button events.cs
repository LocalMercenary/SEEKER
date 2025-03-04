using UnityEngine;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel
    public GameObject cameraController; // Assign the camera script GameObject (if applicable)

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // Ensure the panel is hidden at start
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor at the start
        Cursor.visible = false; // Hide the cursor at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0; // Pause game
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true; // Show cursor

            if (cameraController != null)
            {
                cameraController.SetActive(false); // Disable camera movement script
            }
        }
        else
        {
            Time.timeScale = 1; // Resume game
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor again
            Cursor.visible = false; // Hide cursor

            if (cameraController != null)
            {
                cameraController.SetActive(true); // Enable camera movement script
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}