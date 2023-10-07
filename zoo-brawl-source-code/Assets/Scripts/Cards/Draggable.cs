using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Initialises a variable to store the canvas, and RectTransform, CanvasGroup components
    Canvas canvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    // Initialises a variable to store the objects original parent
    Transform originalParent = null;

    private void Awake()
    {
        // Gets the component for each of the component variables and gets the current canvas in the scene
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // When beginning a drag, sets its opacity to half and does not allow the game object to block raycasting
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        // Sets the original parent to its current parent object
        originalParent = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // When the object is dragged, follow the mouse taking into account the scale of the canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // When finished dragging, set the opacity to 1 and allows the game object to blocks raycasting
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Sets parent to the new parent
        transform.SetParent(originalParent);
    }

    public void setOriginalParent(Transform originalParent)
    {
        // Sets parent to original parent
        this.originalParent = originalParent;
    }
}
