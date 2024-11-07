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

    // Variable estática para almacenar la suma total compartida entre todas las instancias de MvItems
    private static int sumaTotal = 0;

    void Start()
    {
        // Obtenemos el GraphicRaycaster y el EventSystem
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        parentDesMov = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Mover");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Soltar");

        // Detectar si hay otro número en el slot donde soltamos
        ConNum otroNumero = DetectarNumeroDebajo();

        if (otroNumero != null)
        {
            // Obtener el valor numérico del objeto actual y el otro número detectado
            ConNum miNumero = GetComponent<ConNum>();
            int valorMiNumero = miNumero.ObtenerNumeroSprite();
            int valorOtroNumero = otroNumero.ObtenerNumeroSprite();

            // Sumar ambos valores
            int suma = valorMiNumero + valorOtroNumero;

            // Acumular la suma en la variable estática sumaTotal
            sumaTotal += suma;

            // Mostrar la suma acumulada en TxtResultado
            Debug.Log("Suma acumulada:" + sumaTotal.ToString());
            GameObject textSuma = GameObject.Find("TxtResultado");
            textSuma.GetComponent<TMPro.TextMeshProUGUI>().text = sumaTotal.ToString();

            // Eliminar ambos objetos
            Destroy(otroNumero.gameObject); // Elimina el objeto con el otro número
            Destroy(gameObject); // Elimina el objeto actual
        }
        else
        {
            // Volver al padre original si no hay otro número
            transform.SetParent(parentDesMov);
        }

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
}
