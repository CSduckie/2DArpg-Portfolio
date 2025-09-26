using UnityEngine;
using System.Collections;
public class EnemyController : MonoBehaviour,IStateMachineOwner
{
    public GameObject enemyUI;
    public EnemySprite sprite;
    public Rigidbody2D rb;
    protected StateMachine stateMachine;
    
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    
    [Header("转身")]
    public bool isFacingRight = true;
    
    [Header("状态标记")]
    public bool isHurt;
    public bool isDead;
    public bool isStun;
    
    [Header("硬直时间")] 
    public float stunTime;
    
    protected virtual void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.Init(this);
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<EnemySprite>();
    }
    
    public void PlayAnimation(string animationName,float fixedTransitionDuration = 0.25f)
    {
        sprite.anim.CrossFadeInFixedTime(animationName,fixedTransitionDuration);
    }
    
    //转身函数
    public void Flip()
    {
        if(isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0,-180,0);
            isFacingRight = !isFacingRight;
            //FLIP local ui
            enemyUI.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            isFacingRight = !isFacingRight;
            //FLIP local ui
            enemyUI.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
    
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector2.down, groundCheckDistance, whatIsGround);
    
    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * (isFacingRight?1:-1), wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion
    
        
    /// <summary>
    /// 这部分属于硬直计数器
    /// </summary>
    public virtual void Stun()
    {
        StartCoroutine( Stunning());
    }

    private IEnumerator Stunning()
    {
        yield return new WaitForSeconds(stunTime); 
        isStun = false;
    }
}
