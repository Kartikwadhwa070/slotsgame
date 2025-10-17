using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    public ReelSpinner[] reels;
    public Button spinButton;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    [Header("Spin Timing")]
    public float spinPhaseDuration = 2.0f;

    private bool spinning = false;
    private int totalScore = 0;

    void Start()
    {
        if (spinButton != null)
            spinButton.onClick.AddListener(StartSpin);

        if (resultText != null)
            resultText.text = "";

        UpdateScoreText();
    }

    void StartSpin()
    {
        if (spinning) return;
        spinning = true;
        resultText.text = "";
        StartCoroutine(SpinSequence());
    }

    IEnumerator SpinSequence()
    {
        foreach (var reel in reels)
            reel.StartSpin(spinPhaseDuration, 0);

        yield return new WaitUntil(() => AllReelsStopped());
        EvaluatePayout();
        spinning = false;
    }

    bool AllReelsStopped()
    {
        foreach (var reel in reels)
            if (reel.IsSpinning()) return false;
        return true;
    }

    // Calculates and displays rewards based on reel results
    void EvaluatePayout()
    {
        string[] visibleSymbols = new string[reels.Length];
        for (int i = 0; i < reels.Length; i++)
            visibleSymbols[i] = reels[i].GetCenterSymbolName();

        Debug.Log("Results: " + string.Join(", ", visibleSymbols));

        int payout = CalculatePayout(visibleSymbols);
        totalScore += payout;
        UpdateScoreText();

        resultText.text = payout > 0
            ? $"<color=#FFD700><b>+{payout} POINTS!</b></color>"
            : "<color=#FF5555>Try Again!</color>";
    }

    int CalculatePayout(string[] symbols)
    {
        if (symbols.Length < 3) return 0;

        string s1 = symbols[0];
        string s2 = symbols[1];
        string s3 = symbols[2];

        if (s1 == "7" && s2 == "7" && s3 == "7") return 500;
        if (s1 == "Bell" && s2 == "Bell" && s3 == "Bell") return 200;
        if (s1 == "Bar" && s2 == "Bar" && s3 == "Bar") return 150;
        if (s1 == "Cherry" && s2 == "Cherry" && s3 == "Cherry") return 100;

        if (s1 == s2 || s2 == s3 || s1 == s3)
        {
            string matched = s1 == s2 ? s1 : s2 == s3 ? s2 : s1;
            switch (matched)
            {
                case "7": return 50;
                case "Cherry": return 30;
                case "Bell": return 25;
                case "Bar": return 20;
            }
        }

        return 0;
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"Score: <b>{totalScore}</b>";
    }
}
