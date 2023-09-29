using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideManager : MonoBehaviour
{
    public static SlideManager Instance;
    [SerializeField] TextMeshProUGUI _textField;
    [SerializeField] Animator _animator;
    [SerializeField] KeyCode _next, _previous;
    [SerializeField] string[] _sceneTitles;
    [SerializeField] string[] _sceneNames;
    int _index = 0;
    int a_In = Animator.StringToHash("In");
    int a_Gone = Animator.StringToHash("Gone");
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _textField.text = _sceneTitles[_index];
        SceneManager.sceneLoaded += OnLoad;
    }
    private void Update()
    {
        if (Input.GetKeyDown(_next))
        {
            _index = (_index + 1) % _sceneNames.Length;
            Load();
        }
        else if (Input.GetKeyDown(_previous))
        {
            _index = _index - 1;
            if (_index < 0)
            {
                _index = _sceneNames.Length - 1;
            }
            Load();
        }
    }
    void Load() {
        Pools.Instance?.Recall();
        AudioManager.Instance.PlaySound(AudioManager.SFXType.NextSlide, 1f);
        SceneManager.LoadScene(_sceneNames[_index]);
        _textField.text = "";
        _animator.CrossFade(a_Gone, 0f);
        _animator.CrossFade(a_In, 0f);
    }
    void OnLoad(Scene s, LoadSceneMode m)
    {
        _textField.text = _sceneTitles[_index];
    }
}
