using UnityEngine;

public class PlayerHurtState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Hurt",0f);
    }
    
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerState.Jump);
            return;
        }

        
        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Idle);
                return;
            }
        }
    }

    public override void Exit()
    {
        player.isHurt = false;
    }

}
