using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public float displayTime = 4.0f;
    private float timerDisplay;

    public Text dialogText;
    public AudioSource audioSource;
    public AudioClip completeTaskclip;
    private bool hasPlayed;

    void Start()
    {
        // 隐藏对话框
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    // 显示对话框
    public void DisPlayDialog()
    {
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
        UIHealthBar.Instance.hasTask = true;
        if (UIHealthBar.Instance.fixedNum >= 5)
        {
            dialogText.text = "谢谢你Ruby\n`(~)_(~)";
            if (!hasPlayed)
            {
                audioSource.PlayOneShot(completeTaskclip);
                hasPlayed = true;
            }
        }
    }
}