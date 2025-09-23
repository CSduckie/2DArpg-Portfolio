using UnityEngine;
using System.Collections;

public class ShielderController : EnemyController
{
    public bool isCloseAttack;
    public bool isMultiAttack;
    public bool isBullAttack;
    public bool isRangeAttack;
    public bool isSleeping;
    
    [Header("移动参数")] public float chaseSpeed;

    [Header("攻击距离参数")] public float attackRange = 3;
    public float rangeAttackRange = 6;
    public Transform playerTrans;

    [Header("近距离攻击")] public bool canCloseATK { get; private set; }

    [Header("远距离攻击")] public bool canRangeATK { get; private set; }

    [Header("多重近距离攻击")] public bool canMultiCloseATK { get; private set; }

    [Header("蛮牛冲撞")] public bool canBullDash { get; private set; }
    public float bullDashSpeed;

    [Header("持续状态计时器")] public float bullDashStateTime;
    public float hurtStateTime;


    [Header("技能计时器")] [SerializeField] private float closeATKCD;
    [SerializeField] private float rangeATKCD;
    [SerializeField] private float multiATKCD;
    [SerializeField] private float bullDashCD;

    [Header("技能使用概率")] [Range(0, 1)] public float useCloseATKChance;
    [Range(0, 1)] public float useRangeATKChance;
    [Range(0, 1)] public float useMultiCloseATKChance;
    [Range(0, 1)] public float useBullDashChance;

    [Header("攻击盒子")] public GameObject[] shielderWeapons;

    protected override void Start()
    {
        base.Start();
        ChangeState(ShielderState.Sleep);
        CoolClosesAttack();
        CoolMultiAttack();
        CoolRangeAttack();
        CoolBullDash();
        enemyUI.SetActive(false);
    }

    //状态机
    public void ChangeState(ShielderState shielderState)
    {
        switch (shielderState)
        {
            case ShielderState.Sleep:
                stateMachine.ChangeState<ShielderSleepState>();
                break;
            case ShielderState.Idle:
                stateMachine.ChangeState<ShielderIdleState>();
                break;
            case ShielderState.Walk:
                stateMachine.ChangeState<ShielderWalkState>();
                break;
            case ShielderState.Hurt:
                stateMachine.ChangeState<ShielderHurtState>();
                break;
            case ShielderState.Dead:
                stateMachine.ChangeState<ShielderDeadState>();
                break;
            case ShielderState.CloseAttack:
                stateMachine.ChangeState<ShielderCloseAttackState>();
                break;
            case ShielderState.RangeAttack:
                stateMachine.ChangeState<ShielderRangeAttackState>();
                break;
            case ShielderState.MultiCloseAttack:
                stateMachine.ChangeState<ShielderMultiAttackState>();
                break;
            case ShielderState.BullDash:
                stateMachine.ChangeState<ShielderBullDashState>();
                break;
            case ShielderState.Stun:
                stateMachine.ChangeState<ShielderStunState>();
                break;
        }
    }

    #region 盾牌手的攻击冷却携程

    public void CoolClosesAttack()
    {
        StartCoroutine(CloseAttackCooling());
    }

    private IEnumerator CloseAttackCooling()
    {
        canCloseATK = false;
        yield return new WaitForSeconds(closeATKCD);
        canCloseATK = true;
    }

    public void CoolRangeAttack()
    {
        StartCoroutine(RangeAttackCooling());
    }

    private IEnumerator RangeAttackCooling()
    {
        canRangeATK = false;
        yield return new WaitForSeconds(rangeATKCD);
        canRangeATK = true;
    }

    public void CoolMultiAttack()
    {
        StartCoroutine(MultiAttackCooling());
    }

    private IEnumerator MultiAttackCooling()
    {
        canMultiCloseATK = false;
        yield return new WaitForSeconds(multiATKCD);
        canMultiCloseATK = true;
    }

    public void CoolBullDash()
    {
        StartCoroutine(BullDashCooling());
    }

    private IEnumerator BullDashCooling()
    {
        canBullDash = false;
        yield return new WaitForSeconds(bullDashCD);
        canBullDash = true;
    }

    #endregion


    #region 状态切换

    public void GetHurt()
    {
        if (isDead || isMultiAttack || isCloseAttack || isStun)
            return;
        ChangeState(ShielderState.Hurt);
        Debug.Log("Hurt");
    }
    
    //这个函数被用于防反
    public void GetStunning()
    {
        if (isMultiAttack)
            return;
        ChangeState(ShielderState.Hurt);
    }

    public void Die()
    {
        ChangeState(ShielderState.Dead);
        enemyUI.SetActive(false);
    }

    public void GetStun()
    {
        ChangeState(ShielderState.Stun);
    }

    public void StartShielderFight()
    {
        ChangeState(ShielderState.Idle);
    }
    
    #endregion
}
