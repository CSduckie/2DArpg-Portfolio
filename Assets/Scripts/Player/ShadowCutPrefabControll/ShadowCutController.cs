using UnityEngine;

public class ShadowCutController : MonoBehaviour
{
    private float speed;
    private float liveTime;
    private float liveTimeCounter;
    private Vector2 startPos;
    private Rigidbody2D rb;
    private bool isMoving;
    private float moveRange;
    
    public void Init(float _moveSpeed,float _liveTime,int _dir , float _moveRange)
    {
        speed =  _moveSpeed;
        liveTime = _liveTime;
        liveTimeCounter = 0;
        isMoving = true;
        moveRange = _moveRange;
        startPos = transform.position;
        this.transform.localScale = new Vector3(_dir, 1, 1);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        liveTimeCounter += Time.deltaTime;
        if (liveTimeCounter >= liveTime)
        {
            Destroy();
        }
        
        //TODO:添加一个逻辑，碰墙了就停下来
        
        //位置修正
        if (transform.position.x - startPos.x >= moveRange && isMoving)
        {
            transform.position = new Vector2(startPos.x + moveRange, transform.position.y);
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if(isMoving)
            rb.linearVelocity = new Vector2(speed * this.transform.localScale.x, 0f);
        else
            rb.linearVelocity = Vector2.zero;
    }
    
    private void Destroy()
    {
        Destroy(this.gameObject);
    }
    
}

