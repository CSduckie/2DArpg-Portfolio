using UnityEngine;

public class PlayerRestInState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("ToRest",0f);
        player.isResting = true;
        player.rb.linearVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        player.isResting = false;
    }

}
