using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public float speed = 3.0f;//�����˶��ٶ�

    public bool vertical;//�����˶�����trueΪ��ֱ����falseΪˮƽ����

    private int direction = 1;//�����˶�����
    public float changeTime = 3.0f;//����ı�ʱ����
    private float timer;//��ʱ��

    private Animator animator;//�������

    private bool broken;//��ǰ�������Ƿ���ϣ�trueΪ��״̬�����ƶ�����falseΪ�޺�״̬�������ƶ���

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

        //ÿ��һ��ʱ��ʹ���˸�������
        timer -= Time.deltaTime;
        if (timer < 0) 
        {
            direction = -direction;
            //animator.SetFloat("MoveX", direction);
            PlayMoveAnimation();
            timer = changeTime;
        }

        Vector2 position = GetComponent<Rigidbody2D>().position;
        if (vertical)//��ֱ����
        {
            position.y += speed * Time.deltaTime * direction;
        }
        else//ˮƽ����
        {
            position.x += speed * Time.deltaTime * direction;
        }
        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //��Ҵ���ʹ���Ѫ
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null) 
        {
            rubyController.ChangeHealthy(-1);
        }
    }

    //�����ƶ�����
    private void PlayMoveAnimation()
    {
        //ʹ�û����
        if (vertical)//��ֱ����
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else//ˮƽ����
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }

    //�޺û����˷���
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
