using UnityEngine;
using System.Collections.Generic;

public class RagingBull : Skill
{

    public override string Name
    {
        get { return "Raging Bull"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Charges towards an enemy, dealing damage to the first unit hit."; }
        set { }
    }

    public override int MinRange
    {
        get { return 2; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 4; }
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
        get { return 2; }
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
        get { return false; }
        set { }
    }

    public override bool AlignmentNeeded
    {
        get { return true; }
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

    private Unit MoveCasterToTarget(Unit caster, Unit receiver, CellGrid cellGrid){

      Vector3 casterPos = Directions.ConvertToCube(caster.Cell.OffsetCoord);
      Vector3 receiverPos = Directions.ConvertToCube(receiver.Cell.OffsetCoord);
      Vector2 destinationCoord = Directions.NearestNeighbor(casterPos, receiverPos);

      var arrived = false;
      var checkedCellCoord = casterPos;
      Cell obstacleCell = null;
      var direction = Directions.NearestNeighborDirection(casterPos, receiverPos);
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
              obstacleCell = checkedCell;
          }
      }
      path.Reverse();
      if (path.Count > 0) caster.Dash(path[path.Count - 1], path, cellGrid.trapmanager);
      return obstacleCell.Occupent;
    }

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        foreach (var receiver in receivers)
        {
            int damage = Random.Range(MinDamage, MaxDamage+1);
            Unit victim = MoveCasterToTarget(caster, receiver, cellGrid);
            Debug.Log(victim);
            Debug.Log(damage);
            caster.DealDamage2(victim, damage);
        }

        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){}
}
