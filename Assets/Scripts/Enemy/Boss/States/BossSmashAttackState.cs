using UnityEngine;

public class BossSmashAttackState : BossStateBase
{
    private bool hitGround;
    private Vector2 groundPoint;
    public override void Enter()
    {
        hitGround = false;
        //TODO:启动一个下砸速度
        boss.rb.linearVelocityY = -20f;
        
        //检测下方地面位置
        var hit = Physics2D.Raycast(boss.transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        groundPoint = hit.point;
    }

    public override void Update()
    {
        if (boss.IsGroundDetected() && !hitGround)
        {
            hitGround = true;
            boss.rb.linearVelocityY = 0f;
            boss.transform.position = new Vector2(groundPoint.x, groundPoint.y + boss.GetComponent<CircleCollider2D>().radius);
            boss.PlayAnimation("SmashAttack",0f);
        }

        
        if (CheckAnimatorStateName("SmashAttack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                boss.ChangeState(BossStates.Idle);
            }
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
