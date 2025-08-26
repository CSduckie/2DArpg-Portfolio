using UnityEngine;

public class PlayerStunState : PlayerStateBase
{
    private float currentStunTimeCounter;
    public override void Enter()
    {
        player.PlayAnimation("Stun",0f);
        currentStunTimeCounter = 0;
    }
    
    public override void Update()
    {
        currentStunTimeCounter += Time.deltaTime;
        
        //调用减速逻辑
        player.UpdateVelocity(false);
        
        if (currentStunTimeCounter >= player.stunTime)
        {
            player.ChangeState(PlayerState.Idle);
            return;
        }
    }
    
    public override void Exit()
    {
        player.isStun = false;
    }
    
}
