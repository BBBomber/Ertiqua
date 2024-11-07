using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using UnityEngine.Rendering.Universal;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private Material uiShaderMaterial; // Reference to the shader material
    [SerializeField] private string shaderProperty = "progress"; // Name of the shader property
    [SerializeField] private float duration = 2f; // Default duration for transitions
    [SerializeField] private Canvas Current; // Default duration for transitions
    [SerializeField] private Camera OverlayCurrent; // Default duration for transitions

    public Action OnTransitionInComplete;  // Event for when the transition in completes (0 to 1)
    public Action OnTransitionOutComplete; // Event for when the transition out completes (1 to 0)

    private Tween currentTween;

    // Import JavaScript functions from the WebGL plugin
    [DllImport("__Internal")]
    private static extern void OnStartSceneTransition();

    [DllImport("__Internal")]
    private static extern void OnStartSceneTransitionComplete();

    [DllImport("__Internal")]
    private static extern void OnEndSceneTransition();

    [DllImport("__Internal")]
    private static extern void OnEndSceneTransitionComplete();
    Camera[] AllCamera = new Camera[10];

    private void Update()
    {
        /*if (Input.GetKey(KeyCode.Space))
        {
            StartTransitionIn();
        }*/
        Camera.GetAllCameras(AllCamera);
        if (AllCamera.FirstOrDefault(t => t.gameObject.activeSelf) != null)
        {
            Camera Maliputlated = AllCamera.FirstOrDefault(t => t.gameObject.activeSelf && t.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Base);
            var cameraData = Maliputlated.GetUniversalAdditionalCameraData();
            if (cameraData.cameraStack.Count == 0)
            {
                cameraData.cameraStack.Add(OverlayCurrent);
            }
        }
        //if (Current.worldCamera == null && Current.worldCamera != AllCamera.FirstOrDefault(t => t.gameObject.activeSelf))
        //{
        //    Current.worldCamera = AllCamera.FirstOrDefault(t => t.gameObject.activeSelf);
        //}

        /*if (Input.GetKey(KeyCode.LeftShift)) { StartTransitionOut(); }*/
    }
    // Called at the start of the scene transition
    public void StartTransitionIn()
    {
        // Cancel any running tween
        if (currentTween != null) currentTween.Kill();

#if UNITY_WEBGL && !UNITY_EDITOR
        // Notify JavaScript that the start scene transition is beginning
        OnStartSceneTransition();
#endif

        // Start the transition from 0 to 1
        currentTween = DOTween.To(() => uiShaderMaterial.GetFloat(shaderProperty),
                                  value => uiShaderMaterial.SetFloat(shaderProperty, value),
                                  0f,
                                  duration)
                              .OnComplete(() =>
                              {
                                  OnTransitionInComplete?.Invoke(); // Trigger the event
#if UNITY_WEBGL && !UNITY_EDITOR
                                  // Notify JavaScript that the start scene transition is complete
                                  OnStartSceneTransitionComplete();
#endif
                              });
    }

    // Called at the end of the scene transition
    public void StartTransitionOut()
    {
        // Cancel any running tween
        if (currentTween != null) currentTween.Kill();

#if UNITY_WEBGL && !UNITY_EDITOR
        // Notify JavaScript that the end scene transition is beginning
        OnEndSceneTransition();
#endif

        // Start the transition from 1 to 0
        currentTween = DOTween.To(() => uiShaderMaterial.GetFloat(shaderProperty),
                                  value => uiShaderMaterial.SetFloat(shaderProperty, value),
                                  1f,
                                  duration)
                              .OnComplete(() =>
                              {
                                  OnTransitionOutComplete?.Invoke(); // Trigger the event
                                                                     // Notify JavaScript that the end scene transition is complete
#if UNITY_WEBGL && !UNITY_EDITOR
                                  // Notify JavaScript that the end scene transition is complete
                                  OnEndSceneTransitionComplete();
#endif
                              });
    }


    // Cleanup when the object is destroyed
    private void OnDestroy()
    {
        if (currentTween != null) currentTween.Kill();
    }
}
