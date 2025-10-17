using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelSpinner : MonoBehaviour
{
    [Header("Reel Setup")]
    public RectTransform reel;
    public Image[] symbols;
    public Sprite[] symbolSprites;

    [Header("Spin Settings")]
    public float symbolHeight = 150f;
    public float baseSpinSpeed = 800f;
    public float slowDownTime = 1.5f;

    private bool isSpinning = false;
    private int finalIndex = 0;
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
        currentSpinSpeed = Random.Range(baseSpinSpeed * 0.8f, baseSpinSpeed * 1.3f);
        direction = Random.value > 0.5f ? 1 : -1;

        float elapsed = 0f;
        float currentSpeed = currentSpinSpeed;

        //  Fast spin 
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

        //  Slow down 
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

        //  Stop cleanly 
        reel.anchoredPosition = Vector2.zero;
        finalIndex = resultIndex;
        isSpinning = false;
    }

    private void ShiftSymbols(int dir)
    {
        if (dir == 1)
        {
            Sprite last = symbols[symbols.Length - 1].sprite;
            for (int i = symbols.Length - 1; i > 0; i--)
                symbols[i].sprite = symbols[i - 1].sprite;
            symbols[0].sprite = last;
        }
        else
        {
            Sprite first = symbols[0].sprite;
            for (int i = 0; i < symbols.Length - 1; i++)
                symbols[i].sprite = symbols[i + 1].sprite;
            symbols[symbols.Length - 1].sprite = first;
        }
    }

    public string GetCenterSymbolName()
    {
        return symbols[1].sprite != null ? symbols[1].sprite.name : "";
    }

    public int GetFinalResultIndex() => finalIndex;

}
