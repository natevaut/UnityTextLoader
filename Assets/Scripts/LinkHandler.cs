using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class LinkHandler : MonoBehaviour
{
    void Start()
    {
    }

    public void AttachEventsToLinks()
    {
        List<GameObject> allTextObjects = new List<GameObject>();
        // TODO: get all text objects
        foreach (GameObject displayText in allTextObjects)
        {
            // Link clicking
            EventTrigger trigger = displayText.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry(); // create entry for pointer click event
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnPointerClick(displayText, (PointerEventData)data); }); // assign callback to entry
            trigger.triggers.Add(entry);
        }
    }

    public void OnPointerClick(GameObject displayText, PointerEventData eventData)
    {
        Debug.Log("CLICK");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(displayText, Input.mousePosition, eventData.enterEventCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = displayText.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            string[] parts = linkID.Split('|');
            string type = parts[0];
            string uri = parts[1];

            if (type.Equals("ext"))
            {
                ClickExternalLink(uri);
            }
            else if (type.Equals("int"))
            {
                ClickInternalLink(uri);
            }
        }
    }

    private void ClickInternalLink(string file)
    {
        LoadTextFromFile(fullFolder + "/" + file);
    }

    private void ClickExternalLink(string url)
    {
        Application.OpenURL(url);
    }
}
