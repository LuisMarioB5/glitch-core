using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Esta es la función que llamará nuestro botón "Continuar"
    public void GoToLevel1()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f;

        // Cargamos el Nivel 1 (como pide el profesor)
        SceneManager.LoadScene("Level_01");
    }
    
    public void GoToMainMenu()
    {
        // Antes de cargar la escena, ¡debemos reanudar el tiempo!
        Time.timeScale = 1f; 
        
        // Cargamos el Nivel 1 (como pide el profesor)
        SceneManager.LoadScene("MainMenu");
    }
}