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
    public string dataFolder = "";
    public string filename = "file.xml";

    void Start()
    {
        // Start by opening initial file
        OpenFile(FullPath(filename));
    }

    public void OpenFile(string filename)
    {
        ClearPage();
        LoadTextFromFile(FullPath(filename));
    }

    private void ClearPage()
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(textDisplayTag);
        foreach (GameObject textObject in textObjects)
        {
            GameObject.Destroy(textObject);
        }
    }

    private void LoadTextFromFile(string filePath)
    {
        // Load text file
        string extlessFilePath = Regex.Replace(filePath, @"\.\w+$", ""); // remove extension as resource loader does not use it
        var textFile = Resources.Load<TextAsset>(extlessFilePath);
        string rawText = textFile.text;

        // Load XML from text
        XmlLoad xmlLoader = new XmlLoad();
        xmlLoader.ParseXml(rawText);
        TextDisplay textDisplay = new TextDisplay(this, canvasParent);
        textDisplay.DisplayAllPages(xmlLoader.GetPages());
    }

    private string FullPath(string filename)
    {
        if (dataFolder == "")
            return filename;
        else
            return dataFolder + "/" + filename;
    }
}
