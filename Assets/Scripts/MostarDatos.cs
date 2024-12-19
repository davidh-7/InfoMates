using UnityEngine;

public class MostrarDatos : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textPuntos;
    private TMPro.TextMeshProUGUI textVidas;
    private TMPro.TextMeshProUGUI textCoins;


    void Start()
    {
        // Buscar los objetos de texto solo una vez al inicio
        GameObject objPuntos = GameObject.Find("TxtPuntos");
        if (objPuntos != null)
        {
            textPuntos = objPuntos.GetComponent<TMPro.TextMeshProUGUI>();
        }

        GameObject objVidas = GameObject.Find("TxtVidas");
        if (objVidas != null)
        {
            textVidas = objVidas.GetComponent<TMPro.TextMeshProUGUI>();
        }

        GameObject objCoins = GameObject.Find("TxtCoins");
        if (objCoins != null)
        {
            textCoins = objCoins.GetComponent<TMPro.TextMeshProUGUI>();
        }

        // Actualizar los textos iniciales
        ActualizarTextos();
    }

    void Update()
    {
        // Actualiza los textos si los campos existen
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        // Actualizar texto de puntos si el campo existe
        if (textPuntos != null)
        {
            textPuntos.text = "Punts: " + DatosGlobales.puntos.ToString();
        }

        // Actualizar texto de vidas si el campo existe
        if (textVidas != null)
        {
            textVidas.text = "Vides: " + DatosGlobales.vidas.ToString();
        }

        // Actualizar texto de vidas si el campo existe
        if (textCoins != null)
        {
            textCoins.text = "Monedes: " + DatosGlobales.monedero.ToString();
        }
    }
}
