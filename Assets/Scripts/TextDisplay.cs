using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = element.text;
        textComponent.supportRichText = true;

        // Set font
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // default font
        textComponent.fontSize = element.fontSize;
        textComponent.alignment = TextAnchor.UpperRight;

        // Add click handler script
        Hyperlinking clickableText = textObject.AddComponent<Hyperlinking>();
        clickableText.SetPageLoader(this.pageLoader);
        clickableText.textComponent = textComponent;
    }

}
