using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class PortalScript : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad;

    // Esta función se llama automáticamente cuando algo entra en el Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si la cosa que nos tocó es el jugador
        // (La forma más rápida es ver si tiene el script de Bit)
        if (other.GetComponent<BitMovement>() != null)
        {
            // Si el nombre de la escena NO está vacío...
            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                // ...cargamos esa escena.
                SceneManager.LoadScene(sceneNameToLoad);
            }
            else
            {
                // Si se te olvidó poner el nombre en el Inspector, te avisará.
                Debug.LogWarning("¡Se te olvidó poner el nombre de la escena en el Portal!");
            }
        }
    }
}