using UnityEngine;

public class PlayerFallState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Fall",0f);
    }

    public override void Update()
    {
        if (player.IsGroundDetected())
        {
            player.ChangeState(PlayerState.Idle);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && player.canDoubleJump)
        {
            player.ChangeState(PlayerState.DoubleJump);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(PlayerState.Dash);
            return;
        }

        if (player.IsWallDetected())
        {
            player.ChangeState(PlayerState.WallSlide);
            return;
        }
        
        player.UpdateVelocity(true);
        
    }

    public override void Exit()
    {
        
    }

}
