using UnityEngine;

public class DroneStunState : DroneStateBase
{
    public override void Enter()
    {
        drone.PlayAnimation("Hurt",0f);
        drone.isStun = true;
        drone.Stun();
    }
    
    public override void Update()
    {
        if(!drone.isStun)
            drone.ChangeState(DroneState.Battle);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
