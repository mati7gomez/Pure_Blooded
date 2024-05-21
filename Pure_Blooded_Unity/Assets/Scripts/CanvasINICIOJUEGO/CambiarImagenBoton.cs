using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CambiarImagenBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image botonImage;
    private AudioSource audioSource;

    private void Start()
    {
        // Obtener los componentes Image y AudioSource del GameObject
        botonImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        // Inicialmente desactivar el AudioSource y la imagen
        audioSource.enabled = false;
        botonImage.enabled = false;

    }

    public void OnPointerEnter(PointerEventData eventData) // Cuando el ratón entra
    {
        if (botonImage != null && audioSource != null)
        {
            // Activar y reproducir el audio
            audioSource.enabled = true;
            audioSource.Play();

            // Activar la imagen
            botonImage.enabled = true;

        }
    }

    public void OnPointerExit(PointerEventData eventData) // Cuando el ratón sale
    {
        if (botonImage != null && audioSource != null)
        {
            // Detener y desactivar el audio
            audioSource.Stop();
            audioSource.enabled = false;

            // Desactivar la imagen
            botonImage.enabled = false;

        }
    }
}
