using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WidthLayout : MonoBehaviour {

    public bool padding;
    public float paddingValue;
    public bool maxWidth;
    public float maxWidthValue;

    private RectTransform rt, parentRt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        parentRt = transform.parent.GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionsChange()
    {
        Update();
    }

    private void Update()
    {
        if (parentRt == null) return;

        float width = rt.sizeDelta.x;
        if (padding)
        {
            width = parentRt.rect.width - paddingValue * 2;
        }

        if (maxWidth && width > maxWidthValue)
        {
            width = maxWidthValue;
        }

        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);

        if (rt.pivot.x == 0)
        {
            rt.localPosition = new Vector3(-width / 2, rt.localPosition.y, rt.localPosition.z);
        }
    }
}
