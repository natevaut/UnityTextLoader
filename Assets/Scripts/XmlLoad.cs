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
public class WholePage
{
    public string title;
    public List<PageElement> elements;

    public WholePage()
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
    public static WholePage ParseXml(string xml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        // <page> is root node
        XmlNodeList pageNodes = xmlDoc.GetElementsByTagName("page");

        List<WholePage> pagesList = new List<WholePage>();

        foreach (XmlNode pageNode in pageNodes)
        {
            WholePage page = new WholePage();
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

            pagesList.Add(page);

        }

        WholePage root = pagesList[0]; // should only be one root <page> element in the document
        return root;
    }

}
