using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class DroneController : EnemyController
{
    //无人机停止巡逻时在原地静止的时间
    public float restTime;
    //无人机没有锁定玩家的丢失目标时间
    public float lostPlayerTime = 3f;
    //无人机初始位置记录
    private Vector3 spwanPoint;

    //无人机的攻击距离测算
    public float attackDistance;
    
    //基于飞行敌人的寻路AI组件
    [HideInInspector]
    public NavMeshAgent agent;
    
    [Header("移动速度")] 
    public float moveSpeed;
    public float chaseSpeed;
    
    [Header("玩家检测")]
    public Vector2 centerOffest;
    public float checkDistance;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsWall;
    public float patrolDistance;
    //无人机是否可以穿过墙壁检测到玩家
    public bool canDetectBehindWall;
    
    [Header("属于无人机的攻击冷却")] 
    public bool canAttack;
    [SerializeField] private float attackCoolDown;
    public GameObject droneWeapon;
    
    private void Awake()
    {
        spwanPoint = transform.position;
    }

    protected override void Start()
    {
        base.Start(); 
        ChangeState(DroneState.Idle);
        
        //初始化设置敌人寻路组件
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = chaseSpeed;
        //初始设置寻路为关闭
        agent.enabled = false;
        
    }
            
     //状态机
    public void ChangeState(DroneState DroneState)
    { 
        switch(DroneState) 
        { 
            case DroneState.Idle: 
                stateMachine.ChangeState<DroneIdleState>();
                break; 
            case DroneState.Patrol:
                stateMachine.ChangeState<DronePatrolState>();
                break;
            case DroneState.Chase:
                stateMachine.ChangeState<DroneChaseState>();
                break;
            case DroneState.Battle:
                stateMachine.ChangeState<DroneBattleState>();
                break;
            case DroneState.Attack:
                stateMachine.ChangeState<DroneAttackState>();
                break;
            case DroneState.Hurt:
                stateMachine.ChangeState<DroneHurtState>();
                break;
            case DroneState.Die:
                stateMachine.ChangeState<DroneDieState>();
                break;
            case DroneState.Stun:
                stateMachine.ChangeState<DroneStunState>();
                break;
        }
    }

        
    //Check for player
    public bool isPlayerFound()
    {
        Collider2D playerHit = Physics2D.OverlapCircle((Vector2)transform.position + centerOffest, checkDistance, whatIsPlayer);
        if (playerHit != null)
        {
            if (canDetectBehindWall)
            {
                return true; 
            }
            else
            {
                //检查是否被墙挡住
                Vector2 playerPos = playerHit.transform.position;
                RaycastHit2D wallHit = Physics2D.Linecast(transform.position + (Vector3)centerOffest, playerPos, whatIsWall);
        
                // 如果没有撞到墙  视线通畅
                if (wallHit.collider == null)
                {
                    return true; 
                }
            }
        }
        return false;
    }
    
    #region 检测线绘制
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckDistance);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffest ,checkDistance);
        Gizmos.color = Color.red;
        //这个是用于游戏环境内的实时检测
        Gizmos.DrawWireSphere(spwanPoint ,patrolDistance);
        //这个是交给策划判断环境布置是否合理
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position ,patrolDistance);
    }
    #endregion
    
    
    #region 无人机独有的点位生成代码和地面墙体检测
    public override bool IsGroundDetected() => Physics2D.OverlapCircle(transform.position,
        groundCheckDistance, whatIsGround);
    public Vector3 GetNewPoint()
    {
        var randomX = Random.Range(-patrolDistance, patrolDistance);
        var randomY = Random.Range(-patrolDistance, patrolDistance);
        
        return new Vector3(randomX, randomY) + spwanPoint;
    }
    
    //需要优化成一个开销比较小的方式
    public Vector3 GetPlayerPos()
    {
        return GameManager.instance.player.transform.position;
    }
    
    #endregion

    
    #region 无人机受伤死亡相关
    public void GetHurt()
    {
        if(isDead)
            return;
        ChangeState(DroneState.Hurt);
        isHurt = true;
        Debug.Log("Hurt");
    }
    
    public void Die()
    {
        ChangeState(DroneState.Die);
        isDead = true;
        enemyUI.SetActive(false);
    }
    #endregion

    
    #region 攻击冷却携程
    public void AttackCooling()
    {
        StartCoroutine( CoolingAttack());
    }

    private IEnumerator CoolingAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDown); 
        canAttack = true;
    }
    #endregion
    
    public void GetStun()
    {
        ChangeState(DroneState.Stun);
        return;
    }
}
