using System;
using UnityEngine;
using Sirenix.OdinInspector; 
using System.Collections;
public class LaserConsoleControl : MonoBehaviour
{
    private LaserControl laser;
    [SerializeField] private float laserClosedTime;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    
    [HideIf("isMoving")]
    [Header("激光正常间歇性开关参数,非移动型激光使用")]
    private float laserOpenTimeCounter;
    [HideIf("isMoving")]
    [SerializeField] private float laserOpenTime;
    [HideIf("isMoving")]
    private float laserShutdownTimeCounter;
    [HideIf("isMoving")]
    [SerializeField] private float laserShutDownTime;
    
    
    [LabelText("是否为可移动")]
    public bool isMoving;
    [ShowIf("isMoving")]
    public float moveSpeed;
    [ShowIf("isMoving")]
    [SerializeField] protected Transform wallCheck;
    [ShowIf("isMoving")]
    [SerializeField] protected float wallCheckDistance;
    [ShowIf("isMoving")]
    [SerializeField] protected LayerMask whatIsWall;


    public void Start()
    {
        laser = GetComponentInChildren<LaserControl>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isMoving)
            return;
        
        if (laser.gameObject.activeSelf) 
        {
            laserOpenTimeCounter+=Time.deltaTime;
            if (laserOpenTimeCounter >= laserOpenTime)
            {
                laser.gameObject.SetActive(false);
                laserOpenTimeCounter = 0;
            }
        }
        else
        {
            laserShutdownTimeCounter +=  Time.deltaTime;
            if (laserShutdownTimeCounter >= laserShutDownTime)
            { 
                laser.gameObject.SetActive(true); 
                laserShutdownTimeCounter = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = new Vector2(moveSpeed * (isFacingRight ? 1:-1),0);
            if (IsWallDetected())
            {
                Flip();
                Debug.Log("Wall");
            }
        }
    }

    public void DisableLaserForSec()
    {
        StartCoroutine(DisableLaser());
    }

    private IEnumerator DisableLaser()
    {
        laser.gameObject.SetActive(false);
        yield return new WaitForSeconds(laserClosedTime);
        laser.gameObject.SetActive(true);
        laser.hitPlayer = false;
    }
    
    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * (isFacingRight?1:-1), wallCheckDistance, whatIsWall);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    
    //转身函数
    public void Flip()
    {
        if(isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0,-180,0);
            isFacingRight = !isFacingRight;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            isFacingRight = !isFacingRight;
        }
    }
}
