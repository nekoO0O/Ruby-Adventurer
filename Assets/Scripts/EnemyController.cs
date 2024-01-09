using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public float speed = 3.0f;//敌人运动速度

    public bool vertical;//敌人运动轴向，true为垂直轴向，false为水平轴向

    private int direction = 1;//敌人运动方向
    public float changeTime = 3.0f;//方向改变时间间隔
    private float timer;//计时器

    private Animator animator;//动画组件

    private bool broken;//当前机器人是否故障，true为损坏状态（会移动），false为修好状态（不会移动）

    public ParticleSystem smokeEffect;

    private AudioSource audioSource;

    public AudioClip fixedSound;
    public AudioClip[] hitSounds;

    public GameObject hitEffectParticle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;

        animator = GetComponent<Animator>();

        //animator.SetFloat("MoveX", direction);
        //animator.SetBool("Vertical", vertical);

        PlayMoveAnimation();

        broken = true;

        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!broken) 
        {
            return;
        }

        //每过一段时间使敌人更换方向
        timer -= Time.deltaTime;
        if (timer < 0) 
        {
            direction = -direction;
            //animator.SetFloat("MoveX", direction);
            PlayMoveAnimation();
            timer = changeTime;
        }

        Vector2 position = GetComponent<Rigidbody2D>().position;
        if (vertical)//垂直轴向
        {
            position.y += speed * Time.deltaTime * direction;
        }
        else//水平轴向
        {
            position.x += speed * Time.deltaTime * direction;
        }
        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //玩家触碰使其掉血
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null) 
        {
            rubyController.ChangeHealthy(-1);
        }
    }

    //控制移动动画
    private void PlayMoveAnimation()
    {
        //使用混合树
        if (vertical)//垂直轴向
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else//水平轴向
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }

    //修好机器人方法
    public void Fix()
    {
        Instantiate(hitEffectParticle, transform.position, Quaternion.identity);
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");

        int randomNum = Random.Range(0, 2);
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound", 1f);
        UIHealthBar.instance.fixedNum++;
        smokeEffect.Stop();
    }

    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedSound);
    }
}
