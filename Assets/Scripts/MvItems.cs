using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MvItems : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentDesMov;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    // Referencia al campo de texto que muestra el s�mbolo matem�tico
    private TextMeshProUGUI simboloText;
    private OperacionMatematica operacionMatematica;

    // Referencia al controlador de la papelera
    private ConPapelera conPapelera;

    // Variable est�tica para almacenar el resultado total compartido entre todas las instancias de MvItems
    private static int resultadoTotal = 0;

    void Start()
    {
        // Obtenemos el GraphicRaycaster y el EventSystem
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // Buscar autom�ticamente el TextField llamado "Simbolo" en la escena
        if (simboloText == null)
        {
            simboloText = GameObject.Find("Simbolo").GetComponent<TextMeshProUGUI>();
            if (simboloText == null)
            {
                Debug.LogError("No se pudo encontrar el campo de texto 'Simbolo' en la escena.");
            }
        }

        // Buscar autom�ticamente el script OperacionMatematica
        operacionMatematica = GameObject.FindObjectOfType<OperacionMatematica>();
        if (operacionMatematica == null)
        {
            Debug.LogError("No se pudo encontrar el script 'OperacionMatematica' en la escena.");
        }

        // Buscar autom�ticamente el controlador de la papelera
        conPapelera = GameObject.FindObjectOfType<ConPapelera>();
        if (conPapelera == null)
        {
            Debug.LogError("No se pudo encontrar el script 'ConPapelera' en la escena.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        parentDesMov = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;  // Desactivamos raycast para que no interfiera mientras arrastramos
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Mover");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Soltar");

        // Detectar si el objeto se solt� sobre la papelera
        GameObject objetoDebajo = DetectarObjetoDebajo();
        if (objetoDebajo != null && objetoDebajo.CompareTag("Papelera"))
        {
            Debug.Log("N�mero soltado en la papelera.");
            // Intentar usar la papelera
            if (conPapelera != null && conPapelera.UsarPapelera())
            {
                Debug.Log("N�mero eliminado mediante la papelera.");
                Destroy(gameObject);
                return; // Finaliza la operaci�n si el objeto fue destruido
            }
            else
            {
                // No hay usos disponibles, volver al padre original
                Debug.Log("No se pudo eliminar el n�mero porque la papelera no tiene m�s usos. Volviendo al slot original.");
                VolverAlPadreOriginal();
            }
        }
        else
        {
            // Detectar si hay otro n�mero en el slot donde soltamos
            ConNum otroNumero = DetectarNumeroDebajo();

            if (otroNumero != null)
            {
                // Obtener el valor num�rico del objeto actual y el otro n�mero detectado
                ConNum miNumero = GetComponent<ConNum>();
                int valorMiNumero = miNumero.ObtenerNumeroSprite();
                int valorOtroNumero = otroNumero.ObtenerNumeroSprite();

                // Verificar si se ha asignado la referencia del s�mbolo matem�tico
                if (simboloText == null || operacionMatematica == null)
                {
                    Debug.LogError("No se asign� correctamente alguna referencia necesaria.");
                    image.raycastTarget = true;  // Asegurarnos de volver a activar raycastTarget
                    return;
                }

                // Realizar la operaci�n dependiendo del s�mbolo matem�tico
                string simbolo = simboloText.text;
                int resultado = 0;

                switch (simbolo)
                {
                    case "+":
                        resultado = valorMiNumero + valorOtroNumero;
                        break;

                    case "-":
                        // Asegurar que siempre restemos el mayor menos el menor para evitar resultados negativos
                        resultado = Mathf.Abs(valorMiNumero - valorOtroNumero);
                        break;

                    case "*":
                        resultado = valorMiNumero * valorOtroNumero;
                        break;

                    default:
                        Debug.LogError("S�mbolo matem�tico no soportado: " + simbolo);
                        image.raycastTarget = true;  // Asegurarnos de volver a activar raycastTarget
                        return;
                }

                // Acumular el resultado en la variable est�tica resultadoTotal
                resultadoTotal += resultado;

                // Mostrar el resultado acumulado en TxtResultado
                Debug.Log("Resultado acumulado: " + resultadoTotal.ToString());
                GameObject textResultado = GameObject.Find("TxtResultado");
                textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();

                // Verificar si el resultado acumulado coincide con el resultado deseado
                if (resultadoTotal == operacionMatematica.ObtenerResultadoDeseado())
                {
                    Debug.Log("�Operaci�n exitosa! Se alcanz� el resultado deseado.");
                    // Generar una nueva operaci�n
                    operacionMatematica.GenerarOperacionAleatoria();
                    // Reiniciar el resultado acumulado para la siguiente operaci�n
                    resultadoTotal = 0;
                }

                // Eliminar ambos objetos
                Destroy(otroNumero.gameObject); // Elimina el objeto con el otro n�mero
                Destroy(gameObject); // Elimina el objeto actual
            }
            else
            {
                // Volver al padre original si no hay otro n�mero
                VolverAlPadreOriginal();
            }
        }

        // Reactivar el raycastTarget del componente de Image para asegurar que se pueda volver a interactuar
        image.raycastTarget = true;
    }

    private void VolverAlPadreOriginal()
    {
        // M�todo para volver al slot original
        transform.SetParent(parentDesMov);
        transform.position = parentDesMov.position;

        // Reactivar raycastTarget para poder interactuar con el objeto de nuevo
        image.raycastTarget = true;
    }

    private ConNum DetectarNumeroDebajo()
    {
        // Creamos el PointerEventData
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Lista para almacenar los resultados del Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Realizamos el Raycast
        raycaster.Raycast(pointerEventData, results);

        // Verificamos los resultados
        foreach (RaycastResult result in results)
        {
            // Si el resultado es una imagen y no es el objeto actual que estamos arrastrando
            if (result.gameObject != gameObject)
            {
                // Intentamos obtener el componente ConNum del objeto debajo
                ConNum conNum = result.gameObject.GetComponent<ConNum>();
                if (conNum != null)
                {
                    return conNum; // Retornamos el otro n�mero si lo encontramos
                }
            }
        }

        return null; // Retorna null si no se encuentra ning�n otro n�mero
    }

    private GameObject DetectarObjetoDebajo()
    {
        // Creamos el PointerEventData
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Lista para almacenar los resultados del Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Realizamos el Raycast
        raycaster.Raycast(pointerEventData, results);

        // Verificamos los resultados
        foreach (RaycastResult result in results)
        {
            // Retornamos el primer objeto debajo que no sea el mismo que estamos arrastrando
            if (result.gameObject != gameObject)
            {
                return result.gameObject;
            }
        }

        return null; // Retorna null si no se encuentra ning�n objeto debajo
    }
}
