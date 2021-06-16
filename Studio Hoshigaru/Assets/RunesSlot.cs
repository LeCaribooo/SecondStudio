using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunesSlot : MonoBehaviour, IDropHandler
{
    private Runes actualRune;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<DragAndDrop>().droppedOnSlot = true;
            actualRune = eventData.pointerDrag.GetComponent<Runes>();
            actualRune.Choose(true);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

    
}
