using System;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("无限背景")] 
    public GameObject mainCamera;

    public float mapWidth;
    public int mapNum;
    private float totalWidth;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mapWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        totalWidth = mapWidth * mapNum;
    }

    private void Update()
    {
        Vector3 currentPos = transform.position;
        if (mainCamera.transform.position.x > transform.position.x + totalWidth / 2)
        {
            currentPos.x += totalWidth;
            transform.position = currentPos;
        }
        else if (mainCamera.transform.position.x < transform.position.x + totalWidth / 2)
        {
            currentPos.x -= totalWidth;
            transform.position = currentPos;
        }
        
        
    }
}
