using UnityEngine;

public class AssassinATKState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Attack",0f);
        assassin.isAttack = true;
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("Attack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                assassin.ChangeState(AssassinStates.Idle);
                return;
            }
        }
    }
    
    
    public override void Exit()
    {
        assassin.isAttack = false;
        assassin.CoolAttack();
    }
}
