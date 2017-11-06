using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SingleAxis
{
    private Controller.Axis _axis;
    private string _name;

    public float Dead { set { ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Dead).floatValue = value; } get { return ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Dead).floatValue; } }
    public float Sensitivity { set { ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Sensitivity).floatValue = value; } get { return ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Sensitivity).floatValue; } }
    public float Gravity { set { ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Gravity).floatValue = value; } get { return ControllerGenerator.GetPropaty(this._name, InputManagerGenerator.AxisPropaty.Gravity).floatValue; } }

    //閾値
    public float threshold = 0.25f;

    private float _now = 0.0f;
    private float _previous = 0.0f;

    public bool Enabled { get; private set; }

    //有効化
    public SingleAxis(Controller.Axis axis, string name)
    {
        this._axis = axis;
        this._name = name;

        this.Enabled = true;
    }

    //軸トリガー(0 | 1)
    public int GetAxisDown()
    {
        int pre = this._previous > this.threshold ? 1 : (this._previous < -this.threshold ? -1 : 0);
        int now = this._now > this.threshold ? 1 : (this._now < -this.threshold ? -1 : 0);

        return (pre == now) ? 0 : now;
    }

    //軸リリース(0 | 1)
    public int GetAxisUp()
    {
        int pre = this._previous > this.threshold ? 1 : (this._previous < -this.threshold ? -1 : 0);
        int now = this._now > this.threshold ? 1 : (this._now < -this.threshold ? -1 : 0);

        return (now == 0) ? pre : 0;
    }

    //軸ホールド(-1 | 0 | 1)
    public int GetAxisHold()
    {
        //閾値ありで軸をとる ()
        int now = this._now > this.threshold ? 1 : (this._now < -this.threshold ? -1 : 0);

        return now;
    }

    //軸(0.0f ~ 1.0f || -1.0f ~ 1.0f)
    public float GetAxisRaw()
    {
        //閾値なしで軸をとる(-1.0f ~ 1.0f)
        return this._now;
    }

    //更新
    public void Update()
    {
        if (!this.Enabled)
            return;
        try
        {
            this._previous = this._now;
            this._now = Input.GetAxis(this._name);
        }
        catch
        {
            this.Enabled = false;
            if (!string.IsNullOrEmpty(this._name))
            {
                Debug.LogError("Axis error : " + this._name + " is not available");
            }
            else
            {
                Debug.LogError("Not exist axis");
            }
        }
    }
}