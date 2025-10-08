using UnityEngine;

public class BossArmATKState : BossArmStateBase
{
    public override void Enter()
    {
        Debug.Log("BossArmATKState Enter");
        bossArm.armHitBox.SetActive(true);
    }
    
    public override void Update()
    {
        if (bossArm.IsWallDetected())
        {
            bossArm.Flip();
        }
    }

    public override void FixedUpdate()
    {
        bossArm.rb.linearVelocity = new Vector2(bossArm.moveSpeed * (bossArm.isFacingRight? 1:-1), 0);
    }
    
    public override void Exit()
    { 
        bossArm.armHitBox.SetActive(false);
    }
}
