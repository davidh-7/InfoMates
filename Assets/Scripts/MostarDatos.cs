using UnityEngine;

public class MostrarDatos : MonoBehaviour
{
    void Start()
    {
        // Actualiza los textos una vez al inicio
        ActualizarTextos();
    }

    void Update()
    {
        // Actualiza los textos en cada fotograma
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        // Buscar el objeto de texto para los puntos
        GameObject textPuntos = GameObject.Find("TxtPuntos");
        if (textPuntos != null)
        {
            textPuntos.GetComponent<TMPro.TextMeshProUGUI>().text = "Puntos: " + DatosGlobales.puntos.ToString();
        }
        else
        {
            Debug.LogError("No se encontró el objeto TxtPuntos en la escena.");
        }

        // Buscar el objeto de texto para las vidas
        GameObject textVidas = GameObject.Find("TxtVidas");
        if (textVidas != null)
        {
            textVidas.GetComponent<TMPro.TextMeshProUGUI>().text = "Vidas: " + DatosGlobales.vidas.ToString();
        }
        else
        {
            Debug.LogError("No se encontró el objeto TxtVidas en la escena.");
        }
    }
}
