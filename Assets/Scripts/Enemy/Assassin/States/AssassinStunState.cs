using UnityEngine;

public class AssassinStunState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.isStun = true;
        assassin.PlayAnimation("Hurt",0f);
        assassin.RecoverStun();
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
