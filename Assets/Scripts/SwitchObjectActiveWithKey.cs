using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchObjectActiveWithKey : MonoBehaviour
{

    [Header("アクティブ切り替え対象のオブジェクト")]
    public GameObject _gameObject;

    [Header("デフォルトのアクティブ状態")]
    public bool isActive = false;

    [Header("アクティブ切り替えキー")]
    public KeyCode _keyCode;

    [Header("現在の状態を表示するテキスト")]
    public Text _text;

    [Header("オブジェクトがアクティブなときの表示テキスト")]
    public string onActiveText;

    [Header("オブジェクトがアクティブでないときの表示テキスト")]
    public string onInactiveText;

    private void Start()
    {
        _gameObject.SetActive(isActive);
        if (_text != null) _text.text = StatusText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            FlipActive();
            if (_text != null) _text.text = StatusText();
        }
    }

    // 有効・無効の反転
    public void FlipActive()
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    // 有効・向こうに応じたテキストを返す
    public string StatusText()
    {
        if (_gameObject.activeSelf)
        {
            return onActiveText;
        }
        else
        {
            return onInactiveText;
        }
    }
}
