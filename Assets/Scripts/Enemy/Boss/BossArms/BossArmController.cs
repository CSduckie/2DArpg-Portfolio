using DG.Tweening;
using UnityEngine;

public class BossArmController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!IsGroundDetected()) 
            rb.linearVelocity = new Vector2(0, moveSpeed);
        else
            rb.linearVelocity = Vector2.zero;
    }
    
    private bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector2.down, groundCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}
