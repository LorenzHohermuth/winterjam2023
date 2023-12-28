using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    // Method to start the game
    public void PlayGame()
    {
        // Load the game scene, assuming it's named "GameScene"
        SceneManager.LoadScene("SampleScene");
    }

    // Method to exit the game
    public void ExitGame()
    {
        // Close the application
        Application.Quit();
    }
}
