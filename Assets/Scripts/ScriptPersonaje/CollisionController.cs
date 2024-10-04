using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject bear; // El oso
    public float moveSpeed = 1.16f; // Velocidad de movimiento del oso
    public float distanceThreshold = 3f; // Distancia mínima para colisionar
    private int collisionCount = 0; // Contador de colisiones

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si colisiona con uno de los obstáculos
        if (collision.gameObject.CompareTag("Obstacle") || 
            collision.gameObject.CompareTag("Obstacle2") || 
            collision.gameObject.CompareTag("Obstacle3") || 
            collision.gameObject.CompareTag("Obstacle4"))
        {
            collisionCount++;

            // Mover al oso hacia el esquiador
            MoveBearTowardsSkier();

            // Si el jugador colisiona 3 veces, el oso se acercará lo suficiente
            if (collisionCount >= 3)
            {
                // Asegúrate de que el oso esté lo suficientemente cerca
                float distance = Vector2.Distance(bear.transform.position, transform.position);
                if (distance > distanceThreshold)
                {
                    bear.transform.position = Vector2.MoveTowards(bear.transform.position, transform.position, moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void MoveBearTowardsSkier()
    {
        // Mueve el oso hacia el esquiador en el eje Y
        Vector2 bearPosition = bear.transform.position;
        Vector2 skierPosition = transform.position;
        
        if (bearPosition.y < skierPosition.y)
        {
            bearPosition.y += moveSpeed * Time.deltaTime;
        }
        else if (bearPosition.y > skierPosition.y)
        {
            bearPosition.y -= moveSpeed * Time.deltaTime;
        }

        bear.transform.position = bearPosition;
    }
}