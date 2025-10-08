using UnityEngine;

public class BossBombController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float dropSpeed;
    private bool exploded = false;
    
    [SerializeField] private SkillConfig skillConfig;
    private float weaponDamage;
    public float stunValue { get; private set; }
    private Vector2 repelDir;
    
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
    }
    
    private void FixedUpdate()
    {
        if(!IsGroundDetected())
            rb.linearVelocity = new Vector2(0, -dropSpeed);
        else
        {
            rb.linearVelocity = Vector2.zero;
            if (!exploded)
            {
                exploded = true;
                anim.SetTrigger("Explode");
                //TODO:启动攻击盒子
            }
        } 
    }

    public void ActiveAttackBox()
    {
        
    }
    
    //自身毁灭的公开函数
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    
    private bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector2.down, groundCheckDistance, whatIsGround);
    

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.isDead)
                return;

            CharacterStats attackerStats = GetComponentInParent<CharacterStats>();

            int attackDir = transform.position.x - player.transform.position.x > 0 ? -1 : 1;
            
            //TODO:后续需要添加damage的修改值
            //此处传入攻击者自身的伤害数值脚本，用于反伤等计算
            other.GetComponent<CharacterStats>().TakeDamage(weaponDamage,stunValue,attackDir,repelDir,attackerStats);
        }
    }
}
