using UnityEngine;

public class BossStunState : BossStateBase
{
    private float currentStateTimer;
    private bool hitGround;
    private Vector2 groundPoint;
    public override void Enter()
    {
        Debug.Log("Boss Stun Enter");
        boss.isStun = true;
        currentStateTimer = 0;

        if (!boss.IsGroundDetected())
        {
            hitGround = false;
            boss.rb.linearVelocity = new  Vector2(0, -10f);
        }
        else
            hitGround = true;
        
        //检测下方地面位置
        var hit = Physics2D.Raycast(boss.transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        groundPoint = hit.point;
    }


    public override void Update()
    {
        currentStateTimer += Time.deltaTime;
        if (currentStateTimer >= boss.stunTime)
        {
            currentStateTimer = 0;
            boss.ChangeState(BossStates.Idle);
        }
        
        
        if (boss.IsGroundDetected() && !hitGround)
        {
            hitGround = true;
            boss.rb.linearVelocityY = 0f;
            boss.transform.position = new Vector2(groundPoint.x, groundPoint.y + boss.GetComponent<CircleCollider2D>().radius);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        boss.bossStats.clearCoolingUI();
        boss.isStun = false;
        boss.willStun = false;
    }
}
