using UnityEngine;

public class AbrirPuertas : MonoBehaviour
{
    public Transform puerta;
    public bool puedeAbrirPuerta = false;
    public bool tengoLlave = false;

    private void Start()
    {
        puerta = GetComponent<Transform>();
    }
    private void Update()
    {
        AbrirPuerta();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si colisionó contra el jugador y presionó la tecla 'C'
        if (other.gameObject.CompareTag("Player") && tengoLlave)
        {
            puedeAbrirPuerta = true;
            Debug.Log("Puede Abrir la puerta");
        }
        else if (other.gameObject.CompareTag("Player") && !tengoLlave)
        {
            puedeAbrirPuerta = false;
            Debug.Log("No puede Abrir la puerta, le falta una Llave");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !tengoLlave)
        {
            Debug.Log("Buena Suerte");
            puedeAbrirPuerta = false;
        }
    }


    // LOGICA PARA ABRIR LA PUERTA
    private void AbrirPuerta()
    {
        if (Input.GetKeyDown(KeyCode.F) && puedeAbrirPuerta)
        {
            // Girar la puerta -90 grados en el eje Y
            puerta.Rotate(0f, -90f, 0f, Space.Self);

            // Obtener la posición actual de la puerta
            Vector3 posicionActual = puerta.position;

            // Incrementar la posición actual en 1 en el eje X y 2 en el eje Z
            posicionActual.x += 0.90f;
            posicionActual.z += 0.80f;

            // Establecer la nueva posición de la puerta
            puerta.position = posicionActual;

            Debug.Log("Puerta abierta");
            // Quito la llave y por lo tanto el permiso para abrir la puerta
            puedeAbrirPuerta = false;
            tengoLlave = false;
        }
    }
}