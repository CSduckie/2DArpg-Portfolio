using UnityEngine;

public class BossIdleState : BossStateBase
{
    private float currentStateTimer;
    private bool hovering;
    public override void Enter()
    {
        Debug.Log("Boss idle state");
        boss.PlayAnimation("Idle",0f);
        hovering = false;
        currentStateTimer = 0f;
    }

    public override void Update()
    {
        
        
        //检查是否需要stun
        if (boss.willStun)
        {
            boss.willStun = false;
            boss.ChangeState(BossStates.Stun);
            return;
        }
        
        currentStateTimer += Time.deltaTime;
        if(currentStateTimer <= 1f || !hovering)
            return;
        
        
        //如果血量少于1/3，执行强杀技能
        if (boss.bossStats.GetCurrentHealth()/boss.bossStats.GetMaxHP() <= 0.33f && !boss.forceKillTriggered)
        {
            boss.forceKillTriggered = true;
            boss.ChangeState(BossStates.ForceKillSkill);
            return;
        }
        
        
        //计算随机数，然后使用技能
        if (boss.canSmash)
        {
            float skillChance = Random.value;
            if (skillChance <= boss.smashChance)
            {
                boss.ChangeState(BossStates.SmashAttack);
                return;
            }
        }

        if (boss.canCallDrone)
        {
            float skillChance = Random.value;
            if (skillChance <= boss.callDroneChance)
            {
                boss.ChangeState(BossStates.DroneAttack);
                return;
            }
        }

        if (boss.canUseClaw)
        {
            float skillChance = Random.value;
            if (skillChance <= boss.useClawChance)
            {
                boss.ChangeState(BossStates.ArmSmashAttack);
                return;
            }
        }
        
        //计算随机数，然后使用技能
        if (boss.canBomb)
        {
            float skillChance = Random.value;
            if (skillChance <= boss.useBombChance)
            {
                boss.ChangeState(BossStates.BombAttackSkill);
                return;
            }
        }

        if (boss.canLaser)
        {
            float skillChance = Random.value;
            if (skillChance <= boss.laserSkillCD)
            {
                boss.ChangeState(BossStates.LaserAOESkill);
                return;
            }
        }
        
        
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     boss.ChangeState(BossStates.DroneAttack);
        //     return;
        // }
        //
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     boss.ChangeState(BossStates.ArmSmashAttack);
        //     return;
        // }
        //
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     boss.ChangeState(BossStates.ForceKillSkill);
        //     return;
        // }
        //
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     boss.ChangeState(BossStates.BombAttackSkill);
        //     return;
        // }
    }

    public override void FixedUpdate()
    {
        Vector2 dir = (boss.heightPoint.position - boss.transform.position).normalized;
        if (Vector2.Distance(boss.heightPoint.position, boss.transform.position) > 0.1f)
        {
            boss.rb.linearVelocity = dir * boss.bossSpeed;
            Debug.Log("移动中");
        }
        else
        { 
            boss.transform.position = boss.heightPoint.position;
            boss.rb.linearVelocity = Vector2.zero;
            hovering = true;
        }
    }
    
    
    public override void Exit()
    {
        base.Exit();
    }

}
