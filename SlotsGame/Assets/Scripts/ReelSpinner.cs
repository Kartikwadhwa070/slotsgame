using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelSpinner : MonoBehaviour
{
    [Header("Reel Setup")]
    public RectTransform reel;         // The parent RectTransform (Reel1)
    public Image[] symbols;            // Symbol1–4 (children)
    public Sprite[] symbolSprites;     // Optional, unused in this setup

    [Header("Spin Settings")]
    public float symbolHeight = 150f;  // Space between symbols
    public float baseSpinSpeed = 800f; // Base spin speed
    public float slowDownTime = 1.5f;  // How long it takes to decelerate

    private bool isSpinning = false;
    private int finalIndex = 0;

    // Randomized per-spin
    private float currentSpinSpeed;
    private int direction; // 1 = down, -1 = up

    public bool IsSpinning() => isSpinning;

    public void StartSpin(float spinDuration, int resultIndex)
    {
        if (!isSpinning)
            StartCoroutine(SpinRoutine(spinDuration, resultIndex));
    }

    private IEnumerator SpinRoutine(float spinDuration, int resultIndex)
    {
        isSpinning = true;

        // 🎲 Randomize reel behavior each spin
        currentSpinSpeed = Random.Range(baseSpinSpeed * 0.8f, baseSpinSpeed * 1.3f);
        direction = Random.value > 0.5f ? 1 : -1; // randomly spin up or down

        float elapsed = 0f;
        float currentSpeed = currentSpinSpeed;

        // --- Fast spin phase ---
        while (elapsed < spinDuration)
        {
            reel.anchoredPosition += new Vector2(0, direction * currentSpeed * Time.deltaTime);

            if (Mathf.Abs(reel.anchoredPosition.y) >= symbolHeight)
            {
                reel.anchoredPosition -= new Vector2(0, direction * symbolHeight);
                ShiftSymbols(direction);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // --- Slowdown phase ---
        float slowElapsed = 0f;
        while (slowElapsed < slowDownTime)
        {
            reel.anchoredPosition += new Vector2(0, direction * currentSpeed * Time.deltaTime);

            if (Mathf.Abs(reel.anchoredPosition.y) >= symbolHeight)
            {
                reel.anchoredPosition -= new Vector2(0, direction * symbolHeight);
                ShiftSymbols(direction);
            }

            slowElapsed += Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpinSpeed, 0, slowElapsed / slowDownTime);
            yield return null;
        }

        // --- Stop cleanly ---
        reel.anchoredPosition = Vector2.zero;
        finalIndex = resultIndex;
        isSpinning = false;
    }

    private void ShiftSymbols(int dir)
    {
        // 🔁 Loops symbols based on spin direction
        if (dir == 1) // moving downward
        {
            Sprite last = symbols[symbols.Length - 1].sprite;
            for (int i = symbols.Length - 1; i > 0; i--)
                symbols[i].sprite = symbols[i - 1].sprite;
            symbols[0].sprite = last;
        }
        else // moving upward
        {
            Sprite first = symbols[0].sprite;
            for (int i = 0; i < symbols.Length - 1; i++)
                symbols[i].sprite = symbols[i + 1].sprite;
            symbols[symbols.Length - 1].sprite = first;
        }
    }

    // ✅ Get the actual visible center symbol name for win checking
    public string GetCenterSymbolName()
    {
        return symbols[1].sprite != null ? symbols[1].sprite.name : "";
    }

    public int GetFinalResultIndex() => finalIndex;
}
