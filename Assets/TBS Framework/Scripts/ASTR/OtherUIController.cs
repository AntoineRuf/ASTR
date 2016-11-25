using System;
using UnityEngine;
using UnityEngine.UI;

class OtherUIController : MonoBehaviour
{
    public CellGrid CellGrid;
    public Transform UnitsParent;
    public Button NextTurnButton;
    public Text InfoText;
    public Transform HealthbarFull;
    public Transform HealthbarText;
    public Transform BuffPanel;
    public Transform Portrait;
    public Transform UnitName;


    private void Start()
    {
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;
    }

    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;

        OnUnitDehighlighted(sender, e);

        if ((sender as Unit).HitPoints <= 0) return;

        OnUnitHighlighted(sender, e);
    }
    private void OnGameStarted(object sender, EventArgs e)
    {
        foreach (Transform unit in UnitsParent)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
        }
        InfoText.text = "Player " + (CellGrid.CurrentPlayerNumber + 1);

        OnTurnEnded(sender, e);
    }
    private void OnGameEnded(object sender, EventArgs e)
    {
    }
    private void OnTurnEnded(object sender, EventArgs e)
    {
        NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
        InfoText.text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1);
    }

    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        CellGrid.MouseExitUnitUI();
    }
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        if (sender as Unit != CellGrid.UnitList[CellGrid.Turn])
        {
            CellGrid.HealthbarUpdate(sender as Unit, HealthbarText, HealthbarFull);
            CellGrid.NameUpdate(sender as Unit, UnitName.GetComponent<Text>());
            CellGrid.BuffsUpdate(sender as Unit, BuffPanel);
            CellGrid.PortraitUpdate(sender as Unit, Portrait, CellGrid.UnitList[CellGrid.Turn].TeamNumber);
            CellGrid.MouseEnterUnitUI();
        }
        
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}

