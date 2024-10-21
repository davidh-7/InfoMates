using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemPool : MonoBehaviour, IDropHandler//interfaz
{
    public void OnDrop(PointerEventData eventData)
    {
   ConDrag.itemDragging.transform.SetParent(transform);
    }
}
