using UnityEngine;

public class TurretIdleState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Idle",0f);
    }
    public override void Update()
    {
        if (turret.isPlayerFound())
        {
            turret.ChangeState(TurretState.Active);
            return;
        }
            
    }

    public override void Exit()
    {
        base.Exit();
    }

}
