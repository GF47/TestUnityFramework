using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCubeCommand : ControllerCommand 
{
    public override void Execute(IMessage message)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "cmd created cube";
        go.AddComponent<CubeView>();
    }
}
