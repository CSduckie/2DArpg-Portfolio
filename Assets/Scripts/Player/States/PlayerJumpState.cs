using UnityEngine;

public class PlayerJumpState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Jump",0f);
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, player.JumpSpeed);
    }

    public override void Update()
    {
        player.UpdateVelocity(true);
        if (player.rb.linearVelocityY < 0)
        {
            player.ChangeState(PlayerState.Fall);
            return;
        }
    
        //Check for double jump
        if (CheckAnimatorStateName("Jump", out float animationTime))
        {
            if (animationTime >= 0.6f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.ChangeState(PlayerState.DoubleJump);
                    return;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(PlayerState.Dash);
            return;
        }
        
    }
    

    public override void Exit()
    {
        base.Exit();
    }
}
