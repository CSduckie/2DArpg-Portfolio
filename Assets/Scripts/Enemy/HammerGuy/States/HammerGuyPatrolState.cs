using UnityEngine;

public class HammerGuyPatrolState : HammerGuyStateBase
{
    public override void Enter()
    {
        hammerGuy.PlayAnimation("Walk",0f);
    }
    
    public override void Update()
    {
        if (hammerGuy.isPlayerFound())
        {
            hammerGuy.ChangeState(HammerGuyStates.Chase);
            Debug.Log("FoundPlayer!");
            return;
        }
        
        if (!hammerGuy.IsGroundDetected() || hammerGuy.IsWallDetected())
        {
            hammerGuy.ChangeState(HammerGuyStates.Idle);
            return;
        }
    }

    public override void FixedUpdate()
    {
        hammerGuy.rb.linearVelocity =  new Vector2(hammerGuy.walkSpeed * (hammerGuy.isFacingRight? 1:-1), hammerGuy.rb.linearVelocityY);
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
