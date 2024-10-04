using System.Collections;
using System.Collections.Generic;
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

    public float invincibleLength;
    private float invincibleCounter;
    private SpriteRenderer theSR;

    private VoiceCommandController voiceCommandController;

    private void Start()
    {
        voiceCommandController = GetComponent<VoiceCommandController>();
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

        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Verificamos si colisiona con uno de los obstáculos
         if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Obstacle2") )
        {

     
           if (invincibleCounter <= 0)
            {
                collisionCount++;

                playerMoveSpeed = playerMoveSpeed / 1.2f;
                bearAudioSource.Play();
                collisionAudioSource.Play();

                if (globalScript != null)
                {
                    globalScript.UpdateSpeed(playerMoveSpeed);
                }

                if (collisionCount >= 3 || collision.gameObject.CompareTag("Obstacle2") )
                {
                    StartCoroutine(MoveBearToPlayer(transform.position, 2f)); // Mover al oso directamente sobre el esquiador y esperar antes de cargar la escena
                }
                else
                {
                    invincibleCounter = invincibleLength;
                    theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);

                }
                if (collisionCount < 3)
                {
                    StartCoroutine(MoveBearTowardsSkier());
                }
            }

            // Destruir el obstáculo al colisionar con el esquiador
            Destroy(collision.gameObject);
        }

    }

     /**El metodo On TriggerEnter sirve para que al estar en modo Trigger aun asi choque con el obstaculo3 que aunque saltemos no lo esquivaremos*/
    private void OnTriggerEnter2D(Collider2D other)
{
    // Verificar si el collider que se ha activado es el Obstacle3
    if (other.CompareTag("Obstacle2"))
    {
       StartCoroutine(MoveBearToPlayer(transform.position, 2f)); // Mover al oso directamente sobre el esquiador y esperar antes de cargar la escena
       
        /*// Cambiar a la escena de perder
        SceneManager.LoadScene("Lose"); // Asegúrate de que "Lose" sea el nombre correcto de tu escena*/
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
        theSR.color = new Color(theSR.color.r, 0, 0, 0.5f);
        float elapsedTime = 0f;
        Vector2 startingPosition = bear.transform.position;

        while (elapsedTime < duration)
        {
            bear.transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Esperar hasta el siguiente frame
        }

        bear.transform.position = targetPosition; // Asegurarse de que el oso esté exactamente en la posición final

        invincibleCounter = invincibleLength;
        // Cargar la escena de "Perder"
        StartCoroutine(WaitAndLoadScene(1f)); // Esperar 1 segundos
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(waitTime);

        // Cargar la escena de "Perder"
        SceneManager.LoadScene("Lose");
    }
}


