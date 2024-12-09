using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopPanel : MonoBehaviour
{
    public static ShopPanel Instance;

    public Button _startButton;
    public Button _refreshButton;
    public TMP_Text _shopText;
    public TMP_Text _moneyText;
    public TMP_Text _weaponTitle;

    public Transform _attrLayout;
    public Transform _propsLayout;
    public Transform _weaponsLayout;
    public Transform _itemLayout;

    public List<ItemData> props = new List<ItemData>();


    private void Awake()
    {
        Instance = this;

        _startButton = GameObject.Find("StartButton").GetComponent<Button>();
        _refreshButton = GameObject.Find("RefreshButton").GetComponent<Button>();
        _shopText = GameObject.Find("ShopText").GetComponent<TMP_Text>();
        _moneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        _weaponTitle = GameObject.Find("WeaponTitle").GetComponent<TMP_Text>();

        _attrLayout = GameObject.Find("AttrLayout").transform;
        _propsLayout = GameObject.Find("PropsLayout").transform;
        _weaponsLayout = GameObject.Find("WeaponsLayout").transform;
        _itemLayout = GameObject.Find("ItemLayout").transform;


    }

    // Start is called before the first frame update
    void Start()
    {


        _shopText.text = "商店 (第" + (GameManager.Instance.currentWave - 1) + "波)";

        _startButton.transform.GetChild(0).GetComponent<TMP_Text>().text =
            "出发 (第" + (GameManager.Instance.currentWave) + "波)";
        _startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(2);
        });

        _moneyText.text = GameManager.Instance.money.ToString();

        SetAttrUI();




        ShowCurrentWeapon();

        ShowCurrentProp();

        RandomProps();



        _refreshButton.onClick.AddListener(() =>
        {

            RefreshItem();
        });

        SetWeaponSlotIndex();

    }

    private void SetWeaponSlotIndex()
    {
        int count = _weaponsLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            _weaponsLayout.GetChild(i).GetComponent<WeaponSlot>().slotCount = i;
        }
    }

    private void RefreshItem()
    {
        if (GameManager.Instance.money < 3)
        {
            return;
        }

        GameManager.Instance.money -= 3;
        _moneyText.text = GameManager.Instance.money.ToString();
        RandomProps();
    }

    private void RandomProps()
    {
        props.Clear();


        int propCount = Random.Range(2, 4);


        for (int i = 0; i < propCount; i++)
        {
            props.Add(GameManager.Instance.RandomOne(GameManager.Instance.propDatas) as ItemData);
        }


        for (int i = 0; i < 4 - propCount; i++)
        {
            props.Add(GameManager.Instance.RandomOne(GameManager.Instance.weaponDatas) as ItemData);
        }


        ShowPropUI();
    }

    private void ShowPropUI()
    {
        int i = 0;

        foreach (ItemData item in props)
        {

            _itemLayout.GetChild(i).GetComponent<ItemCardUI>().itemData = item;
            _itemLayout.GetChild(i).GetComponent<ItemCardUI>()._canvasGroup.alpha = 1;
            _itemLayout.GetChild(i).GetComponent<ItemCardUI>()._canvasGroup.interactable = true;

            _itemLayout.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = item.name;

            if (item is WeaponData)
            {
                _itemLayout.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = "武器";

                _itemLayout.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite
                    = Resources.Load<Sprite>(item.avatar);
            }
            else
            {
                _itemLayout.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = "道具";

                _itemLayout.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite
                    = GameManager.Instance.propsAtlas.GetSprite(item.name);
            }

            _itemLayout.GetChild(i).GetChild(3).GetComponent<TMP_Text>().text = item.describe;

            _itemLayout.GetChild(i).GetChild(4).GetChild(0).GetComponent<TMP_Text>().text
                = item.price.ToString();

            i++;

        }
    }

    public void ShowCurrentWeapon()
    {
        int count = _weaponsLayout.childCount;

        for (int i = 0; i < count; i++)
        {
            WeaponSlot slot = _weaponsLayout.GetChild(i).GetComponent<WeaponSlot>();


            if (i < GameManager.Instance.currentWeapons.Count)
            {


                slot._weaponIcon.enabled = true;
                slot.weaponData = GameManager.Instance.currentWeapons[i];
                slot._weaponIcon.sprite = Resources.Load<Sprite>(slot.weaponData.avatar);


                switch (slot.weaponData.grade)
                {
                    case 1:
                        slot._weaponBG.color = new Color(29 / 255f, 29 / 255f, 29 / 255f);
                        break;
                    case 2:
                        slot._weaponBG.color = new Color(58 / 255f, 83 / 255f, 99 / 255f);
                        break;
                    case 3:
                        slot._weaponBG.color = new Color(79 / 255f, 58 / 255f, 99 / 255f);
                        break;
                    case 4:
                        slot._weaponBG.color = new Color(99 / 255f, 50 / 255f, 50 / 255f);
                        break;
                }
            }

            else
            {

                slot._weaponIcon.enabled = false;
                slot._weaponBG.color = new Color(29 / 255f, 29 / 255f, 29 / 255f);
                slot.weaponData = null;
            }
        }


        _weaponTitle.text = "武器(" + GameManager.Instance.currentWeapons.Count + "/" +
                            GameManager.Instance.propData.slot + ")";
    }

    private void ShowCurrentProp()
    {
        int count = _propsLayout.childCount;

        for (int i = 0; i < count; i++)
        {
            if (i < GameManager.Instance.currentProps.Count)
            {

                _propsLayout.GetChild(i).GetChild(0).GetComponent<Image>().enabled = true;
                _propsLayout.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    GameManager.Instance.propsAtlas.GetSprite(GameManager.Instance.currentProps[i].name);

            }
            else
            {

                _propsLayout.GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;


            }

        }

    }

    private void SetAttrUI()
    {

        _attrLayout.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text =
            ((int)(GameManager.Instance.exp / 12)).ToString();

        _attrLayout.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text =
           GameManager.Instance.propData.maxHp.ToString();


        _attrLayout.GetChild(2).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.revive.ToString();


        _attrLayout.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.short_damage.ToString();



        _attrLayout.GetChild(4).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.long_damage.ToString();


        _attrLayout.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.critical_strikes_probability.ToString();


        _attrLayout.GetChild(6).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.pickRange.ToString();


        _attrLayout.GetChild(7).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.speedPer.ToString();



        _attrLayout.GetChild(8).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.harvest.ToString();


        _attrLayout.GetChild(9).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.expMuti.ToString();


        _attrLayout.GetChild(10).GetChild(2).GetComponent<TMP_Text>().text =
            GameManager.Instance.propData.shopDiscount.ToString();


    }

    // Update is called once per frame
    void Update()
    {

    }



    public bool Shopping(ItemData itemData)
    {
        if (GameManager.Instance.money < 3)
        {
            return false;
        }


        if (itemData is WeaponData && GameManager.Instance.currentWeapons.Count >= GameManager.Instance.propData.slot)
        {
            return false;
        }


        if (itemData is PropData && GameManager.Instance.currentProps.Count >= 20)
        {
            return false;
        }

        GameManager.Instance.money -= itemData.price;
        _moneyText.text = GameManager.Instance.money.ToString();



        if (itemData is WeaponData)
        {
            WeaponData tempItem = JsonConvert.DeserializeObject<WeaponData>(JsonConvert.SerializeObject(itemData));
            GameManager.Instance.currentWeapons.Add(tempItem);
            ShowCurrentWeapon();
        }

        else
        {
            PropData tempItem = JsonConvert.DeserializeObject<PropData>(JsonConvert.SerializeObject(itemData));

            GameManager.Instance.currentProps.Add(tempItem);
            ShowCurrentProp();


            GameManager.Instance.FusionAttr(tempItem);
            SetAttrUI();
        }

        return true;


    }
}
