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
    private string _language;

    public XmlLoad(string lang)
    {
        _pages = new List<Page>();
        _language = lang;
    }

    public void ParseXml(string xml)
    {
        // parse document into XML
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);

        // load the pages from the XML document
        LoadPages(xmlDoc);
    }

    private void LoadPages(XmlDocument xmlDoc)
    {
        // <page> is root node
        XmlNodeList pageNodes = xmlDoc.GetElementsByTagName("page");
        // process pages in document
        foreach (XmlNode pageNode in pageNodes)
        {
            // load selected page data
            Page page = LoadPage(pageNode);
            _pages.Add(page);
        }
    }

    private Page LoadPage(XmlNode pageNode)
    {
        Page page = new Page();

        // singular <title> element
        XmlNode titleNode = pageNode.SelectSingleNode("title");
        if (titleNode != null)
        {
            string titleText = XmlFormatParser.Parse(titleNode.ChildNodes);
            page.Title = titleText.Trim();
        }
        else
        {
            page.Title = "";
        }

        // get list of <element>s contains the page data
        XmlNodeList elementNodes = pageNode.SelectNodes("element");
        // process page elements
        foreach (XmlNode elementNode in elementNodes)
        {
            // load selected element data
            PageElement element = LoadElement(elementNode);
            page.AddElement(element);
        }

        return page;
    }

    /**
     * Takes an &lt;element> and loads data from its applicable &lt;translate lang> child if present or plain children otherwise.
     * Returns the created PageElement.
     */
    private PageElement LoadElement(XmlNode elementNode)
    {
        // get node containing the selected translation
        XmlNode translationNode = elementNode.SelectSingleNode($"translate[@lang='{_language}']");
        // fallback: just the first translation element, for if the selected language is not present
        // (better than showing something broken or empty)
        XmlNode backupTranslationNode = elementNode.SelectSingleNode("translate");
        // set base to either the <translation> child (if present) or the parent <element> itself otherwise
        XmlNode contentBaseNode = translationNode ?? backupTranslationNode ?? elementNode;
        // load contents from this base node
        return LoadElementContent(elementNode, contentBaseNode);
    }

    /**
     * Loads the data of the XML chilren of a given node and returns the created PageElement.
     * The <param name="elementParentNode">element parent node</param> is the parent &lt;element> whose children are being parsed.
     * The <param name="contentBaseNode">content base node</param> is the child whose content is being processed;
     * i.e., either a &lt;translate> element or the plain children of the parent &lt;element>.
     */
    private PageElement LoadElementContent(XmlNode elementParentNode, XmlNode contentBaseNode) {
        PageElement element = new PageElement();
        // Get attributes content with defaults if unset
        element.X = TryGetIntAttr(elementParentNode, "x", 0);
        element.Y = TryGetIntAttr(elementParentNode, "y", 0);
        element.Width = TryGetIntAttr(elementParentNode, "width", 100);
        element.Height = TryGetIntAttr(elementParentNode, "height", 100);
        element.FontSize = TryGetIntAttr(elementParentNode, "fontSize", 20);

        // Get and parse XML text content
        string textContent = XmlFormatParser.Parse(contentBaseNode.ChildNodes);
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

        return element;
    }

    /**
     * Retrieves the int attribute from the element,
     * or the fallback <param name="defaultVal">default value</param> if not specified.
     */
    private int TryGetIntAttr(XmlNode node, string attr, int defaultVal)
    {
        return int.TryParse(node.Attributes[attr]?.Value, out int val) ? val : defaultVal;
    }

    public List<Page> GetPages()
    {
        return _pages;
    }

}
