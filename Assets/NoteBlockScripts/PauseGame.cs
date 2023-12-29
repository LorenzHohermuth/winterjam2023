using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Animator targetAnimator; 
    private bool isPaused = false;
    [SerializeField] private GameObject block;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        block.SetActive(isPaused);
    }
}
