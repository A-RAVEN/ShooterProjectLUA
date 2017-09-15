using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

[System.Serializable]
public class NumberInjection
{
    public string name;
    public float value;
}

[System.Serializable]
public class Injection
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
public class test : MonoBehaviour {
    public TextAsset luaScript;
    public Injection[] injections;
    public NumberInjection[] nInjections;
    //public Text txt;
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 
    //private XLua.LuaEnv luaEnv;

    private Action luaStart;
    private Action luaUpdate;
    private Action luaFixedUpdate;
    private Action luaOnDestroy;

    public LuaTable scriptEnv;

    [CSharpCallLua]
    public delegate void HitDelegate(double damage);

    [CSharpCallLua]
    public delegate void OnTriggerDelegate(Collider other);

    HitDelegate hit;
    OnTriggerDelegate tenter;
    OnTriggerDelegate texit;

    private void Awake()
    {
        //luaEnv = new XLua.LuaEnv();
        //LUAENV.initLuaEnv();
        EventTrigger trigger;
        
        scriptEnv = LUAENV.envInstance.NewTable();

        LuaTable meta = LUAENV.envInstance.NewTable();
        meta.Set("__index", LUAENV.envInstance.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("mono", this);
        foreach (var injection in injections)
        {
            scriptEnv.Set(injection.name, injection.value);
        }

        foreach (var injection in nInjections)
        {
            scriptEnv.Set(injection.name, injection.value);
        }
        LUAENV.envInstance.DoString(luaScript.text, "LuaBehaviour", scriptEnv);

        //luaEnv.Global.Get("takemax", out luamax);

        Action luaAwake = scriptEnv.Get<Action>("awake");
        scriptEnv.Get("start", out luaStart);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("fixedupdate", out luaFixedUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);
        scriptEnv.Get("onDamage", out hit);
        scriptEnv.Get("ontriggerenter", out tenter);
        scriptEnv.Get("ontriggerexit", out texit);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }

    // Use this for initialization
    void Start () {

        if (luaStart != null)
        {
            luaStart();
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (tenter != null)
        {
            tenter(other);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (texit != null)
        {
            texit(other);
        }

    }

    // Update is called once per frame
    void FixedUpdate () {
        if (luaFixedUpdate != null)
        {
            luaFixedUpdate();
        }
    }

    private void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - test.lastGCTime > GCInterval)
        {

            test.lastGCTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaStart = null;
        luaUpdate = null;
        luaFixedUpdate = null;
        luaOnDestroy = null;
        scriptEnv.Dispose();
        injections = null;
    }

    public void reboot()
    {
        //var nav = new UnityEngine.AI.NavMeshAgent;
        //nav.e
        //Rigidbody rig = new Rigidbody();
        LUAENV.envInstance.DoString(luaScript.text, "LuaBehaviour", scriptEnv);
        scriptEnv.Get("update", out luaUpdate);
        scriptEnv.Get("fixedupdate", out luaFixedUpdate);
        scriptEnv.Get("ondestroy", out luaOnDestroy);
        //scriptEnv.Get("takemax", out luamax);
    }

    public double getAxisRaw(string p)
    {
        return Input.GetAxisRaw(p);
    }

    public bool luaRayCastWithHit(Ray ray,out RaycastHit hitinfo,float rayLength,int layerMask)
    {
        return Physics.Raycast(ray, out hitinfo, rayLength, layerMask);
    }

    public void Hit(double damage)
    {
        if (hit != null)
            hit(damage);
    }
}
