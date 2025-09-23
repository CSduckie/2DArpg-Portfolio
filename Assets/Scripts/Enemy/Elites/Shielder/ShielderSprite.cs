using UnityEngine;

public class ShielderSprite : EnemySprite
{
    private ShielderController shielderController;

    protected void Awake()
    {
        shielderController = enemy as ShielderController;
    }
    
    
    //动画事件
    public void EnableAttackBox(int _index)
    {
        shielderController.shielderWeapons[_index].SetActive(true);
    }

    public void DisableAttackBox(int _index)
    {
        shielderController.shielderWeapons[_index].SetActive(false);
    }
}
