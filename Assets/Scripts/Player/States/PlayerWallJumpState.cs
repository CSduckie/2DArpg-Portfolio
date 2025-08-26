using UnityEngine;

public class PlayerWallJumpState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("WallJump",0f);
        player.rb.linearVelocity = new Vector2(player.wallJumpSpeed.x * -(player.isFacingRight?1:-1) , (player.wallJumpSpeed.y));
        //登墙跳以后让玩家强制FLip
        player.FlipWithNoInput();
        player.isWallJumping = true;
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("WallJump", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Fall);
                return;
            }
        }

        if (player.IsWallDetected())
        {
            player.ChangeState(PlayerState.WallSlide);
            return;
        }
        
        
        if (player.IsGroundDetected())
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    public override void Exit()
    {
        player.isWallJumping = false;
    }

}
