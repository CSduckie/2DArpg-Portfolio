using UnityEngine;

public class TurretDieState : TurretStateBase
{
    public override void Enter()
    {
        //TODO:目前直接使用播放关闭动画来实现这个效果
        turret.PlayAnimation("Close",0f);
        Debug.Log("进入死亡状态");
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
