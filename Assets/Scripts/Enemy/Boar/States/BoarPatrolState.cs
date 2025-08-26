using UnityEngine;

public class BoarPatrolState : BoarStateBase
{
    public override void Enter()
    {
        boar.PlayAnimation("Patrol",0f);
    }
    
    public override void Update()
    {
        if (boar.isPlayerFound())
        {
            boar.ChangeState(BoarState.Chase);
            return;
        }
        
        if (!boar.IsGroundDetected() || boar.IsWallDetected())
        {
            boar.ChangeState(BoarState.Idle);
            return;
        }
    }

    public override void FixedUpdate()
    {
        boar.rb.linearVelocity = new Vector2(boar.moveSpeed * (boar.isFacingRight? 1:-1), boar.rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
