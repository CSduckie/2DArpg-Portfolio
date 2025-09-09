using UnityEngine;

public class ShielderCloseAttackState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("CloseAttack",0f);
    }

    public override void Update()
    {
        base.Update();
    }
    
    
    public override void Exit()
    {
        base.Exit();
    }

}
