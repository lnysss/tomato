using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public RoleData currentRole;
    public List<WeaponData> currentWeapons = new List<WeaponData>();

    [SerializeField]
    public PropData propData = new PropData();
    public List<PropData> currentProps = new List<PropData>();



    public DifficultyData currentDifficulty;
    public int currentWave = 1;


    public GameObject enemyBullet_prefab;
    public GameObject moeny_prefab;
    public GameObject redCircle_prefab;
    public GameObject arrowBullet_prefab;
    public GameObject pistolBullet_prefab;
    public GameObject medicalBullet_prefab;

    public List<RoleData> roleDatas = new List<RoleData>();
    public TextAsset roleTextAsset;
    public List<DifficultyData> difficultyDatas = new List<DifficultyData>();
    public TextAsset difficultyTextAsset;

    public List<WeaponData> weaponDatas = new List<WeaponData>();
    public TextAsset weaponTextAsset;

    public List<EnemyData> enemyDatas = new List<EnemyData>();
    public TextAsset enemyTextAsset;


    public List<PropData> propDatas = new List<PropData>();
    public TextAsset propTextAsset;


    public float hp = 15f;
    public int money = 30;
    public float exp = 0;

    public SpriteAtlas propsAtlas;
    public GameObject number_prefab;

    public GameObject attackMusic;
    public GameObject shootMusic;
    public GameObject menuMusic;
    public GameObject hurtMusic;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("多面手"))
        {
            PlayerPrefs.SetInt("多面手", 0);
        }
        if (!PlayerPrefs.HasKey("公牛"))
        {
            PlayerPrefs.SetInt("公牛", 0);
        }


        enemyTextAsset = Resources.Load<TextAsset>("Data/enemy");
        enemyDatas = JsonConvert.DeserializeObject<List<EnemyData>>(enemyTextAsset.text);

        enemyBullet_prefab = Resources.Load<GameObject>("Prefabs/EnemyBullet");
        moeny_prefab = Resources.Load<GameObject>("Prefabs/Money");
        redCircle_prefab = Resources.Load<GameObject>("Prefabs/RedCircle");
        number_prefab = Resources.Load<GameObject>("Prefabs/Number");

        //音效
        attackMusic = Resources.Load<GameObject>("Prefabs/AttackMusic");
        shootMusic = Resources.Load<GameObject>("Prefabs/ShootMusic");
        menuMusic = Resources.Load<GameObject>("Prefabs/MenuMusic");
        hurtMusic = Resources.Load<GameObject>("Prefabs/HurtMusic");



        difficultyTextAsset = Resources.Load<TextAsset>("Data/difficulty");
        difficultyDatas = JsonConvert.DeserializeObject<List<DifficultyData>>(difficultyTextAsset.text);

        roleTextAsset = Resources.Load<TextAsset>("Data/role");
        roleDatas = JsonConvert.DeserializeObject<List<RoleData>>(roleTextAsset.text);

        weaponTextAsset = Resources.Load<TextAsset>("Data/weapon");
        weaponDatas = JsonConvert.DeserializeObject<List<WeaponData>>(weaponTextAsset.text);


        propTextAsset = Resources.Load<TextAsset>("Data/prop");
        propDatas = JsonConvert.DeserializeObject<List<PropData>>(propTextAsset.text);

        arrowBullet_prefab = Resources.Load<GameObject>("Prefabs/ArrowBullet");
        pistolBullet_prefab = Resources.Load<GameObject>("Prefabs/PistolBullet");
        medicalBullet_prefab = Resources.Load<GameObject>("Prefabs/MedicalBullet");

        propsAtlas = Resources.Load<SpriteAtlas>("Image/其他/Props");
    }


    private void Start()
    {
    }

    private void Update()
    {
    }

    public object RandomOne<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }

        Random random = new Random();
        int index = random.Next(0, list.Count);

        return list[index];
    }


    public void InitProp()
    {


        if (currentRole.name == "全能者")
        {
            propData.maxHp += 5;
            propData.speedPer += 0.05f;
            propData.harvest += 8;

        }
        else if (currentRole.name == "斗士")
        {
            propData.short_attackSpeed += 0.5f;
            propData.long_range -= 0.5f;
            propData.short_range -= 0.5f;
            propData.long_damage -= 0.5f;

        }
        else if (currentRole.name == "医生")
        {
            propData.revive += 5f;
            propData.short_attackSpeed -= 0.5f;
            propData.long_attackSpeed -= 0.5f;
        }
        else if (currentRole.name == "公牛")
        {
            propData.maxHp += 20f;
            propData.revive += 15f;
            propData.slot = 0;

        }
        else if (currentRole.name == "多面手")
        {
            propData.long_damage += 0.2f;
            propData.short_damage += 0.2f;
            propData.slot = 12;
        }


        hp = propData.maxHp;
        money = 30;
        exp = 0;

    }


    public void FusionAttr(PropData shopProp)
    {
        propData.maxHp += shopProp.maxHp;
        propData.revive += shopProp.revive;
        propData.short_damage += shopProp.short_damage;
        propData.short_range += shopProp.short_range;
        propData.short_attackSpeed += shopProp.short_attackSpeed;
        propData.long_damage += shopProp.long_damage;
        propData.long_range += shopProp.long_range;
        propData.long_attackSpeed += shopProp.long_attackSpeed;
        propData.speedPer += shopProp.speedPer;
        propData.harvest += shopProp.harvest;
        propData.shopDiscount += shopProp.shopDiscount;
        propData.expMuti += shopProp.expMuti;
        propData.pickRange += shopProp.pickRange;
        propData.critical_strikes_probability += shopProp.critical_strikes_probability;

    }
}