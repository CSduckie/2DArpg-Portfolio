using System;
using UnityEngine;
using System.Collections;
public class BoarController : EnemyController
{
    [Header("移动速度")] 
    public float moveSpeed;
    public float chaseSpeed;
    
    [Header("巡逻/追击参数")] 
    //巡逻的休息时间
    public float restTime;
    //追击最大丢失玩家时间
    public float maxLostTime;
    
    [Header("玩家检测")]
    public Vector2 centerOffest;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsWall;


    [Header("攻击盒")] 
    public GameObject attackBox;

    
    protected override void Start()
    {
        base.Start();
        ChangeState(BoarState.Idle);
    }
    
    //状态机
    public void ChangeState(BoarState boarState)
    {
        switch(boarState)
        {
            case BoarState.Idle:
                stateMachine.ChangeState<BoarIdleState>();
                break;
            case BoarState.Patrol:
                stateMachine.ChangeState<BoarPatrolState>();
                break;
            case BoarState.Chase:
                stateMachine.ChangeState<BoarChaseState>();
                break;
            case BoarState.Hurt:
                stateMachine.ChangeState<BoarHurtState>();
                break;
            case BoarState.Die:
                stateMachine.ChangeState<BoarDieState>();
                break;
            case BoarState.Stun:
                stateMachine.ChangeState<BoarStunState>();
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

    #region 受伤，死亡，被玩家完美弹反导致的硬直
    public void GetHurt()
    {
        if(isDead)
            return;
        ChangeState(BoarState.Hurt);
        isHurt = true;
    }
    
    public void Die()
    {
        ChangeState(BoarState.Die);
        isDead = true;
        enemyUI.SetActive(false);
    }

    public void GetStun()
    {
        ChangeState(BoarState.Stun);
        return;
    }
    
    #endregion
    
}
