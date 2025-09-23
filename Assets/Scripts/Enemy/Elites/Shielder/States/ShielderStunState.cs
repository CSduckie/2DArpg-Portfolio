using UnityEngine;

public class ShielderStunState : ShielderStateBase
{
    public override void Enter()
    {
        Debug.Log("Entering Shielder Stun State");
        shielder.isStun = true;
        shielder.Stun();
        shielder.PlayAnimation("Hurt",0f);
    }

    public override void Update()
    {
        if ( shielder.isStun == false)
        {
            shielder.ChangeState(ShielderState.Idle);
            return;
        }
    }
    
    
    public override void Exit()
    {
        shielder.GetComponent<ShielderStats>().clearCoolingUI();
    }

}
