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
    private List<XmlNode> _imageNodes = new List<XmlNode>();

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
            XmlFormatParser parser = new XmlFormatParser();
            string titleText = parser.Parse(titleNode.ChildNodes);
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

    /// <summary>
    /// Takes an &lt;element> and loads data from its applicable &lt;translate lang> child if present or plain children otherwise.
    /// </summary>
    /// <param name="elementNode"></param>
    /// <returns>The created PageElement.</returns>
    private PageElement LoadElement(XmlNode elementNode)
    {
        // get node containing the selected translation
        XmlNode translationNode = elementNode.SelectSingleNode($"translate[@lang='{_language}']");
        // fallback element for if the selected language is not present or if the text is the same for all languages
        XmlNode fallbackNode = elementNode.SelectSingleNode("default");
        // set base to either the <translation> child (if present) or the parent <element> itself otherwise
        XmlNode contentBaseNode = translationNode ?? fallbackNode ?? elementNode;
        // load contents from this base node
        return LoadElementContent(elementNode, contentBaseNode);
    }

    /// <summary>
    /// Loads the data of the XML chilren of a given node.
    /// </summary>
    /// <param name="elementParentNode">The parent &lt; element> whose children are being parsed.</param>
    /// <param name="contentBaseNode">
    ///     The child whose content is being processed;
    ///     i.e., either a &lt;translate> element or the plain children of the parent &lt;element>.
    /// </param>
    /// <returns>The created PageElement.</returns>
    private PageElement LoadElementContent(XmlNode elementParentNode, XmlNode contentBaseNode)
    {
        PageElement element = new PageElement();
        // Get attributes content with defaults if unset
        element.X = Helper.TryGetIntAttr(elementParentNode, "x", 0);
        element.Y = Helper.TryGetIntAttr(elementParentNode, "y", 0);
        element.Width = Helper.TryGetIntAttr(elementParentNode, "width", 100);
        element.Height = Helper.TryGetIntAttr(elementParentNode, "height", 100);
        element.FontSize = Helper.TryGetIntAttr(elementParentNode, "fontSize", 20);

        // Get and parse XML text content
        XmlFormatParser parser = new XmlFormatParser();
        string textContent = parser.Parse(contentBaseNode.ChildNodes);
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

        // Get and save images
        foreach (XmlNode node in parser.GetImageNodes())
        {
            _imageNodes.Add(node);
        }

        return element;
    }

    public List<Page> GetPages()
    {
        return _pages;
    }

    public List<XmlNode> GetImageNodes()
    {
        return _imageNodes;
    }

}
