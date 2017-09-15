using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[LuaCallCSharp]
public class Score : MonoBehaviour {

    public EventTrigger trig;
    public Text txt;
    public Text over;
    public Image fade;
    public CanvasGroup group;
    public Image hurtImage;
    public bool gameover = false;
    public float hurt = 0.0f;
    public static Score scoreInstance = null;
    public Slider HealthBar;
    public Text ScoreText;
    private int score = 0;

    private void Awake()
    {
        scoreInstance = this;
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (gameover)
        {

            Color backcolordst = fade.color;
            Color overcolordst = over.color;
            float alpha = Mathf.Lerp(backcolordst.a, 1.0f, 0.2f);
            backcolordst.a = alpha;
            overcolordst.a = alpha;
            fade.color = backcolordst;
            over.color = overcolordst;
            group.blocksRaycasts = true;
        }
        hurt = Mathf.Lerp(hurt, 0.0f, 0.3f);
        Color hurtcolordst = hurtImage.color;
        hurtcolordst.a = Mathf.Lerp(hurtcolordst.a, hurt, 0.5f);
        hurtImage.color = hurtcolordst;
    }
    public void AddScore(int Score)
    {
        score += Score;
        txt.text = "score:" + score;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
