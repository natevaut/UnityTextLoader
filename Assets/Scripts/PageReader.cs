using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PageReader
{
    public static XmlLoad ReadXmlFile(string filepath, string language)
    {
        // Load text file
        string extlessFilePath = Regex.Replace(filepath, @"\.\w+$", ""); // remove extension as resource loader does not use it
        var textFile = Resources.Load<TextAsset>(extlessFilePath);
        string rawText = textFile.text;

        // Load XML from text
        XmlLoad xmlLoader = new XmlLoad(language);
        xmlLoader.ParseXml(rawText);

        return xmlLoader;
    }
}
