using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
    
    [Header("全局访问点组件")]
    public PlayerController player;
}
