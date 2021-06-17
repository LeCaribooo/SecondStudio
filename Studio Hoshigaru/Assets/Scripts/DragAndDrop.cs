using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 defaultPos;
    [HideInInspector] public bool droppedOnSlot;
    [SerializeField] private Canvas canvas;
    private Runes rune;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        defaultPos = rectTransform.localPosition;
        droppedOnSlot = false;
        rune = GetComponent<Runes>();
    }

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        rune.Choose(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(checkIfDropped());
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator checkIfDropped()
    {
        yield return new WaitForEndOfFrame();
        if (!droppedOnSlot)
            rectTransform.localPosition = defaultPos;
        droppedOnSlot = false;
    }
}
