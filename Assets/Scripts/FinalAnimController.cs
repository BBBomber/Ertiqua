using UnityEngine;
using DG.Tweening;

public class FinalAnimController : MonoBehaviour
{
    public Material skyboxMaterial;  // Assign your Skybox material in the Inspector
    public float targetExposure = 0.5f;  // The exposure value to lerp to
    public float duration = 2.0f;        // Duration for the lerp
    public bool startanim = false;       // Trigger for the animation
    public GameObject text;              // Text object to animate
    public GameObject[] fireworks;

    public float textYOffset = 130.0f;     // The target Y offset for the text position animation (in world space)
    private float originalExposure;

    public Animator textController;

    void Start()
    {
        if (skyboxMaterial != null && text != null)
        {
            // Store the original exposure value
            originalExposure = skyboxMaterial.GetFloat("_Exposure");
        }
    }

    void OnDisable()
    {
        // Reset material properties when the object is destroyed or disabled (e.g., play mode ends)
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", originalExposure);
        }

        // Reset the text position to the original position
        if (text != null)
        {
            text.transform.DOKill();  // Kill any active DOTween animations on this object
        }
    }

    private void Update()
    {
        if (startanim)
        {
            TriggerLerp();
            startanim = false;
        }

    }

    // This function is called to trigger the exposure change and animate the skybox exposure
    public void TriggerLerp()
    {
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.Paused = true;
        }
        PlayAnimationTrigger("move");
        // Animate the exposure using DOTween
        // Animate the exposure using DOTween and call SetFireworksActive() when done
        DOTween.To(() => skyboxMaterial.GetFloat("_Exposure"),
                   x => skyboxMaterial.SetFloat("_Exposure", x),
                   targetExposure, duration)
               .OnComplete(SetFireworksActive);
    }

    // Call this function to set the trigger
    public void PlayAnimationTrigger(string triggerName)
    {
        if (textController != null)
        {
            // Set the trigger in the Animator Controller
            textController.SetTrigger(triggerName);
            Debug.Log("trigger called");
        }
    }

    private void SetFireworksActive()
    {
        foreach (GameObject firework in fireworks)
        {
            if (firework != null)
            {
                firework.SetActive(true);  // Activate the firework GameObject
            }
        }
        Debug.Log("Fireworks activated");
    }


}
