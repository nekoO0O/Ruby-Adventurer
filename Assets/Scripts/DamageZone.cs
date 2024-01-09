using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null)//����������佻�������±���
        {
            rubyController.ChangeHealthy(-1);
            //Debug.Log("Ruby��ǰ������ֵ�ǣ�" + rubyController.CurrentHealthy);
        }
    }
}
