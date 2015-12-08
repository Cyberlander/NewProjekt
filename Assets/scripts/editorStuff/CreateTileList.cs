using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class CreateTileList : MonoBehaviour
{

    public MapPainter _mp;
    public GameObject _pre;

	void Start ()
    {

        var _tiles = Resources.LoadAll("tiles", typeof(Sprite));
        float recWidth = Screen.width * 0.15f;     
        foreach(Sprite s in _tiles)
        {
            GameObject g = Instantiate(_pre);
            g.name = s.name;       
            g.transform.SetParent(gameObject.transform);            
            g.GetComponent<LayoutElement>().preferredWidth = Mathf.Abs(recWidth);
            g.GetComponent<LayoutElement>().preferredHeight = Mathf.Abs(recWidth);           
            g.GetComponent<Image>().sprite = s;          
        }
        
        GetComponent<ContentSizeFitter>().enabled = true;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y);
    }
			
}



