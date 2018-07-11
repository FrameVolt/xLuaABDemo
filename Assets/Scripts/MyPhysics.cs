using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class MyPhysics
{
    public static RaycastHit RaycastWithHit(Ray ray) {
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }
}
