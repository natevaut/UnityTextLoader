using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class PageLoader : MonoBehaviour
{
    public GameObject textObjectPrefab;
    public TextMeshProUGUI displayText;
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
            xmlLoader.SetCanvas(canvasParent);
            xmlLoader.ParseXml(rawText);
            xmlLoader.DisplayAllPages(textObjectPrefab);

            // gpt below
            // Add an EventTrigger component to the displayText object
            EventTrigger trigger = displayText.gameObject.AddComponent<EventTrigger>();
            // Create a new entry for pointer click event
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            // Assign a callback to the entry
            entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
            // Add the entry to the trigger
            trigger.triggers.Add(entry);
            // gpt above
        }
        else
        {
            Debug.LogError("File not found at: " + fullPath);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(displayText, Input.mousePosition, eventData.enterEventCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = displayText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            string[] parts = linkID.Split('|');
            string type = parts[0];
            string uri = parts[1];

            if (type.Equals("ext"))
            {
                ClickExternalLink(uri);
            }
            else if (type.Equals("int"))
            {
                ClickInternalLink(uri);
            }
        }
    }

    private void ClickInternalLink(string file)
    {
        LoadTextFromFile(fullFolder + "/" + file);
    }

    private void ClickExternalLink(string url)
    {
        Application.OpenURL(url);
    }
}
