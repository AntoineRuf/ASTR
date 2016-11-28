using UnityEngine;
using System.Collections.Generic;

public class ComeBackHere : Skill
{

    public override string Name
    {
        get { return "Come Back Here!"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Pulls a target with the swipe of an axe, dealing damage."; }
        set { }
    }

    public override int MinRange
    {
        get { return 2; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 3; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 15; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 19; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 3; }
        set { }
    }

    public override int CurrentCooldown
    {
        get { return 0; }
        set { }
    }

    public override bool CanTargetEmptyCell
    {
        get { return false; }
        set { }
    }

    public override bool CanTargetEnemies
    {
        get { return true; }
        set { }
    }

    public override bool CanTargetAllies
    {
        get { return true; }
        set { }
    }

    public override bool AlignmentNeeded
    {
        get { return true; }
        set { }
    }

    public override int isAoE
    {
        get { return 0; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
        set { }
    }

    private void MoveTargetToCaster(Unit caster, Unit receiver, CellGrid cellGrid){

      Vector3 casterPos = Directions.ConvertToCube(caster.Cell.OffsetCoord);
      Vector3 receiverPos = Directions.ConvertToCube(receiver.Cell.OffsetCoord);
      Vector2 destinationCoord = Directions.NearestNeighbor(casterPos, receiverPos);

      var arrived = false;
      var checkedCellCoord = receiverPos;
      var direction = Directions.NearestNeighborDirection(receiverPos, casterPos);
      List<Cell> path = new List<Cell>();

      while (!arrived) {
          checkedCellCoord -= direction;
          var checkedCellOffsetCoord = Directions.ConvertToOffsetCoord(checkedCellCoord);
          var checkedCell = cellGrid.Cells.Find(x => x.OffsetCoord.Equals(checkedCellOffsetCoord));
          if (!checkedCell.IsTaken && !checkedCell.Equals(receiver.Cell)) {
              path.Add(checkedCell);
          }
          else {
              arrived = true;
          }
      }
      if (path.Count > 0) receiver.Dash(path[path.Count - 1], path, cellGrid.trapmanager);
    }

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        foreach (var receiver in receivers)
        {
            int damage = Random.Range(MinDamage, MaxDamage+1);
            MoveTargetToCaster(caster, receiver, cellGrid);
            caster.DealDamage2(receiver, damage);
        }

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {

        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        foreach (var currentCell in cells)
        {
            if (currentCell.Occupent != null)
            {
                int damage = Random.Range(MinDamage, MaxDamage+1);
                MoveTargetToCaster(caster, currentCell.Occupent, cellGrid);
                caster.DealDamage2(currentCell.Occupent, damage);
            }
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
