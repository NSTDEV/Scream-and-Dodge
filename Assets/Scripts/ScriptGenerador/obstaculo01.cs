using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaculo01 : MonoBehaviour
{
     [SerializeField] private float speedY = 2f; 
    
    void Update()
    {
        transform.Translate(0, speedY * Time.deltaTime, 0); 
        DestroyBall();
    }

    public void DestroyBall()
    {
        // Cambiamos las condiciones para el eje Y
        if ( transform.position.y >= 5.5)
        {
            Destroy(gameObject);
        }
    }
  
}
