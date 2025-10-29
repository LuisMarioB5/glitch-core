using UnityEngine;
using UnityEngine.InputSystem; 

public class BitMovement : MonoBehaviour
{
    // VARIABLES DE MOVIMIENTO
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput; 

    // VARIABLES DE SALTO (NUEVAS)
    [SerializeField] private float jumpForce = 10f; // <-- NUEVO: Qué tan alto saltamos
    private int jumpsRemaining; // <-- NUEVO: Contador para el doble salto
    [SerializeField] private int totalJumps = 2; // <-- NUEVO: Total de saltos (para que sea 2 = doble salto)
    private bool jumpPressed = false; // <-- NUEVO: Para registrar la pulsación de tecla

    // START
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = totalJumps; // <-- NUEVO: Empezamos con todos nuestros saltos
    }

    // UPDATE (Leer el Input)
    void Update()
    {
        // ----- MOVIMIENTO HORIZONTAL -----
        moveInput = 0f; 
        if (Keyboard.current != null && (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed))
        {
            moveInput = -1f; 
        }
        if (Keyboard.current != null && (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed))
        {
            moveInput = 1f; 
        }

        // ----- LECTURA DE SALTO (NUEVO) -----
        // wasPressedThisFrame significa que solo se activa 1 vez cuando la pulsas, no si la dejas pulsada
        if (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame))
        {
            jumpPressed = true; // <-- NUEVO: Damos la "orden" de saltar
        }
    }

    // FIXED UPDATE (Aplicar la Física)
    void FixedUpdate()
    {
        // ----- APLICAR MOVIMIENTO -----
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ----- APLICAR SALTO (NUEVO) -----
        if (jumpPressed)
        {
            // Solo saltamos si nos quedan saltos
            if (jumpsRemaining > 0)
            {
                // Aplicamos la fuerza de salto vertical
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                
                // Restamos un salto
                jumpsRemaining--; 
            }
            
            // Reseteamos la "orden" de saltar
            jumpPressed = false; 
        }
    }

    // ----- DETECTOR DE COLISIONES (NUEVO) -----
    // Esta función se llama automáticamente cuando Bit choca con CUALQUIER COSA
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobamos si la cosa con la que chocamos tiene la etiqueta "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Si es el suelo, reseteamos nuestro contador de saltos
            jumpsRemaining = totalJumps; 
        }
    }
}