using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalGunWeapon : WeaponLong
{

    public new void Start()
    {

        base.Start();

        //如果是医生, 攻击速度+200%
        if (GameManager.Instance.currentRole.name == "医生")
        {
            data.cooling /= 3;
        }

    }

    public override GameObject GenerateBullet(Vector2 dir)
    {
        
        GameObject bulletGo = GameManager.Instance.GetBullet("medical");
        Bullet bullet;
        if (bulletGo == null)
        {
            bullet = Instantiate(GameManager.Instance.medicalBullet_prefab, transform.position, Quaternion.identity)
            .GetComponent<Bullet>();
            bullet.isPool = false;
        }
        else
        {
            //bulletGo.transform.SetParent(transform, true);
            bulletGo.transform.position = transform.position;
            bulletGo.transform.rotation = Quaternion.identity;
            bullet = bulletGo.GetComponent<Bullet>();
            bullet.isPool = true;
        }

        bullet.type = "medical";
        bullet.dir = dir;

        return bullet.gameObject;
    }
}
