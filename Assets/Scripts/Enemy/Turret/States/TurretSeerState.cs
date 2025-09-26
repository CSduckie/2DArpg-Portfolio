using UnityEngine;

public class TurretSeerState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Seer",0f);
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Seer", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                turret.ChangeState(TurretState.Battle);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
