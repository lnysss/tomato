using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardUI : MonoBehaviour
{
    public ItemData itemData;
    public Button _button;
    public CanvasGroup _canvasGroup;

    private void Awake()
    {
        _button = transform.GetChild(4).GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(() =>
        {

            bool result = ShopPanel.Instance.Shopping(itemData);
            if (result)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
            }

        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
