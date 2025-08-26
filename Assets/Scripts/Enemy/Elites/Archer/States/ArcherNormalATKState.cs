using UnityEngine;

public class ArcherNormalATKState : ArcherStateBase
{
    public override void Enter()
    {
        archer.isNormalAttack = true;
        archer.PlayAnimation("NormalATK",0f);
        archer.rb.linearVelocity = Vector2.zero;
        
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
        
        Debug.Log("普通攻击");
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("NormalATK", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                archer.ChangeState(ArcherStates.Idle);
                return;
            }
        }
        
    }
    
    public override void Exit()
    {
        archer.CoolNormalAttack();
        //强制关闭攻击判定盒
        archer.archerWeapons[0].SetActive(false);
        //标记状态
        archer.isNormalAttack = false;
    }

}
