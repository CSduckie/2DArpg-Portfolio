using UnityEngine;
using DG.Tweening;
public class BossForceKillSkillState : BossStateBase
{
    private float spwanVFXTimer;
    private float currentChargingTimeCounter;
    private float enemySpwanTimer;
    private Tween current;
    private enum ForceKillChildState
    {
        Stage1,
        Stage2,
        Stage3,
    }
    
    private ForceKillChildState forceKillChildState;
    private ForceKillChildState forcekillchildState
    {
        get =>forceKillChildState;
        set{
            forceKillChildState = value;
            switch(forceKillChildState)
            {
                case ForceKillChildState.Stage1:
                    boss.PlayAnimation("ForceKillCharge",0f);
                    current = boss.transform.DOMove(boss.bossForceKillChargingTrans.position, 2f);
                    break;
                case ForceKillChildState.Stage2:
                    
                    break;
                case ForceKillChildState.Stage3:
                    
                    break;
            }
        }
    }
    
    
    public override void Enter()
    {
        currentChargingTimeCounter = 0;
        enemySpwanTimer = 0;
        spwanVFXTimer = 0;
        forcekillchildState = ForceKillChildState.Stage1;
    }

    
    public override void Update()
    {
        switch(forcekillchildState)
        {
            case ForceKillChildState.Stage1:
                Stage1OnUpdate();
                break;
            case ForceKillChildState.Stage2:
                Stage2OnUpdate();
                break;
            case ForceKillChildState.Stage3:
                Stage3OnUpdate();
                break;
        }
        enemySpwanTimer += Time.deltaTime;
        if (enemySpwanTimer >= boss.enemySpwanDuration)
        {
            boss.SpwanEnemies();
            enemySpwanTimer = 0;
        }
        
        //CheckForStun
        if (boss.willStun)
        {
            boss.ChangeState(BossStates.Stun);
            return;
        }
    }

    #region 状态机内置Update

    private void Stage1OnUpdate()
    {
        //阶段一充能
        currentChargingTimeCounter += Time.deltaTime;
        if (currentChargingTimeCounter >= 15f)
        {
            forcekillchildState = ForceKillChildState.Stage2;
            currentChargingTimeCounter = 0;
        }
    }

    private void Stage2OnUpdate()
    {
        //生成VFX
        spwanVFXTimer += Time.deltaTime;
        //阶段2充能
        currentChargingTimeCounter += Time.deltaTime;
        
        if (spwanVFXTimer >= 2f)
        {
            spwanVFXTimer = 0;
            boss.enableRandomSparks();
        }
        
        if (currentChargingTimeCounter >= 15f)
        {
            forcekillchildState = ForceKillChildState.Stage3;
            currentChargingTimeCounter = 0;
            spwanVFXTimer = 0;
        }
    }

    private void Stage3OnUpdate()
    {
        //生成VFX
        spwanVFXTimer += Time.deltaTime;
        //阶段3充能
        currentChargingTimeCounter += Time.deltaTime;
        
        if (spwanVFXTimer >= 0.5f)
        {
            spwanVFXTimer = 0;
            boss.enableRandomSparks();
        }
        
        if (currentChargingTimeCounter >= 10f)
        {
            GameManager.instance.player.ChangeState(PlayerState.Die);
            boss.ChangeState(BossStates.Idle);
            currentChargingTimeCounter = 0;
            spwanVFXTimer = 0;
        }
    }

    #endregion
    
    public override void Exit()
    {
        base.Exit();
    }
}
