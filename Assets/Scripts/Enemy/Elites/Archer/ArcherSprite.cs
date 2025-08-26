using UnityEngine;

public class ArcherSprite : EnemySprite
{
    private ArcherController archerController;

    protected void Awake()
    {
        archerController = enemy as ArcherController;
    }
    
    
    //动画事件
    public void EnableAttackBox(int _index)
    {
        archerController.archerWeapons[_index].SetActive(true);
    }

    public void DisableAttackBox(int _index)
    {
        archerController.archerWeapons[_index].SetActive(false);
    }
}
