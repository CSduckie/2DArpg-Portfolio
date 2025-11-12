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
            turret.ChangeState(TurretState.Seer);
            //Debug.Log("Found!");
            return;
        }
            
    }

    public override void Exit()
    {
        base.Exit();
    }

}
