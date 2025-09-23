
using System.Collections;
using UnityEngine;

public class FlyingCarController : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private int moveDir = -1;
    [SerializeField] private float target_xPos = -34;
    private Vector2 startPos;
    
    private bool canMove = true;
    private TrailRenderer trailRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        startPos =  transform.position;
        moveSpeed = UnityEngine.Random.Range(1f, 5f);
    }

    private void Update()
    {
        if (transform.position.x < target_xPos)
        {
            trailRenderer.enabled = false;
            transform.position = startPos;
            canMove = false;
            StartCoroutine(ActiveTrail());
        }
    }

    private IEnumerator ActiveTrail()
    {
        yield return new WaitForSeconds(1f);
        trailRenderer.enabled = true;
        canMove = true;
        moveSpeed = UnityEngine.Random.Range(1f, 5f);
    }
    
    private void FixedUpdate()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(moveSpeed * moveDir, rb.linearVelocityY);
        else
            rb.linearVelocity = Vector2.zero;
    }
}
