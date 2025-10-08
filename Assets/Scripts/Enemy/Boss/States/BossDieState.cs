using UnityEngine;

public class BossDieState : BossStateBase
{
    private bool hitGround;
    private Vector2 groundPoint;
    private float groundTimeCounter;
    public override void Enter()
    {
        boss.PlayAnimation("Die",0f);
        
        if (!boss.IsGroundDetected())
        {
            hitGround = false;
            boss.rb.linearVelocityY = -20f;
        }
        else
            hitGround = true;
        
        //检测下方地面位置
        var hit = Physics2D.Raycast(boss.transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        groundPoint = hit.point;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (boss.IsGroundDetected() && !hitGround)
        {
            hitGround = true;
            boss.smashHitBox.SetActive(false);
            boss.rb.linearVelocityY = 0f;
            boss.transform.position = new Vector2(groundPoint.x, groundPoint.y + boss.GetComponent<CircleCollider2D>().radius);
        }
    }
}
