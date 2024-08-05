using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        // Add click handler script
        EventTrigger trigger = textObject.gameObject.AddComponent<EventTrigger>();
        // Create a new entry for pointer click event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        // Assign a callback to the entry
        entry.callback.AddListener((data) => {
            OnPointerClick((PointerEventData)data, textComponent);
        });
        // Add the entry to the trigger
        trigger.triggers.Add(entry);
    }

    void OnPointerClick(PointerEventData eventData, TextMeshProUGUI text)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, eventData.enterEventCamera);

        if (linkIndex != -1) // if the user has intersected with a link
        {
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
            string link = linkInfo.GetLinkID();

            if (link.StartsWith("http"))
            {
                // External/web URL
                Application.OpenURL(link);
            }
            else
            {
                // Internal page: load the file
                pageLoader.OpenFile(link);
            }
        }
    }

}
