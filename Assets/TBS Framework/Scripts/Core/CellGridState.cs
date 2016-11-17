using System.Linq;
using UnityEngine;

public abstract class CellGridState
{
    protected CellGrid _cellGrid;
    
    public CellGridState(CellGrid cellGrid)
    {
        _cellGrid = cellGrid;
    }

    public virtual void OnUnitClicked(Unit unit)
    {
    }

    public virtual void OnUnitSelected(Unit unit)
    { 
    }

    public virtual void OnUnitDeselected(Unit unit)
    {
    }

    public virtual void OnUnitForTargetSelected(Unit unit)
    {
    }

    public virtual void OnUnitForTargetDeselected(Unit unit)
    {
    }

    public virtual void OnCellDeselected(Cell cell)
    {
        cell.UnMark();
    }
    public virtual void OnCellSelected(Cell cell)
    {
        //Debug.Log("Selected Cell : " + cell.OffsetCoord + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord)));
        cell.MarkAsHighlighted();
    }
    public virtual void OnCellClicked(Cell cell)
    {
        /*Vector3 up = new Vector3(0, 1, -1);
        Vector3 down = new Vector3(0, -1, 1);
        Vector3 upR = new Vector3(+1, 0, -1);
        Vector3 upL = new Vector3(-1, 1, 0);
        Vector3 downR = new Vector3(+1, -1, 0);
        Vector3 downL = new Vector3(-1, 0, 1);
        Debug.Log("Selected Cell : " + cell.OffsetCoord + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord)));
        Debug.Log("Down L ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + downL));
        Debug.Log("Down R ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + downR));
        Debug.Log("UP L ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + upL));
        Debug.Log("UP R ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + upR));
        Debug.Log("Down ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + down));
        Debug.Log("Up ? : " + ConvertToOffsetCoord(ConvertToCube(cell.OffsetCoord) + up));*/
    }

    public virtual void OnStateEnter()
    {
        if (_cellGrid.Units.Select(u => u.PlayerNumber).Distinct().ToList().Count == 1)
        {
            _cellGrid.CellGridState = new CellGridStateGameOver(_cellGrid);
        }
    }
    public virtual void OnStateExit()
    {
    }

    /// <summary>
    /// Convert OffsetCoord to Cube
    /// </summary>
    Vector2 ConvertToOffsetCoord(Vector3 v)
    {
        return new Vector2(v.x, (v.z + (v.x + (Mathf.Abs(v.x) % 2)) / 2));

    }

    /// <summary>
    /// Convert OffsetCoord to Cube
    /// </summary>
    Vector3 ConvertToCube(Vector2 v)
    {
        float x = v.x;
        float z = v.y - (v.x + (Mathf.Abs(v.x) % 2)) / 2;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    

}