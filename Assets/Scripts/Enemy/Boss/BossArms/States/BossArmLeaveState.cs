using UnityEngine;
using VHierarchy.Libs;

public class BossArmLeaveState : BossArmStateBase
{
    public override void Enter()
    {
        bossArm.DestroySelfOn(5f);
        Debug.Log("BossArmLeaveState Enter");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        bossArm.rb.linearVelocity = new Vector2(bossArm.moveSpeed * 1.5f * (bossArm.isFacingRight? 1:-1), bossArm.rb.linearVelocityY);
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
