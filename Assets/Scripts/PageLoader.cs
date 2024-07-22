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
    public string dataFolder = "Data";
    public string filename = "file.xml";
    public int baseFontSize = 18;

    private string fullFolder;

    void Awake()
    {
        fullFolder = "Assets/" + dataFolder;
    }

    void Start()
    {
        string fullPath = fullFolder + "/" + filename;
        LoadTextFromFile(fullPath);
    }

    void LoadTextFromFile(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            // Load XML
            XmlLoad xmlLoader = new XmlLoad();
            string rawText = File.ReadAllText(fullPath);
            xmlLoader.ParseXml(rawText);
            xmlLoader.DisplayAllPages(canvasParent);
        }
        else
        {
            Debug.LogError("File not found at: " + fullPath);
        }
    }
}
