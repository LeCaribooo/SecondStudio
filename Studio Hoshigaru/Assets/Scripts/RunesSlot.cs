using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunesSlot : MonoBehaviour, IPointerDownHandler
{
    public GameObject RunesManager;
    public bool isOccupied;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOccupied)
        {
            GameObject getted = eventData.pointerEnter;
            getted.GetComponent<Runes>().Choose(false);
            getted.GetComponent<RectTransform>().SetParent(RunesManager.transform);
            isOccupied = false;
        }
        
    }
}
