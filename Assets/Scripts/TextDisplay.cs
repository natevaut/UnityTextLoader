using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TextDisplay
{

    private PageLoader _pageLoader;
    private GameObject _canvasParent;

    public TextDisplay(PageLoader pageLoader, GameObject canvasParent)
    {
        _pageLoader = pageLoader;
        _canvasParent = canvasParent;
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
        textObject.transform.SetParent(_canvasParent.transform);
        textObject.tag = _pageLoader.TextDisplayTag;

        // Set position
        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(element.X, element.Y);
        rect.sizeDelta = new Vector2(element.Width, element.Height);

        // Set text content
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = element.Text;
        textComponent.fontSize = element.FontSize;
        textComponent.richText = true;

        // Add click handler script
        EventTrigger trigger = textObject.AddComponent<EventTrigger>();
        // Create a new entry for pointer click event
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        // Assign a callback to the entry
        entry.callback.AddListener((data) =>
            {
                OnPointerClick((PointerEventData)data, textComponent);
            }
        );
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
                _pageLoader.OpenFile(link);
            }
        }
    }

}
