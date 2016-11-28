using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    public class TrapManager
    {
        public List<Trap> TrapList;
        public CellGrid _cellgrid;
        
        public TrapManager(CellGrid cellgrid)
        {
            TrapList = new List<Trap>();
            _cellgrid = cellgrid;
        }

        public void AddTrap(Trap trapToAdd)
        {
            TrapList.Add(trapToAdd);
            UnityEngine.Debug.Log("Trap cree.");
        }

        public void Trigger(Unit unit)
        {
            Cell location = unit.Cell;
            Trap trapToTrigger = TrapList.Find(t => t.Location = location);
            UnityEngine.Debug.Log("Trap declenche.");
            trapToTrigger.Apply(unit);
            TrapList.Remove(trapToTrigger);
            UnityEngine.Debug.Log("Trap detruit.");
        }

        public void Update(Unit unit)
        {
            foreach(Trap t in TrapList)
            {
                if(t.Owner == unit)
                {
                    t.Duration--;
                    if (t.Duration <= 0)
                    {
                        t.Expires();
                        TrapList.Remove(t);
                    }
                }
            }
        }

        public bool findTrap(Cell location)
        {
            if (TrapList.Any())
            { 
                Trap potentialTrap = TrapList.Find(t => t.Location = location);
                if (potentialTrap != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
