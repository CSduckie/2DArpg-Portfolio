using UnityEngine;

public class ShielderController : EnemyController
{
    protected override void Start()
    {
        base.Start();
        
        ChangeState(ShielderState.Idle);
    }
    
    //状态机
    public void ChangeState(ShielderState shielderState)
    {
        switch(shielderState)
        {
            case ShielderState.Idle:
                break;
            case ShielderState.Walk:
                break;
            case ShielderState.Hurt:
                break;
            case ShielderState.Dead:
                break;
            case ShielderState.CloseAttack:
                break;
            case ShielderState.RangeAttack:
                break;
        }
    }
}
