using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class Manager : MonoBehaviour {

    public static Manager managerInstance = null;

    public bool playerDead = false;
    // Use this for initialization

    private void Awake()
    {
        managerInstance = this;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}
}
