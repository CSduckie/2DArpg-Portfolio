using UnityEngine;

public class TurretActiveState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Active",0f);
        //turret.laserController.DeactiveLaser();
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Active", out float animationTime))
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
