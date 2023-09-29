using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] float _sleepTime;
    float _sleep;
    [SerializeField] float _slowEffect, _slowTime;
    float _isSlow;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLoad;
    }
    private void Update()
    {
        _sleep -= Time.unscaledDeltaTime;
        _isSlow -= Time.unscaledDeltaTime;
        if (_sleep < 0f)
        {
            Time.timeScale = (_isSlow > 0f) ? _slowEffect : 1f;
        }
    }
    public void SetSlow()
    {
        _isSlow = _slowTime;
    }
    public void SetSleep()
    {
        _sleep = _sleepTime;
        Time.timeScale = 0f;
    }
    private void OnLoad(Scene s, LoadSceneMode m)
    {
        _sleep = 0f;
        _isSlow = 0f;
    }

}
