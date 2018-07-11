using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InputManager : Singleton<InputManager> {

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
           EventManager.TriggerEvent(new Notification(EventConsts.AXIS_EVENT,new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0)));
        }
    }
}
