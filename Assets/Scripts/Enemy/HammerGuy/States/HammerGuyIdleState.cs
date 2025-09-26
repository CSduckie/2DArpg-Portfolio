using UnityEngine;

public class HammerGuyIdleState : HammerGuyStateBase
{
    private float stateTimeCounter;
    public override void Enter()
    {
        hammerGuy.rb.linearVelocity = Vector2.zero;
        hammerGuy.PlayAnimation("Idle",0f);
        stateTimeCounter = 0;
    }

    public override void Update()
    {
        stateTimeCounter += Time.deltaTime;
        
        if (stateTimeCounter >= hammerGuy.idleStateTime)
        {
            hammerGuy.Flip();
            hammerGuy.ChangeState(HammerGuyStates.Patrol);
            return;
        }
        
        
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
