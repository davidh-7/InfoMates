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
    public GameObject PrefabNumero; // Agrega el prefab del número aquí

    private Image[] casillas;

    // Variable estática para almacenar la suma total compartida entre todas las instancias de MvItems
    private static int sumaTotal = 0;

    void Start()
    {
        // Obtenemos el GraphicRaycaster y el EventSystem
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        // Llamar a BuscarHijos en cada frame
        BuscarHijos();
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

            // Llamar al método para buscar los hijos
            BuscarHijos();
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

    // Metodo para buscar los slots y si tiene hijos
    void BuscarHijos()
    {
        // Encontrar todos los objetos con el tag "Slot"
        GameObject[] objetosConTag = GameObject.FindGameObjectsWithTag("Slot");

        // Crear un array para almacenar solo los componentes Image
        casillas = new Image[objetosConTag.Length];

        // Rellenar el array con los componentes Image de cada objeto
        for (int i = 0; i < objetosConTag.Length; i++)
        {
            casillas[i] = objetosConTag[i].GetComponent<Image>();

            if (casillas[i] == null)
            {
                Debug.LogWarning("El objeto " + objetosConTag[i].name + " no tiene un componente Image.");
            }
            else
            {
                // Verificar si el objeto tiene hijos
                int numeroHijos = objetosConTag[i].transform.childCount;

                if (numeroHijos > 0)
                {
                    Debug.Log("El objeto " + objetosConTag[i].name + " tiene " + numeroHijos + " hijo(s).");
                }
                else
                {
                    Debug.Log("El objeto " + objetosConTag[i].name + " no tiene hijos.");

                    // Instanciar el PrefabNumero como hijo del slot actual
                    GameObject nuevoNumero = Instantiate(PrefabNumero, objetosConTag[i].transform);

                    // Configurar la posición local para centrarlo en el slot
                    nuevoNumero.transform.localPosition = Vector3.zero;

                    // Asegurarse de que el prefab tenga el script ConNum y que este se inicialice correctamente
                    ConNum conNum = nuevoNumero.GetComponent<ConNum>();
                    if (conNum != null)
                    {
                        Debug.Log("Número creado y configurado en el slot: " + objetosConTag[i].name);
                    }
                    else
                    {
                        Debug.LogWarning("El PrefabNumero no tiene un componente ConNum.");
                    }
                }
            }
        }
    }
}
