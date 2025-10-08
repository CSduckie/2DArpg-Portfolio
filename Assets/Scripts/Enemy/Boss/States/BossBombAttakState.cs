using UnityEngine;

public class BossBombAttackState : BossStateBase
{
    private float currentStateTimer;
    private float spwanBombTimer;
    private enum BombAttackChildState
    {
        HatchOpen,
        DropBombs,
        HatchClose,
    }
    
    private BombAttackChildState bombAttackChildState;
    private BombAttackChildState bombattackchildState
    {
        get =>bombAttackChildState;
        set{
            bombAttackChildState = value;
            switch(bombAttackChildState)
            {
                case BombAttackChildState.HatchOpen:
                    boss.PlayAnimation("HatchOpen",0f);
                    break;
                case BombAttackChildState.DropBombs:
                    break;
                case BombAttackChildState.HatchClose:
                    boss.PlayAnimation("HatchClose",0f);
                    break;
            }
        }
    }
    
    public override void Enter()
    {
        currentStateTimer = 0;
        spwanBombTimer = 0;
        boss.canBomb = false;
        bombattackchildState = BombAttackChildState.HatchOpen;
    }
    
    public override void Update()
    {
        switch(bombattackchildState)
        {
            case BombAttackChildState.HatchOpen:
                //检测动画播放完毕没有，如果有就进入下一个子状态
                if (CheckAnimatorStateName("HatchOpen", out float time))
                {
                    if (time >= 0.9f)
                    {
                        bombattackchildState = BombAttackChildState.DropBombs;
                        return;
                    }
                }
                break;
            case BombAttackChildState.DropBombs:
                DropBombsOnUpdate();
                //执行具体的翻转逻辑
                break;
            case BombAttackChildState.HatchClose:
                //检测动画播放完毕没有，如果有就进入下一个子状态
                if (Mathf.Abs(boss.heightPoint.transform.position.x - boss.transform.position.x) <= 0.1f)
                {
                    boss.transform.position = boss.heightPoint.transform.position;
                    boss.ChangeState(BossStates.Idle);
                }
                break;
        }
    }

    public override void FixedUpdate()
    {
        switch(bombattackchildState)
        {
            case BombAttackChildState.HatchOpen:
                break;
            case BombAttackChildState.DropBombs:
                //执行具体的移动逻辑
                boss.rb.linearVelocity = new Vector2(boss.bossSpeed * (boss.isFacingRight? 1:-1), 0);
                break;
            case BombAttackChildState.HatchClose:
                //移动回bossheight
                var dir = (boss.heightPoint.transform.position.x - boss.transform.position.x) < 0? Vector2.left : Vector2.right;
                boss.rb.linearVelocity = new Vector2(boss.bossSpeed * dir.x, 0);
                break;
        }
    }
    
    public override void Exit()
    {
        boss.rb.linearVelocity = Vector2.zero;
        boss.CoolBombSkill();
    }


    private void DropBombsOnUpdate()
    {
        currentStateTimer += Time.deltaTime;
        spwanBombTimer += Time.deltaTime;
        if (currentStateTimer >= 15f)
        {
            bombattackchildState = BombAttackChildState.HatchClose;
        }
        
        //每隔一段时间生成炸弹
        if (spwanBombTimer >= boss.bombSpwanDuration)
        {
            boss.SpwanBombs();
            spwanBombTimer = 0;
        }
        
        
        if (boss.IsWallDetected())
        {
            boss.Flip();
        }
    }
}
