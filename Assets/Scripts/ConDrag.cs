using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler //Interfazes 
{

    public static GameObject itemDragging;

    //Variables
    Vector2 startPosition;
    Transform startParent;
    Transform dragParent;

    void Start()
    {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform ;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        itemDragging = gameObject;

        startPosition = transform.position;
        startParent = transform.parent;

        transform.SetParent(dragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;// posicion del item = mouse

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        itemDragging = null;

        if (transform.parent == dragParent) { 
        transform.position = startPosition;
            transform.SetParent (startParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
