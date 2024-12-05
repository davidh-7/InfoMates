using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConBotones : MonoBehaviour
{
    //Global
    public void BotonExit()
    {
        SceneManager.LoadScene("EscenaInicio");
    }
    //Inicio
    public void BotonJuego() {
        SceneManager.LoadScene("EscenaNiveles");
    }
    public void BotonHistoria()
    {
        SceneManager.LoadScene("EscenaHistoria");
    }
    public void BotonInstrucciones()
    {
        SceneManager.LoadScene("EscenaInstrucciones");
    }

    //Niveles
    public void BotonNv1()
    {
        SceneManager.LoadScene("ScenaNivel");
    }
    public void BotonTienda()
    {
        SceneManager.LoadScene("EscenaTienda");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Para detener el juego en el editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Para cerrar el juego en una compilación
        Application.Quit();
#endif
    }


}

