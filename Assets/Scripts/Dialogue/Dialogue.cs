using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;   // 谁在说
    [TextArea(3, 10)]
    public string line;                   // 他说的内容
}

[System.Serializable]
public class Dialogues
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}


public class Dialogue : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] private TextMeshProUGUI text;      // 对话文字UI
    [SerializeField] private TextMeshProUGUI nameText;  // 说话者名字UI(可选)
    [SerializeField] private Image speakerIcon;         // 头像UI

    [Header("对话数据")]
    [SerializeField] private Dialogues dialogueData;    // 这一段对话包含的所有台词

    [Header("打字机设置")]
    [SerializeField] private float textSpeed = 0.03f;   // 每个字出现的间隔
    
    private int index = 0;                              // 当前句子的索引
    private bool isTyping = false;                      // 是否正在打字中
    private Coroutine typingCoroutine;

    protected virtual void Start()
    {
        // 初始化
        ClearTextUI();
        StartDialogue();
    }

    private void Update()
    {
        // 这里你可以用按键推进，比如按下空格进入下一句
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnAdvance();
        }
    }

    // 开始整段对话
    public void StartDialogue()
    {
        index = 0;
        ShowCurrentLine();
    }

    // 推进一行对话：如果在打字中 -> 直接跳到整句；否则 -> 下一句
    public void OnAdvance()
    {
        if (isTyping)
        {
            // 正在打字，先跳过打字机动画，直接显示完整句子
            FinishTypingInstantly();
        }
        else
        {
            // 已经完整显示，进入下一句
            NextLine();
        }
    }

    // 显示当前 index 这一句（开始打字协程）
    private void ShowCurrentLine()
    {
        if (index < 0 || index >= dialogueData.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine lineData = dialogueData.dialogueLines[index];

        // 更新头像和名字
        if (speakerIcon != null)
        {
            speakerIcon.sprite = lineData.character != null ? lineData.character.icon : null;
            speakerIcon.enabled = (speakerIcon.sprite != null);
        }

        if (nameText != null)
        {
            nameText.text = lineData.character != null ? lineData.character.name : "";
        }

        // 开始打字
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(lineData.line));
    }

    // 打字机协程：逐字显示
    private IEnumerator TypeLine(string fullLine)
    {
        isTyping = true;
        text.text = string.Empty;

        foreach (char c in fullLine)
        {
            text.text += c;
            //TODO:播放音效
            
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    // 在打字中按下空格时，直接跳出整句
    private void FinishTypingInstantly()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        DialogueLine currentLine = dialogueData.dialogueLines[index];
        text.text = currentLine.line;
        isTyping = false;
    }

    // 切下一句
    private void NextLine()
    {
        index++;

        if (index < dialogueData.dialogueLines.Count)
        {
            ShowCurrentLine();
        }
        else
        {
            // 没有更多句子 -> 结束对话
            EndDialogue();
        }
    }

    // 对话结束时你想做什么就放这里（比如隐藏UI、触发事件等）
    protected virtual void EndDialogue()
    {
        Debug.Log("Dialogue finished.");
        this.gameObject.SetActive(false);
        FindAnyObjectByType<PlayerController>().EnableInput();
        // 你可以在这里把对话框整体 SetActive(false);
    }

    private void ClearTextUI()
    {
        if (text != null) text.text = string.Empty;
        if (nameText != null) nameText.text = string.Empty;
    }
}
