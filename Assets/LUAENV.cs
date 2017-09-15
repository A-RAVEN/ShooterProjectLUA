using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LUAENV : MonoBehaviour {

    public static LuaEnv envInstance = null;

    private void Awake()
    {
        initLuaEnv();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void initLuaEnv()
    {
        if(envInstance == null)
        {
            envInstance = new LuaEnv();
        }
    }

    private void OnDestroy()
    {
        //if(envInstance != null)
        //{
        //    envInstance.Dispose();
        //}
    }

}
