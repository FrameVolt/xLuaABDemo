using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class Test : MonoBehaviour {


	void Start () {
        //this.GetAsset<GameObject>("prefabbundle", "Coin", x => { Instantiate(x); });

        this.GetAsset<Sprite>("pngbundle", "UI_Graphic_Resource_Food", x => { gameObject.AddComponent<SpriteRenderer>().sprite = x; });
        
    }
	

}
