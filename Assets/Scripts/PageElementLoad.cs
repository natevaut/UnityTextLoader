using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class PageElementLoad : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public RectTransform canvasRect;

    public string dataFolder = "Data";
    public string filename = "file.xml";
    public int baseFontSize = 18;

    private string fullFolder;
    private List<Text> textElements = new List<Text>(); // track text elements


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
            WholePage wholePage = XmlLoad.ParseXml(rawText);

            foreach (PageElement element in wholePage.elements)
            {
                CreateTextElement(element);
            }
        }
        else
        {
            Debug.LogError("File not found at: " + fullPath);
        }
    }

    void CreateTextElement(PageElement element)
    {
        //gpt below
        // TODO doesnt work

        TextMeshProUGUI newTextObj = Instantiate(displayText, canvasRect);
        Text newText = newTextObj.GetComponent<Text>();
        newText.text = element.text;

        // Set position based on element.x and element.y
        RectTransform newTextRect = newTextObj.GetComponent<RectTransform>();
        newTextRect.anchoredPosition = new Vector2(element.x, element.y);

        // Add the new text element to the list
        textElements.Add(newText);
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
