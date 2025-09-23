using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    [Header("参数调整")]
    public Color color = new Color(195/255,36/255,0);
    public float colorInstensity; //HDR color, >1 bloom
    public float beamColorEnhance;
    public Transform trans;

    public float maxLength;
    public float thickness;
    public float noiseScale;
    public GameObject startVFX;
    public GameObject endVFX;

    public LineRenderer lineRenderer;
     [Header("激光发射方向")]
    public Vector2 laserDirection = Vector2.right;  // 在 Inspector 中调整激光方向

    public LayerMask hitable;
    
    
    [Header("攻击数据包")]
    [SerializeField] private SkillConfig skillConfig;
    private float weaponDamage;
    private float stunValue;
    private Vector2 repelDir;
    private bool hitPlayer;
    
    private void Awake()
    {
        trans = GetComponent<Transform>();
        lineRenderer = GetComponentInChildren<LineRenderer>();

        lineRenderer.material.color = color * colorInstensity;
        lineRenderer.material.SetFloat("_LaserThickness",thickness);
        lineRenderer.material.SetFloat("_LaserScale",noiseScale);

        ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            Renderer r = p.GetComponent<Renderer>();
            r.material.SetColor("_EmissionColor", color * (colorInstensity + beamColorEnhance));
        }
        
    }

        // Start is called before the first frame update
    void Start()
    {
        UpdateEndPosition();
        
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEndPosition();
    }

    public void UpdatePosition(Vector2 StartPosition)
    {

        Vector2 direction = laserDirection.normalized;
        transform.position = StartPosition;
        float rotationZ = Mathf.Atan2(direction.y,direction.x); // radian
        transform.rotation = Quaternion.Euler(0,0,rotationZ * Mathf.Rad2Deg);
    }

    private void UpdateEndPosition()
    {
        Vector2 startPosition = transform.position;
        float rotationZ = transform.rotation.eulerAngles.z * Mathf.Deg2Rad; // degree

        Vector2 direction = laserDirection;

        RaycastHit2D hit = Physics2D.Raycast(startPosition,direction.normalized,maxLength,hitable);
        
        float length = maxLength;
        float laserEndRotation = 180;

        
        if(hit)
        {
            length = (hit.point - startPosition).magnitude;
            laserEndRotation = Vector2.SignedAngle(Vector2.right,hit.normal);
            
            //玩家检测相关，默认关闭需要开启
            PlayerStats playerStats = hit.collider.GetComponent<PlayerStats>();
            if(playerStats!= null && !hitPlayer)
            {
                int attackDir = hit.transform.position.x - playerStats.transform.position.x >0? 1:-1;
                playerStats.TakeDamagebyTraps(weaponDamage,stunValue,attackDir,repelDir);
                hitPlayer = true;
                ResetPlayerHit();
            }
        }
        
        Vector2 endPosition = startPosition + length *direction;
        startVFX.transform.position = startPosition;
        lineRenderer.SetPosition(0,startPosition);
        lineRenderer.SetPosition(1,endPosition);
        endVFX.transform.position = endPosition;
        endVFX.transform.rotation = Quaternion.Euler(0,0,laserEndRotation);

    }

    private void ResetPlayerHit()
    {
        StartCoroutine(ResetPlayerHitAfterSec());
    }

    private IEnumerator ResetPlayerHitAfterSec()
    {
        yield return new WaitForSeconds(0.2f);
        hitPlayer = false;
    }
}
