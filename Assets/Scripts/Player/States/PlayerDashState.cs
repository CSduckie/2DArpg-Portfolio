using UnityEngine;

public class PlayerDashState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Dash",0f);
    }
    
    public override void Update()
    {
        if(CheckAnimatorStateName("Dash", out float animationTime))
        {
            if (animationTime <= 0.5f)
            {
                player.rb.linearVelocity = new Vector2(player.dashSpeed  *(player.isFacingRight ? 1:-1), 0);
            }
            else if (animationTime > 0.5f && animationTime < 0.7f)
            {
                player.UpdateVelocity(false);
            }
            else if (animationTime >= 0.7f)
            {
                if (!player.IsGroundDetected())
                    player.ChangeState(PlayerState.Fall);
                else
                    player.ChangeState(PlayerState.Idle);
                return;
            }
        }
    }
    
    
    public override void Exit()
    {
        
    }

}
