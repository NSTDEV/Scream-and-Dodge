using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommandController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2.5f;
    public float invincibleLength = 2f; // Duración de la invulnerabilidad tras el salto
    private Vector3 initialPosition;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords;

    private Rigidbody2D rb;

    // Nuevas variables para la invulnerabilidad
    private float invincibleCounter;
    private SpriteRenderer theSR;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        // Inicializamos el SpriteRenderer
        theSR = GetComponent<SpriteRenderer>();

        keywords = new Dictionary<string, System.Action>
        {
            { "izquierda", MoveLeft },
            { "derecha", MoveRight },
            { "medio", MoveToCenter },
            { "salta", Jump }
        };

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs args)
    {
        keywords[args.text].Invoke();
    }

    private void Update()
    {
        // Controlar el movimiento horizontal
        if (isMovingLeft && transform.position.x > -3f) // Límite izquierdo
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (isMovingRight && transform.position.x < 3f) // Límite derecho
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Controlar invulnerabilidad
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f); // Restaurar la transparencia
            }
        }

        // Controlar posición en Y
        if (transform.position.y <= initialPosition.y)
        {
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
            rb.velocity = Vector2.zero;
            isGrounded = true;
            GetComponent<Collider2D>().isTrigger = false; // Restaurar el collider
        }
    }

    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isMovingCenter = false;

    private void MoveLeft()
    {
        isMovingLeft = true;
        isMovingRight = false;
    }

    private void MoveRight()
    {
        isMovingRight = true;
        isMovingLeft = false;
    }

    private void MoveToCenter()
    {
        isMovingCenter = true;
        isMovingRight = false;
        isMovingLeft = false;
        StartCoroutine(MoveToPosition(new Vector3(0, transform.position.y, transform.position.z)));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
    }

    public bool isGrounded = true;
    private void Jump()
    {
        isMovingCenter = false;
        isMovingRight = false;
        isMovingLeft = false;

        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(ReturnToGround());
            isGrounded = false;

            Vector3 newPosition = transform.position;
            newPosition.z = -3; // Cambiar solo el valor en el eje Z
            transform.position = newPosition;
            GetComponent<Collider2D>().isTrigger = true;

            // Activar invulnerabilidad
            invincibleCounter = invincibleLength;
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f); // Cambiar transparencia
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private IEnumerator ReturnToGround()
    {
        yield return new WaitForSeconds(1.5f);

        rb.velocity = new Vector2(rb.velocity.x, -jumpForce); // Simular la caída

        if (transform.position.y <= initialPosition.y)
        {
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
            isGrounded = true;

            // Desactivar invulnerabilidad al tocar el suelo
            invincibleCounter = 0;
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f); // Restaurar el color
        }
        else
        {
            // Si aún no ha llegado al suelo, puedes seguir aplicando gravedad
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - jumpForce * Time.deltaTime); // Ejemplo de gravedad adicional
        }
    }
}
