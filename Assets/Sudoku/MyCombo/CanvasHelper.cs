using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasHelper : MonoBehaviour
{
    public float minAspect, maxAspect, minMatch, maxMatch;
    private CanvasScaler canvasScaler;

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        Update();
    }

    private void Update()
    {
        float aspect = Screen.width / (float)Screen.height;
        if (aspect > maxAspect) canvasScaler.matchWidthOrHeight = maxMatch;
        else if (aspect < minAspect) canvasScaler.matchWidthOrHeight = minMatch;
        else
        {
            canvasScaler.matchWidthOrHeight = (aspect - minAspect) / (maxAspect - minAspect) * (maxMatch - minMatch) + minMatch;
        }
    }
}
