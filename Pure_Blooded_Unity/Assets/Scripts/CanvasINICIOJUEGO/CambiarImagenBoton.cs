using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CambiarImagenBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image botonImage;
    private AudioSource AudioSource;

    private void Start()
    {
        // Obtener la imagen del componente Button
        botonImage = GetComponent<Image>();
        AudioSource = GetComponent<AudioSource>();

        AudioSource.enabled = false;
        botonImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData) // CUANDO ENTRA EL MOUSE
    {
        AudioSource.enabled = true;
        botonImage.enabled = true;
        // Invertir el valor de fillCenter cuando el ratón se pase por encima
        if (botonImage != null)
        {
            AudioSource.Play();
            botonImage.fillCenter = !botonImage.fillCenter;
        }
    }

    public void OnPointerExit(PointerEventData eventData) // CUANDO SALE EL MOUSE
    {
        AudioSource.enabled = true;
        botonImage.enabled = false;
        // Volver a invertir el valor de fillCenter cuando el ratón salga del botón
        if (botonImage != null)
        {
            AudioSource.Stop();
            botonImage.fillCenter = !botonImage.fillCenter;
        }
    }
}
