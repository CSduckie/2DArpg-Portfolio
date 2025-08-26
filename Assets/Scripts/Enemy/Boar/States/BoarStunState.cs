using UnityEngine;

public class BoarStunState : BoarStateBase
{
    public override void Enter()
    {
        boar.PlayAnimation("Hurt",0f);
        boar.isStun = true;
        boar.Stun();
    }
    public override void Update()
    {
        if(!boar.isStun)
            boar.ChangeState(BoarState.Patrol);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
