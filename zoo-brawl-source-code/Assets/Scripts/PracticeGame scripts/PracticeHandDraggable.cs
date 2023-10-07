using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PracticeHandDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Canvas canvas;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    Transform originalParent = null;

    public int handPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        originalParent = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent);

        PracticeGameObjectManager.playerInteract = false;
    }

    public void setOriginalParent(Transform originalParent)
    {
        this.originalParent = originalParent;
    }
}
