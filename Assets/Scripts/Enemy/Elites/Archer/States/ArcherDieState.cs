using UnityEngine;

public class ArcherDieState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Dead",0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
