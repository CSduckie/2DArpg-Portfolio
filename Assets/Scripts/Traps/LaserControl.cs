using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    [Header("Light VAR")]
    public Color color = new Color(195/255,36/255,0);
    public float colorInstensity; //HDR color, >1 bloom
    public float beamColorEnhance;


    public float maxLength;
    public float thickness;
    public float noiseScale;
    public GameObject startVFX;
    public GameObject endVFX;

    public LineRenderer lineRenderer;
     [Header("LaserDir")]
    public Vector2 laserDirection = Vector2.right;  // 在 Inspector 中调整激光方向
    public LayerMask hitable;
    private LaserConsoleControl console;
    
    [Header("AttackData")]
    [SerializeField] private SkillConfig skillConfig;
    private float weaponDamage;
    private float stunValue;
    private Vector2 repelDir;
    public bool hitPlayer;
    
    private void Awake()
    {
        console = GetComponentInParent<LaserConsoleControl>();
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
                int attackDir = transform.position.x - playerStats.transform.position.x > 0 ? -1:1;
                Debug.Log(playerStats.transform.position.x);
                Debug.Log(transform.position.x);
                playerStats.TakeDamagebyTraps(weaponDamage,stunValue,attackDir,repelDir);
                hitPlayer = true;
                console.DisableLaserForSec();
            }
        }
        
        Vector2 endPosition = startPosition + length *direction;
        startVFX.transform.position = startPosition;
        lineRenderer.SetPosition(0,startPosition);
        lineRenderer.SetPosition(1,endPosition);
        endVFX.transform.position = endPosition;
        endVFX.transform.rotation = Quaternion.Euler(0,0,laserEndRotation);

    }
    
}
