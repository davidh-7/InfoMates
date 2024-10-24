using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnNvMst : MonoBehaviour
{
    // Referencias a los botones de historia e instrucciones
    public GameObject botonHistoria;
    public GameObject botonInstrucciones;
    private bool mostrantDesplegable;
    public void start()
    {
        mostrantDesplegable = false;
    }
    // Método que será llamado al presionar el botón principal
    public void Mostrar()
    {
        mostrantDesplegable = !mostrantDesplegable;

        if (mostrantDesplegable)
        {
            botonHistoria.SetActive(true);
            botonInstrucciones.SetActive(true);
        }
        else 
        {
            botonHistoria.SetActive(false);
            botonInstrucciones.SetActive(false);
        }
        
    }
    
}
