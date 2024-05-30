using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class TextFromFileLoader : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public string dataFolder = "Data";
    public string filename = "file.txt";
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
            string rawText = File.ReadAllText(fullPath);
            JsonLoad jsonData = JsonLoad.ParseJson(rawText);
            XmlLoad.ParseXml(File.ReadAllText(fullFolder + "/" + "lorem.xml")); // debug

            displayText.richText = true;
            displayText.text = "<h1>" + jsonData.title + "</h1>";

            foreach (PageContent item in jsonData.content)
            {
                // TODO: x,y,width,height
                string parsedText = ParseWikiText(item.text);
                displayText.text += parsedText;
            }

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
        }
        else
        {
            Debug.LogError("File not found at: " + fullPath);
        }
    }

    string ParseWikiText(string input)
    {
        string text = input;

        // apply defaults

        // formatting
        text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<b>$1</b>"); // bold
        text = Regex.Replace(text, @"\*(.+?)\*", "<i>$1</i>"); // italic
        text = Regex.Replace(text, @"__(.+?)__", "<u>$1</u>"); // underline
        text = Regex.Replace(text, @"~~(.+?)~~", "<s>$1</s>"); // strike
        //  headings
        text = Regex.Replace(text, @"^####\s*(.+)$", $"<size=\"{baseFontSize * 1.25}\">$1</size>", RegexOptions.Multiline); // h4
        text = Regex.Replace(text, @"^###\s*(.+)$", $"<size=\"{baseFontSize * 1.5}\">$1</size>", RegexOptions.Multiline); // h3
        text = Regex.Replace(text, @"^##\s*(.+)$", $"<size=\"{baseFontSize * 1.75}\">$1</size>", RegexOptions.Multiline); // h2
        text = Regex.Replace(text, @"^#\s*(.+)$", $"<size=\"{baseFontSize * 2}\">$1</size>", RegexOptions.Multiline); // h1
        // links
        text = Regex.Replace(text, @"\[\[(.+?)\|(.+?)\]\]", "<link=\"int|$1\"><color=\"lightblue\"><u>$2</u></color></link>"); // [[internal_link|display text]]
        text = Regex.Replace(text, @"\[\[(.+?)\]\]", "<link=\"int|$1\"><color=\"lightblue\"><u>$1</u></color></link>"); // [[internal_link]]
        text = Regex.Replace(text, @"\[([^\[\] ]+?) (.+?)\]", "<link=\"ext|$1\"><color=\"lightblue\"><u>$2</u>^</color></link>"); // [external_link display text]
        // lists
        text = Regex.Replace(text, @"^(\d+).\s*", "\t$1. ", RegexOptions.Multiline); // numbered
        text = Regex.Replace(text, @"^-\s*", "\t• ", RegexOptions.Multiline); // bulleted

        return text;
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
