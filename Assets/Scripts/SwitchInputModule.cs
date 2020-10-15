using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchInputModule : MonoBehaviour
{

    [Header("SteamInputModule")]
    public SteamInputModule _steamInputModule;

    [Header("現在の状態を表示するテキスト")]
    public Text _text;

    [Header("アクティブ切り替えキー")]
    public KeyCode _keyCode;

    [Header("VRInputモジュールが有効なときの表示テキスト")]
    public string onEnableText;

    [Header("VRInputモジュールが無効なときの表示テキスト")]
    public string onDisableText;

    private void Start()
    {
        _steamInputModule.enabled = true;
        if (_text != null) _text.text = StatusText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            _steamInputModule.enabled = FlipEnable();
            if(_text != null)_text.text = StatusText();
        }
    }

    // 有効・無効の反転
    public bool FlipEnable()
    {
        if (_steamInputModule.enabled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // 有効・向こうに応じたテキストを返す
    public string StatusText()
    {
        if (_steamInputModule.enabled)
        {
            return onEnableText;
        }
        else
        {
            return onDisableText;
        }
    }
}
