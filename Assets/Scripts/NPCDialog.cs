using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        //隐藏对话框
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    // Update is called once per frame
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

    //显示对话框
    public void DisPlayDialog()
    {
        dialogBox.SetActive(true);
        timerDisplay = displayTime;
        UIHealthBar.instance.hasTask = true;
        if (UIHealthBar.instance.fixedNum >= 5)
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
