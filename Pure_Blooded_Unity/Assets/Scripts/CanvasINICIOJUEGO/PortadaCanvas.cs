using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PortadaCanvas : MonoBehaviour
{
    // ESCENA 0
    public GameObject B_Play;
    public GameObject B_Options;
    public GameObject B_Exit;
    // Hay que asignar este metodo al boton en la escena
    // ESCENA 1
    public GameObject B_Pausa;
    public GameObject B_Despausa;
    public GameObject FondoPanel;
    public GameObject OptionsInGame;
    public GameObject Exit;
    // ESCENA 2
    private void Start()
    {
        if (B_Pausa != null)
        {
            Time.timeScale = 1f;
            B_Pausa.SetActive(true);
            B_Despausa.SetActive(false);
            FondoPanel.SetActive(false);
            OptionsInGame.SetActive(false);
            Exit.SetActive(false);
        }
    }

    public void CambiarEscena(int escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BotonPausa()
    {
        Time.timeScale = 0f;
        B_Pausa.SetActive(false);
        B_Despausa.SetActive(true);
        FondoPanel.SetActive(true);
        OptionsInGame.SetActive(true);
        Exit.SetActive(true);
    }
    public void BotonReanudar()
    {
        Time.timeScale = 1f;
        B_Pausa.SetActive(true);
        B_Despausa.SetActive(false);
        FondoPanel.SetActive(false);
        OptionsInGame.SetActive(false);
        Exit.SetActive(false) ;
    }

}
