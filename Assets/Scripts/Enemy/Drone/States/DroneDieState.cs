using UnityEngine;

public class DroneDieState : DroneStateBase
{
    public override void Enter()
    {
        drone.PlayAnimation("Die",0f);
        //启用无人机的重力
        drone.rb.gravityScale = 1;
    }
    
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
