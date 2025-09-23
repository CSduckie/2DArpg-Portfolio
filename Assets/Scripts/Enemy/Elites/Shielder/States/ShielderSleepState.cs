using UnityEngine;

public class ShielderSleepState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("Idle",0f);
        shielder.isSleeping = true;
    }

    public override void Exit()
    {
        shielder.enemyUI.SetActive(true);
        shielder.isSleeping = false;
    }
    
}
