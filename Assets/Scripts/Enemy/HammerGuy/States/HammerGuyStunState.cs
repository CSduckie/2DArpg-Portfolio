using UnityEngine;

public class HammerGuyStunState : HammerGuyStateBase
{
    public override void Enter()
    {
        hammerGuy.isStun = true;
        hammerGuy.PlayAnimation("Hurt",0f);
        hammerGuy.RecoverStun();
    }

    public override void Update()
    {
        if (!hammerGuy.isStun)
        {
            hammerGuy.ChangeState(HammerGuyStates.Battle);
            return;
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
