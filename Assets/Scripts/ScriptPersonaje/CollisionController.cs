using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject bear; // El oso
    public float bearMoveSpeed = 2f; // Velocidad de movimiento del oso
    public float playerMoveSpeed = 4f; // Velocidad de movimiento del jugador
    public float distanceThreshold = 0.5f; // Distancia mínima para colisionar antes de perder
    private int collisionCount = 0; // Contador de colisiones

    private VoiceCommandController voiceCommandController;

    private void Start()
    {
        voiceCommandController = GetComponent<VoiceCommandController>();
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si colisiona con uno de los obstáculos
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Obstacle2"))
        {
            Debug.Log("Colisión detectada");
            collisionCount++;
            Debug.Log("Contador de colisiones: " + collisionCount);

            // Si el jugador colisiona 3 veces
            if (collisionCount >= 3)
            {
                // Mover al oso directamente sobre el esquiador
                bear.transform.position = new Vector2(transform.position.x, transform.position.y); // Colocar el oso encima del esquiador

                // Iniciar la corrutina para esperar antes de cargar la escena
                StartCoroutine(WaitAndLoadScene(2f)); // Esperar 2 segundos
            }
            else
            {
                MoveBearTowardsSkier(); // Mover al oso hacia el jugador
            }

            // Destruir el obstáculo al colisionar con el esquiador
            Destroy(other.gameObject);
        }
    }

    private void MoveBearTowardsSkier()
    {
        Vector2 bearPosition = bear.transform.position;
        Vector2 skierPosition = transform.position;

        // Calcular la distancia total entre el oso y el jugador
        float totalDistance = Vector2.Distance(bearPosition, skierPosition);
        
        // Mover al oso un tercio de la distancia total hacia el jugador
        float moveDistance = totalDistance / 3;

        // Calcular la nueva posición
        Vector2 newBearPosition = Vector2.MoveTowards(bearPosition, skierPosition, moveDistance);
        
        // Actualizar la posición del oso
        bear.transform.position = newBearPosition;
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(waitTime);

        // Cargar la escena de "Perder"
        SceneManager.LoadScene("Lose");
    }
}

