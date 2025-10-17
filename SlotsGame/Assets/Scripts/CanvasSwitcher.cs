using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    [Header("Canvas References")]
    public Canvas[] canvases;
    private Canvas activeCanvas;

    void Start()
    {
        if (canvases.Length > 0)
            ShowCanvas(canvases[0].name);
    }

    // Switch to a canvas by its name
    public void ShowCanvas(string canvasName)
    {
        bool found = false;

        foreach (var canvas in canvases)
        {
            if (canvas == null) continue;

            bool isTarget = canvas.name.Equals(canvasName, System.StringComparison.OrdinalIgnoreCase);
            canvas.gameObject.SetActive(isTarget);

            if (isTarget)
            {
                activeCanvas = canvas;
                found = true;
                Debug.Log(" Switched to canvas: " + canvas.name);
            }
        }

        if (!found)
            Debug.LogWarning(" Canvas not found: " + canvasName);
    }

    // Switch to a canvas directly using a reference
    public void ShowCanvas(Canvas targetCanvas)
    {
        if (targetCanvas == null)
        {
            Debug.LogWarning(" Target canvas is null!");
            return;
        }

        foreach (var canvas in canvases)
        {
            if (canvas == null) continue;

            bool isTarget = canvas == targetCanvas;
            canvas.gameObject.SetActive(isTarget);

            if (isTarget)
            {
                activeCanvas = canvas;
                Debug.Log("Switched to canvas: " + canvas.name);
            }
        }
    }

    // Returns the currently active canvas
    public Canvas GetActiveCanvas()
    {
        return activeCanvas;
    }
}
