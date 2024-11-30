using UnityEngine;
using TMPro;

public class OperacionMatematica : MonoBehaviour
{
    public TextMeshProUGUI Operaciones;  // Campo de texto que muestra el resultado deseado
    public TextMeshProUGUI Simbolo;      // Campo de texto que muestra el s�mbolo de la operaci�n

    private int resultadoDeseado;        // Almacena el resultado deseado
    private string simboloMatematico;    // Almacena el s�mbolo de la operaci�n

    void Start()
    {
        // Generar una operaci�n aleatoria al inicio del juego
        GenerarOperacionAleatoria();
    }

    public void GenerarOperacionAleatoria()
    {
        // Determinar el tipo de operaci�n: 0 -> Suma, 1 -> Resta, 2 -> Multiplicaci�n
        int tipoOperacion = Random.Range(0, 3);

        switch (tipoOperacion)
        {
            case 0: // Suma
                simboloMatematico = "+";
                // Generar un resultado deseado razonable que pueda alcanzarse sumando n�meros entre 1 y 9
                resultadoDeseado = Random.Range(5, 50);  // Ajustar el rango seg�n la dificultad deseada
                break;

            case 1: // Resta
                simboloMatematico = "-";
                // Generar un resultado deseado que pueda alcanzarse restando n�meros entre 1 y 9
                resultadoDeseado = Random.Range(0, 9);  // Asegurarse de que sea un resultado alcanzable
                break;

            case 2: // Multiplicaci�n
                simboloMatematico = "*";
                // Generar un resultado deseado razonable que pueda alcanzarse multiplicando n�meros entre 1 y 9
                resultadoDeseado = Random.Range(1, 81);  // 9 * 9 = 81 es el valor m�ximo posible con n�meros entre 1 y 9
                break;

            default:
                simboloMatematico = "+";
                resultadoDeseado = 10;  // Valor por defecto en caso de error
                break;
        }

        // Actualizar los campos de texto con la operaci�n generada
        Operaciones.text = resultadoDeseado.ToString();
        Simbolo.text = simboloMatematico;
    }

    // M�todos para acceder al resultado y s�mbolo desde otros scripts
    public int ObtenerResultadoDeseado()
    {
        return resultadoDeseado;
    }

    public string ObtenerSimboloMatematico()
    {
        return simboloMatematico;
    }
}
