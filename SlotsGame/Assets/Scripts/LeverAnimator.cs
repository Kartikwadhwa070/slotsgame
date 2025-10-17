using UnityEngine;
using UnityEngine.UI;

public class LeverAnimator : MonoBehaviour
{
    [Header("References")]
    public Sprite leverUpSprite;                 // slot-machine2.png
    public Sprite leverDownSprite;               // slot-machine3.png
    public SlotMachineController slotController; // Reference to SlotMachineController

    [Header("Settings")]
    public float pullDownTime = 0.2f;    // How long lever stays down before returning
    public bool autoReturn = true;       // Should it automatically go back up?

    private Image leverImage;            // Reference to the Image component
    private bool isPulled = false;

    void Awake()
    {
        leverImage = GetComponent<Image>();
        if (leverImage == null)
        {
            Debug.LogError(" No Image component found on this object!");
        }
        else
        {
            Debug.Log(" Found Image component: " + leverImage.name);
        }
    }

    void Start()
    {
        if (leverImage != null && leverUpSprite != null)
        {
            leverImage.sprite = leverUpSprite;
            Debug.Log("[LeverAnimator] Lever initialized to UP sprite.");
        }
        else
        {
            Debug.LogWarning("[LeverAnimator] Lever image or up sprite not assigned.");
        }
    }

    // This should be assigned in your Button's OnClick() event
    public void PullLever()
    {
        if (isPulled)
        {
            Debug.Log(" Lever already pulled — ignoring extra click.");
            return;
        }

        isPulled = true;
        Debug.Log(" Lever pulled!");

        // Switch to down sprite
        if (leverImage != null && leverDownSprite != null)
        {
            leverImage.sprite = leverDownSprite;
            Debug.Log("Lever sprite set to DOWN.");
        }
        else
        {
            Debug.LogWarning(" Lever image or down sprite not assigned!");
        }

        // Trigger slot machine spin
        if (slotController != null)
        {
            Debug.Log(" Sending spin command to SlotMachineController...");
            slotController.SendMessage("StartSpin", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.LogWarning(" No SlotMachineController assigned!");
        }

        // Auto return lever up
        if (autoReturn)
        {
            Debug.Log("Lever will return up in " + pullDownTime + " seconds.");
            Invoke(nameof(ReturnLever), pullDownTime);
        }
    }

    void ReturnLever()
    {
        if (leverImage != null && leverUpSprite != null)
        {
            leverImage.sprite = leverUpSprite;
            Debug.Log(" Lever returned to UP position.");
        }
        else
        {
            Debug.LogWarning(" Lever image or up sprite missing when trying to return!");
        }

        isPulled = false;
    }
}
