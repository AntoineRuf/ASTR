using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomUnitGenerator : MonoBehaviour, IUnitGenerator
{
    public Transform UnitsParent;
    public Transform CellsParent;
    public static int CurrentUnit;

    /// <summary>
    /// Returns units that are already children of UnitsParent object.
    /// </summary>
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> ret = new List<Unit>();
        Vector2[] spawnPos = new Vector2[6];
        spawnPos[0] = new Vector2(3, 1);
        spawnPos[1] = new Vector2(1, 1);
        spawnPos[2] = new Vector2(1, 3);
        spawnPos[3] = new Vector2(13, 8);
        spawnPos[4] = new Vector2(13, 10);
        spawnPos[5] = new Vector2(11, 10);
        SquadSelectionToFightScene Go = FindObjectOfType<SquadSelectionToFightScene>();
        Debug.Log(Go.squad1.playerData[0].Skills[0].Name);
        List<UnitData> units = new List<UnitData>();
        foreach (UnitData unitdata in Go.squad1.playerData)
        {
            units.Add(unitdata);
        }
        foreach (UnitData unitdata2 in Go.squad2.playerData)
        {
            units.Add(unitdata2);
        }
        for (int i = 0; i < units.Count; i++)
        {
            CurrentUnit = i;
            if (units[i].Class == "Warrior")
            {
                Transform Prefab = Instantiate(Resources.Load<Transform>("Prefabs/ASTR/Unit/Warrior0"));
                Warrior unit = Prefab.GetComponent<Warrior>();
                if (unit != null)
                {
                    var cell = cells.Find(x => x.OffsetCoord.Equals(spawnPos[i]));
                    if (!cell.IsTaken)
                    {
                        cell.IsTaken = true;
                        unit.Cell = cell;
                        unit.transform.position = cell.transform.position;
                        unit.Initialize();
                        unit.PlayerNumber = i;
                        unit.TeamNumber = i/3;
                        unit.UnitName = units[i].Name;
                        Prefab.transform.parent = UnitsParent;
                        ret.Add(unit);
                    }//Unit gets snapped to the nearest cell
                    else
                    {
                        Destroy(unit.gameObject);
                    }//If the nearest cell is taken, the unit gets destroyed.
                }
                else
                {
                    Debug.LogError("Invalid object in Units Parent game object");
                }
            }
            else if (units[i].Class == "Mage")
            {
                Transform Prefab = Instantiate(Resources.Load<Transform>("Prefabs/ASTR/Unit/Mage0"));
                Mage unit = Prefab.GetComponent<Mage>();
                if (unit != null)
                {
                    var cell = cells.Find(x => x.OffsetCoord.Equals(spawnPos[i]));
                    if (!cell.IsTaken)
                    {
                        cell.IsTaken = true;
                        unit.Cell = cell;
                        unit.transform.position = cell.transform.position;
                        unit.Initialize();
                        unit.PlayerNumber = i;
                        unit.TeamNumber = i / 3;
                        unit.UnitName = units[i].Name;
                        Prefab.transform.parent = UnitsParent;
                        ret.Add(unit);
                    }//Unit gets snapped to the nearest cell
                    else
                    {
                        Destroy(unit.gameObject);
                    }//If the nearest cell is taken, the unit gets destroyed.
                }
                else
                {
                    Debug.LogError("Invalid object in Units Parent game object");
                }
            }
            else
            {
                Transform Prefab = Instantiate(Resources.Load<Transform>("Prefabs/ASTR/Unit/Rogue0") );
                Rogue unit = Prefab.GetComponent<Rogue>();
                if (unit != null)
                {
                    var cell = cells.Find(x => x.OffsetCoord.Equals(spawnPos[i]));
                    if (!cell.IsTaken)
                    {
                        cell.IsTaken = true;
                        unit.Cell = cell;
                        unit.transform.position = cell.transform.position;
                        unit.Initialize();
                        unit.PlayerNumber = i;
                        unit.TeamNumber = i / 3;
                        unit.UnitName = units[i].Name;
                        Prefab.transform.parent = UnitsParent;
                        ret.Add(unit);
                    }//Unit gets snapped to the nearest cell
                    else
                    {
                        Destroy(unit.gameObject);
                    }//If the nearest cell is taken, the unit gets destroyed.
                }
                else
                {
                    Debug.LogError("Invalid object in Units Parent game object");
                }
            }
        }
        return ret;
    }

    public void SnapToGrid()
    {
        List<Transform> cells = new List<Transform>();

        foreach (Transform cell in CellsParent)
        {
            cells.Add(cell);
        }

        foreach (Transform unit in UnitsParent)
        {
            var closestCell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
            if (!closestCell.GetComponent<Cell>().IsTaken)
            {
                Vector3 offset = new Vector3(0, 0, closestCell.GetComponent<Cell>().GetCellDimensions().z);
                unit.position = closestCell.transform.position - offset;
            }//Unit gets snapped to the nearest cell
        }
    }
}