using UnityEngine;

public class ArcherSpecialATKState : ArcherStateBase
{
    public override void Enter()
    {
        archer.isSpecialAttack = true;
        archer.PlayAnimation("SpecialATK",0f);
        
        if (archer.player.position.x > archer.transform.position.x)
        {
            if (!archer.isFacingRight)
            {
                archer.Flip();
            }
        }
        else
        {
            if(archer.isFacingRight)
                archer.Flip();
        }
        
        archer.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("SpecialATK", out float animationTime))
        {
            if ( animationTime >= 0.9f)
            {
                archer.ChangeState(ArcherStates.Idle);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        //强制关闭攻击判定盒
        archer.archerWeapons[1].SetActive(false);
        
        //释放大招后会强制冷却所有技能，允许玩家靠近
        archer.CoolNormalAttack();
        archer.CoolTeleport();
        archer.CoolSpecialAttack();
        archer.isSpecialAttack = false;
    }

}
