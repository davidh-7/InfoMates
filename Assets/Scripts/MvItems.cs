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
    public GameObject PrefabNumero; // Prefab del número

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
            Destroy(gameObject); // Elimina el objeto actual
            Destroy(otroNumero.gameObject); // Elimina el objeto con el otro número

            // Crear un nuevo número en el slot del número eliminado
            InstanciarNuevoNumero(parentDesMov);
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

    private void InstanciarNuevoNumero(Transform parentSlot)
    {
        // Verificar que el prefab y el slot padre no sean nulos
        if (PrefabNumero == null)
        {
            Debug.LogError("El prefab 'PrefabNumero' no está asignado en el inspector.");
            return;
        }

        if (parentSlot == null)
        {
            Debug.LogError("El slot padre (parentSlot) es nulo. No se puede instanciar un nuevo número.");
            return;
        }

        // Instanciar el prefab en la escena
        GameObject nuevoNumero = Instantiate(PrefabNumero, parentSlot);

        // Asegurar que el prefab tenga la escala correcta y esté centrado en el slot
        nuevoNumero.transform.localScale = Vector3.one;
        nuevoNumero.transform.localPosition = Vector3.zero;

        // Asegurarse de que el prefab y todos sus componentes estén habilitados
        nuevoNumero.SetActive(true); // Asegurar que el GameObject está activo
        foreach (var component in nuevoNumero.GetComponentsInChildren<Behaviour>()) // Incluye todos los componentes Behaviour (scripts, Image, etc.)
        {
            component.enabled = true; // Habilitar cada componente
        }

        // Verificar que el prefab tiene los scripts necesarios
        MvItems mvItems = nuevoNumero.GetComponent<MvItems>();
        ConNum conNum = nuevoNumero.GetComponent<ConNum>();

        if (mvItems == null || conNum == null)
        {
            Debug.LogError("El prefab 'Numeros' debe tener los scripts 'MvItems' y 'ConNum'.");
            return;
        }

        // Asegurarse de que el script ConNum asigne un número aleatorio correctamente
        conNum.AsignarNumeroAleatorio();

        // Log de confirmación
        Debug.Log("Nuevo número instanciado correctamente en el slot: " + parentSlot.name);
    }
}
