using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    private RectTransform parentRt;
    public RectTransform rt;
    private GridLayoutGroup gridLayout;

    private void Start()
    {
        parentRt = transform.parent.GetComponent<RectTransform>();
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Update()
    {
        float cellSize = parentRt.rect.width / 3f;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
