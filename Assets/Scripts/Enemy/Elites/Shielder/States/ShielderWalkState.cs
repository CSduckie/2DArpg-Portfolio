using UnityEngine;

public class ShielderWalkState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("Walk",0f);
    }
    
    public override void Update()
    {
        //检查，进入攻击距离，攻击
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
