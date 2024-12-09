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
                return;
            }
            else
            {
                Debug.Log("No se pudo eliminar el n�mero porque la papelera no tiene m�s usos. Volviendo al slot original.");
                VolverAlPadreOriginal();
            }
        }
        else
        {
            ConNum otroNumero = DetectarNumeroDebajo();
            if (otroNumero != null)
            {
                ConNum miNumero = GetComponent<ConNum>();
                int valorMiNumero = miNumero.ObtenerNumeroSprite();
                int valorOtroNumero = otroNumero.ObtenerNumeroSprite();

                if (simboloText == null || operacionMatematica == null)
                {
                    Debug.LogError("No se asign� correctamente alguna referencia necesaria.");
                    image.raycastTarget = true;
                    return;
                }

                string simbolo = simboloText.text;
                int resultado = 0;

                switch (simbolo)
                {
                    case "+":
                        resultado = valorMiNumero + valorOtroNumero;
                        break;

                    case "-":
                        resultado = Mathf.Abs(valorMiNumero - valorOtroNumero);
                        break;

                    case "*":
                        resultado = valorMiNumero * valorOtroNumero;
                        break;

                    default:
                        Debug.LogError("S�mbolo matem�tico no soportado: " + simbolo);
                        image.raycastTarget = true;
                        return;
                }

                resultadoTotal += resultado;

                Debug.Log("Resultado acumulado: " + resultadoTotal);
                GameObject textResultado = GameObject.Find("TxtResultado");
                textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();

                //contrala las opereraciones 

                if (resultadoTotal > operacionMatematica.ObtenerResultadoDeseado())
                {
                    Debug.Log("�Te has pasado del resultado deseado! Reiniciando operaci�n.");

                    // Reiniciar el resultado total
                    resultadoTotal = 0;

                    // Actualizar el texto del resultado en pantalla
                  textResultado = GameObject.Find("TxtResultado");
                    if (textResultado != null)
                    {
                        textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();
                    }

                    // Reiniciar la operaci�n matem�tica y generar una nueva operaci�n
                    if (operacionMatematica != null)
                    {
                        operacionMatematica.GenerarOperacionAleatoria(); // Llama al m�todo que genera una nueva operaci�n
                        Debug.Log("Nueva operaci�n generada.");
                    }
                }
                else if (resultadoTotal == operacionMatematica.ObtenerResultadoDeseado())
                {
                    // Si el resultado acumulado es igual al resultado deseado

                    Debug.Log("�Has alcanzado el resultado deseado! Generando nueva operaci�n.");

                    // Reiniciar el resultado total
                    resultadoTotal = 0;

                    // Actualizar el texto del resultado en pantalla
                   textResultado = GameObject.Find("TxtResultado");
                    if (textResultado != null)
                    {
                        textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();
                    }

                    // Reiniciar la operaci�n matem�tica y generar una nueva operaci�n
                    if (operacionMatematica != null)
                    {
                        operacionMatematica.GenerarOperacionAleatoria(); // Llama al m�todo que genera una nueva operaci�n
                        Debug.Log("Nueva operaci�n generada.");
                    }
                }


                Destroy(otroNumero.gameObject);
                Destroy(gameObject);
            }
            else
            {
                VolverAlPadreOriginal();
            }
        }

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
