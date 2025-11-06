using UnityEngine;
using UnityEngine.Events;
public class PortalScript : MonoBehaviour
{
    // ¡LA LÍNEA MÁGICA!
    // Esto crea la "caja" en el Inspector, igual que la de un botón.
    [SerializeField] private UnityEvent onPortalEntered;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si la cosa que nos tocó fue Bit...
        if (other.GetComponent<BitMovement>() != null)
        {
            // ...invocamos el evento (le decimos a la "caja" que se active)
            onPortalEntered.Invoke();
        }
    }
}