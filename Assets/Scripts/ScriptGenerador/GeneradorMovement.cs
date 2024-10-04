using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorMovement : MonoBehaviour
{
    /*
    [SerializeField] private GameObject Enemy01;
   // [SerializeField] private GameObject Obstaculo;
    [SerializeField] private float initTime;
    [SerializeField] private float repeatTime;
    [SerializeField] private float speedXGenerator01 = 4f;
    // private int Counter = 0;
    //private float randomTime = 0;
    void Start()
    {
       InvokeRepeating("GenerateEnemy", initTime, repeatTime);
        //
       // Invoke("ShootBall", randomTime);
    }
    void Update()
    {
        moveGenerator();
    }

    public void GenerateEnemy()
    {
        Instantiate(Enemy01,transform.position, transform.rotation);
        // Counter ++;
        // if (Counter >=5)
        // {
        //     CancelInvoke("GenerateEnemy");
       // }
        
    }
    public void moveGenerator()
    {
        if(transform.position.x < -3f || transform.position.x > 2.5f)
                {
                    speedXGenerator01 *=-1; 
                }
                transform.Translate(speedXGenerator01*Time.deltaTime,0,0);
        
    }

    /// <summary>
    /// /////
    /// </summary>
    /*public void ShootBall()
    {
        Instantiate (Obstaculo,transform.position,transform.rotation);
        
            randomTime = Random.Range(1,3);
            Invoke("ShootBall", randomTime);
        
    }*/
    
   // [SerializeField] private float speedXGenerator01 = 4f;

    public GameObject[] prefabs; // Array para los prefabs
    public float tiempoMinimo = 3f; // Tiempo mínimo de espera
    public float tiempoMaximo = 7f; // Tiempo máximo de espera

    private void Start()
    {
        StartCoroutine(GenerarObjetos());
    }
    /*void Update()
    {
        moveGenerator();
    }*/

    private IEnumerator GenerarObjetos()
    {
        while (true)
        {
            // Espera un tiempo aleatorio entre 3 y 7 segundos
            float tiempoEspera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(tiempoEspera);
            
            // Selecciona un prefab aleatorio
            int indice = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[indice];

            // Genera el objeto en la posición del generador
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
    /*public void moveGenerator()
    {
        if(transform.position.x < -3f || transform.position.x > 2.5f)
                {
                    speedXGenerator01 *=-1; 
                }
                transform.Translate(speedXGenerator01*Time.deltaTime,0,0);
        
    }*/
}
