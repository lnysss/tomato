using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{

    public static GamePanel Instance;

    public Slider _hpSlider;
    public Slider _expSlider;
    public TMP_Text _moenyCount;
    public TMP_Text _expCount;
    public TMP_Text _hpCount;
    public TMP_Text _countDown;
    public TMP_Text _waveCount;




    private void Awake()
    {
        Instance = this;
        _hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        _expSlider = GameObject.Find("ExpSlider").GetComponent<Slider>();
        _moenyCount = GameObject.Find("MoneyCount").GetComponent<TMP_Text>();
        _expCount = GameObject.Find("ExpCount").GetComponent<TMP_Text>();
        _hpCount = GameObject.Find("HpCount").GetComponent<TMP_Text>();
        _countDown = GameObject.Find("CountDown").GetComponent<TMP_Text>();
        _waveCount = GameObject.Find("WaveCount").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

        RenewExp();

        RenewHp();

        RenewMoney();

        RenewWaveCount();
    }

    public void RenewMoney()
    {
        _moenyCount.text = GameManager.Instance.money.ToString();
    }

    public void RenewHp()
    {
        _hpCount.text = GameManager.Instance.hp + "/" + GameManager.Instance.propData.maxHp;
        _hpSlider.value = GameManager.Instance.hp / GameManager.Instance.propData.maxHp;
    }

    public void RenewExp()
    {

        _expSlider.value = GameManager.Instance.exp % 12 / 12;
        _expCount.text = "LV." + (int)(GameManager.Instance.exp / 12);
    }



    // Update is called once per frame
    void Update()
    {

    }


    public void RenewCountDown(float time)
    {
        _countDown.text = time.ToString("F0");


        if (time <= 5)
        {
            _countDown.color = new Color(255 / 255f, 0, 0);
        }
    }

    public void RenewWaveCount()
    {
        _waveCount.text = "第" + GameManager.Instance.currentWave.ToString() + "关";
    }




}
