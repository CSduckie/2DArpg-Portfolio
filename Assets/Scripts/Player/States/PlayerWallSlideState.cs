using UnityEngine;

public class PlayerWallSlideState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("WallSlide",0f);
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerState.WallJump);
            return;
        }


        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            float facDir = player.isFacingRight ? 1 : -1;
            if (facDir != Input.GetAxisRaw("Horizontal"))
            {
                player.ChangeState(PlayerState.Idle);
                return;
            }
        }
        
        if(Input.GetAxisRaw("Vertical") < 0)
            player.rb.linearVelocity = new Vector2(0,  player.rb.linearVelocity.y);
        else
            player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y * .7f);
        
        if(player.IsGroundDetected() || !player.IsWallDetected())
            player.ChangeState(PlayerState.Idle);
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
