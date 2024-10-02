using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class VoiceCommandController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Vector3 initialPosition;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        keywords = new Dictionary<string, System.Action>
        {
            { "izquierda", MoveLeft },
            { "derecha", MoveRight },
            { "centro", MoveToCenter },
            { "saltar", Jump }
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
    // Movimientos basados en los comandos de voz
    if (isMovingLeft && transform.position.x > -2.3f) // Límite izquierdo
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
    else if (isMovingRight && transform.position.x < 2.3f) // Límite derecho
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    
    // Comprobación de posición Y
    if (transform.position.y <= -0.1f)
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        rb.velocity = Vector2.zero; // Detener cualquier movimiento vertical
        isGrounded = true; // Permitir volver a saltar
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
    // Puedes usar una interpolación suave aquí
    StartCoroutine(MoveToPosition(new Vector3(0, transform.position.y, transform.position.z)));
    //isMovingRight = false;
    //isMovingLeft = false;
}

private IEnumerator MoveToPosition(Vector3 targetPosition)
{
    // Calcular la distancia total que debe recorrer el objeto
    float journeyLength = Vector3.Distance(transform.position, targetPosition);

    // Continuar moviendo hasta que esté cerca de la posición objetivo
    while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
    {
        // Calcular la dirección del movimiento
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Mover el objeto hacia la posición objetivo
        transform.position += direction * moveSpeed * Time.deltaTime;

        yield return null; // Esperar un frame
    }

    transform.position = targetPosition; // Asegúrate de llegar exactamente al destino
}

private bool isGrounded = true;
    private void Jump()
    {
        
    isMovingCenter = false;
    isMovingRight = false;
    isMovingLeft = false;

     if (isGrounded)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        StartCoroutine(ReturnToGround());
        isGrounded = false; // No permitir saltar de nuevo hasta aterrizar
    }
    }

    private IEnumerator ReturnToGround()
{
    yield return new WaitForSeconds(0.5f);
    
    rb.velocity = new Vector2(rb.velocity.x, -jumpForce); // Regresa a la posición inicial de manera más realista
    
    // Aseguramos que el personaje regrese a la posición Y 0 si cae por debajo de 0.1
    if (transform.position.y <= 0.1f)
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isGrounded = true; // Permitir volver a saltar
    }
    else
    {
        // Si aún no ha llegado al suelo, puedes seguir aplicando gravedad
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - jumpForce * Time.deltaTime); // Ejemplo de gravedad adicional
    }
}

    private void OnDestroy()
    {
        keywordRecognizer.Stop();
        keywordRecognizer.Dispose();
    }
}