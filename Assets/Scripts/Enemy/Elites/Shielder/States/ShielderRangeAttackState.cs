using UnityEngine;

public class ShielderRangeAttackState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("RangedAttack",0f);
        shielder.isRangeAttack = true;
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("RangedAttack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                shielder.ChangeState(ShielderState.Idle);
                return;
            }
        }
    }
    
    
    public override void Exit()
    {
        shielder.CoolRangeAttack();
        shielder.isRangeAttack = false;
        shielder.shielderWeapons[1].SetActive(false);
    }

}
