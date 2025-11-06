using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Esta función la llamará el botón
    public void StartGame()
    {
        // Carga tu primer nivel
        SceneManager.LoadScene("Level_01");
    }
}