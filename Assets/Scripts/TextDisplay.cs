using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay
{

    public static void DisplayAllPages(List<Page> pages, GameObject canvasParent)
    {
        foreach (Page page in pages)
        {
            foreach (PageElement element in page.GetElements())
            {
                DisplayPageElement(canvasParent, element);
            }
        }
    }

    private static void DisplayPageElement(GameObject canvasParent, PageElement element)
    {
        // Instantiate new text object
        GameObject textObject = new GameObject("TextObject");
        textObject.transform.SetParent(canvasParent.transform);

        // Set position
        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(element.x, element.y);
        rect.sizeDelta = new Vector2(element.width, element.height);

        // Set text content
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = element.text;

        // Set font
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // default font
        textComponent.fontSize = element.fontSize;
        textComponent.alignment = TextAnchor.UpperRight;
    }

}
