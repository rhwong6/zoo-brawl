using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads AI game scene
    public void StartAIGame()
    {
        SceneManager.LoadScene("AI deck selection");
    }

    // Loads deck menu scene
    public void OpenDeckMenu()
    {
        SceneManager.LoadScene("Deck Menu");
    }

    // Loads practice area scene
    public void StartPracticeArea()
    {
        SceneManager.LoadScene("Practice Deck Selection");
    }

    // Exits the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
