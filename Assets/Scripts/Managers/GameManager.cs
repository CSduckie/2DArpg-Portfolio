using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        // 单例初始化逻辑
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 场景切换时保留该对象
        }
        else
        {
            Destroy(gameObject); // 如果已经存在一个实例，销毁多余的
        }
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
    
    [Header("全局访问点组件")]
    public PlayerController player;

    private void Start()
    {
        FindPlayer();
        
    }

    public void FindPlayer()
    {
        player = FindFirstObjectByType<PlayerController>();
        Debug.Log("尝试寻找玩家");
    }
}
