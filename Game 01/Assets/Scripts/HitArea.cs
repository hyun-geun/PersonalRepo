using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttackRange")
        {
            PlayerStatus coll = collision.GetComponentInParent<PlayerStatus>();
            int damage = coll.offencePoint;
            SendMessageUpwards("OnDamage", damage);
        }
    }
}
