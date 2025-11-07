using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

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
    [SerializeField] private AudioClip pickUpCoinSound;
    [SerializeField] private AudioClip heartPickUpSound;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI vidasText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Stats")]
    private static int lives = 3;
    private static int score = 0;

    // START
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = totalJumps;
        myAudioSource = GetComponent<AudioSource>();

        GameObject vidasObject = GameObject.FindWithTag("VidasUI");
        
        if (vidasObject != null)
        {
            vidasText = vidasObject.GetComponent<TextMeshProUGUI>();
        }

        GameObject scoreObject = GameObject.FindWithTag("ScoreUI");
        if (scoreObject != null)
        {
            scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        }
        
        UpdateUI(); 
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

        UpdateUI();
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
        if (other.CompareTag("Collectible"))
        {
            score += 10; // Suma 10 puntos
            UpdateUI(); // Actualiza el texto
            Destroy(other.gameObject); // Destruye la moneda

            myAudioSource.PlayOneShot(pickUpCoinSound);
        } else if (other.CompareTag("Vida"))
        {
            lives++; // Suma una vida
            UpdateUI(); // Actualiza el texto
            Destroy(other.gameObject); // Destruye el corazon

            myAudioSource.PlayOneShot(heartPickUpSound);
        }
        
    }

    // ----- ACCIÓN DE MUERTE -----
    private void BitDies()
    {
        myAudioSource.PlayOneShot(deathSound);
        lives--; // Resta una vida
        UpdateUI(); // Actualiza el texto

        if (lives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            lives = 3; // Resetea las vidas para la próxima vez
            deathScreenPanel.SetActive(true);
            Time.timeScale = 0f;

            // Desactivar el control de Bit
            this.enabled = false;
        }
    }
    
    // Esta función actualiza los textos
    private void UpdateUI()
    {
        if (vidasText != null)
        {
            vidasText.text = "Vidas: " + lives;
        }
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}