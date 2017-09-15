using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using XLua;
using System;

[LuaCallCSharp]
public class SceneLoadFinishEvent : UnityEvent<int> { }

[LuaCallCSharp]
public class BasicEnv : MonoBehaviour {
    // public UnityAction<int> LoadFinishedAction;
    [CSharpCallLua]
    public delegate void OnSceneLoadFinishedDelegate(string SceneName);

    [CSharpCallLua]
    public delegate void OnBundleLoadFinishedDelegate(string BundlePath, AssetBundle bundle);

    private OnSceneLoadFinishedDelegate SceneLoadFinished;
    private OnBundleLoadFinishedDelegate BundleLoadFinished;
    private Action luaBegin;
    public LuaTable scriptEnv;
    public TextAsset luaScript;
    public AssetBundle assetsbundle;
    public AssetBundle scenebundle;
    GameObject pref;
    private AsyncOperation async_operation;
    private void Awake()
    {
        //DontDestroyOnLoad(SceneLoadFinished);
        DontDestroyOnLoad(gameObject);
        //LoadFinishedAction = new UnityAction<int>(test);
        //SceneLoadFinished.AddListener(LoadFinishedAction);
    }
    // Use this for initialization
    void Start () {
        //SceneLoadFinished = new SceneLoadFinishEvent();
        scriptEnv = LUAENV.envInstance.NewTable();

        LuaTable meta = LUAENV.envInstance.NewTable();
        meta.Set("__index", LUAENV.envInstance.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("mono", this);
        //LUAENV.envInstance.Global.Set("mono", this);
        LUAENV.envInstance.DoString(luaScript.text, "LuaBehaviour", scriptEnv);

        scriptEnv.Get("Begin", out luaBegin);
        scriptEnv.Get("LoadFinished", out SceneLoadFinished);
        scriptEnv.Get("bundleloadfinished", out BundleLoadFinished);

        if (luaBegin != null)
            luaBegin();
        //LoadScene(1);
    }

    public void LoadScene(string SceneName)
    {
        StartCoroutine(corotueLoadScene(SceneName));
    }

    IEnumerator corotueLoadScene(string SceneName)
    {
        async_operation = SceneManager.LoadSceneAsync(SceneName);
        yield return async_operation;
        //SceneLoadFinished.Invoke(1);
        if (SceneLoadFinished != null)
            SceneLoadFinished(SceneName);
        //pref = (GameObject)Resources.Load("LuaEnvPrefab");
        //if (pref)
        //    GameObject.Instantiate(pref);
    }

    public void LoadAssetbundle(string path)
    {
        StartCoroutine(corotueLoadAsset(path));
    }

    IEnumerator corotueLoadAsset(string path)
    {
        WWW Import = new WWW("file://" + path);
        yield return Import;
        if (Import.error != null)
        {
            Debug.LogError(Import.error);
        }

        var bundle = Import.assetBundle;
        if (BundleLoadFinished != null)
            BundleLoadFinished(path,bundle);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
