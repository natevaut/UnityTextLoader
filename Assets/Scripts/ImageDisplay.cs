using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay
{
    private PageLoader _pageLoader;
    private GameObject _canvasParent;

    public ImageDisplay(PageLoader pageLoader, GameObject canvasParent)
    {
        _pageLoader = pageLoader;
        _canvasParent = canvasParent;
    }

    /// <summary>
    /// Parses and loads several image nodes into the scene.
    /// </summary>
    public void LoadImages(List<XmlNode> imageNodes)
    {
        foreach (XmlNode node in imageNodes)
        {
            LoadImage(node);
        }
    }

    /// <summary>
    /// Loads an image resource into the scene from a parsed node.
    /// </summary>
    public void LoadImage(XmlNode node)
    {
        var srcAttribute = node.Attributes["src"]?.Value;
        if (string.IsNullOrEmpty(srcAttribute)) return;

        int x = Helper.TryGetIntAttr(node, "x", 0);
        int y = Helper.TryGetIntAttr(node, "y", 0);
        float scale = Helper.TryGetFloatAttr(node, "scale", 1f);

        // Get image resource and make sprite from it
        string resPath = srcAttribute.Replace(".png", "").Replace(".jpg", "");
        Texture2D texture = Resources.Load<Texture2D>(resPath);
        Sprite sprite = Sprite.Create
        (
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        // Create game object and store texture sprite
        GameObject imageObject = new GameObject("LoadedImage");
        Image uiImage = imageObject.AddComponent<Image>();
        uiImage.sprite = sprite;

        // Set transform to attr values
        imageObject.transform.SetParent(_canvasParent.transform);
        imageObject.transform.position = new Vector3(x, y, 0);
        imageObject.transform.localScale = new Vector3(scale, scale, 1);

        imageObject.tag = _pageLoader.TextDisplayTag;
    }

}
