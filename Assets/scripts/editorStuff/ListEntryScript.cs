using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class ListEntryScript : MonoBehaviour
{
    public MapPainter _mp;
	
	void Start ()
    {
        _mp =  GameObject.FindGameObjectWithTag("GameController").GetComponent<MapPainter>();
        EventTrigger t = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => _mp.SetCurrentTile(this.gameObject.name));
        t.triggers.Add(entry);
    }

}
