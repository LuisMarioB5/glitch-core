using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Esta es la función que llamará nuestro botón "Continuar"
    public void GoToMainMenu()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f; 
        
        // Se carga el menú principal
        SceneManager.LoadScene("MainMenu");
    }

    // Esta es la función que llamará nuestro botón "Continuar"
    public void GoToLevel1()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f;

        // Se carga el nivel `Level_01`
        SceneManager.LoadScene("Level_01");
    }

    public void GoToLevel2()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f;

        // Se carga el nivel `Level_02`
        SceneManager.LoadScene("Level_02");
    }

    public void GoToLevel3BossFirewall()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f;

        // Se carga el nivel `Level_03`
        SceneManager.LoadScene("Level_03_Firewall");
    }

    public void GoToLevel4()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f;

        // Se carga el nivel `Level_04`
        SceneManager.LoadScene("Level_04");
    }
}