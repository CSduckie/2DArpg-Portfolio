using UnityEngine;

public class HammerGuyDieState : HammerGuyStateBase
{
    public override void Enter()
    {
        hammerGuy.PlayAnimation("Die",0f);
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
