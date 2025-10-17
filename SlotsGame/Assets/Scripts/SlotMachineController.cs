using UnityEngine;
using UnityEngine.UI;
using TMPro; // ✅ Required for TextMeshPro
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    [System.Serializable]
    public class Reel
    {
        public Image display;
        public Sprite[] symbols;
        [HideInInspector] public int currentIndex;
    }

    [Header("Reels Setup")]
    public Reel[] reels;

    [Header("UI")]
    public Button spinButton;
    public TextMeshProUGUI resultText; // ✅ Changed from Text → TextMeshProUGUI

    [Header("Spin Settings")]
    public float spinDuration = 2f;
    public float spinSpeed = 0.1f;

    private bool isSpinning = false;

    void Start()
    {
        spinButton.onClick.AddListener(StartSpin);
        resultText.text = "";
    }

    void StartSpin()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        resultText.text = "";

        float timer = 0f;

        // Random final results for each reel
        int[] results = new int[reels.Length];
        for (int i = 0; i < reels.Length; i++)
            results[i] = Random.Range(0, reels[i].symbols.Length);

        // Animate spinning
        while (timer < spinDuration)
        {
            for (int i = 0; i < reels.Length; i++)
            {
                int index = Random.Range(0, reels[i].symbols.Length);
                reels[i].display.sprite = reels[i].symbols[index];
            }

            timer += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        // Stop and set to final results
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].currentIndex = results[i];
            reels[i].display.sprite = reels[i].symbols[results[i]];
        }

        CheckResult();
        isSpinning = false;
    }

    void CheckResult()
    {
        bool allSame = true;
        int first = reels[0].currentIndex;

        for (int i = 1; i < reels.Length; i++)
        {
            if (reels[i].currentIndex != first)
            {
                allSame = false;
                break;
            }
        }

        if (allSame)
        {
            resultText.text = "<color=#FFD700>🎉 JACKPOT!</color>"; // ✅ Styled TMP text
        }
        else
        {
            resultText.text = "<color=#FF5555>Try Again!</color>";
        }
    }
}
