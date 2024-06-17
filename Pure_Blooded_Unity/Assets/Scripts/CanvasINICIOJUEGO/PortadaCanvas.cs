using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortadaCanvas : MonoBehaviour
{
    // ESCENA 0 PORTADA
    public GameObject B_Play;
    public GameObject B_Options;
    public GameObject B_Exit;

    // ESCENA 1 GAME
    public GameObject B_Pausa;
    public GameObject B_Despausa;
    public GameObject FondoPanel;
    public GameObject OptionsInGame;
    public GameObject Exit;

    // ESCENA 2 MENU OPTIONS
    public GameObject B_Options_de1a2;

    // Variable estática para almacenar el nombre de la escena anterior
    private static string ultimaEscena = "";
    private static string escenaActual = "";
    // Booleana para manejar la lógica de volver de la escena 2
    private static bool VengoDeLa2 = false;

    private void Start()
    {
        if (B_Pausa != null)
        {
            Time.timeScale = 1f;
            B_Pausa.SetActive(true);
            B_Despausa.SetActive(false);
            FondoPanel?.SetActive(false);
            OptionsInGame?.SetActive(false);
            Exit?.SetActive(false);
        }
    }
    private void Update()
    {
        // Guardo la escenaActual como el nombre de la escenaActual
        escenaActual = (SceneManager.GetActiveScene().name);

        ComprobacionEscenaAnterior();
    }

    public void CambiarEscena(string escena)
    {
        // Antes de cambiar la escena, almacenamos el nombre de la escena actual
        ultimaEscena = escenaActual;
        SceneManager.LoadSceneAsync(escena);
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
        FondoPanel?.SetActive(true);
        OptionsInGame?.SetActive(true);
        Exit?.SetActive(true);
    }

    public void BotonReanudar()
    {
        Time.timeScale = 1f;
        B_Pausa.SetActive(true);
        B_Despausa.SetActive(false);
        FondoPanel?.SetActive(false);
        OptionsInGame?.SetActive(false);
        Exit?.SetActive(false);
    }

    public void ComprobacionEscenaAnterior()
    {
        if (escenaActual == "2_MenúOptions" && ultimaEscena == "0_PortadaArranque") // Si estoy en Options y vengo de portada
        {
            B_Play.SetActive(true);
            B_Options_de1a2.SetActive(false);
        }
        else if (escenaActual == "2_MenúOptions" && ultimaEscena == "1.1_Juego") // Si estoy en Options y vengo del Juego
        {
            B_Play.SetActive(false);
            B_Options_de1a2.SetActive(true);
        }
        /*else if (escenaActual == "2_MenúOptions" && ultimaEscena == "2_MenúOptions" && !VengoDeLa2)
        {
            Time.timeScale = 0f;
            B_Pausa.SetActive(true);
            B_Despausa.SetActive(true);
            FondoPanel?.SetActive(true);
            OptionsInGame?.SetActive(true);
            Exit?.SetActive(true);
            VengoDeLa2 = true;
        }
        else if (VengoDeLa2 && escenaActual == "2_MenúOptions")
        {
            VengoDeLa2 = false;
        }*/
    }
}
