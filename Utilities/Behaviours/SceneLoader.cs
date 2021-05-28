using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// Slap this on a gameobject in your first-loaded gameLogic scene to handle loading in other scenes.
    /// Loading scenes through this script will ensure the gameLogic stays loaded at all times.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Tooltip("The scene that handles your game logic. This scene will be kept loaded at all times.")]
        public string logicScene;
        [Tooltip("The scene you would like loaded up as soon as the game starts, or if no other scene is loaded in the editor. Leave blank if you do not want to load anything.")]
        public string startupScene;

        /// <summary>
        /// If there is a scene loaded other than the gamelogic, returns true.
        /// </summary>
        public bool IsLevelLoaded => loadedScenes.Count > 0;

        private List<Scene> loadedScenes = new List<Scene>();
        private List<Action> callbacksToActivate = new List<Action>();

        public void AddSceneLoadCallback(Action callback)
        {
            if (callback != null)
                callbacksToActivate.Add(callback);
        }

        public void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            // Checks all the scenes loaded, and adds them to the list, except the gamelogic
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name != logicScene)
                    loadedScenes.Add(SceneManager.GetSceneAt(i));
            }

            // Loads the world scene if nothing else is loaded
            if (!string.IsNullOrEmpty(startupScene) && loadedScenes.Count == 0)
            {
                SceneManager.LoadScene(startupScene, LoadSceneMode.Additive);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        // PUBLIC METHODS

        /// <summary>
        /// Unloads all scenes except for logic scenes, and loads in the given scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadScene(string sceneName, Action callback)
        {
            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogErrorFormat("Scene {0} doesn't exist and cannot be loaded!", sceneName);
                return;
            }

            if (loadedScenes.Count > 0)
                UnloadAllScenes();

            if (callback != null)
                callbacksToActivate.Add(callback);

            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Unloads all scenes but the logic scene.
        /// </summary>
        public void UnloadAllScenes()
        {
            foreach (Scene s in loadedScenes)
            {
                SceneManager.UnloadSceneAsync(s);
            }
        }

        // CALLBACKS

        protected virtual void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name != logicScene) // As long as the loaded scene ISN'T this scene, the gamelogic...
            {
                loadedScenes.Add(loadedScene);
                SceneManager.SetActiveScene(loadedScene);

                foreach (var action in callbacksToActivate)
                {
                    action.Invoke();
                }
                callbacksToActivate.Clear();
            }
        }

        protected virtual void OnSceneUnloaded(Scene unloadedScene)
        {
            if (!loadedScenes.Remove(unloadedScene)) // Removes the scene from the loaded list, otherwise logs
                Debug.Log(unloadedScene.name + "wasn't loaded through this manager.");

            if (loadedScenes.Count == 0)
                Debug.Log("Scenes unloaded"); // All scenes unloaded
        }
    }
}