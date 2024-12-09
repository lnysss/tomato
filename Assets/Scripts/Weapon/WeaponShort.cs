using System;
using System.Collections;
using UnityEngine;

public class WeaponShort : WeaponBase
{

    public new void Awake()
    {
        base.Awake();
        moveSpeed = 10;
    }

    //开火
    public override void Fire()
    {
        if (isCooling)
        {
            return;
        }

        //打开碰撞器
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        isAiming = false;
        StartCoroutine(GoPosition());  //武器向敌人位置移动

        isCooling = true;

    }


    IEnumerator GoPosition()
    {

        var enemyPos = enemy.position + new Vector3(0, enemy.GetComponent<SpriteRenderer>().size.y / 2, 0);


        while (Vector2.Distance(transform.position, enemyPos) > 0.1f)
        {
            Vector3 direction = (enemyPos - transform.position).normalized;

            Vector3 moveAmount = direction * moveSpeed * Time.deltaTime;

            transform.position += moveAmount;

            yield return null;
        }



        //gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(ReturnPosition());

        //关闭碰撞器

    }


    IEnumerator ReturnPosition()
    {
        Vector3 weaponPos = Player.Instance.weaponsPos.GetChild(index).position;
        while ((weaponPos - transform.localPosition).magnitude > 0.1f)
        {
            weaponPos = Player.Instance.weaponsPos.GetChild(index).position;
            Vector3 direction = (weaponPos - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
            if ((weaponPos - transform.position).magnitude < 0.6f)
            {
                transform.position = weaponPos;
                //transform.position.Set(weaponPos.x, weaponPos.y, weaponPos.z);
                break;
            }
        }

        //回到原点后可以进行瞄准, 防止在攻击过程中方向转动
        isAiming = true;
    }




    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //判断是否暴击
            bool isCritical = CriticalHits();
            if (isCritical)
            {
                col.GetComponent<EnemyBase>().Injured(data.damage * data.critical_strikes_multiple);

                //文字
                Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                number.text.text = (data.damage * data.critical_strikes_multiple).ToString();
                number.text.color = new Color(255 / 255f, 188 / 255f, 0);
                number.transform.position = transform.position;
            }
            else
            {
                col.GetComponent<EnemyBase>().Injured(data.damage);
                //文字
                Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                number.text.text = (data.damage).ToString();
                number.text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                number.transform.position = transform.position;
            }

            //音效
            Instantiate(GameManager.Instance.attackMusic);

            // gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }

    }


}