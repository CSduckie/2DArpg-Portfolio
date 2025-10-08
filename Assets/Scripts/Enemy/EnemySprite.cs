using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    protected EnemyController enemy;
    public SpriteRenderer sprite;
    public Material material;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        material = sprite.material;
    }
}
