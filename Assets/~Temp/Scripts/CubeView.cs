/***************************************************************
 * @File Name       : CubeView
 * @Author          : GF47
 * @Description     : TODO what's the use of the [CubeView]
 * @Date            : 2017/8/4/星期五 16:31:55
 * @Edit            : none
 **************************************************************/

using UnityEngine;

public class CubeView : View
{
    private Material _mat;

    void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        RegisterMessage(this, new[] { NotiConst.CUBE_RED, NotiConst.CUBE_BLUE });
    }

    public override void OnMessage(IMessage message)
    {
        string msgName = message.Name;
        object msgBody = message.Body;

        switch (msgName)
        {
            case NotiConst.CUBE_RED:
                _mat.color = Color.red;
                break;
            case NotiConst.CUBE_BLUE:
                _mat.color = Color.blue;
                break;
        }
    }
}
