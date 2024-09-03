using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Helper
{
    /**
     * Retrieves the int attribute from the element,
     * or the fallback <param name="defaultVal">default value</param> if not specified.
     */
    public static int TryGetIntAttr(XmlNode node, string attr, int defaultVal)
    {
        return int.TryParse(node.Attributes[attr]?.Value, out int val) ? val : defaultVal;
    }
}
