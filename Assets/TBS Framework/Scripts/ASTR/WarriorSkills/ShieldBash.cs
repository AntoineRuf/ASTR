using UnityEngine;
using System.Collections.Generic;

public class ShieldBash : Skill
{

    public override string Name
    {
        get { return "Shield Bash"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Bashes the target, knocking it back 2 spaces."; }
        set { }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 1; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 17; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 21; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 3; }
        set { }
    }

    public override bool CanTargetEmptyCell
    {
        get { return true; }
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
        get { return false; }
        set { }
    }

    public override int isAoE
    {
        get { return 1; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
        set { }
    }

    private int KnockbackTarget(Unit caster, Unit receiver, CellGrid cellGrid, int kbRange){

      Vector3 casterPos = Directions.ConvertToCube(caster.Cell.OffsetCoord);
      Vector3 receiverPos = Directions.ConvertToCube(receiver.Cell.OffsetCoord);
      Vector2 destinationCoord = Directions.NearestNeighbor(casterPos, receiverPos);

      var arrived = false;
      var checkedCellCoord = receiverPos;
      Cell obstacleCell = null;
      var direction = Directions.NearestNeighborDirection(receiverPos, casterPos);
      List<Cell> path = new List<Cell>();

      while (!arrived) {
          checkedCellCoord += direction;
          var checkedCellOffsetCoord = Directions.ConvertToOffsetCoord(checkedCellCoord);
          var checkedCell = cellGrid.Cells.Find(x => x.OffsetCoord.Equals(checkedCellOffsetCoord));
          if (checkedCell != null && !checkedCell.IsTaken && path.Count < kbRange) {
              path.Add(checkedCell);
          }
          else {
              arrived = true;
              obstacleCell = checkedCell;
          }
      }
      if (path.Count > 0 && (receiver.CCImmunity==0)) {
          receiver.Dash(path[path.Count - 1], path, cellGrid.trapmanager);
          if (obstacleCell == null) return 3;
          else if (obstacleCell.Occupent != null && path.Count < kbRange) {
              caster.DealDamage2(obstacleCell.Occupent, 3);
              return 3;
          }
          else return 0;
      }
      else return 0;
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
            var hit = KnockbackTarget(caster, receiver, cellGrid, 2);
            caster.DealDamage2(receiver, damage + hit);
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
                var hit = KnockbackTarget(caster, currentCell.Occupent, cellGrid, 2);
                caster.DealDamage2(currentCell.Occupent, damage);
            }
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
