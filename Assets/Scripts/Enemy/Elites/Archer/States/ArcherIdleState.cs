using UnityEngine;

public class ArcherIdleState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Idle",0f);
    }

    public override void Update()
    {
        if (archer.player.position.x > archer.transform.position.x)
        {
            if (!archer.isFacingRight)
            {
                archer.Flip();
            }
        }
        else
        {
            if(archer.isFacingRight)
                archer.Flip();
        }
        
        
        if (archer.canSpecialAttack )
        {
            float skillChance = Random.value;
            if (skillChance <= archer.useSpecialATKChance)
            {
                archer.ChangeState(ArcherStates.SpecialATK);
                return;
            }
        }

        if (archer.canNormalAttack)
        {
            float skillChance = Random.value;
            if (skillChance <= archer.useNormalATKChance)
            {
                archer.ChangeState(ArcherStates.NormalATK);
                return;
            }
        }

        if (Mathf.Abs(archer.transform.position.x - archer.player.position.x) < 3f)
        {
            float skillChance = Random.value;
            if (archer.canTeleport  && skillChance <= archer.useTeleportChance)
            {
                archer.ChangeState(ArcherStates.Teleport);
                return;
            }
        }

        if (Mathf.Abs(archer.transform.position.x - archer.player.position.x) > archer.attackDistance)
        {
            archer.ChangeState(ArcherStates.Chase);
            return;
        }
        
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
