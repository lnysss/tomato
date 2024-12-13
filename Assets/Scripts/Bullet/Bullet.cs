using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1;
    public float deadTime = 5;
    public float speed = 8;
    public float timer;
    public Vector2 dir = Vector2.zero;
    public string tagName;
    public bool isCritical = false;
    public bool isPool = false;
    public string type;

    public void Awake()
    {

    }

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        timer += Time.deltaTime;
        if (timer >= deadTime)
        {
            DestroyBullet(gameObject);
        }


        transform.position += (Vector3)dir * speed * Time.deltaTime;

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(tagName))
        {

            if (tagName == "Player")
            {
                Player.Instance.Injured(damage);


            }

            else if (tagName == "Enemy")
            {

                if (isCritical)
                {

                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = damage.ToString();
                    number.text.color = new Color(255 / 255f, 178 / 255f, 0);
                    number.transform.position = transform.position;
                }
                else
                {

                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = damage.ToString();
                    number.text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                    number.transform.position = transform.position;
                }

                col.gameObject.GetComponent<EnemyBase>().Injured(damage);


            }
            
            
            DestroyBullet(gameObject);



        }
    }

    public void DestroyBullet(GameObject bulletGo)
    {
        Bullet bullet = bulletGo.GetComponent<Bullet>();
        
        
        if (bullet.isPool)
        {
            
            //bulletGo.transform.SetParent(GameManager.Instance.transform);
            bulletGo.SetActive(false);
            switch (bullet.type)
            {
                case "medical":
                    GameManager.Instance.medicalBulletPool.Push(bullet.gameObject);
                    break;
                case "pistol":
                    GameManager.Instance.pistolBulletPool.Push(bullet.gameObject);
                    break;
                case "arrow":
                    GameManager.Instance.arrowBulletPool.Push(bullet.gameObject);
                    break;
                default:

                    break;
            }
        }
        else
        {
            Destroy(bulletGo);
        }
        


    }
}
