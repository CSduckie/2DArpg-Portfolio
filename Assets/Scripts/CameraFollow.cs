using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    private PlayerController player;
    private bool isFacingRight;
    
    [Header("旋转跟随延迟参数")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;

    private void Awake()
    {
        player = playerTrans.gameObject.GetComponent<PlayerController>();

        isFacingRight = player.isFacingRight;
    }


    private void Update()
    {
        transform.position = playerTrans.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }
    
    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotationAmount = DetermineEndRoation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            
            //Lerp Y 
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRoation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
