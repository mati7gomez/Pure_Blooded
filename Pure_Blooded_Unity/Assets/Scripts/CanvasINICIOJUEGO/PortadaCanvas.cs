using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PortadaCanvas : MonoBehaviour
{
    // Hay que asignar este metodo al boton en la escena
    public GameObject B_Pausa;
    public GameObject B_Despausa;
    public GameObject FondoPanel;
    private void Start()
    {
        if (B_Pausa != null)
        {
            Time.timeScale = 1f;
            B_Pausa.SetActive(true);
            B_Despausa.SetActive(false);
            FondoPanel.SetActive(false);
        }
    }

    public void CambiarEscena()
    {
        // Cambiar a la escena deseada (asegúrate de tenerla en tu Build Settings)
        SceneManager.LoadScene(1);
    }

    public void BotonPausa()
    {
        Time.timeScale = 0f;
        B_Pausa.SetActive(false);
        B_Despausa.SetActive(true);
        FondoPanel.SetActive(true);
    }
    public void BotonReanudar()
    {
        Time.timeScale = 1f;
        B_Pausa.SetActive(true);
        B_Despausa.SetActive(false);
        FondoPanel.SetActive(false);
    }

}
