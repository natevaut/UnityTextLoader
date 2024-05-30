using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

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
    public List<PageElement> elements;

    public Page()
    {
        this.elements = new List<PageElement>();
    }

    public void AddElement(PageElement element)
    {
        elements.Add(element);
    }
}

[System.Serializable]
public class XmlLoad
{
    public static XmlDocument ParseXml(string xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        // <page> is root node
        XmlNodeList pageNodes = xmlDoc.GetElementsByTagName("page");

        foreach (XmlNode pageNode in pageNodes)
        {
            Page page = new Page();
            page.title = pageNode.Attributes["title"].Value;

            Debug.Log("Page Title: " + page.title);

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

                // TODO: Add XML data to Unity page
                Debug.Log("--Element--");
                Debug.Log("Element X: " + element.x);
                Debug.Log("Element Y: " + element.y);
                Debug.Log("Element Width: " + element.width);
                Debug.Log("Element Height: " + element.height);
                Debug.Log("Element Text: " + element.text);
            }

        }

        return xmlDoc;
    }

}
