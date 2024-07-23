using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class XmlLoad
{
    private const int fontSize = 24;

    private List<Page> pages;

    public XmlLoad()
    {
        this.pages = new List<Page>();
    }

    public void ParseXml(string xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        // <page> is root node
        XmlNodeList pageNodes = xmlDoc.GetElementsByTagName("page");

        foreach (XmlNode pageNode in pageNodes)
        {
            Page page = new Page();
            page.title = pageNode.Attributes["title"].Value;

            // list of <element>s contains the page data
            XmlNodeList elementNodes = pageNode.SelectNodes("element");
            foreach (XmlNode elementNode in elementNodes)
            {
                PageElement element = new PageElement();
                element.x = int.Parse(elementNode.Attributes["x"].Value);
                element.y = int.Parse(elementNode.Attributes["y"].Value);
                element.width = int.Parse(elementNode.Attributes["width"].Value);
                element.height = int.Parse(elementNode.Attributes["height"].Value);
                element.text = elementNode.InnerText.Trim(); // get text content

                page.AddElement(element);
            }

            this.pages.Add(page);

        }
    }

    public List<Page> GetPages()
    {
        return this.pages;
    }

    public void DisplayAllPages(GameObject canvasParent)
    {
        foreach (Page page in this.pages)
        {
            foreach (PageElement element in page.GetElements())
            {
                DisplayPageElement(canvasParent, element);
            }
        }
    }

    private void DisplayPageElement(GameObject canvasParent, PageElement element)
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
        textComponent.fontSize = fontSize;
        textComponent.alignment = TextAnchor.UpperRight;
    }

}
