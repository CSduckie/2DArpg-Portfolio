using UnityEngine;

public class BoarDieState : BoarStateBase
{
    public override void Enter()
    {
        Debug.Log("似了");
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
