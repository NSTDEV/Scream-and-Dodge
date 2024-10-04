using TMPro;
using UnityEngine;

public class GLOBAL : MonoBehaviour
{
    public CollisionController player;
    public Parallax parallax1, parallax2;
    public TextMeshProUGUI speed, km;

    float general_speed;
    float totalKilometraje = 0f; // Variable acumulativa para el kilometraje

    void Start()
    {
        km.text = "0";
        general_speed = player.playerMoveSpeed;
        parallax1.scrollSpeed = -general_speed;
        parallax2.scrollSpeed = -general_speed * 0.95f;

        print("Velocidad inicial: " + general_speed);
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed.text = general_speed.ToString("F2") + " km/h";

        general_speed = newSpeed;
        parallax1.scrollSpeed = -general_speed;
        parallax2.scrollSpeed = -general_speed * 0.95f;

        print("Velocidad actualizada a: " + general_speed);
    }

    void Update()
    {
        totalKilometraje += (general_speed * 0.5f) * Time.deltaTime;
        km.text = totalKilometraje.ToString("F2") + " Metros";
        speed.text = general_speed.ToString("F2") + " km/h";
    }
}
