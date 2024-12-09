using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    public WeaponData weaponData;
    public Image _weaponIcon;
    public Image _weaponBG;
    public int slotCount;

    private void Awake()
    {
        _weaponIcon = transform.GetChild(1).GetComponent<Image>();
        _weaponBG = transform.GetChild(0).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }



        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }


    private void OnRightClick()
    {

        if (weaponData == null)
        {
            return;
        }

        GameManager.Instance.money += (int)(weaponData.price * 0.5f);
        ShopPanel.Instance._moneyText.text = GameManager.Instance.money.ToString();
        weaponData = null;
        _weaponIcon.enabled = false;
        GameManager.Instance.currentWeapons.RemoveAt(slotCount);
        ShopPanel.Instance.ShowCurrentWeapon();

    }

    private void OnLeftClick()
    {
        Debug.Log("点击了左键");

        if (weaponData == null)
        {
            return;
        }

        for (int i = 0; i < GameManager.Instance.currentWeapons.Count; i++)
        {

            if (i == slotCount)
            {
                continue;
            }

            if (weaponData.id == GameManager.Instance.currentWeapons[i].id &&
            weaponData.grade == GameManager.Instance.currentWeapons[i].grade
                )
            {


                GameManager.Instance.currentWeapons[slotCount].grade += 1;
                GameManager.Instance.currentWeapons[slotCount].price *= 2;

                GameManager.Instance.currentWeapons.RemoveAt(i);
                ShopPanel.Instance.ShowCurrentWeapon();

                break;
            }


        }

    }
}
