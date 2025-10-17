using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    [Header("Canvas References")]
    public Canvas[] canvases; // All canvases in your scene (e.g. MainMenu, Game, Settings)

    private Canvas activeCanvas;

    void Start()
    {
        // Optional: activate the first one by default
        if (canvases.Length > 0)
        {
            ShowCanvas(canvases[0].name);
        }
    }

    /// <summary>
    /// Switch to a canvas by its name.
    /// </summary>
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
                Debug.Log("[CanvasSwitcher] 🎬 Switched to canvas: " + canvas.name);
            }
        }

        if (!found)
            Debug.LogWarning("[CanvasSwitcher] ⚠ Canvas not found with name: " + canvasName);
    }

    /// <summary>
    /// Switch to a canvas by reference (if you already have the Canvas object).
    /// </summary>
    public void ShowCanvas(Canvas targetCanvas)
    {
        if (targetCanvas == null)
        {
            Debug.LogWarning("[CanvasSwitcher] ⚠ Target canvas is null!");
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
                Debug.Log("[CanvasSwitcher] 🎬 Switched to canvas: " + canvas.name);
            }
        }
    }

    /// <summary>
    /// Get the currently active canvas.
    /// </summary>
    public Canvas GetActiveCanvas()
    {
        return activeCanvas;
    }
}
