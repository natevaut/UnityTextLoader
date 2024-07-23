using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class XmlLoad
{
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
                // Get attributes content with defaults if unset
                element.x = tryGetIntAttr(elementNode, "x", 0);
                element.y = tryGetIntAttr(elementNode, "y", 0);
                element.width = tryGetIntAttr(elementNode, "width", 100);
                element.height = tryGetIntAttr(elementNode, "height", 100);
                element.fontSize = tryGetIntAttr(elementNode, "fontSize", 20);
                // Get text content
                element.text = elementNode.InnerText.Trim();

                page.AddElement(element);
            }

            this.pages.Add(page);

        }
    }

    private int tryGetIntAttr(XmlNode node, string attr, int defaultVal)
    {
        return int.TryParse(node.Attributes[attr]?.Value, out int val) ? val : defaultVal;
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
        textComponent.fontSize = element.fontSize;
        textComponent.alignment = TextAnchor.UpperRight;
    }

}
