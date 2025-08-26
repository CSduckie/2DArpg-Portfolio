using System;
using System.Collections;
using UnityEngine;

public class TurretLaserController : MonoBehaviour
{
    public LineRenderer laserBeam;
    private float laserLength;
    private TurretController turretController;
    private bool isLaserActive;
    
    //用于保存镭射起点位置
    private Vector2 laserStartPos;
    //用于存贮镭射方向
    private Vector2 direction;
    
    [Header("激光抖动参数")]
    //实现激光的抖动效果
    public float amplitude = 0.1f;         // 抖动幅度
    public float frequency = 50f;           // 抖动频率
    public bool usePerlinNoise = false;    // 可选，用更自然的抖动
    
    

    private void Start()
    {
        turretController = GetComponentInParent<TurretController>();
        laserBeam = GetComponent<LineRenderer>();
        
        laserStartPos = gameObject.transform.position;

        direction = Vector2.right * turretController.transform.localScale.x;
        
        // 设置激光初始形状
        laserBeam.positionCount = 2;
        laserBeam.SetPosition(0, laserStartPos);
        laserBeam.SetPosition(1, new Vector3(laserStartPos.x + turretController.checkDistance, laserStartPos.y, 0));
    }

    private void Update()
    {
        UpdateLaserEndPoint();
    }

    //用于实时更新激光命中点，让玩家可以跟激光产生互动
    private void UpdateLaserEndPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserStartPos,direction,turretController.checkDistance,turretController.whatIsPlayer);
        
        float length = turretController.checkDistance;

        if(hit)
        {
            length = Mathf.Abs(hit.point.x - laserStartPos.x);
        }
        
        Vector2 endPosition = laserStartPos + length * direction;
        
        //设置激光的抖动
        float yOffset = usePerlinNoise
            ? Mathf.PerlinNoise(Time.time * frequency, 0f) * amplitude * 2f - amplitude
            : Mathf.Sin(Time.time * frequency) * amplitude;

        endPosition.y += yOffset;
        
        laserBeam.SetPosition(0,laserStartPos);
        laserBeam.SetPosition(1,endPosition);
    }

    
    //TODO：目前没有相应用法
    public void DeactiveLaser()
    {
        //StartCoroutine(DisableLaser());
    }

    private IEnumerator DisableLaser()
    {
        // 闪一下（隐藏0.1秒再显示）
        laserBeam.enabled = false;
        yield return new WaitForSeconds(0.1f);
        laserBeam.enabled = true;
        yield return new WaitForSeconds(0.1f);

        // 然后彻底关闭激光
        laserBeam.enabled = false;
    }

}
