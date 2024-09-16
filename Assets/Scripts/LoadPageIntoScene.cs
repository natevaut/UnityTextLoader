using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPageIntoScene
{
    private string _filePath;
    private string _sceneToLoad;

    public LoadPageIntoScene(string filePath, string sceneToLoad)
    {
        _filePath = filePath;
        _sceneToLoad = sceneToLoad;

        // Load selected scene asynchronously and add event handler for when loaded
        SceneManager.LoadSceneAsync(_sceneToLoad).completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation)
    {
        GameObject eventObject = GameObject.Find("EventSystem");
        if (eventObject == null)
        {
            eventObject = new GameObject("EventSystem");
        }

        // Create new page loader script
        // Copies across the old values to ensure consistency
        // TODO: Find a better way to do this - re-constructor for PageLoader or something? static PageLoader.ExtendFrom(PageLoader)?
        PageLoader existingPageLoader = eventObject.GetComponent<PageLoader>();
        PageLoader newPageLoader = eventObject.AddComponent<PageLoader>();
        string dataFolder = existingPageLoader.DataFolder;
        newPageLoader.CanvasParent = existingPageLoader.CanvasParent;
        newPageLoader.DataFolder = dataFolder;
        newPageLoader.Language = existingPageLoader.Language;
        newPageLoader.TextDisplayTag = existingPageLoader.TextDisplayTag;
        newPageLoader.File = _filePath.Replace(dataFolder + "/", "");
    }
}
