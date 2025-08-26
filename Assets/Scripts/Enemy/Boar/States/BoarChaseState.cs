using UnityEngine;

public class BoarChaseState : BoarStateBase
{
    private float lostTimer;
    public override void Enter()
    {
        boar.PlayAnimation("Chase",0f);
        boar.attackBox.SetActive(true);
    }
    
    public override void Update()
    {
        boar.rb.linearVelocity = new Vector2(boar.chaseSpeed * (boar.isFacingRight? 1:-1), boar.rb.linearVelocityY);
        //检查丢失仇恨
        if (!boar.isPlayerFound())
            lostTimer += Time.deltaTime;
        else
            lostTimer = 0;
        
        if (lostTimer >= boar.maxLostTime)
        {
            boar.ChangeState(BoarState.Patrol);
            return;
        }
        
        if (!boar.IsGroundDetected() || boar.IsWallDetected())
        {
            boar.Flip();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        boar.attackBox.SetActive(false);
    }

}
