using UnityEngine;
using System.Collections.Generic;

public class MyOtherHexagon : Hexagon
{
    public GroundType GroundType;
    public bool IsSkyTaken;//Indicates if a flying unit is occupying the cell.
    public CellColors cellGridColors;
    public void Start()
    {
        cellGridColors = transform.parent.gameObject.GetComponent<CellColors>();
        SetColor(cellGridColors.start);
    }

    public override void MarkAsReachable()
    {
        SetColor(cellGridColors.reachable);
    }
    public override void MarkAsPath()
    {
        SetColor(cellGridColors.path);
    }
    public override void MarkAsHighlighted()
    {
        SetColor(cellGridColors.higlighted);
    }
    public override void MarkAsSkillRange()
    {
        SetColor(cellGridColors.skillRange);
    }
    public override void MarkAsSkillRangeSelected()
    {
        SetColor(cellGridColors.skillRangeSelected);
    }
    public override void UnMark()
    {
        SetColor(new Color(1, 1, 1, 0));
    }

    private void SetColor(Color color)
    {
        var highlighter = transform.FindChild("Highlighter");
        var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        foreach (Transform child in highlighter.transform)
        {
            var childColor = new Color(color.r,color.g,color.b,1);
            spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) continue;

            child.GetComponent<SpriteRenderer>().color = childColor;
        }
    }

    /// <summary>
    /// Method indicates if it is possible to target a cell
    /// </summary>
    public override bool IsCellTargetable(Cell targetCell, int minRange, int maxRange, bool inLine)
    {
        if (GetDistance(targetCell) <= maxRange && GetDistance(targetCell) >= minRange)
            return true;

        return false;
    }

    /// <summary>
    /// Indicates if there is a line between two cells. The parameter maxRange is used for optimization
    /// </summary>
    public override bool isCellInLine(Cell targetCell, int maxRange)
    {
        Vector2 tmpOffset = new Vector2();
        Vector3 tmpCube = new Vector3();
        foreach (var direction in Directions.getDirections())
        {
            tmpOffset = OffsetCoord;
            tmpCube = Directions.ConvertToCube(tmpOffset);
            for (int i = 0; i < maxRange; ++i)
            {
                tmpCube += direction;
                if (Directions.ConvertToOffsetCoord(tmpCube).Equals(targetCell.OffsetCoord))
                {
                    return true;
                }

            }
        }
        return false;
    }

    public override Vector3 GetCellDimensions()
    {
        var ret = GetComponent<SpriteRenderer>().bounds.size;
        return ret*0.98f;
    }
}

public enum GroundType
{
    Land,
    Water
};