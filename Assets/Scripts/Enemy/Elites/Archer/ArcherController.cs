using System.Collections;
using UnityEngine;

public class ArcherController : EnemyController
{
    [Header("计时器")]
    [SerializeField] private float specialAttackCD;
    [SerializeField] private float teleportCD;
    [SerializeField] private float normalAttackCD;
    
    [Header("移动速度")] 
    public float chaseSpeed;
    
    [Header("Arena")] 
    [SerializeField] private BoxCollider2D arena;

    
    [Header("特殊攻击")] 
    public float attackDistance;
    public bool canSpecialAttack{ get; private set; }
    
    
    [Header("传送")]
    public bool canTeleport { get; private set; }
    
    [Header("普通攻击")] 
    public bool canNormalAttack{ get; private set; }
    
    
    [Header("技能使用概率")] 
    [Range(0, 1)] public float useNormalATKChance; 
    [Range(0, 1)] public float useSpecialATKChance;
    [Range(0, 1)] public float useTeleportChance;

    
    [Header("攻击盒")] 
    public GameObject[] archerWeapons;
    
    public Transform player;
    
    [Header("状态")]
    public bool isTeleporting = false;
    public bool isNormalAttack = false;
    public bool isSpecialAttack = false;
    
    
    protected override void Start()
    {
        base.Start();
        //开始时冷却一边所有技能
        CoolSpecialAttack();
        CoolTeleport();
        CoolNormalAttack();
        ChangeState(ArcherStates.Appear);
    }
    
    //状态机
    public void ChangeState(ArcherStates archerStates)
    {
        switch(archerStates)
        {
            case ArcherStates.Idle:
                stateMachine.ChangeState<ArcherIdleState>();
                break;
            case ArcherStates.Teleport:
                stateMachine.ChangeState<ArcherTeleportState>();
                break;
            case ArcherStates.SpecialATK:
                stateMachine.ChangeState<ArcherSpecialATKState>();
                break;
            case ArcherStates.NormalATK:
                stateMachine.ChangeState<ArcherNormalATKState>();
                break;
            case ArcherStates.Hurt:
                stateMachine.ChangeState<ArcherHurtState>();
                break;
            case ArcherStates.Dead:
                stateMachine.ChangeState<ArcherDieState>();
                break;
            case ArcherStates.Chase:
                stateMachine.ChangeState<ArcherChaseState>();
                break;
            case ArcherStates.Stun:
                stateMachine.ChangeState<ArcherStunState>();
                break;
            case ArcherStates.Appear:
                stateMachine.ChangeState<ArcherAppearState>();
                break;
        }
    }

    #region 技能冷却携程

    public void CoolSpecialAttack()
    {
        StartCoroutine(SpecialAttackCooling());
    }
    
    private IEnumerator SpecialAttackCooling()
    {
        canSpecialAttack = false;
        yield return new WaitForSeconds(specialAttackCD);
        canSpecialAttack = true;
    }

    public void CoolNormalAttack()
    {
        StartCoroutine(NormalAttackCooling());
    }

    private IEnumerator NormalAttackCooling()
    {
        canNormalAttack = false;
        yield return new WaitForSeconds(normalAttackCD);
        canNormalAttack = true;
    }
    
    public void CoolTeleport()
    {
        StartCoroutine(TeleportCooling());
    }
    
    private IEnumerator TeleportCooling()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCD);
        canTeleport = true;
    }
    
    #endregion
    
    public Vector2 FindPosition()
    {
        //随机选择左或右
        int dirX = Random.value < 0.5f ? -1 : 1; // -1 = left, +1 = right
        
        //计算生成位置是否在竞技场外
        float x = player.position.x + dirX * attackDistance;
        if (x > arena.bounds.max.x)
        {
            return new Vector2(player.position.x - dirX * attackDistance, GroundBelow().point.y + GetComponentInParent<CapsuleCollider2D>().size.y/2);
        }

        if (x < arena.bounds.min.x)
        {
            return new Vector2(player.position.x - dirX * attackDistance, GroundBelow().point.y + GetComponentInParent<CapsuleCollider2D>().size.y/2);
        }
        
        return new Vector2(player.position.x + dirX * chaseSpeed, GroundBelow().point.y + GetComponentInParent<CapsuleCollider2D>().size.y/2);
    } 
    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);


    #region 状态切换
    public void GetHurt()
    {
        if(isDead || isTeleporting)
            return;
        ChangeState(ArcherStates.Hurt);
        Debug.Log("Hurt");
    }

    public void Die()
    {
        ChangeState(ArcherStates.Dead);
        enemyUI.SetActive(false);
    }

    public void GetStun()
    {
        ChangeState(ArcherStates.Stun);

    }

    public void StartEliteFight()
    {
        //ChangeState(ArcherStates.Appear);
    }
    #endregion
    
}
