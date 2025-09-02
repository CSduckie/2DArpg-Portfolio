using UnityEngine;

public class BossController : EnemyController
{
    [Header("Boss飞行参数")] 
    public Transform heightPoint;

    public float bossSpeed;
    public float bossClimbUpSpeed;
    protected override void Start()
    {
        base.Start();
        //TODO:添加过场后，删除这个代码
        ChangeState(BossStates.Idle);
    }
    
    //状态机
    public void ChangeState(BossStates bossStates)
    {
        switch(bossStates)
        {
            case BossStates.Idle:
                stateMachine.ChangeState<BossIdleState>();
                break;
            case BossStates.DroneAttack:
                stateMachine.ChangeState<BossDroneAttackState>();
                break;
            case BossStates.SmashAttack:
                stateMachine.ChangeState<BossSmashAttackState>();
                break;
        }
    }
}
