using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hyperlinking : MonoBehaviour, IPointerClickHandler
{
    public Text textComponent;

    private PageLoader pageLoader;

    public void SetPageLoader(PageLoader pageLoader)
    {
        this.pageLoader = pageLoader;
    }

    public void ClickInternalLink(string file)
    {
        this.pageLoader.OpenFile(file);
    }

    public void ClickExternalLink(string url)
    {
        Application.OpenURL(url);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        const string externPattern = @"\[\[(.+?)\]\]";
        const string internPattern = @"\{\{(.+?)\}\}";

        Match internMatch = new Regex(internPattern).Match(textComponent.text);
        Match externMatch = new Regex(externPattern).Match(textComponent.text);

        if (internMatch.Success) // internal link
        {
            string file = internMatch.Groups[1].Value;
            ClickInternalLink(file);
        }

        if (externMatch.Success) // external link
        { 
            string url = externMatch.Groups[1].Value;
            ClickExternalLink(url);
        }
    }
}
