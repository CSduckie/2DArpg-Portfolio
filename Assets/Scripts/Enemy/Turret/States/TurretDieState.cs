using UnityEngine;

public class TurretDieState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Death",0f);
        
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
