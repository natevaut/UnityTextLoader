using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PageElement
{
    public int x;
    public int y;
    public int width;
    public int height;
    public string text;
}

[System.Serializable]
public class Page
{
    public string title;

    private List<PageElement> elements;

    public Page()
    {
        this.elements = new List<PageElement>();
    }

    public void AddElement(PageElement element)
    {
        this.elements.Add(element);
    }

    public List<PageElement> GetElements()
    {
        return this.elements;
    }
}

// Main class:

[System.Serializable]
public class XmlLoad
{
    private List<Page> pages;
    private GameObject outputParent;

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

    public void DisplayAllPages(GameObject prefab)
    {
        foreach (Page page in this.pages)
        {
            foreach (PageElement element in page.GetElements())
            {
                DisplayPageElement(prefab, element);
            }
        }
    }

    public void SetCanvas(GameObject canvas)
    {
        this.outputParent = canvas;
    }

    private void DisplayPageElement(GameObject textObjectPrefab, PageElement element)
    {
        // Instantiate new text object
        GameObject textObject = Object.Instantiate(textObjectPrefab);
        textObject.transform.SetParent(this.outputParent.transform);

        // Set position
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(element.x, element.y);
        rect.sizeDelta = new Vector2(element.width, element.height);

        // Set text content
        TextMeshProUGUI textMeshPro = textObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = element.text;
    }

}
