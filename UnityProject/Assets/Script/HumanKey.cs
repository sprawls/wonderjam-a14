using UnityEngine;
using System.Collections;

public class HumanKey : IKeyGetter {

    delegate bool KeyGetter();

    KeyGetter up;
    KeyGetter down;
    KeyGetter left;
    KeyGetter right;

    public HumanKey(string up, string down, string left, string right)
    {
        this.up = make_delegate(up);
        this.down = make_delegate(down);
        this.left = make_delegate(left);
        this.right = make_delegate(right);
    }

    public HumanKey(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
    {
        this.up = make_delegate(up);
        this.down = make_delegate(down);
        this.left = make_delegate(left);
        this.right = make_delegate(right);
    }

    private KeyGetter make_delegate(string s)
    {
        return delegate() { return Input.GetKeyDown(s); };
    }

    private KeyGetter make_delegate(KeyCode s)
    {
        return delegate() { return Input.GetKeyDown(s); };
    }

    public bool GetKeys(BeatEnum e)
    {
        switch(e)
        {
            case BeatEnum.Down:
                return down();
            case BeatEnum.Up:
                return up();
            case BeatEnum.Left:
                return left();
            case BeatEnum.Right:
                return right();
            default:
                return false;
        }
    
    }
}
