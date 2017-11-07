using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SingleButton
{
    private Controller.Button _button;
    private string _name;

    private bool _now = false;
    private bool _previous = false;

    public bool Enabled { get; private set; }

    public SingleButton(Controller.Button button, string name)
    {
        this._button = button;
        this._name = name;

        this.Enabled = true;
    }

    public bool GetButtonDown()
    {
        return (!this._previous && this._now);
    }

    public bool GetButtonHold()
    {
        return this._now;
    }

    public bool GetButtonUp()
    {
        return (this._previous && !this._now);
    }

    public void Update()
    {
        if (!this.Enabled)
            return;

        try
        {
            this._previous = this._now;
            this._now = Input.GetButton(this._name);
        }
        catch
        {
            this.Enabled = false;
            if (!string.IsNullOrEmpty(this._name))
            {
                Debug.LogError("Button error : " + this._name + " is not available");
            }
            else
            {
                Debug.LogError("Not exist button");
            }
        }
    }
}