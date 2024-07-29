using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class PageLoader : MonoBehaviour
{
    public GameObject canvasParent;
    public string textDisplayTag = "Finish";
    public string dataFolder = "Data";
    public string filename = "file.xml";

    private string fullFolder;
    private RectTransform rect = null;

    void Awake()
    {
        fullFolder = "Assets/" + dataFolder;
    }

    void Start()
    {
        string fullPath = fullFolder + "/" + filename;
        LoadTextFromFile(fullPath);

        if (rect == null)
            rect = canvasParent.GetComponent<RectTransform>();
    }

    public void OpenFile(string file)
    {
        ClearPage();
        LoadTextFromFile(fullFolder + "/" + file );
    }

    private void ClearPage()
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(textDisplayTag);
        foreach (GameObject textObject in textObjects)
        {
            GameObject.Destroy(textObject);
        }
    }

    private void LoadTextFromFile(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            // Load XML
            XmlLoad xmlLoader = new XmlLoad();
            string rawText = File.ReadAllText(fullPath);
            xmlLoader.ParseXml(rawText);
            TextDisplay textDisplay = new TextDisplay(this, canvasParent);
            textDisplay.DisplayAllPages(xmlLoader.GetPages());
        }
        else
        {
            Debug.LogError("File not found at: " + fullPath);
        }
    }

    public RectTransform GetRect()
    {
        return rect;
    }

    public Camera GetCamera()
    {
        return GetComponent<Camera>();
    }
}
