using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

public class XmlFormatParser
{
    public static string Parse(XmlNodeList xmlNodes)
    {
        StringBuilder stringBuilder = new StringBuilder();

        ParseNodes(xmlNodes, stringBuilder);

        // Return stringified output
        string output = stringBuilder.ToString();
        Debug.Log(output);
        return output;
    }

    private static void ParseNodes(XmlNodeList nodes, StringBuilder output)
    {
        // Parse each child node
        foreach (XmlNode node in nodes)
        {
            ParseNode(node, output);
        }
    }

    private static void ParseNode(XmlNode node, StringBuilder output)
    {
        // Parse this node
        switch (node.Name.ToLower())
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
                output.Append("\n");
                break;
            case "hr":
                // Horizontal rule
                output.Append("\n--\n");
                break;
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

    /** Coerce XML-compliant tags into Unity Rich Text format */
    private static void HandleFontElement(XmlNode node, StringBuilder output)
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
}
