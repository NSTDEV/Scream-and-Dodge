using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject bear; // El oso
    public float moveSpeed = 1.16f; // Velocidad de movimiento del oso
    public float distanceThreshold = 3f; // Distancia mínima para colisionar
    private int collisionCount = 0; // Contador de colisiones

     private VoiceCommandController voiceCommandController;

     private Rigidbody2D rb;

    private void Start()
    {
        // Obtener la referencia al script VoiceCommandController
        voiceCommandController = GetComponent<VoiceCommandController>();

         rb = GetComponent<Rigidbody2D>();

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si colisiona con uno de los obstáculos
        if (collision.gameObject.CompareTag("Obstacle") || 
            collision.gameObject.CompareTag("Obstacle2") ||
            collision.gameObject.CompareTag("Obstacle3") || 
            collision.gameObject.CompareTag("Obstacle4") )
        {
            collisionCount++;

            // Mover al oso hacia el esquiador
            MoveBearTowardsSkier();

            // Si el jugador colisiona 3 veces, el oso se acercará lo suficiente o si colisionar con el arbol gigante perdera directamente
            if (collisionCount >= 3 || collision.gameObject.CompareTag("Obstacle3") )
            {
                
                // Asegúrate de que el oso termine lo suficientemente cerca
                float distance = Vector2.Distance(bear.transform.position, transform.position);
                if (distance > distanceThreshold)
                {
                    bear.transform.position = Vector2.MoveTowards(bear.transform.position, transform.position, moveSpeed * Time.deltaTime);
                }
                // Cambiar de escena
                SceneManager.LoadScene("Lose"); // Reemplaza con el nombre de la escena a la que quieres ir
                
            }

           // Destruir el obstáculo al colisionar con el esquiador
         Destroy(collision.gameObject);
        }
         
    }

    /**El metodo On TriggerEnter sirve para que al estar en modo Trigger aun asi choque con el obstaculo3 que aunque saltemos no lo esquivaremos*/
    private void OnTriggerEnter2D(Collider2D other)
{
    // Verificar si el collider que se ha activado es el Obstacle3
    if (other.CompareTag("Obstacle3"))
    {
        // Asegúrate de que el oso termine lo suficientemente cerca
                float distance = Vector2.Distance(bear.transform.position, transform.position);
                if (distance > distanceThreshold)
                {
                    bear.transform.position = Vector2.MoveTowards(bear.transform.position, transform.position, moveSpeed * Time.deltaTime);
                }
        // Cambiar a la escena de perder
        SceneManager.LoadScene("Lose"); // Asegúrate de que "Lose" sea el nombre correcto de tu escena
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