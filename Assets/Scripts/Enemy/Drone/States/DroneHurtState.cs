using UnityEngine;

public class DroneHurtState : DroneStateBase
{
    public override void Enter()
    {
        //drone.rb.linearVelocity = Vector2.zero;
        drone.PlayAnimation("Hurt",0f);
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                drone.ChangeState(DroneState.Chase);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        drone.isHurt = false;
    }
}
