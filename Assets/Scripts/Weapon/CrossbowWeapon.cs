using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrossbowWeapon : WeaponLong
{
    public override GameObject GenerateBullet(Vector2 dir)
    {

        GameObject bulletGo = GameManager.Instance.GetBullet("arrow");
        Bullet bullet;
        if (bulletGo == null)
        {
             bullet = Instantiate(GameManager.Instance.arrowBullet_prefab, transform.position, Quaternion.identity)
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


        bullet.type = "arrow";
        bullet.dir = dir;

        return bullet.gameObject;
    }
}
