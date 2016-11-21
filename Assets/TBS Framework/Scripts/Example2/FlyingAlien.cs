using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAlien : Alien
{
    public void Initialize()
    {
        base.Initialize();
        (Cell as MyOtherHexagon).IsSkyTaken = true;
    }

    public override bool IsCellTraversable(Cell cell)
    {
        return !(cell as MyOtherHexagon).IsSkyTaken;//Allows unit to move through any cell that is not occupied by a flying unit.
    }
    

    protected override IEnumerator MovementAnimation(List<Cell> path)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 6;
        yield return StartCoroutine(base.MovementAnimation(path));
        GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    protected override void OnDestroyed()
    {
        (Cell as MyOtherHexagon).IsSkyTaken = false;
        base.OnDestroyed();
    }
}