using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BSOScript : MonoBehaviour
{
    public Text BSOText;
    public int _strike;
    public int _ball;
    public int _out;

    private string s_strike;
    private string s_ball;
    private string s_out;

    // Start is called before the first frame update
    void Start()
    {
        SetString();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetString()
    {
        s_strike = "S" + ToStr(_strike,'S');
        s_ball = "B" + ToStr(_ball, 'B');
        s_out = "O" + ToStr(_out, 'O');
    }

    void SetText()
    {
        BSOText.text = s_ball + "\n" + s_strike + "\n" + s_out;
    }

    public void AddBSO(char type)
    {
        switch (type)
        {
            case 'S':
                _strike++;
                if (_strike >= 3 ){ _strike = 0; _ball = 0; _out++ ; }
                break;
            case 'B':
                _ball++;
                if (_ball >= 4){ _strike = 0; _ball = 0;}
                break;
            case 'O':
                _out++;
                _strike = 0;
                _ball = 0;
                break;
            case 'F':
                if (_strike < 2) { _strike++; }
                break;
            case 'H':
                _strike = 0; _ball = 0;
                break;
        }

        if (_out >= 3) { _out = 0; }

        SetString();
        SetText();
    }


    string ToStr(int num,char type)
    {
        string str = "";
        int i;

        switch (type)
        {
            case 'S':
                for (i = 0; i < num; i++)
                {
                    str += "●";
                }
                for (i = 0; i + num < 2; i++)
                {
                    str += "○";
                }
                break;
            case 'B':
                for (i = 0; i < num; i++)
                {
                    str += "●";
                }
                for (i = 0; i + num < 3; i++)
                {
                    str += "○";
                }
                break;
            case 'O':
                for (i = 0; i < num; i++)
                {
                    str += "●";
                }
                for (i = 0; i + num < 2; i++)
                {
                    str += "○";
                }
                break;
        }

        return str;
    }

}
