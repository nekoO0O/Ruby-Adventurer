using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null) // 避免敌人与其交互，导致报错
        {
            rubyController.ChangeHealthy(-1);
            // Debug.Log("Ruby当前的生命值是：" + rubyController.CurrentHealthy);
        }
    }
}