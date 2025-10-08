using UnityEngine;

public class BossSmashAttackState : BossStateBase
{
    private bool hitGround;
    private Vector2 groundPoint;
    private float groundTimeCounter;
    private enum SmashAttackChildState
    {
        FindTarget,
        Smash,
        GoBack,
    }
    
    private SmashAttackChildState smashAttackChildState;
    private SmashAttackChildState smashattackchildState
    {
        get =>smashAttackChildState;
        set{
            smashAttackChildState = value;
            switch(smashAttackChildState)
            {
                case SmashAttackChildState.FindTarget:
                    
                    break;
                case SmashAttackChildState.Smash:
                    //boss.PlayAnimation("DropBombs",0f);
                    //TODO:启动一个下砸速度
                    boss.rb.linearVelocityY = -20f;
                    //检测下方地面位置
                    var hit = Physics2D.Raycast(boss.transform.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
                    groundPoint = hit.point;
                    break;
                case SmashAttackChildState.GoBack:
                    break;
            }
        }
    }
    
    public override void Enter()
    {
        //移动到玩家正上方，然后下砸
        groundTimeCounter = 0;
        boss.canSmash = false;
        hitGround = false;
        boss.smashHitBox.SetActive(true);
        smashattackchildState = SmashAttackChildState.FindTarget;
    }

    public override void Update()
    {
        switch(smashattackchildState)
        {
            case SmashAttackChildState.FindTarget:
                break;
            case SmashAttackChildState.Smash:
                if (boss.IsGroundDetected() && !hitGround)
                {
                    hitGround = true;
                    boss.smashHitBox.SetActive(false);
                    boss.rb.linearVelocityY = 0f;
                    boss.transform.position = new Vector2(groundPoint.x, groundPoint.y + boss.GetComponent<CircleCollider2D>().radius);
                    boss.PlayAnimation("SmashAttack",0f);
                }
                groundTimeCounter += Time.deltaTime;
                //在地面停留5秒
                if (groundTimeCounter >= 5f)
                {
                    smashattackchildState = SmashAttackChildState.GoBack;
                }
                break;
            case SmashAttackChildState.GoBack:
                break;
        }
        //CheckForStun
        if (boss.willStun)
        {
            boss.ChangeState(BossStates.Stun);
            return;
        }
    }

    public override void FixedUpdate()
    {
        switch(smashattackchildState)
        {
            case SmashAttackChildState.FindTarget:
                FindTargetOnFixedUpdate();
                break;
            case SmashAttackChildState.Smash:
                break;
            case SmashAttackChildState.GoBack:
                GoBackToBossOnFixedUpdate();
                break;
        }
    }

    private void FindTargetOnFixedUpdate()
    {
        var dir = GameManager.instance.player.transform.position.x - boss.transform.position.x > 0 ? 1 : -1;
        if (Mathf.Abs(boss.transform.position.x - GameManager.instance.player.transform.position.x) > 0.1f)
        {
            boss.rb.linearVelocity = new Vector2(dir * boss.bossSpeed,0f);
        }
        else
        {
            boss.rb.linearVelocity = Vector2.zero;
            smashattackchildState = SmashAttackChildState.Smash;
        }
    }
    
    private void GoBackToBossOnFixedUpdate()
    {
        Vector2 dir = (boss.heightPoint.position - boss.transform.position).normalized;
        if (Vector2.Distance(boss.heightPoint.position, boss.transform.position) > 0.1f)
        {
            boss.rb.linearVelocity = dir * boss.bossSpeed;
        }
        else
        {
            boss.transform.position = boss.heightPoint.position;
            boss.rb.linearVelocity = Vector2.zero;
            boss.ChangeState(BossStates.Idle);
        }
    }
    
    public override void Exit()
    {
        boss.CoolSmashSkill();
        
    }

}
