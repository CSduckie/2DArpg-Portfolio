using UnityEngine;

public class ArcherHurtState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Hurt",0f);
        archer.isHurt = true;
    }


    public override void Update()
    {
        if(CheckAnimatorStateName("Hurt",out float animationTime))
        {
            if (animationTime >= 0.9f )
            {
                archer.ChangeState(ArcherStates.Teleport);
                return;
            }
        }
    }
    public override void Exit()
    {
        archer.isHurt = false;
    }
}
