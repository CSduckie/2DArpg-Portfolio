using UnityEngine;

public class DroneIdleState : DroneStateBase
{
    private float currentRestTime;
    public override void Enter()
    {
        drone.rb.linearVelocity = Vector2.zero;
        currentRestTime = 0;
        drone.PlayAnimation("Idle",0f);
    }
    
    public override void Update()
    {
        currentRestTime += Time.deltaTime;
        if (currentRestTime >= drone.restTime)
        {
            drone.ChangeState(DroneState.Patrol);
            return;
        }

        if (drone.isPlayerFound())
        {
            drone.ChangeState(DroneState.Chase);
            return;
        }
    }
    
    
    public override void Exit()
    {
        base.Exit();
    }

}
