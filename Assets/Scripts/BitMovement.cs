using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class BitMovement : MonoBehaviour
{
    // VARIABLES DE MOVIMIENTO
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;

    // VARIABLES DE SALTO
    [SerializeField] private float jumpForce = 10f;
    private int jumpsRemaining;
    [SerializeField] private int totalJumps = 2;
    private bool jumpPressed = false;

    // START
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = totalJumps;
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

        // ----- LECTURA DE SALTO -----
        if (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame))
        {
            jumpPressed = true;
        }
    }

    // FIXED UPDATE (Aplicar la Física)
    void FixedUpdate()
    {
        // ----- APLICAR MOVIMIENTO -----
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ----- APLICAR SALTO -----
        if (jumpPressed)
        {
            if (jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpsRemaining = jumpsRemaining - 1;
            }
            jumpPressed = false;
        }
    }

    // ----- DETECTOR DE COLISIONES -----
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // --- LÓGICA DE RESETEO DE SALTO ---
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = totalJumps;
        }

        // --- LÓGICA DE ENEMIGO (STOMP O MUERTE) --- 
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ----- ¡LA NUEVA LÓGICA DE STOMP! -----
            // Obtenemos el primer punto de contacto de la colisión
            ContactPoint2D contact = collision.contacts[0];
            
            // "contact.normal" es una flecha que apunta LEJOS de la superficie que golpeamos.
            // Si golpeamos la CABEZA del BugByte, la flecha apuntará HACIA ARRIBA (Y = 1).
            // Si golpeamos el LADO del BugByte, la flecha apuntará a un LADO (Y = 0).
            
            // Usamos 0.5f como un margen de seguridad, pero si "Y" es positivo, golpeamos desde arriba.
            if (contact.normal.y > 0.5f)
            {
                // ----- ACCIÓN DE STOMP -----
                
                // 1. Destruir al enemigo
                Destroy(collision.gameObject);
                
                // 2. ¡Hacer que Bit rebote un poco! (Mejora el "Game Feel")
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.75f); // 75% de la fuerza de un salto
                
                // 3. (Opcional) Restaurar un salto si estaba en el aire
                jumpsRemaining++; // O puedes poner = 1 si prefieres que solo gane 1 salto
            }
            else
            {
                // ----- ACCIÓN DE MUERTE -----
                // Si la "normal" no apuntaba hacia arriba, golpeamos por el lado.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}