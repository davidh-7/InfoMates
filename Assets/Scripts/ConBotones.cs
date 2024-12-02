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

}

