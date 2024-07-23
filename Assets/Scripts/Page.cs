using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Page
{
    public string title;

    private List<PageElement> elements;

    public Page()
    {
        this.elements = new List<PageElement>();
    }

    public void AddElement(PageElement element)
    {
        this.elements.Add(element);
    }

    public List<PageElement> GetElements()
    {
        return this.elements;
    }
}
