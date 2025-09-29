using System.Collections;
using UnityEngine;

public class HammerGuyController : EnemyController
{
    [Header("状态计时器")] 
    public float idleStateTime;
    
    
    [Header("玩家检测")]
    public Vector2 centerOffest;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsWall;


    [Header("速度配置")] 
    public float chaseSpeed;
    public float walkSpeed;

    [Header("攻击参数")] 
    public float attackRange = 5;
    public float attackDuration = 3;
    public bool canAttack;
    public GameObject hammerGuyWeapon;
    
    protected override void Start()
    {
        base.Start();
        ChangeState(HammerGuyStates.Idle);
    }
    
    //状态机
    public void ChangeState(HammerGuyStates hammerGuyStates)
    {
        switch(hammerGuyStates)
        {
            case HammerGuyStates.Idle:
                stateMachine.ChangeState<HammerGuyIdleState>();
                break;
            case HammerGuyStates.Chase:
                stateMachine.ChangeState<HammerGuyChaseState>();
                break;
            case HammerGuyStates.Hurt:
                stateMachine.ChangeState<HammerGuyHurtState>();
                break;
            case HammerGuyStates.Die:
                stateMachine.ChangeState<HammerGuyDieState>();
                break;
            case HammerGuyStates.Attack:
                stateMachine.ChangeState<HammerGuyAttackState>();
                break;
            case HammerGuyStates.Patrol:
                stateMachine.ChangeState<HammerGuyPatrolState>();
                break;
            case HammerGuyStates.Battle:
                stateMachine.ChangeState<HammerGuyBattleState>();
                break;
            case HammerGuyStates.Stun:
                stateMachine.ChangeState<HammerGuyStunState>();
                break;
        }
    }
    
    
    //Check for player
    public bool isPlayerFound()
    {
        Vector2 faceDir = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D playerHit = Physics2D.BoxCast(transform.position + (Vector3)centerOffest, checkSize, 0,faceDir , checkDistance, whatIsPlayer);
        if (playerHit.collider != null)
        {   
            //检查是否被墙挡住
            Vector2 playerPos = playerHit.collider.transform.position;
            RaycastHit2D wallHit = Physics2D.Linecast(transform.position + (Vector3)centerOffest, playerPos, whatIsWall);

            // 如果没有撞到墙  视线通畅
            if (wallHit.collider == null)
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffest + new Vector3(checkDistance * (isFacingRight?1:-1),0),.2f);
    }

    #region 攻击冷却携程

    public void CoolAttack()
    {
        StartCoroutine(attackCoolCoroutine());
    }

    private IEnumerator attackCoolCoroutine()
    {
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    #endregion


    #region MyRegion

    public void GetHurt()
    {
        if(isDead)
            return;
        ChangeState(HammerGuyStates.Hurt);
    }

    public void Die()
    {
        ChangeState(HammerGuyStates.Die);
        enemyUI.SetActive(false);
    }

    public void GetStun()
    {
        ChangeState(HammerGuyStates.Stun);
        return;
    }

    public void RecoverStun()
    {
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(stunTime);
        isStun = false;
    }

    #endregion
}
