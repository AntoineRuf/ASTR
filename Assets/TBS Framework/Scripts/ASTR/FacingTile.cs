using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class FacingTile : MonoBehaviour
{

    public List<Sprite> SpriteList = new List<Sprite>();
    
    public void Start()
    {
    }

    public void ChangeFacingTile(Unit._directions dir)
    {
        switch (dir)
        {
            case Unit._directions.up:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[0];
                break;
            case Unit._directions.up_right:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[1];
                break;
            case Unit._directions.up_left:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[2];
                break;
            case Unit._directions.down:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[3];
                break;
            case Unit._directions.down_right:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[4];
                break;
            case Unit._directions.down_left:
                transform.GetComponentInChildren<SpriteRenderer>().sprite = SpriteList[5];
                break;
        }
    }
}
