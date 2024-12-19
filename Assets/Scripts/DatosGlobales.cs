using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosGlobales : MonoBehaviour
{
    // Variables est�ticas que mantienen los valores globales
    public static int vidas = 5;  // Vidas iniciales
    public static int puntos = 0; // Puntos iniciales
    public static int monedero = 2000; // Puntos iniciales

    // Instancia est�tica de la clase DatosGlobales
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
