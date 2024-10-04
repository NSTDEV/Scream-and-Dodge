using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject bear; // El oso
    public float bearMoveSpeed = 0.5f; // Velocidad de movimiento del oso
    public float playerMoveSpeed = 4f; // Velocidad de movimiento del jugador
    private int collisionCount = 0; // Contador de colisiones
    public GLOBAL globalScript;

    public AudioSource collisionAudioSource, bearAudioSource;

    public float invincibleLength; // Duración de la invulnerabilidad
    private float invincibleCounter; // Contador de invulnerabilidad
    private SpriteRenderer theSR;
    private VoiceCommandController controller;

    private void Start()
    {
        controller = FindObjectOfType<VoiceCommandController>();
        theSR = GetComponent<SpriteRenderer>();
        globalScript = FindObjectOfType<GLOBAL>();
    }

    private void Update()
    {
        // Verificamos la posición en y de los obstáculos para destruirlos si están debajo del oso
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle.transform.position.y >= bear.transform.position.y - 4)
            {
                Destroy(obstacle);
            }
        }

        // Controlar la duración de la invulnerabilidad
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f); // Restaurar el color
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el collider que se ha activado es un obstáculo
        if (other.CompareTag("Obstacle") || other.CompareTag("Obstacle2"))
        {
            if (invincibleCounter <= 0) // Solo colisionar si no está invulnerable
            {
                Debug.Log("Colisión detectada");
                collisionCount++;
                Debug.Log("Contador de colisiones: " + collisionCount);

                // Si el jugador colisiona 3 veces
                if (collisionCount >= 3 && other.CompareTag("Obstacle2"))
                {
                    StartCoroutine(MoveBearToPlayer(transform.position, 2f)); // Mover al oso directamente sobre el jugador y esperar antes de cargar la escena
                }
                else
                {
                    // Activar invulnerabilidad
                    invincibleCounter = invincibleLength;
                    theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);
                }
                
                // Mover el oso hacia el jugador si no se ha alcanzado el conteo de colisiones
                if (collisionCount < 3)
                {
                    StartCoroutine(MoveBearTowardsSkier());
                }

                // Destruir el obstáculo al colisionar con el jugador
                Destroy(other.gameObject);
            }
        }
    }

    private IEnumerator MoveBearTowardsSkier()
    {
        Vector2 bearPosition = bear.transform.position;
        Vector2 skierPosition = transform.position;

        // Calcular la distancia total entre el oso y el jugador
        float totalDistance = Vector2.Distance(bearPosition, skierPosition);

        // Mover al oso un tercio de la distancia total hacia el jugador
        float moveDistance = totalDistance / 3;

        // Calcular la nueva posición
        Vector2 newBearPosition = Vector2.MoveTowards(bearPosition, skierPosition, moveDistance);

        // Mover de forma fluida
        float elapsedTime = 0f;
        float duration = 0.5f; // Duración del movimiento

        while (elapsedTime < duration)
        {
            bear.transform.position = Vector2.Lerp(bearPosition, newBearPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; // Esperar hasta el siguiente frame
        }

        bear.transform.position = newBearPosition; // Asegurarse de que el oso esté exactamente en la posición final
    }

    private IEnumerator MoveBearToPlayer(Vector2 targetPosition, float duration)
    {
        theSR.color = new Color(theSR.color.r, 0, 0, 0.5f); // Cambiar el color del jugador al mover el oso
        float elapsedTime = 0f;
        Vector2 startingPosition = bear.transform.position;

        while (elapsedTime < duration)
        {
            bear.transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Esperar hasta el siguiente frame
        }

        bear.transform.position = targetPosition; // Asegurarse de que el oso esté exactamente en la posición final

        // Activar invulnerabilidad tras colisión
        invincibleCounter = invincibleLength;
        StartCoroutine(WaitAndLoadScene(1f)); // Esperar antes de cargar la escena de pérdida
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Lose"); // Cargar la escena de "Perder"
    }
}
