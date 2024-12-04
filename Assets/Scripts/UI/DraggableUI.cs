using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Transform current = transform.parent;

        while (current != null)
        {
            canvas = current.GetComponent<Canvas>();
            if (canvas != null)
                break;

            current = current.parent;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Bring to top
        if (eventData.button == PointerEventData.InputButton.Left)
            rectTransform.SetAsLastSibling();
    }
}
