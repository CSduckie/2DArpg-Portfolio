using UnityEngine;

public class TurretCloseState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Close",0f);
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Close", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                turret.ChangeState(TurretState.Idle);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
