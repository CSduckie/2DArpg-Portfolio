using System.Collections;
using UnityEngine;

public class AssassinController : EnemyController
{
    [Header("移动速度")] 
    public float chaseSpeed;

    [Header("获取玩家位置信息")] 
    public PlayerController player;

    [Header("攻击距离")] 
    public float attackDistance;
    public GameObject assassinWeapon;
    
    
    [Header("状态标记")] 
    public bool isAttack;
    public bool canAttack;


    [Header("攻击cd")] 
    [SerializeField] private float attackCD = 2f;
    
    protected override void Start()
    {
        base.Start();
        ChangeState(AssassinStates.Appear);
    }
    
    //状态机
    public void ChangeState(AssassinStates assassinStates)
    {
        switch(assassinStates)
        {
            case AssassinStates.Idle:
                stateMachine.ChangeState<AssassinIdleState>();
                break;
            case AssassinStates.Chase:
                stateMachine.ChangeState<AssassinChaseState>();
                break;
            case AssassinStates.Hurt:
                stateMachine.ChangeState<AssassinHurtState>();
                break;
            case AssassinStates.Die:
                stateMachine.ChangeState<AssassinDieState>();
                break;
            case AssassinStates.Appear:
                stateMachine.ChangeState<AssassinAppearState>();
                break;
            case AssassinStates.Attack:
                stateMachine.ChangeState<AssassinATKState>();
                break;
            case AssassinStates.Stun:
                stateMachine.ChangeState<AssassinStunState>();
                break;
        }
    }
    
    
    #region 状态机调用的函数

    public void CoolAttack()
    {
        StartCoroutine(CoolingAttack());
    }

    private IEnumerator CoolingAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    public void GetHurt()
    {
        if(isDead)
            return;
        ChangeState(AssassinStates.Hurt);
    }

    public void Die()
    {
        ChangeState(AssassinStates.Die);
        enemyUI.SetActive(false);
    }

    public void GetStun()
    {
        ChangeState(AssassinStates.Stun);
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
