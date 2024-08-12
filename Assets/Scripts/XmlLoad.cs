using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class XmlLoad
{
    private List<Page> _pages;

    public XmlLoad()
    {
        _pages = new List<Page>();
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
            page.Title = pageNode.Attributes["title"].Value;

            // list of <element>s contains the page data
            XmlNodeList elementNodes = pageNode.SelectNodes("element");
            foreach (XmlNode elementNode in elementNodes)
            {
                PageElement element = new PageElement();
                // Get attributes content with defaults if unset
                element.X = TryGetIntAttr(elementNode, "x", 0);
                element.Y = TryGetIntAttr(elementNode, "y", 0);
                element.Width = TryGetIntAttr(elementNode, "width", 100);
                element.Height = TryGetIntAttr(elementNode, "height", 100);
                element.FontSize = TryGetIntAttr(elementNode, "fontSize", 20);

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
                element.Text = textContent;

                page.AddElement(element);
            }

            _pages.Add(page);

        }
    }

    private int TryGetIntAttr(XmlNode node, string attr, int defaultVal)
    {
        return int.TryParse(node.Attributes[attr]?.Value, out int val) ? val : defaultVal;
    }

    public List<Page> GetPages()
    {
        return _pages;
    }

}
