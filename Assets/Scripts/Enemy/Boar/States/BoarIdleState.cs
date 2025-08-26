using UnityEngine;

public class BoarIdleState : BoarStateBase
{
    private float currentRestTime;
    public override void Enter()
    {
        boar.PlayAnimation("Idle", 0f);
        currentRestTime = 0f;
        boar.rb.linearVelocity = new Vector2(0, boar.rb.linearVelocityY);
    }
    
    public override void Update()
    {
        currentRestTime += Time.deltaTime;
        if (currentRestTime >= boar.restTime)
        {
            boar.ChangeState(BoarState.Patrol);
            boar.Flip();
            return;
        }

    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
