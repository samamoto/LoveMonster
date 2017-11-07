using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SingleAxis
{
    private Controller.Axis _axis;
    private string _name;

    public float Dead { set { ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Dead).floatValue = value; } get { return ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Dead).floatValue; } }
    public float Sensitivity { set { ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Sensitivity).floatValue = value; } get { return ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Sensitivity).floatValue; } }
    public float Gravity { set { ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Gravity).floatValue = value; } get { return ControllerGenerator.GetPropaty(_name, InputManagerGenerator.AxisPropaty.Gravity).floatValue; } }

    private float _now = 0.0f;
    private float _previous = 0.0f;

    public bool Enabled { get; private set; }

    //有効化
    public SingleAxis(Controller.Axis axis, string name)
    {
        _axis = axis;
        _name = name;

        Enabled = true;
    }

    //軸トリガー(0 | 1)
    public int GetAxisDown(float threshold)
    {
        int pre = _previous > threshold ? 1 : (_previous < -threshold ? -1 : 0);
        int now = _now > threshold ? 1 : (_now < -threshold ? -1 : 0);

        return (pre == now) ? 0 : now;
    }

    //軸リリース(0 | 1)
    public int GetAxisUp(float threshold)
    {
        int pre = _previous > threshold ? 1 : (_previous < -threshold ? -1 : 0);
        int now = _now > threshold ? 1 : (_now < -threshold ? -1 : 0);

        return (now == 0) ? pre : 0;
    }

    //軸ホールド(-1 | 0 | 1)
    public int GetAxisHold(float threshold)
    {
        //閾値ありで軸をとる ()
        int now = _now > threshold ? 1 : (_now < -threshold ? -1 : 0);

        return now;
    }

    //軸(0.0f ~ 1.0f || -1.0f ~ 1.0f)
    public float GetAxisRaw()
    {
        //閾値なしで軸をとる(-1.0f ~ 1.0f)
        return _now;
    }

    //更新
    public void Update()
    {
        if (!Enabled)
            return;
        try
        {
            _previous = _now;
            _now = Input.GetAxis(_name);
        }
        catch
        {
            Enabled = false;
            if (!string.IsNullOrEmpty(_name))
            {
                Debug.LogError("Axis error : " + _name + " is not available");
            }
            else
            {
                Debug.LogError("Not exist axis");
            }
        }
    }
}