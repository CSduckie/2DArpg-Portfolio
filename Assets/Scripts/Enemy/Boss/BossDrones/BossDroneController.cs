using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using VHierarchy.Libs;

public class BossDroneController : EnemyController
{
    [Header("攻击参数")]
    //无人机的攻击半径
    [SerializeField] private float attackRange;
    public GameObject bossDroneWeapon;
    public BossController boss;
    
    //攻击状态标记
    public bool hasAttacked = false;
    public bool shouldAttack = false;
    
    //这个坐标是等待播放完毕去无人机挂载点的动画后，使用的攻击玩家时候处于的坐标
    public Vector2 targetPos { get; private set; }
    //实际要攻击的坐标
    public Vector2 posToAttack{ get; private set; }
    //生成的时候，飞向boss身边的准备坐标
    public Vector2 readyPos{ get; private set; }
    
    //传入了一个攻击玩家时的挂载点，先移动到boss身边挂载点，然后移动到攻击玩家的挂载点
    public void Init(Vector2 _posToGo, Vector2 _playerPos, float duration,BossController bossScript,Vector2 _hookingPos)
    {
        boss = bossScript;
        targetPos = _posToGo;
        posToAttack = _playerPos;
        readyPos = _hookingPos;
        if (_hookingPos.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            isFacingRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,-180,0);
            isFacingRight = false;
        }
    }


    protected override void Start()
    {
        base.Start();
        ChangeState(BossDroneState.Idle);
    }
    
    //状态机
    public void ChangeState(BossDroneState bossDroneState)
    {
        switch(bossDroneState)
        {
            case  BossDroneState.Idle:
                stateMachine.ChangeState<BossDroneIdleState>();
                break;
            case BossDroneState.MoveToReadyPos:
                stateMachine.ChangeState<BossDroneMoveToReadyState>();
                break;
            case BossDroneState.MoveToShootingPos:
                stateMachine.ChangeState<BossDroneMoveToShootingState>();
                break;
            case BossDroneState.Shooting:
                stateMachine.ChangeState<BossDroneShootingState>();
                break;
            case BossDroneState.GoBackToBossPos:
                stateMachine.ChangeState<BossDroneGoBackState>();
                break;
            case BossDroneState.Hurt:
                break;
            case BossDroneState.Die:
                break; 
        }
    }
    
    public void DoMoveTo(Vector3 _targetPos, float duration,string currentStateName)
    {
        transform.DOMove(_targetPos, duration).OnComplete((() => CompleteBehaviour(currentStateName)));
    }
    
    //设定完成动画序列时调用的函数
    private void CompleteBehaviour(string _currentStateName)
    {
        switch (_currentStateName)
        {
            case "MoveToReady":
                ChangeState(BossDroneState.MoveToShootingPos);
                break;
            case "MoveToShooting":
                ChangeState(BossDroneState.Shooting);
                break;
            case "Shooting":
                break;
            case "GoBackToBoss":
                boss.droneNum--;
                Destroy(this.gameObject);
                break;
        }
    }

    #region 攻击相关函数，包括动画状态机调用，动画事件

    public void StartAttack()
    {
        StartCoroutine(attackCoroutine());
    }

    private IEnumerator attackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        PlayAnimation("Attack",0f);
    }

    public void EnableWeapon()
    {
        bossDroneWeapon.SetActive(true);
    }

    public void DisableWeapon()
    {
        bossDroneWeapon.SetActive(false);
    }

    #endregion
    
    /// <summary>
    /// 计算应设置的 Z 角（度），让本地 X 轴指向 targetPos。
    /// 如果 selfPos 与 targetPos 几乎重合，返回 fallbackAngle（默认用当前角度）。
    /// </summary>
    public float GetLookZAngle(Vector3 selfPos, Vector3 targetPos, float fallbackAngle)
    {
        Vector2 dir = (Vector2)(targetPos - selfPos);
        if (dir.sqrMagnitude < 1e-8f) return fallbackAngle; // 避免除零/NaN
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
