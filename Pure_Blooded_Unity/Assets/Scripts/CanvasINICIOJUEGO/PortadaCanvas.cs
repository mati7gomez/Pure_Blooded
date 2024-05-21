using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PortadaCanvas : MonoBehaviour
{
    // ESCENA 0 PORTADA
    public GameObject B_Play;
    public GameObject B_Options;
    public GameObject B_Exit;
    // Hay que asignar este metodo al boton en la escena
    // ESCENA 1 GAME
    public GameObject B_Pausa;
    public GameObject B_Despausa;
    // public Image B_Despausa_image;
    public GameObject FondoPanel;
    public GameObject OptionsInGame;
    public GameObject Exit;
    // ESCENA 2 MENU OPTIONS
    public GameObject B_Options_de1a2;

    // Variable estática para almacenar el índice de la escena anterior
    private static int previousSceneIndex = -1;
    // Booleana para que todo vaya bien
    private static bool VengoDeLa2 = false;

    private void Start()
    {
        if (B_Pausa != null)
        {
            Time.timeScale = 1f;
            B_Pausa.SetActive(true);
            B_Despausa.SetActive(false);
            if (FondoPanel != null) FondoPanel.SetActive(false);
            if (OptionsInGame != null) OptionsInGame.SetActive(false);
            if (Exit != null) Exit.SetActive(false);
        }
    }

    private void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 2 && previousSceneIndex == 0) // Si estoy en Options y vengo de portada
        {
            B_Play.SetActive(true);
            B_Options_de1a2.SetActive(false);
        }
        else if (currentSceneIndex == 2 && previousSceneIndex == 1) // Si estoy en Options y vengo de Game
        {
            B_Play.SetActive(false);
            B_Options_de1a2.SetActive(true);
        }
        else if (currentSceneIndex == 1 && previousSceneIndex == 2 && !VengoDeLa2)
        {
            Time.timeScale = 0f;
            B_Pausa.SetActive(false);
            B_Despausa.SetActive(true);
            FondoPanel.SetActive(true);
            OptionsInGame.SetActive(true);
            Exit.SetActive(true);
            VengoDeLa2 = true;
        }
        else if (VengoDeLa2 && currentSceneIndex == 2)
        {
            VengoDeLa2 = false;
        }
    }

    public void CambiarEscena(int escena)
    {
        // Antes de cambiar la escena, almacenamos el índice de la escena actual
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        FondoPanel.SetActive(false);    //if (FondoPanel != null) FondoPanel.SetActive(false);
        OptionsInGame.SetActive(false); // if (OptionsInGame != null) OptionsInGame.SetActive(false);
        Exit.SetActive(false);          // if (Exit != null) Exit.SetActive(false);

    }

}
