using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumberDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform rt;
    private int currentNumber = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        HandleDrag(eventData);
    }

    private void HandleDrag(PointerEventData eventData)
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        var localPosition = transform.InverseTransformPoint(worldPosition);

        int number = Mathf.FloorToInt(localPosition.x / (rt.rect.width / 9f)) + 1;
        int row = Mathf.FloorToInt(localPosition.y / rt.rect.height);

        if (row == 0 && number >= 1 && number <= 9 && number != currentNumber)
        {
            SudokuManager.intance.SetNumber(number);
            currentNumber = number;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentNumber = -1;
    }
}
