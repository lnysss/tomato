using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DifficultyData difficultyData;

    public Image _backImage;
    public Image _avatar;
    public Button _button;


    private void Awake()
    {
        _backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        _avatar = transform.GetChild(0).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(DifficultyData d)
    {
        difficultyData = d;

        _avatar.sprite = Resources.Load<SpriteAtlas>("Image/UI/危险等级").GetSprite(difficultyData.name);

        SetBackColor(difficultyData.id);

        _button.onClick.AddListener(() =>
        {

            GameManager.Instance.currentDifficulty = difficultyData;


            SceneManager.LoadScene(2);
        });
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _backImage.color = new Color(207f / 255f, 207 / 255f, 207 / 255f);
        RenewUI();
    }

    private void RenewUI()
    {
        DifficultySelectPanel.Instance._difficultyAvatar.sprite = _avatar.sprite;
        DifficultySelectPanel.Instance._difficultyName.text = difficultyData.name;
        DifficultySelectPanel.Instance._difficultyDescribe.text = GetDifficultyDescribe();
    }

    private string GetDifficultyDescribe()
    {
        string result = "";

        foreach (DifficultyData d in GameManager.Instance.difficultyDatas)
        {
            result += d.describe + "\n";

            if (d == difficultyData)
            {
                break;
            }
        }


        if (result == "\n")
        {
            result = "无修改";
        }
        else
        {
            result = result.TrimStart('\n');
        }

        return result;
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        SetBackColor(difficultyData.id);
    }



    private void SetBackColor(int id)
    {
        switch (id)
        {
            case 1:
                _backImage.color = new Color(33 / 255f, 33 / 255f, 33 / 255f);
                break;
            case 2:
                _backImage.color = new Color(63 / 255f, 88 / 255f, 104 / 255f);
                break;
            case 3:
                _backImage.color = new Color(83 / 255f, 62 / 255f, 103 / 255f);
                break;
            case 4:
                _backImage.color = new Color(103 / 255f, 54 / 255f, 54 / 255f);
                break;
            case 5:
                _backImage.color = new Color(103 / 255f, 69 / 255f, 54 / 255f);
                break;
            case 6:
                _backImage.color = new Color(91 / 255f, 87 / 255f, 55 / 255f);
                break;
        }
    }
}
