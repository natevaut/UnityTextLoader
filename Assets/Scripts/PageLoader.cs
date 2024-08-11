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
    public string filename = "file.xml";

    void Start()
    {
        LoadTextFromFile(filename);
    }

    public void OpenFile(string filename)
    {
        ClearPage();
        LoadTextFromFile(filename);
    }

    private void ClearPage()
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(textDisplayTag);
        foreach (GameObject textObject in textObjects)
        {
            GameObject.Destroy(textObject);
        }
    }

    private void LoadTextFromFile(string filename)
    {
        // Load text file
        string baseFilename = Regex.Replace(filename, @"\.\w+$", ""); // remove extension
        var textFile = Resources.Load<TextAsset>(baseFilename);
        string rawText = textFile.text;

        // Load XML from text
        XmlLoad xmlLoader = new XmlLoad();
        xmlLoader.ParseXml(rawText);
        TextDisplay textDisplay = new TextDisplay(this, canvasParent);
        textDisplay.DisplayAllPages(xmlLoader.GetPages());
    }
}
