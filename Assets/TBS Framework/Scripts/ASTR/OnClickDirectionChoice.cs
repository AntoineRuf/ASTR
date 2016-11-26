using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

class OnClickDirectionChoice : MonoBehaviour
{
    public Sprite Start;
    public Sprite OnMouseOver;
    public bool hovering = false;
    public bool clicked = false;

    void OnMouseEnter()
    {
        hovering = true;
        GetComponent<SpriteRenderer>().sprite = OnMouseOver;
        //GetComponent<SpriteRenderer>().color = OnMouseOverColor;
    }

    void OnMouseExit()
    {
        hovering = false;
        GetComponent<SpriteRenderer>().sprite = Start;
    }

    void OnMouseDown()
    {
        //GetComponent<SpriteRenderer>().color = OnClickColor;
        clicked = true;
    }
        
}
