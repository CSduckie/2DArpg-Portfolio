using UnityEngine;

public class BossDroneIdleState : BossDroneStateBase
{
    public override void Enter()
    {
        bossDrone.ChangeState(BossDroneState.MoveToReadyPos);
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
