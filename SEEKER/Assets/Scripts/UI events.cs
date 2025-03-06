using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel
    public GameObject cameraController; // Assign the camera script GameObject (if applicable)

    private bool isPaused = false;
    public GameObject SplashText1;
    public GameObject SplashText2;
    public GameObject SplashText3;
    public GameObject SplashText4;
    RaycastScript interact;
    Puzzle2 puzzle2;
    public bool text1;
    public bool text2;
    public bool text3;
    void Start()
    {
        pausePanel.SetActive(false); // Ensure the panel is hidden at start
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor at the start
        Cursor.visible = false; // Hide the cursor at the start
        interact = FindObjectOfType<RaycastScript>();
        puzzle2 = FindObjectOfType<Puzzle2>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && pausePanel != null)
        {
            TogglePauseMenu();
        }
        if (!text2 && !text3)
        {
            text1 = true;
        }
        if (interact != null && interact.hasCollectedAll)
        {
            text2 = true;
        }
        if (puzzle2 != null && puzzle2.AllRotated)
        {
            text3 = true;
        }
        if (SplashText1 != null && SplashText2 != null && SplashText3 != null)
        {
            if (text1)
            {
                SplashText1.SetActive(true);
                SplashText2.SetActive(false);
                SplashText3.SetActive(false);
            }
            if (text2)
            {
                SplashText1.SetActive(false);
                SplashText2.SetActive(true);
                SplashText3.SetActive(false);
            }
            if (text3)
            {
                SplashText1.SetActive(false);
                SplashText2.SetActive(false);
                SplashText3.SetActive(true);
            }
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
    public void HowToPlay()
    {
        SplashText4.SetActive(true);
    }
    public void start()
    {
        SceneManager.LoadScene("lvl 1");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Start");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}