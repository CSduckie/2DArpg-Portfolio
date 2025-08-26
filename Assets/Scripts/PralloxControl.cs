using UnityEngine;

public class PralloxControl : MonoBehaviour
{
    public Transform backGroundTrans,foreGroundTrans;
    public float BGparalloxSpeed,FGparalloxSpeed;
    public Vector2 lastPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y);

        backGroundTrans.position = new Vector2(backGroundTrans.position.x +amountToMove.x * BGparalloxSpeed,backGroundTrans.position.y);
        foreGroundTrans.position = new Vector2(foreGroundTrans.position.x + amountToMove.x * FGparalloxSpeed, foreGroundTrans.position.y);
        lastPos = transform.position;
    }
}
