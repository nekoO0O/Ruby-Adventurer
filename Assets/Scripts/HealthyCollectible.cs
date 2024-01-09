using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyCollectible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null)//避免敌人与其交互，导致报错
        {
            //Ruby是否满血
            if (rubyController.CurrentHealthy < rubyController.maxHealthy)
            {
                rubyController.ChangeHealthy(1);
                rubyController.playSound(audioClip);
                Instantiate(effectParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
