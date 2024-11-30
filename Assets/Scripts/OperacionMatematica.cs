using UnityEngine;
using TMPro;

public class OperacionMatematica : MonoBehaviour
{
    public TextMeshProUGUI Operaciones;  // Campo de texto que muestra el resultado deseado
    public TextMeshProUGUI Simbolo;      // Campo de texto que muestra el símbolo de la operación

    private int resultadoDeseado;        // Almacena el resultado deseado
    private string simboloMatematico;    // Almacena el símbolo de la operación

    void Start()
    {
        // Generar una operación aleatoria al inicio del juego
        GenerarOperacionAleatoria();
    }

    public void GenerarOperacionAleatoria()
    {
        // Determinar el tipo de operación: 0 -> Suma, 1 -> Resta, 2 -> Multiplicación
        int tipoOperacion = Random.Range(0, 3);

        switch (tipoOperacion)
        {
            case 0: // Suma
                simboloMatematico = "+";
                // Generar un resultado deseado razonable que pueda alcanzarse sumando números entre 1 y 9
                resultadoDeseado = Random.Range(5, 50);  // Ajustar el rango según la dificultad deseada
                break;

            case 1: // Resta
                simboloMatematico = "-";
                // Generar un resultado deseado que pueda alcanzarse restando números entre 1 y 9
                resultadoDeseado = Random.Range(0, 9);  // Asegurarse de que sea un resultado alcanzable
                break;

            case 2: // Multiplicación
                simboloMatematico = "*";
                // Generar un resultado deseado razonable que pueda alcanzarse multiplicando números entre 1 y 9
                resultadoDeseado = Random.Range(1, 81);  // 9 * 9 = 81 es el valor máximo posible con números entre 1 y 9
                break;

            default:
                simboloMatematico = "+";
                resultadoDeseado = 10;  // Valor por defecto en caso de error
                break;
        }

        // Actualizar los campos de texto con la operación generada
        Operaciones.text = resultadoDeseado.ToString();
        Simbolo.text = simboloMatematico;
    }

    // Métodos para acceder al resultado y símbolo desde otros scripts
    public int ObtenerResultadoDeseado()
    {
        return resultadoDeseado;
    }

    public string ObtenerSimboloMatematico()
    {
        return simboloMatematico;
    }
}
