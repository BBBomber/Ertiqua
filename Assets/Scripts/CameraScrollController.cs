using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System;

public class CameraScrollController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string stateName = "CameraMovement"; // The name of the state in the Animator
    public float scrollSpeed = 0.005f; // Adjusted scroll speed factor
    public float keyPressScrollSpeed = 0.2f; // Adjusted scroll speed for key press
    public float inertiaDecay = 0.5f; // Inertia decay factor
    private float animationProgress = 0f; // Track the current position in the animation
    private float animationVelocity = 0f; // This will act like velocity for inertia
    public float threshold = 0.95f; // Scene change threshold
    public bool toggleScroll = true;
    public GameDataManager manager;
    private string currentScenee;
    private bool ChangeCalled;
    private Dictionary<int, float> hotspotTimings1 = new Dictionary<int, float>
        {

            { 1, 0.056f },
            { 2, 0.119f },
            { 3, 0.16f },
            { 4, 0.27f },
            { 5, 0.34f }

        };
    private Dictionary<int, float> hotspotTimings2 = new Dictionary<int, float>
        {

            { 1, 0.032f },
            { 2, 0.118f },
            { 3, 0.205f },
            { 4, 0.3184f },
            { 5, 0.407f },
            { 6, 0.523f },
            { 7, 0.628f },
            { 8, 0.764f },
            { 9, 0.864f },
            { 10, 0.944f },
        };

    private Dictionary<int, float> hotspotTimings3 = new Dictionary<int, float>
        {
            { 1, 0.1f },
            { 2, 0.3f },
            { 3, 0.5f },
            { 4, 0.7f },
            { 5, 0.9f },
        };
    private Dictionary<int, float> hotspotTimings4 = new Dictionary<int, float>
        {
            { 1, 0.150f },
            { 2, 0.350f },
            { 3, 0.550f },
            { 4, 0.750f },
            { 5, 0.950f },
        };

    private Dictionary<int, float> hotspotTimings5 = new Dictionary<int, float>
        {
            { 1, 0.09f },
            { 2, 0.192f },
            { 3, 0.284f },
            { 4, 0.373f },
            { 5, 0.47f },
            { 6, 0.55f },
            { 7, 0.66f },
            { 8, 0.74f },
            { 9, 0.82f },
            { 10, 0.90f },
        };
    private Dictionary<int, float> hotspotTimings6 = new Dictionary<int, float>
        {
            { 1, 0.0625f },
            { 2, 0.127f },
            { 3, 0.193f },
            { 4, 0.257f },
            { 5, 0.322f },
            { 6, 0.387f },
            { 7, 0.452f },
            { 8, 0.518f },
            { 9, 0.583f },
            { 10, 0.648f },
            { 11, 0.714f },
            { 12, 0.778f },
            { 13, 0.844f },
            { 14, 0.908f },
            { 15, 0.97f },
        };

    private Dictionary<int, float> hotspotTimings7 = new Dictionary<int, float>
        {
            { 1, 0.159f },
            { 2, 0.290f },
            { 3, 0.437f },
            { 4, 0.591f },
            { 5, 0.708f },
        };


    private HashSet<int> triggeredHotspots = new HashSet<int>();

    public GameObject[] lightPrefabs; // Array of hotspot prefabs

    private List<HotspotData> hotspots;

    [DllImport("__Internal")]
    private static extern void SendHotspotEventJS(string json);

    [DllImport("__Internal")]
    private static extern void SceneInitializedJS(string sceneName);

    [DllImport("__Internal")]
    private static extern void UnityLoaded();

    [DllImport("__Internal")]
    private static extern void SequenceEnded();

    static bool LoadedFirstTime = false;

    private void Awake()
    {
        if (!LoadedFirstTime)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            UnityLoaded();
#endif
            LoadedFirstTime = true;
        }
    }

    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif

        StartCoroutine(WaitForDataAndInitialize());
    }

    private IEnumerator WaitForDataAndInitialize()
    {
        if (GameDataManager.Instance != null)
        {
            // Wait until the GameDataManager instance is ready and data is initialized
            while (!GameDataManager.Instance.isDataReady)
            {
                //Debug.Log("Waiting for GameDataManager to be ready...");
                yield return null; // Wait for the next frame before checking again
            }
        }

        // Once data is ready, initialize the scene
        //Debug.Log("GameDataManager is ready, initializing scene.");
        InitializeScene();
    }

    private void InitializeScene()
    {
        // Get current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;
        currentScenee = currentSceneName;
        Debug.Log(currentSceneName);

        // Fetch scene data from GameDataManager
        SceneData currentSceneData = GameDataManager.Instance?.sceneSequenceData?.sceneSequence
            .Find(scene => scene.scene == currentSceneName);

        foreach (var prefab in lightPrefabs)
        {
            prefab.SetActive(false);
        }

        if (currentSceneData != null)
        {


            Debug.Log(currentSceneData.initialPosition);
            // Set initial camera position
            InitCameraFrame(currentSceneData.initialPosition);


            hotspots = currentSceneData.hotspots;




            // If hotspots are not null and contain data, activate the necessary lightPrefabs
            if (hotspots != null && hotspots.Count > 0 && lightPrefabs != null)
            {
                for (int i = 0; i < hotspots.Count && i < lightPrefabs.Length; i++)
                {
                    lightPrefabs[i].SetActive(hotspots[i].active);
                }
            }

            // Notify website that the scene has initialized
            SceneInitialized(currentSceneName);

        }
        else
        {
            Debug.Log("Scene data not found for current scene: " + currentSceneName);
        }

        if (GameDataManager.Instance != null)
        {
            if (GameDataManager.Instance.LoadingSpace != null)
            {
                SetCameraIndex(GameDataManager.Instance.LoadingSpace.Value);
                GameDataManager.Instance.LoadingSpace = null;
            }
        }

    }

    private void InitCameraFrame(int initialPosition)
    {
        bool found = false;
        if (currentScenee == "Scene1")
        {
            if (hotspotTimings1.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene2")
        {
            if (hotspotTimings2.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene3")
        {
            if (hotspotTimings3.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene4")
        {
            if (hotspotTimings4.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene5")
        {
            if (hotspotTimings5.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene6")
        {
            if (hotspotTimings6.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }
        else if (currentScenee == "Scene7")
        {
            if (hotspotTimings7.TryGetValue(initialPosition, out float progress))
            {
                animationProgress = progress;
                found = true;
            }
        }

        if (!found)
        {
            animationProgress = 0f; // Fallback to default value
        }

    }

    void Update()
    {
        if (GameDataManager.Instance != null)
        {
            if (GameDataManager.Instance.Paused)
            {
                return;
            }
        }
        if (toggleScroll)
        {
            // Handle scrolling
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKey(KeyCode.UpArrow))
            {
                scrollInput += keyPressScrollSpeed;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                scrollInput -= keyPressScrollSpeed;
            }

            animationVelocity += scrollInput * scrollSpeed;
            animationVelocity *= inertiaDecay;
            animationProgress += animationVelocity;
            animationProgress = Mathf.Clamp(animationProgress, 0f, .9999f);
        }

        //Debug.Log(animationProgress);
        // Play animation based on progress
        for (int i = 0; i < ((currentScenee == "Scene4") ? 3 : 1); i++)
        {
            animator.Play(stateName, i, animationProgress);
        }
        CheckForHotspotEvents();

        // Change scene if threshold is reached
        if (animationProgress >= threshold)
        {
            if (!ChangeCalled)
            {
                ChangeScene();
                ChangeCalled = true;
            }
        }
        else
        {
            ChangeCalled = false;
        }
    }

    Dictionary<int, float> GetTimimg()
    {
        Dictionary<int, float> hotspotTimings = null;

        if (currentScenee == "Scene1")
        {
            hotspotTimings = hotspotTimings1;
        }
        else if (currentScenee == "Scene2")
        {
            hotspotTimings = hotspotTimings2;
        }
        else if (currentScenee == "Scene3")
        {
            hotspotTimings = hotspotTimings3;
        }
        else if (currentScenee == "Scene4")
        {
            hotspotTimings = hotspotTimings4;
        }
        else if (currentScenee == "Scene5")
        {
            hotspotTimings = hotspotTimings5;
        }
        else if (currentScenee == "Scene6")
        {
            hotspotTimings = hotspotTimings6;
        }
        else if (currentScenee == "Scene7")
        {
            hotspotTimings = hotspotTimings7;
        }
        return hotspotTimings;
    }

    private void CheckForHotspotEvents()
    {

        Dictionary<int, float> hotspotTimings = GetTimimg();

        if (hotspotTimings == null)
            return;

        foreach (var kvp in hotspotTimings)
        {
            int hotspotIndex = kvp.Key;
            float triggerProgress = kvp.Value;

            // Check if the hotspotIndex is within the range of the hotspots list
            if (hotspots != null)
            {
                if (hotspotIndex - 1 >= 0 && hotspotIndex - 1 < hotspots.Count() &&
                    animationProgress >= triggerProgress &&
                    !triggeredHotspots.Contains(hotspotIndex))
                {
                    TriggerHotspotEvent(hotspotIndex);
                    triggeredHotspots.Add(hotspotIndex);
                }
            }

        }
    }


    // This method will be called by Animation Events
    public void TriggerHotspotEvent(int hotspotIndex)
    {
        int adjustedIndex = hotspotIndex - 1;
        Debug.Log($"Hotspot Index: {hotspotIndex}, Adjusted Index: {adjustedIndex}");

        if (hotspots != null && adjustedIndex >= 0 && adjustedIndex < hotspots.Count())
        {
            var hotspotData = hotspots[adjustedIndex];

            // Serialize hotspot data to JSON
            string hotspotJson = JsonConvert.SerializeObject(hotspotData);
            Debug.Log("Hotspot Event: " + hotspotJson);

            // Call the JavaScript function
#if UNITY_WEBGL && !UNITY_EDITOR
            SendHotspotEventJS(hotspotJson);
#endif
        }
        else
        {
            Debug.LogError($"Invalid hotspot index: {hotspotIndex}. Adjusted index: {adjustedIndex}. Hotspots count: {hotspots?.Count() ?? 0}");
        }
    }

    private void SceneInitialized(string sceneName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SceneInitializedJS(sceneName);
#else
        Debug.Log("Scene Initialized: " + sceneName);
#endif
    }

    // Function to change the scene once animation progress reaches threshold
    private void ChangeScene()
    {
        Debug.Log(nameof(ChangeScene));
        GameDataManager GDMInstance = GameDataManager.Instance;
        if (GDMInstance == null || GDMInstance.sceneSequenceData == null)
        {
            return;
        }
        int SceneCount = GDMInstance.sceneSequenceData.sceneSequence.Count;
        int ScenetoLoad = GDMInstance.CurrentIndex + 1;

        if (ScenetoLoad < SceneCount)
        {
            GDMInstance.TCInstance.OnTransitionOutComplete = () =>
            {
                SceneManager.LoadScene(GDMInstance.sceneSequenceData.sceneSequence[ScenetoLoad].scene);
                GDMInstance.CurrentIndex++;
                GDMInstance.TCInstance.OnTransitionOutComplete = null;
                GDMInstance.TCInstance.StartTransitionIn();
            };
            GDMInstance.TCInstance.StartTransitionOut();
        }
        else
        {
            //Scene Finished
#if !UNITY_EDITOR && UNITY_WEBGL
            SequenceEnded();
#endif
        }
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int totalScenes = SceneManager.sceneCountInBuildSettings;

        //if (currentSceneIndex < totalScenes - 1)
        //{
        //    SceneManager.LoadScene(currentSceneIndex + 1);
        //}
        //else
        //{
        //    SceneManager.LoadScene(0);
        //}
    }

    public void SetCameraIndex(int index)
    {
        Dictionary<int, float> hotspotTimings = GetTimimg();
        for (int i = 0; i < ((currentScenee == "Scene4") ? 3 : 1); i++)
        {
            animator.Play(stateName, i, hotspotTimings[index]);
        }
        animationProgress = hotspotTimings[index];
    }
}
