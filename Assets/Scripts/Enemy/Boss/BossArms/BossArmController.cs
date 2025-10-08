using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BossArmController : EnemyController
{
    //无人机的生成挂载点
    public Vector2 hookPos { get; private set; }
    private Vector2 spwanPos;
    private bool currentFacing;
    public BossController boss;
    [Header("移动速度")]
    public float moveSpeed;
    
    public GameObject armHitBox;
    
    public void Init(Vector2 _hookPos,BossController _boss)
    {
        spwanPos = transform.position;
        hookPos = _hookPos;
        if(hookPos.x < 0)
            currentFacing = true;
        else 
            currentFacing = false;
        boss = _boss;
        this.transform.SetParent(boss.transform);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        ChangeState(BossArmState.Idle);
        MoveToHookPos();
    }

    public void MoveToHookPos()=>transform.DOLocalMove(hookPos, 1f);

    
    //状态机
    public void ChangeState(BossArmState bossArmState)
    {
        switch(bossArmState)
        {
            case BossArmState.Idle:
                stateMachine.ChangeState<BossArmIdleState>();
                break;
            case BossArmState.Attack:
                stateMachine.ChangeState<BossArmATKState>();
                break;
            case BossArmState.Leave:
                stateMachine.ChangeState<BossArmLeaveState>();
                break;
            case BossArmState.Hide:
                stateMachine.ChangeState<BossArmHideState>();
                break;
            case BossArmState.Hurt:
                break;
        }
    }

    public void DestroySelfOn(float time)
    {
        StartCoroutine(DestroyCorou(time));
    }

    private IEnumerator DestroyCorou(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(BossArmState.Hide);
        this.gameObject.SetActive(false);
        this.transform.position = spwanPos;
        isFacingRight = currentFacing;
    }
    
}
