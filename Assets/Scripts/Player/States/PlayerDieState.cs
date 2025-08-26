using UnityEngine;

public class PlayerDieState : PlayerStateBase
{
    public override void Enter()
    {
        player.rb.linearVelocity = Vector2.zero;
        player.isDead = true;
        player.PlayAnimation("Die",0f);
    }
    
    public override void Update()
    {
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

}
