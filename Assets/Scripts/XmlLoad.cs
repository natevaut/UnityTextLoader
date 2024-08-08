using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

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

                // Get and parse XML text content
                string textContent = XmlFormatParser.Parse(elementNode.ChildNodes);
                textContent = textContent.Trim();
                var replacements = new Dictionary<string, string>
                {
                    { "^[ \t]+", "" }, // remove indents
                    { "(>)\r?\n", "$1" }, // remove newlines around tags
                    { "\r?\n", " " }, // collapse newlines to a space when between text
                };
                foreach (var replacement in replacements)
                {
                    textContent = Regex.Replace(textContent, replacement.Key, replacement.Value, RegexOptions.Multiline);
                }
                element.text = textContent;

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

}
