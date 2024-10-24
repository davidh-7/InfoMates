
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MvEnItems : MonoBehaviour, IDropHandler

{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        MvItems mvItems = dropped.GetComponent<MvItems>();
        mvItems.parentDesMov = transform;

    }
}
