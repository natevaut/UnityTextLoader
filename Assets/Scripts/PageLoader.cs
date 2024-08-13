using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class PageLoader : MonoBehaviour
{
    public GameObject CanvasParent;
    public string TextDisplayTag = "Finish";
    public string DataFolder = "";
    public string File = "file.xml";
    public string Language = "en";

    void Start()
    {
        // Start by opening initial file
        OpenFile(File);
    }

    public void OpenFile(string filename)
    {
        ClearPage();
        LoadTextFromFile(FullPath(filename));
    }

    private void ClearPage()
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(TextDisplayTag);
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
        XmlLoad xmlLoader = new XmlLoad(Language);
        xmlLoader.ParseXml(rawText);
        TextDisplay textDisplay = new TextDisplay(this, CanvasParent);
        textDisplay.DisplayAllPages(xmlLoader.GetPages());
    }

    private string FullPath(string filename)
    {
        if (DataFolder == "")
            return filename;
        else
            return DataFolder + "/" + filename;
    }
}
