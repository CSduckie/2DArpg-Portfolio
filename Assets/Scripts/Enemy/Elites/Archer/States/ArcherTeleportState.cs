using UnityEngine;

public class ArcherTeleportState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Teleport",0f);
        archer.isTeleporting = true;
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Teleport", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                archer.transform.position = archer.FindPosition();
                archer.ChangeState(ArcherStates.Idle);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        archer.CoolTeleport();
        archer.isTeleporting = false;
    }

}
