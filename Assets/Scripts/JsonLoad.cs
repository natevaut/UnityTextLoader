using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonLoad
{
    public string title;
    public PageContent[] content;

    public static JsonLoad ParseJson(string json)
    {
        return JsonUtility.FromJson<JsonLoad>(json);
    }
}

[System.Serializable]
public class PageContent
{
    public string text;
    public int x, y;
    public int width, height;
}
