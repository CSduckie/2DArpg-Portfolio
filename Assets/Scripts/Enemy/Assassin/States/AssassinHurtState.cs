using UnityEngine;

public class AssassinHurtState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Hurt",0f);
        assassin.isHurt = true;
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("Hurt", out float animationTime))
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
        assassin.isHurt = false;
    }

}
