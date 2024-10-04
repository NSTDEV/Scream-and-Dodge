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
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Obstacle2"))
        {
            collisionCount++;

            if (collisionCount >= 3)
            {
                float distance = Vector2.Distance(bear.transform.position, transform.position);
                if (distance > distanceThreshold)
                {
                    bear.transform.position = Vector2.MoveTowards(bear.transform.position, transform.position, bearMoveSpeed * Time.deltaTime);
                }
                else
                {
                    SceneManager.LoadScene("Lose");
                }
            }

            MoveBearTowardsSkier();
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
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
        Vector2 bearPosition = bear.transform.position;
        Vector2 skierPosition = transform.position;

        bear.transform.position = Vector2.MoveTowards(bearPosition, skierPosition, bearMoveSpeed * Time.deltaTime);
    }
}
