using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Helper
{
    /// <summary>
    /// Retrieves the int attribute from the element, or a fallback if not specified.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="attr">The attribute key to fetch.</param>
    /// <param name="defaultVal">The default value to return if the attribute is unset.</param>
    /// <returns></returns>
    public static int TryGetIntAttr(XmlNode node, string attr, int defaultVal)
    {
        return int.TryParse(node.Attributes[attr]?.Value, out int val) ? val : defaultVal;
    }
}
