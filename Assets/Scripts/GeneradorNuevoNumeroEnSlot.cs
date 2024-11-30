using UnityEngine;
using UnityEngine.EventSystems;

public class GeneradorNuevoNumeroEnSlot : MonoBehaviour, IDropHandler
{
    public GameObject prefabNumero;  // Prefab del n�mero a generar (con ConNum y MvItems)

    private GameObject numeroActual; // Referencia al n�mero instanciado en este slot

    void Start()
    {
        // Verificamos que este objeto tenga el tag "Slot"
        if (CompareTag("Slot"))
        {
            // Generamos un n�mero en el Slot al inicio del juego si el objeto tiene el tag correcto
            GenerarNuevoNumeroEnSlot();
        }
        else
        {
            Debug.LogWarning("Este objeto no tiene el tag 'Slot'.");
        }
    }

    // M�todo que genera un nuevo n�mero en el Slot si est� vac�o
    public void GenerarNuevoNumeroEnSlot()
    {
        if (prefabNumero != null)
        {
            // Si no hay un n�mero en el slot, generamos uno nuevo
            if (numeroActual == null)
            {
                // Instanciar el nuevo n�mero y almacenarlo en la variable 'numeroActual'
                numeroActual = Instantiate(prefabNumero, transform.position, Quaternion.identity, transform);
                Debug.Log("Nuevo n�mero generado en el slot con tag Slot: " + gameObject.name);
            }
        }
    }

    // Este m�todo se llama cuando un n�mero es soltado sobre el Slot
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        // Si el n�mero ha sido soltado en un slot diferente y este slot est� vac�o
        if (droppedObject != null && droppedObject.transform.parent != transform)
        {
            // Si el slot est� vac�o, generamos un nuevo n�mero
            if (numeroActual == null)
            {
                GenerarNuevoNumeroEnSlot();
            }
        }
    }

    // M�todo para detectar cuando el n�mero es arrastrado fuera del Slot
    public void NumeroMovido()
    {
        // Eliminar la referencia al n�mero actual porque se est� moviendo
        numeroActual = null;
        Debug.Log("N�mero movido desde el slot con tag Slot: " + gameObject.name);
    }

    // M�todo para verificar si el slot est� vac�o cada vez que se mueve un n�mero
    void Update()
    {
        // Si el n�mero fue movido y el slot est� vac�o, generamos un nuevo n�mero
        if (numeroActual == null)
        {
            GenerarNuevoNumeroEnSlot();
        }
    }
}
