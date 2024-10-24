using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MvItems : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentDesMov;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        // Obtenemos el GraphicRaycaster y el EventSystem
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
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

        // Detectamos si hay una imagen debajo
        DetcImgDebajo();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Soltar");
        transform.SetParent(parentDesMov);
        image.raycastTarget = true;
    }

    private void DetcImgDebajo()
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
            // Si el resultado es una imagen y no es la que estamos arrastrando
            if (result.gameObject != gameObject && result.gameObject.GetComponent<Image>() != null)
            {
                Debug.Log("Imagen detectada debajo: " + result.gameObject.name);
                // Aquí puedes realizar la lógica que desees cuando detectes una imagen debajo
            }
        }
    }
}
