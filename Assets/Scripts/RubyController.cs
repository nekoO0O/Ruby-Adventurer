using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;//Ruby�ĸ������
    public float speed = 4;//Ruby���ٶ�

    //Ruby������ֵ
    public int maxHealthy = 5;//Ruby���������ֵ
    private int currentHealthy;//Ruby��ǰ����ֵ
    public int CurrentHealthy { get { return currentHealthy; } }
    
    //Ruby���޵�ʱ��
    private float timeInvincible = 2.0f;//�޵�ʱ�䳣��
    private bool isInvincible;
    private float invincibleTimer;//��ʱ��

    private Vector2 lookDirection = new Vector2(1,0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d= GetComponent<Rigidbody2D>();
        
        //��������ֵ
        currentHealthy = maxHealthy;

        animator = GetComponent<Animator>();

        //audioSource = GetComponent<AudioSource>();

        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�޵�ʱ��
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0) 
            {
                isInvincible = false;
            }
        }

        //����
        if(Input.GetKeyDown(KeyCode.H))
        {
            Launch();
        }

        //����Ƿ���NPC�Ի�
        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null) 
            {
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog != null)
                {
                    npcDialog.DisPlayDialog();
                }
            }
        }
    }

    //�̶����£�ÿ��50��
    void FixedUpdate()
    {
        //ʹ�ø����ƶ�
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //����������ĳһ������Ϊ0
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        rigidbody2d.MovePosition(rigidbody2d.position + move * speed * Time.deltaTime);
    }

    //�ı�����ֵ���÷���
    public void ChangeHealthy(int amount)
    {
        if (amount < 0) 
        {
            if (isInvincible) 
            {
                return;
            }

            //�ܵ��˺�
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            playSound(playerHit);
        }

        currentHealthy = Mathf.Clamp(currentHealthy + amount, 0, maxHealthy);

        //����ǰѪ�������Ѫ�����������̨
        //Debug.Log(currentHealthy + "/" + maxHealthy);
        UIHealthBar.instance.SetValue(currentHealthy / (float)maxHealthy);

        if (currentHealthy<=0)
        {
            Respawan();
        }
    }

    //��������
    private void Launch()
    {
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }

        GameObject projectileObject = Instantiate(projectilePrefab,
            rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        playSound(attackSoundClip);
    }

    //������Ч����
    public void playSound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void Respawan()
    {
        ChangeHealthy(maxHealthy);
        transform.position = respawnPosition;
    }
}
