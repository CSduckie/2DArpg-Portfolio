using UnityEngine;

public class AssassinIdleState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Idle",0f);
    }

    public override void Update()
    {
        if (assassin.player.transform.position.x > assassin.transform.position.x)
        {
            if (!assassin.isFacingRight)
            {
                assassin.Flip();
            }
        }
        else
        {
            if(assassin.isFacingRight)
                assassin.Flip();
        }
        
        
        if (!assassin.player.isDead)
        {
            if (Mathf.Abs(assassin.transform.position.x - assassin.player.transform.position.x) > assassin.attackDistance)
            {
                assassin.ChangeState(AssassinStates.Chase);
                return;
            }
        }

        if (assassin.canAttack)
        {
            assassin.ChangeState(AssassinStates.Attack);
            return;
        }
        
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
