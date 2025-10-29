using UnityEngine;

public class BugByteAI : MonoBehaviour
{
    // 1. VARIABLES
    [SerializeField] private float moveSpeed = 2f; // Qué tan rápido se mueve
    private Rigidbody2D rb;

    // 2. START (Se ejecuta al inicio)
    void Start()
    {
        // Guardamos el componente de física
        rb = GetComponent<Rigidbody2D>();
    }

    // 3. FIXED UPDATE (Para la física)
    void FixedUpdate()
    {
        // Hacemos que el BugByte SIEMPRE se mueva.
        // La variable "moveSpeed" (positiva o negativa) decide la dirección.
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    // 4. ON TRIGGER ENTER (Detectar los TurnPoints)
    // Esta función se llama automáticamente cuando BugByte entra en un "Trigger"
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto invisible que tocamos tiene la etiqueta "TurnPoint"...
        if (other.CompareTag("TurnPoint"))
        {
            // ...invertimos nuestra velocidad.
            // (Si moveSpeed era 2, ahora será -2)
            // (Si moveSpeed era -2, ahora será 2)
            moveSpeed *= -1;
            
            // Opcional: Voltear el sprite (lo haremos luego)
            // transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
}