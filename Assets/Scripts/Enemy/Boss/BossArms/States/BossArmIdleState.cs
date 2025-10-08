using UnityEngine;
using DG.Tweening;
public class BossArmIdleState : BossArmStateBase
{
    public override void Enter()
    {
        bossArm.rb.linearVelocity = Vector2.zero;
        Debug.Log("BossArmIdleState Enter");
    }

    public override void Update()
    {
        bossArm.rb.linearVelocity = Vector2.zero;
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
