var MyPlugin = {
    DataInitializedJS: function() {
        if (typeof window.onDataInitialized === 'function') {
            window.onDataInitialized();
        } else {
            console.log("window.onDataInitialized is not defined.");
        }
    },

    SendHotspotEventJS: function(jsonStrPtr) {
        var jsonStr = UTF8ToString(jsonStrPtr);
        if (typeof window.onHotspotEvent === 'function') {
            window.onHotspotEvent(jsonStr);
        } else {
            console.log("window.onHotspotEvent is not defined.");
        }
    },

    SceneInitializedJS: function(sceneNamePtr) {
        var sceneName = UTF8ToString(sceneNamePtr);
        if (typeof window.onSceneInitialized === 'function') {
            window.onSceneInitialized(sceneName);
        } else {
            console.log("window.onSceneInitialized is not defined.");
        }
    },

     UnityLoaded: function() {
        OnUnityLoaded();
    },

    SequenceEnded: function() {
        OnSequenceEnded();
    },OnStartSceneTransition: function() {
        if (typeof window.onStartSceneTransition === 'function') {
            window.onStartSceneTransition();
        } else {
            console.log("window.onStartSceneTransition is not defined.");
        }
    },

    OnStartSceneTransitionComplete: function() {
        if (typeof window.onStartSceneTransitionComplete === 'function') {
            window.onStartSceneTransitionComplete();
        } else {
            console.log("window.onStartSceneTransitionComplete is not defined.");
        }
    },

    OnEndSceneTransition: function() {
        if (typeof window.onEndSceneTransition === 'function') {
            window.onEndSceneTransition();
        } else {
            console.log("window.onEndSceneTransition is not defined.");
        }
    },

    OnEndSceneTransitionComplete: function() {
        if (typeof window.onEndSceneTransitionComplete === 'function') {
            window.onEndSceneTransitionComplete();
        } else {
            console.log("window.onEndSceneTransitionComplete is not defined.");
        }
    }
};

mergeInto(LibraryManager.library, MyPlugin);

