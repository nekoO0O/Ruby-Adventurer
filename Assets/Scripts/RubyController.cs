using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d; // Ruby的刚体组件
    public float speed = 4; // Ruby的速度

    // Ruby的生命值
    public int maxHealthy = 5; // Ruby的最大生命值
    private int currentHealthy; // Ruby当前生命值

    public int CurrentHealthy
    {
        get { return currentHealthy; }
    }

    // Ruby的无敌时间
    private float timeInvincible = 2.0f; // 无敌时间常量
    private bool isInvincible;
    private float invincibleTimer; // 计时器

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        // 设置生命值
        currentHealthy = maxHealthy;

        animator = GetComponent<Animator>();

        // audioSource = GetComponent<AudioSource>();

        respawnPosition = transform.position;
    }

    void Update()
    {
        // 无敌时间
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        // 攻击
        if (Input.GetKeyDown(KeyCode.H))
        {
            Launch();
        }

        // 检测是否与NPC对话
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

    // 固定更新，每秒50次
    void FixedUpdate()
    {
        // 使用刚体移动
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        // 当玩家输入的某一个轴向不为0
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

    // 改变生命值调用方法
    public void ChangeHealthy(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            // 受到伤害
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            playSound(playerHit);
        }

        currentHealthy = Mathf.Clamp(currentHealthy + amount, 0, maxHealthy);

        // 将当前血量和最大血量输出到控制台
        // Debug.Log(currentHealthy + "/" + maxHealthy);
        UIHealthBar.Instance.SetValue(currentHealthy / (float)maxHealthy);

        if (currentHealthy <= 0)
        {
            Respawan();
        }
    }

    // 攻击方法
    private void Launch()
    {
        if (!UIHealthBar.Instance.hasTask)
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

    // 播放音效方法
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