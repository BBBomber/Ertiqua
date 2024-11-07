using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

[System.Serializable]
public class SceneSequenceData
{
    public List<SceneData> sceneSequence;
    public bool isMale = true;

}

[System.Serializable]
public class SceneData
{
    public string scene;
    public List<HotspotData> hotspots;
    public int initialPosition;
    

}

[System.Serializable]
public class HotspotData
{
    public bool active;
    public string questId;
    public JArray questions; // Use JArray to hold an array of generic JSON objects
}
