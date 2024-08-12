using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Page
{
    public string Title;

    private List<PageElement> _elements;

    public Page()
    {
        _elements = new List<PageElement>();
    }

    public void AddElement(PageElement element)
    {
        _elements.Add(element);
    }

    public List<PageElement> GetElements()
    {
        return _elements;
    }
}
