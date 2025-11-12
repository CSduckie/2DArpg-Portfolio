using UnityEngine;

public class ArcherAppearState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("TeleportExit",0f);
    }
    
    public override void Update()
    {
        if(CheckAnimatorStateName("TeleportExit",out float animationTime))
        {
            if (animationTime >= 0.9f )
            {
                archer.ChangeState(ArcherStates.Idle);
                return;
            }
        }
    }
    
}
