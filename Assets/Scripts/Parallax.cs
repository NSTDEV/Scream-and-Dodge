using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float scrollSpeed = -5f;
    private Vector3 startPos;
    private float accumulatedDisplacement = 0f; // Variable acumulativa para el desplazamiento

    void Start()
    {
        startPos = transform.position;
        startPos.y = 14f; // Fijamos la posición inicial Y
    }

    void Update()
    {
        // Acumulamos el desplazamiento en función de la velocidad y el tiempo transcurrido
        accumulatedDisplacement += scrollSpeed * Time.deltaTime;

        // Calculamos la nueva posición en función del desplazamiento acumulado, usando Mathf.Repeat para hacer el loop
        float newPos = Mathf.Repeat(accumulatedDisplacement, startPos.y);
        
        // Actualizamos la posición del objeto
        transform.position = new Vector3(startPos.x, startPos.y - newPos, startPos.z);
    }
}
