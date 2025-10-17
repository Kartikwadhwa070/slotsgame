using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    public ReelSpinner[] reels;
    public Button spinButton;
    public TextMeshProUGUI resultText;

    [Header("Spin Timing")]
    public float spinPhaseDuration = 2.0f; // how long the main spin phase runs

    private bool spinning = false;

    void Start()
    {
        if (spinButton != null)
            spinButton.onClick.AddListener(StartSpin);
        if (resultText != null)
            resultText.text = "";
    }

    void StartSpin()
    {
        if (spinning) return;
        spinning = true;
        if (resultText != null) resultText.text = "";

        StartCoroutine(SpinSequence());
    }

    IEnumerator SpinSequence()
    {
        // Start all reels spinning
        foreach (var reel in reels)
        {
            reel.StartSpin(spinPhaseDuration, 0); // 0 is unused (no randomization)
        }

        // Wait until all reels have finished spinning
        yield return new WaitUntil(() => AllReelsStopped());

        // Now check visible results
        CheckWin();
        spinning = false;
    }

    bool AllReelsStopped()
    {
        foreach (var reel in reels)
        {
            if (reel.IsSpinning())
                return false;
        }
        return true;
    }

    void CheckWin()
    {
        string firstSymbol = reels[0].GetCenterSymbolName();
        bool win = true;

        for (int i = 1; i < reels.Length; i++)
        {
            if (reels[i].GetCenterSymbolName() != firstSymbol)
            {
                win = false;
                break;
            }
        }

        resultText.text = win
            ? "<color=#FFD700><b> JACKPOT!</b></color>"
            : "<color=#FF5555>Try Again!</color>";

        Debug.Log("Results: " + string.Join(", ", System.Array.ConvertAll(reels, r => r.GetCenterSymbolName())));
    }
}
