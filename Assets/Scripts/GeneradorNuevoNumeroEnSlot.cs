using UnityEngine;
using UnityEngine.EventSystems;

public class GeneradorNuevoNumeroEnSlot : MonoBehaviour, IDropHandler
{
    public GameObject prefabNumero;  // Prefab del número a generar (con ConNum y MvItems)

    private GameObject numeroActual; // Referencia al número instanciado en este slot

    void Start()
    {
        // Verificamos que este objeto tenga el tag "Slot"
        if (CompareTag("Slot"))
        {
            // Generamos un número en el Slot al inicio del juego si el objeto tiene el tag correcto
            GenerarNuevoNumeroEnSlot();
        }
        else
        {
            Debug.LogWarning("Este objeto no tiene el tag 'Slot'.");
        }
    }

    // Método que genera un nuevo número en el Slot si está vacío
    public void GenerarNuevoNumeroEnSlot()
    {
        if (prefabNumero != null)
        {
            // Si no hay un número en el slot, generamos uno nuevo
            if (numeroActual == null)
            {
                // Instanciar el nuevo número y almacenarlo en la variable 'numeroActual'
                numeroActual = Instantiate(prefabNumero, transform.position, Quaternion.identity, transform);
                Debug.Log("Nuevo número generado en el slot con tag Slot: " + gameObject.name);
            }
        }
    }

    // Este método se llama cuando un número es soltado sobre el Slot
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        // Si el número ha sido soltado en un slot diferente y este slot está vacío
        if (droppedObject != null && droppedObject.transform.parent != transform)
        {
            // Si el slot está vacío, generamos un nuevo número
            if (numeroActual == null)
            {
                GenerarNuevoNumeroEnSlot();
            }
        }
    }

    // Método para detectar cuando el número es arrastrado fuera del Slot
    public void NumeroMovido()
    {
        // Eliminar la referencia al número actual porque se está moviendo
        numeroActual = null;
        Debug.Log("Número movido desde el slot con tag Slot: " + gameObject.name);
    }

    // Método para verificar si el slot está vacío cada vez que se mueve un número
    void Update()
    {
        // Si el número fue movido y el slot está vacío, generamos un nuevo número
        if (numeroActual == null)
        {
            GenerarNuevoNumeroEnSlot();
        }
    }
}
