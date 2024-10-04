using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject bear; // El oso
    public float bearMoveSpeed = 0.16f; // Velocidad de movimiento del oso
    public float playerMoveSpeed = 4f; // Velocidad de movimiento del jugador
    public float distanceThreshold = 3f; // Distancia mínima para colisionar
    private int collisionCount = 0; // Contador de colisiones

    private VoiceCommandController voiceCommandController;

    private void Start()
    {
        // Obtener la referencia al script VoiceCommandController
        voiceCommandController = GetComponent<VoiceCommandController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si colisiona con uno de los obstáculos
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Obstacle2"))
        {
            collisionCount++;

            // Si el jugador colisiona 3 veces, el oso se acercará lo suficiente o si colisiona con el árbol gigante perderá directamente
            if (collisionCount >= 3)
            {
                // Asegúrate de que el oso termine lo suficientemente cerca
                float distance = Vector2.Distance(bear.transform.position, transform.position);
                if (distance > distanceThreshold)
                {
                    // Mueve al oso hacia el jugador
                    bear.transform.position = Vector2.MoveTowards(bear.transform.position, transform.position, bearMoveSpeed * Time.deltaTime);
                    print(distance);
                }
                else
                {
                    // Cambiar de escena cuando el oso está lo suficientemente cerca
                    SceneManager.LoadScene("Lose"); // Reemplaza con el nombre de la escena a la que quieres ir
                }
            }

            // Mover al oso hacia el esquiador
            MoveBearTowardsSkier();

            // Destruir el obstáculo al colisionar con el esquiador
            Destroy(other.gameObject);
        }
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

    private void MoveBearTowardsSkier()
    {
        // Mueve al oso hacia el jugador/esquiador en ambos ejes (x e y)
        Vector2 bearPosition = bear.transform.position;
        Vector2 skierPosition = transform.position;

        // Mover al oso hacia el jugador/esquiador de manera gradual
        bear.transform.position = Vector2.MoveTowards(bearPosition, skierPosition, bearMoveSpeed * Time.deltaTime);
    }
}
