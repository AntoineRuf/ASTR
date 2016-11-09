using UnityEngine;
using System.Collections;

class CellGridStateWaitingForInput : CellGridState
{
    public CellGridStateWaitingForInput(CellGrid cellGrid) : base(cellGrid)
    {
        Debug.Log("CellGridState : " + cellGrid.CellGridState);
    }

    public override void OnUnitClicked(Unit unit)
    {
        if(unit.PlayerNumber.Equals(_cellGrid.CurrentPlayerNumber))
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, unit); 
    }
}
