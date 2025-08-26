using UnityEngine;

public class PlayerRecoverSkillState : PlayerStateBase
{
    private bool healed;
    public override void Enter()
    {
        player.PlayAnimation("Heal",0f);
        player.isUsingSkill = true;
        healed = false;
    }
    
    public override void Update()
    {
        //调用减速逻辑
        player.UpdateVelocity(false);
        
        if (CheckAnimatorStateName("Heal", out float animationTime))
        {
            if (animationTime >= 0.7f && !healed)
            {
                player.playerStats.Heal();
                player.playerStats.UseSkill(player.healSkillConfig.releaseData.attackData.hitData.energyCost);
                healed = true;
            }
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Idle);
                return;
            }
        }
    }

    public override void Exit()
    {
        player.isUsingSkill = false;
    }

}
