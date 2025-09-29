using UnityEngine;

public class BossDroneMoveToReadyState : BossDroneStateBase
{
    public override void Enter()
    {
        bossDrone.PlayAnimation("MoveToReady",0f);
        bossDrone.DoMoveTo(bossDrone.readyPos,1f,"MoveToReady");
    }
    
    
    public override void Exit()
    {
        base.Exit();
    }

}
