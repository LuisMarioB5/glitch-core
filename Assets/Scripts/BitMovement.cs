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

    // Variable para "conectar" con el panel de muerte
    [SerializeField] private GameObject deathScreenPanel;

    // --- VARIABLES DE SONIDO ---
    private AudioSource myAudioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip stompSound;
    [SerializeField] private AudioClip deathSound;

    // START
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = totalJumps;
        myAudioSource = GetComponent<AudioSource>();
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
                jumpsRemaining--;
                myAudioSource.PlayOneShot(jumpSound);
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
            ContactPoint2D contact = collision.contacts[0];

            if (contact.normal.y > 0.5f)
            {
                // ----- ACCIÓN DE STOMP -----

                // Destruye al enemigo
                Destroy(collision.gameObject);

                // Reproduce el sonido de stomp
                myAudioSource.PlayOneShot(stompSound);

                // ¡Hace que Bit rebote un poco!
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.75f); // 75% de la fuerza de un salto

                // Restaura un salto si estaba en el aire
                jumpsRemaining++;
            }
            else
            {
                BitDies();
            }
        }
    }

    // Esta función se llama cuando Bit entra en un "Trigger"
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que tocamos tiene la etiqueta "Portal"...
        if (other.CompareTag("Portal"))
        {
            // ...cargamos la siguiente escena.

            // ¡OJO! Esto carga "Level_02".
            // Para que funcione, tienes que crear una escena llamada "Level_02"
            // y añadirla al Build Settings (File > Build Settings).
            SceneManager.LoadScene("Level_02");
        }
    }
    
    // ----- ACCIÓN DE MUERTE -----
    private void BitDies()
    {
        // Reproduce el sonido de muerte
        myAudioSource.PlayOneShot(deathSound);

        // Activar el panel de muerte
        deathScreenPanel?.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;

        // Desactivar el control de Bit
        this.enabled = false;
    }
}