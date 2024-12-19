using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosGlobales : MonoBehaviour
{
    // Variables estáticas que mantienen los valores globales
    public static int vidas = 3;  // Vidas iniciales
    public static int puntos = 0; // Puntos iniciales
    public static int monedero = 500; // Puntos iniciales

    // Instancia estática de la clase DatosGlobales
    public static DatosGlobales instancia;

    void Awake()
    {
        // Verificamos si ya existe una instancia de DatosGlobales
        if (instancia == null)
        {
            // Si no existe, asignamos esta instancia
            instancia = this;
            // Aseguramos que no se destruya al cargar una nueva escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe, destruimos el objeto duplicado
            Destroy(gameObject);
        }
    }
}
