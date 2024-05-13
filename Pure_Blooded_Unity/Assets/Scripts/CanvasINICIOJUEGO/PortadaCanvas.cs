using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortadaCanvas : MonoBehaviour
{
    // Hay que asignar este metodo al boton en la escena
    public void CambiarEscena()
    {
        // Cambiar a la escena deseada (asegúrate de tenerla en tu Build Settings)
        SceneManager.LoadScene(1);
    }
}
