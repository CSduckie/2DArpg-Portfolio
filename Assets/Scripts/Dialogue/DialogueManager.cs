using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Dialogue[] dialogues;

    private void Start()
    {
        dialogues[0].gameObject.SetActive(true);
        FindAnyObjectByType<PlayerController>().DisableInput();
    }

    //开启对话，切换玩家状态为Idle，禁用输入
    public void StartDialogue(int _index)
    {
        dialogues[_index].gameObject.SetActive(true);
        var playerController = FindAnyObjectByType<PlayerController>();
        playerController.DisableInput();
        playerController.ChangeState(PlayerState.Idle);
    }
}
