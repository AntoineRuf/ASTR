using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

class OnClickDirectionChoice : MonoBehaviour
{
    public Color StartColor;
    public Color OnClickColor;
    public Color OnMouseOverColor;
    public bool hovering = false;
    public bool clicked = false;

    void OnMouseEnter()
    {
        hovering = true;
        GetComponent<SpriteRenderer>().color = OnMouseOverColor;
        //GetComponent<SpriteRenderer>().color = OnMouseOverColor;
    }

    void OnMouseExit()
    {
        hovering = false;
        GetComponent<SpriteRenderer>().color = StartColor;
    }

    void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = OnClickColor;
        clicked = true;
    }
        
}
