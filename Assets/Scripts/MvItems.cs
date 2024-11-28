using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MvItems : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image image;
    [HideInInspector] public Transform slotPrimerNum; // Slot original del número arrastrado
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
        Debug.Log("Begin Drag - Empezando el arrastre");

        slotPrimerNum = transform.parent; // Guardar el slot original
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Desactivar raycasts en este objeto mientras se arrastra
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Detectar el slot debajo del número arrastrado
        Transform slotDebajo = DetectarSlotDebajo();

        if (slotDebajo != null && slotDebajo != slotPrimerNum)
        {
            Debug.Log($"Slot detectado debajo: {slotDebajo.name}");

            // Buscar el número dentro del slot detectado
            ConNum otroNumero = slotDebajo.GetComponentInChildren<ConNum>();

            if (otroNumero != null)
            {
                Debug.Log($"Número detectado dentro del slot: {otroNumero.gameObject.name}");

                // Obtener el valor del número arrastrado y el número detectado
                ConNum miNumero = GetComponent<ConNum>();
                int valorMiNumero = miNumero.ObtenerNumeroSprite();
                int valorOtroNumero = otroNumero.ObtenerNumeroSprite();

                // Realizar la suma
                int suma = valorMiNumero + valorOtroNumero;
                Debug.Log($"Sumando los valores: {valorMiNumero} + {valorOtroNumero} = {suma}");

                // Acumular la suma total
                sumaTotal += suma;

                // Actualizar el texto del resultado
                Debug.Log("Suma acumulada: " + sumaTotal.ToString());
                GameObject textSuma = GameObject.Find("TxtResultado");
                textSuma.GetComponent<TextMeshProUGUI>().text = sumaTotal.ToString();

                // Eliminar ambos números
                Debug.Log("Eliminando los números combinados...");
                Destroy(gameObject); // Eliminar el número arrastrado
                Destroy(otroNumero.gameObject); // Eliminar el número detectado

                // Confirmar que los slots son distintos
                Debug.Log($"Slot 1: {slotPrimerNum.name}, Slot 2: {slotDebajo.name}");

                // Generar nuevos números en los slots afectados
                StartCoroutine(GenerarNumerosConRetraso(slotPrimerNum, slotDebajo));
            }
            else
            {
                Debug.Log("El slot detectado no contiene un número.");
                // Volver al slot original si el slot detectado está vacío
                transform.SetParent(slotPrimerNum);
                transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            Debug.Log("No se detectó un slot válido debajo o es el mismo slot original.");
            // Volver al slot original si no hay un slot válido debajo
            transform.SetParent(slotPrimerNum);
            transform.localPosition = Vector3.zero;
        }

        // Reactivar raycasts después de finalizar el arrastre
        image.raycastTarget = true;
    }

    private Transform DetectarSlotDebajo()
    {
        // Crear el PointerEventData para el raycast
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Lista para almacenar los resultados del Raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Desactivar raycasts en el objeto actual para evitar colisiones consigo mismo
        image.raycastTarget = false;

        // Realizar el Raycast
        raycaster.Raycast(pointerEventData, results);

        // Reactivar raycasts en el objeto actual
        image.raycastTarget = true;

        Debug.Log("Resultados del Raycast (Detección de Slots):");
        foreach (RaycastResult result in results)
        {
            Debug.Log($"Resultado detectado: {result.gameObject.name}");

            // Verificar si el objeto detectado es un slot
            if (result.gameObject.name.StartsWith("Slot")) // Cambia según el nombre de tus slots
            {
                return result.gameObject.transform; // Retornar el slot detectado
            }
        }

        Debug.Log("No se detectó ningún slot válido debajo.");
        return null; // Retornar null si no se detecta un slot válido
    }

    private IEnumerator GenerarNumerosConRetraso(Transform slot1, Transform slot2)
    {
        yield return new WaitForSeconds(0.1f);

        // Instanciar nuevo número en el primer slot
        Debug.Log($"Instanciando número en el primer slot: {slot1.name}");
        InstanciarNuevoNumero(slot1);

        // Instanciar nuevo número en el segundo slot
        Debug.Log($"Instanciando número en el segundo slot: {slot2.name}");
        InstanciarNuevoNumero(slot2);
    }

    private void InstanciarNuevoNumero(Transform parentSlot)
    {
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

        GameObject nuevoNumero = Instantiate(PrefabNumero, parentSlot);
        Debug.Log("Nuevo número instanciado en el slot: " + parentSlot.name);

        nuevoNumero.transform.localScale = Vector3.one;
        nuevoNumero.transform.localPosition = Vector3.zero; // Forzar la posición local a (0, 0, 0)

        nuevoNumero.SetActive(true);
        foreach (var component in nuevoNumero.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = true;
        }

        Image imageComponent = nuevoNumero.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.enabled = true;
            imageComponent.raycastTarget = true;
        }

        MvItems mvItems = nuevoNumero.GetComponent<MvItems>();
        if (mvItems != null) mvItems.enabled = true;

        ConNum conNum = nuevoNumero.GetComponent<ConNum>();
        if (conNum != null)
        {
            conNum.enabled = true;
            conNum.AsignarNumeroAleatorio();
        }

        Debug.Log("Nuevo número instanciado correctamente en el slot: " + parentSlot.name);
    }
}
