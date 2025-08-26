using UnityEngine;

public class PlayerDoubleJumpState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("DoubleJump",0f);
        player.canDoubleJump = false;
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocityX, player.DoubleJumpSpeed);
    }

    public override void Update()
    {
        player.UpdateVelocity(true);
        
        if (player.rb.linearVelocityY < 0)
        {
            player.ChangeState(PlayerState.Fall);
            return;
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
