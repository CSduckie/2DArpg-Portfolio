using UnityEngine;

public class PralloxControl : MonoBehaviour
{
    public Transform bg1,bg2,bg3,bg4,bg5;
    public float speedFor1,speedFor2,speedFor3,speedFor4,speedFor5;
    public Vector2 lastPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y );

        if (bg1 != null)
        {
            bg1.position = new Vector2(bg1.position.x +amountToMove.x * speedFor1,bg1.position.y ); 
        }
        if (bg2 != null)
        {
            bg2.position = new Vector2(bg2.position.x +amountToMove.x * speedFor2,bg2.position.y ); 
        }
        if (bg3 != null)
        {
            bg3.position = new Vector2(bg3.position.x +amountToMove.x * speedFor3,bg3.position.y); 
        }
        if (bg4 != null)
        {
            bg4.position = new Vector2(bg4.position.x +amountToMove.x * speedFor4,bg4.position.y ); 
        }
        if (bg5 != null)
        {
            bg5.position = new Vector2(bg5.position.x +amountToMove.x * speedFor5,bg5.position.y ); 
        }
        lastPos = transform.position;
    }
}
