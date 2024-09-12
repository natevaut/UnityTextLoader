using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

public class XmlFormatParser
{
    private List<XmlNode> _imageNodes = new List<XmlNode>();

    public XmlFormatParser()
    {
    }

    public string Parse(XmlNodeList xmlNodes)
    {
        StringBuilder stringBuilder = new StringBuilder();

        ParseNodes(xmlNodes, stringBuilder);

        // Return stringified output
        string output = stringBuilder.ToString();
        return output;
    }

    private void ParseNodes(XmlNodeList nodes, StringBuilder output)
    {
        // Parse each child node
        foreach (XmlNode node in nodes)
        {
            ParseNode(node, output);
        }
    }

    private void ParseNode(XmlNode node, StringBuilder output)
    {
        // Parse this node
        string tag = node.Name.ToLower();
        switch (tag)
        {
            case "#text":
                // Text element
                output.Append(node.Value);
                break;
            case "font":
                // Handle font->Unity Rich Text tag parsing
                HandleFontElement(node, output);
                break;
            case "br":
                // Newline
                output.Append("<br>");
                break;
            case "hr":
                // Horizontal rule
                output.Append("<br>--<br>");
                break;
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
                var sizeMap = new Dictionary<string, int>
                {
                    { "h1", 100 },
                    { "h2", 80 },
                    { "h3", 70 },
                    { "h4", 60 },
                    { "h5", 50 },
                    { "h6", 40 }
                };
                output.AppendFormat("<size={0}><b>", sizeMap[tag]);
                ParseNodes(node.ChildNodes, output);
                output.Append("</b></size>");
                break;
            case "link":
                // Internal hyperlink
                HandleLink("to", node, output);
                break;
            case "a":
                // Web URL
                HandleLink("href", node, output);
                break;
            case "img":
                // Save image node for later processing
                _imageNodes.Add(node);
                break;

            // Anything else:
            default:
                if (node.OuterXml.EndsWith("/>"))
                {
                    // Ignore self-closing tags not dealt with above
                    break;
                }

                // Pass through any other tag
                string xmlCloser = "</" + node.Name + ">";
                string xmlOpener = node.OuterXml
                    .Replace(node.InnerText, "") // <x a=b>y</x> -> <x a=b></x>
                    .Replace(xmlCloser, "") // <x a=b></x> -> <x a=b>
                    ;
                output.Append(xmlOpener);
                ParseNodes(node.ChildNodes, output);
                output.Append(xmlCloser);
                break;
        }
    }

    /// <summary>
    /// Coerce XML-compliant tags into Unity Rich Text format.
    /// </summary>
    /// <param name="node">The node to parse the tags from.</param>
    /// <param name="output">The string builder object into which output is appended.</param>
    private void HandleFontElement(XmlNode node, StringBuilder output)
    {
        // Handle attributes
        if (node.Attributes["size"] != null)
            output.AppendFormat("<size={0}>", node.Attributes["size"].Value);
        if (node.Attributes["color"] != null)
            output.AppendFormat("<color={0}>", node.Attributes["color"].Value);

        // Recursively parse children
        ParseNodes(node.ChildNodes, output);

        // Close tags if opened
        if (node.Attributes["color"] != null)
            output.Append("</color>");
        if (node.Attributes["size"] != null)
            output.Append("</size>");
    }

    private void HandleLink(string linkAttr, XmlNode node, StringBuilder output)
    {
        string href = node.Attributes[linkAttr]?.Value ?? string.Empty;
        string linkText = node.InnerText;

        if (!string.IsNullOrEmpty(href))
        {
            // Create link
            // Manual formatting for link display (blue)
            output.AppendFormat("<link={0}><color=blue>{1}</color></link>", href, linkText);
        }
        else
        {
            output.Append(linkText);
        }
    }

    public List<XmlNode> GetImageNodes()
    {
        return _imageNodes;
    }
}
