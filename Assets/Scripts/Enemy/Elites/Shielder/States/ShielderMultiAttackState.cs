using UnityEngine;

public class ShielderMultiAttackState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("MultiAttack",0f);
        shielder.isMultiAttack = true;
    }


    public override void Update()
    {
        if (CheckAnimatorStateName("MultiAttack", out float animationTime))
        {
            if (animationTime >= 1f)
            {
                shielder.ChangeState(ShielderState.Idle);
                return;
            }
        }
    }

    public override void Exit()
    {
        //由于技能太厉害，退出时将冷却敌人所有技能
        shielder.CoolMultiAttack();
        shielder.CoolClosesAttack();
        shielder.CoolRangeAttack();
        shielder.isMultiAttack = false;
        shielder.shielderWeapons[3].SetActive(false);
    }
    
}
