using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TextDisplay
{

    private PageLoader pageLoader;
    private GameObject canvasParent;

    public TextDisplay(PageLoader pageLoader, GameObject canvasParent)
    {
        this.pageLoader = pageLoader;
        this.canvasParent = canvasParent;
    }

    public void DisplayAllPages(List<Page> pages)
    {
        foreach (Page page in pages)
        {
            foreach (PageElement element in page.GetElements())
            {
                DisplayPageElement(element);
            }
        }
    }

    private void DisplayPageElement(PageElement element)
    {
        // Instantiate new text object
        GameObject textObject = new GameObject("TextObject");
        textObject.transform.SetParent(this.canvasParent.transform);
        textObject.tag = pageLoader.textDisplayTag;

        // Set position
        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(element.x, element.y);
        rect.sizeDelta = new Vector2(element.width, element.height);

        // Set text content
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = element.text;
        textComponent.richText = true;
        textComponent.fontSize = element.fontSize;

        // Add link handling script
        EventTrigger trigger = textComponent.gameObject.AddComponent<EventTrigger>();
        // Create a new entry for pointer click event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        // Assign a callback to the entry
        entry.callback.AddListener((data) => { OnPointerClick(textComponent, (PointerEventData)data); });
        // Add the entry to the trigger
        trigger.triggers.Add(entry);
    }

    public void OnPointerClick(TextMeshProUGUI displayText, PointerEventData eventData)
    {
        Debug.Log("CLICK");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(displayText, Input.mousePosition, eventData.enterEventCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = displayText.textInfo.linkInfo[linkIndex];
            string link = linkInfo.GetLinkID();

            if (link.StartsWith("http"))
            {
                ClickExternalLink(link);
            }
            else
            {
                ClickInternalLink(link);
            }
        }
    }

    private void ClickInternalLink(string file)
    {
        pageLoader.OpenFile(file);
    }

    private void ClickExternalLink(string url)
    {
        Application.OpenURL(url);
    }

}
