using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public SceneSequenceData sceneSequenceData;
    public TransitionController TCInstance;
    public bool isDataReady = false; //test
    internal int CurrentIndex = 0;
    internal int? LoadingSpace;
    internal bool Paused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
                                           //ReceiveInitialData(@"{ 
                                           //  ""sceneSequence"": [
                                           //    {
                                           //      ""scene"": ""Scene1"",
                                           //      ""hotspots"": [
                                           //        {
                                           //          ""questId"": ""quest1"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest2"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest3"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest4"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        }
                                           //      ],
                                           //      ""initialPosition"": 0
                                           //    },
                                           //    {
                                           //      ""scene"": ""Scene3"",
                                           //      ""hotspots"": [
                                           //        {
                                           //          ""questId"": ""quest5"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest6"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest7"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest8"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest9"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        }
                                           //      ],
                                           //      ""initialPosition"": 0
                                           //    },
                                           //    {
                                           //      ""scene"": ""Scene4"",
                                           //      ""hotspots"": [
                                           //        {
                                           //          ""questId"": ""quest5"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest6"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest7"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest8"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest9"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        }
                                           //      ],
                                           //      ""initialPosition"": 0
                                           //    },
                                           //    {
                                           //      ""scene"": ""Scene5"",
                                           //      ""hotspots"": [
                                           //        {
                                           //          ""questId"": ""quest5"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest6"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest7"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest8"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest9"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        }
                                           //      ],
                                           //      ""initialPosition"": 0
                                           //    },
                                           //    {
                                           //      ""scene"": ""Scene6"",
                                           //      ""hotspots"": [
                                           //        {
                                           //          ""questId"": ""quest5"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest6"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest7"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest8"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        },
                                           //        {
                                           //          ""questId"": ""quest9"",
                                           //          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
                                           //        }
                                           //      ],
                                           //      ""initialPosition"": 0
                                           //    }  
                                           //  ]
                                           //}");            

/*            ReceiveInitialData(@"{ 
  ""sceneSequence"": [
    {
      ""scene"": ""Scene3"",
      ""hotspots"": [
        {
          ""questId"": ""quest5"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest6"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest7"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest8"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest9"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        }
      ],
      ""initialPosition"": 0
    },
    {
      ""scene"": ""Scene4"",
      ""hotspots"": [
        {
          ""questId"": ""quest5"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest6"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest7"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest8"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest9"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        }
      ],
      ""initialPosition"": 0
    },
    {
      ""scene"": ""Scene5"",
      ""hotspots"": [
        {
          ""questId"": ""quest5"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest6"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest7"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest8"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        },
        {
          ""questId"": ""quest9"",
          ""questions"": [{}, {}, {}, {}, {}, {}, {}, {}]
        }
      ],
      ""initialPosition"": 0
    }
  ]
}");*/

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method called by the website to send initial data
    public void ReceiveInitialData(string json)
    {
        InitializeData(json);
        Debug.Log("json recieved: " + json);
        // After initializing data, notify the website
        //isDataReady= true;
        DataInitialized();
    }

    public void LoadScene(string LoadingSpace)
    {
        string[] Data = LoadingSpace.Split('|');
        this.LoadingSpace = int.Parse(Data[1]);
        TCInstance.OnTransitionOutComplete = () =>
        {
            SceneManager.LoadScene(Data[0]);
            TCInstance.OnTransitionOutComplete = null;
            TCInstance.StartTransitionIn();
        };
        TCInstance.StartTransitionOut();
    }
    public void TogglePlay(string Pause)
    {
        Paused = bool.Parse(Pause.ToLower().ToString());
    }

    private void InitializeData(string json)
    {
        // Deserialize the JSON data into SceneSequenceData object
        sceneSequenceData = JsonConvert.DeserializeObject<SceneSequenceData>(json);
        SceneManager.LoadScene(sceneSequenceData.sceneSequence[0].scene);
        CurrentIndex = 0;
    }

    private void DataInitialized()
    {
        isDataReady = true;
#if UNITY_WEBGL && !UNITY_EDITOR
        DataInitializedJS();
#else
        Debug.Log("Data Initialized in Unity.");
#endif
    }

    // Import the JavaScript function from the .jslib file
    [DllImport("__Internal")]
    private static extern void DataInitializedJS();
}
