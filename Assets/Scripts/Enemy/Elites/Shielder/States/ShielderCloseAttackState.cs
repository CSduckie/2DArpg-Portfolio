using UnityEngine;

public class ShielderCloseAttackState : ShielderStateBase
{
    public override void Enter()
    {
        Debug.Log("进入closeAttack");
        shielder.PlayAnimation("CloseAttack",0f);
        shielder.isCloseAttack = true;
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("CloseAttack", out float animationTime))
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
        shielder.CoolClosesAttack();
        shielder.shielderWeapons[0].SetActive(false);
    }

}
