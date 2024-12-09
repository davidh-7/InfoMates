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

    // Referencia al campo de texto que muestra el símbolo matemático
    private TextMeshProUGUI simboloText;
    private OperacionMatematica operacionMatematica;

    // Referencia al controlador de la papelera
    private ConPapelera conPapelera;

    // Variable estática para almacenar el resultado total compartido entre todas las instancias de MvItems
    private static int resultadoTotal = 0;

    void Start()
    {
        // Obtenemos el GraphicRaycaster y el EventSystem
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // Buscar automáticamente el TextField llamado "Simbolo" en la escena
        if (simboloText == null)
        {
            simboloText = GameObject.Find("Simbolo").GetComponent<TextMeshProUGUI>();
            if (simboloText == null)
            {
                Debug.LogError("No se pudo encontrar el campo de texto 'Simbolo' en la escena.");
            }
        }

        // Buscar automáticamente el script OperacionMatematica
        operacionMatematica = GameObject.FindObjectOfType<OperacionMatematica>();
        if (operacionMatematica == null)
        {
            Debug.LogError("No se pudo encontrar el script 'OperacionMatematica' en la escena.");
        }

        // Buscar automáticamente el controlador de la papelera
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

        // Detectar si el objeto se soltó sobre la papelera
        GameObject objetoDebajo = DetectarObjetoDebajo();
        if (objetoDebajo != null && objetoDebajo.CompareTag("Papelera"))
        {
            Debug.Log("Número soltado en la papelera.");
            // Intentar usar la papelera
            if (conPapelera != null && conPapelera.UsarPapelera())
            {
                Debug.Log("Número eliminado mediante la papelera.");
                Destroy(gameObject);
                return;
            }
            else
            {
                Debug.Log("No se pudo eliminar el número porque la papelera no tiene más usos. Volviendo al slot original.");
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
                    Debug.LogError("No se asignó correctamente alguna referencia necesaria.");
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
                        Debug.LogError("Símbolo matemático no soportado: " + simbolo);
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
                    Debug.Log("¡Te has pasado del resultado deseado! Reiniciando operación.");

                    // Reiniciar el resultado total
                    resultadoTotal = 0;

                    // Actualizar el texto del resultado en pantalla
                  textResultado = GameObject.Find("TxtResultado");
                    if (textResultado != null)
                    {
                        textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();
                    }

                    // Reiniciar la operación matemática y generar una nueva operación
                    if (operacionMatematica != null)
                    {
                        operacionMatematica.GenerarOperacionAleatoria(); // Llama al método que genera una nueva operación
                        Debug.Log("Nueva operación generada.");
                    }
                }
                else if (resultadoTotal == operacionMatematica.ObtenerResultadoDeseado())
                {
                    // Si el resultado acumulado es igual al resultado deseado

                    Debug.Log("¡Has alcanzado el resultado deseado! Generando nueva operación.");

                    // Reiniciar el resultado total
                    resultadoTotal = 0;

                    // Actualizar el texto del resultado en pantalla
                   textResultado = GameObject.Find("TxtResultado");
                    if (textResultado != null)
                    {
                        textResultado.GetComponent<TMPro.TextMeshProUGUI>().text = resultadoTotal.ToString();
                    }

                    // Reiniciar la operación matemática y generar una nueva operación
                    if (operacionMatematica != null)
                    {
                        operacionMatematica.GenerarOperacionAleatoria(); // Llama al método que genera una nueva operación
                        Debug.Log("Nueva operación generada.");
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
        // Método para volver al slot original
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
                    return conNum; // Retornamos el otro número si lo encontramos
                }
            }
        }

        return null; // Retorna null si no se encuentra ningún otro número
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

        return null; // Retorna null si no se encuentra ningún objeto debajo
    }
}
