using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public float waveTimer;
    public GameObject _failPanel;
    public GameObject _successPanel;
    public List<EnemyBase> enemy_list;
    public Transform _map;

    public GameObject enemy1_prefab;
    public GameObject enemy2_prefab;
    public GameObject enemy3_prefab;
    public GameObject enemy4_prefab;
    public GameObject enemy5_prefab;
    public Transform enemyFather;

    public GameObject redfork_prefab;
    public TextAsset levelTextAsset;
    public List<LevelData> levelDatas = new List<LevelData>();
    public LevelData currentLevelData;

    public Dictionary<string, GameObject> enemyPrefabDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        _failPanel = GameObject.Find("FailPanel");
        _successPanel = GameObject.Find("SuccessPanel");
        enemy1_prefab = Resources.Load<GameObject>("Prefabs/Enemy1");
        enemy2_prefab = Resources.Load<GameObject>("Prefabs/Enemy2");
        enemy3_prefab = Resources.Load<GameObject>("Prefabs/Enemy3");
        enemy4_prefab = Resources.Load<GameObject>("Prefabs/Enemy4");
        enemy5_prefab = Resources.Load<GameObject>("Prefabs/Enemy5");


        redfork_prefab = Resources.Load<GameObject>("Prefabs/RedFork");
        _map = GameObject.Find("Map").transform;

        levelTextAsset = Resources.Load<TextAsset>("Data/" + GameManager.Instance.currentDifficulty.levelName);
        levelDatas = JsonConvert.DeserializeObject<List<LevelData>>(levelTextAsset.text);

        enemyFather = GameObject.Find("Enemys").transform;

        enemyPrefabDic.Add("enemy1", enemy1_prefab);
        enemyPrefabDic.Add("enemy2", enemy2_prefab);
        enemyPrefabDic.Add("enemy3", enemy3_prefab);
        enemyPrefabDic.Add("enemy4", enemy4_prefab);
        enemyPrefabDic.Add("enemy5", enemy5_prefab);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevelData = levelDatas[GameManager.Instance.currentWave - 1];
        waveTimer = currentLevelData.waveTimer;

        GenerateEnemy();


        GenerateWeapon();
    }

    private void GenerateWeapon()
    {
        Debug.Log("生成武器开始");

        int i = 0;
        foreach (WeaponData weaponData in GameManager.Instance.currentWeapons)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/" + weaponData.name);
            WeaponBase wb = Instantiate(go, Player.Instance.weaponsPos.GetChild(i)).GetComponent<WeaponBase>();
            wb.data = weaponData;
            wb.index = i;
            i++;
        }



        Debug.Log("生成武器结束");
    }


    private void GenerateEnemy()
    {

        foreach (WaveData waveData in currentLevelData.enemys)
        {

            for (int i = 0; i < waveData.count; i++)
            {
                StartCoroutine(SwawnEnemies(waveData));
            }

        }


    }

    IEnumerator SwawnEnemies(WaveData waveData)
    {
        yield return new WaitForSeconds(waveData.timeAxis);

        if (waveTimer > 0 && !Player.Instance.isDead)
        {
            var spawnPoint = GetRandomPosition(_map.GetComponent<SpriteRenderer>().bounds);

            GameObject go = Instantiate(redfork_prefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(1);
            Destroy(go);

            if (waveTimer > 0 && !Player.Instance.isDead)
            {
                EnemyBase enemy = Instantiate(enemyPrefabDic[waveData.enemyName], spawnPoint, Quaternion.identity).GetComponent<EnemyBase>();
                enemy.transform.parent = enemyFather;

                foreach (EnemyData e in GameManager.Instance.enemyDatas)
                {
                    if (e.name == waveData.enemyName)
                    {
                        enemy.enemyData = e;


                        if (waveData.elite == 1)
                        {
                            enemy.SetElite();
                        }
                    }
                }



                enemy_list.Add(enemy);
            }
        }
    }




    private Vector3 GetRandomPosition(Bounds bounds)
    {
        float safeDistance = 3.5f;

        float randomX = Random.Range(bounds.min.x + safeDistance, bounds.max.x - safeDistance);
        float randomY = Random.Range(bounds.min.y + safeDistance, bounds.max.y - safeDistance);
        float randomZ = 0f;
        return new Vector3(randomX, randomY, randomZ);

    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0)
            {
                waveTimer = 0;

                if (GameManager.Instance.currentWave < 2)
                {
                    NextWave();
                }
                else
                {
                    GoodGame();
                }

            }
        }

        GamePanel.Instance.RenewCountDown(waveTimer);

    }


    private void NextWave()
    {
        GameManager.Instance.money += GameManager.Instance.propData.harvest;
        SceneManager.LoadScene("04-Shop");
        GameManager.Instance.currentWave += 1;


    }


    public void GoodGame()
    {

        _successPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());


        for (int i = 0; i < enemy_list.Count; i++)
        {
            if (enemy_list[i])
            {
                enemy_list[i].Dead();
            }

        }

        if (PlayerPrefs.GetInt("多面手") == 0)
        {
            Debug.Log("多面手解锁");
            PlayerPrefs.SetInt("多面手", 1);

            for (int i = 0; i < GameManager.Instance.roleDatas.Count; i++)
            {
                if (GameManager.Instance.roleDatas[i].name == "多面手")
                {
                    GameManager.Instance.roleDatas[i].unlock = 1;
                }
            }
        }

    }




    public void BadGame()
    {
        _failPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());



        for (int i = 0; i < enemy_list.Count; i++)
        {
            if (enemy_list[i])
            {
                enemy_list[i].Dead();
            }

        }

    }

    IEnumerator GoMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
