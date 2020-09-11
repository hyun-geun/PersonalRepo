using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//hp, 공격력, 사망 유무/사망 매서드, 피격 매서드 공격 매서드
//hp, 공격력, 사망 유무, 리지드 바디, 콜라이더, 애니메이터,.../사망 매서드, 피격 매서드
public class PlayerStatus :LifeEntity
{
    BoxCollider2D attackRange;

    public Slider hpSlider;
    void Awake()
    {
        attackRange = this.transform.Find("AttackRange").GetComponent<BoxCollider2D>();

        offencePoint = 20;
        healthPoint = 50;
        isDead = false;

        hpSlider.maxValue = healthPoint;
        hpSlider.value = healthPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int damage = collision.gameObject.GetComponent<EnemyStatus>().offencePoint;
            OnDamage(damage);
        }
    }

    void TurnOnAttackRange(bool isFlipped)
    {
        int directionX;

        if ((!isFlipped && attackRange.offset.x > 0) || (isFlipped && attackRange.offset.x < 0))
        {
            directionX = 1;
        }
        else
        {
            directionX = -1;
        }
        attackRange.offset = new Vector2(attackRange.offset.x * directionX, attackRange.offset.y);
        attackRange.enabled = true;

    }
    void TurnOffAttackRange()
    {
        attackRange.enabled = false;
    }
    public override void OnDamage(int damage)
    {
        healthPoint -= damage;
        hpSlider.value = healthPoint;
        if (healthPoint > 0)
        {
            SendMessage("BeHitted");
        }
        else
        {
            SendMessage("Dead");
            OnDead();
        }
    }
    public override void OnDead()
    {
        Debug.Log("player is Dead");
        GameManager.instance.isGameOver = true;
    }
}
