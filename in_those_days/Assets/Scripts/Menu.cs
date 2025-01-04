using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public CanvasGroup ButtonGroup;
    public float ButtonAlphaTime = 0.4f;
    private float _buttonAlphaTimer = 0;
    private bool _buttonsDone = true;
    private bool _started = false;
    public string NextScene;

    public UnityEvent Started;
    public UnityEvent ButtonsGone;

    public void Update()
    {
        if (!_started)
        {
            Started.Invoke();
            _started = true;
        }
        if(_buttonAlphaTimer > 0)
        {
            _buttonAlphaTimer -= Time.deltaTime;
            ButtonGroup.alpha = _buttonAlphaTimer / ButtonAlphaTime;
        }
        if(_buttonAlphaTimer < 0)
        {
            _buttonAlphaTimer = 0;
        }
        if(_buttonAlphaTimer == 0 && !_buttonsDone)
        {
            _buttonsDone = true;
            ButtonsGone.Invoke();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(NextScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGameTransition()
    {
        if (ButtonGroup.interactable)
        {
            _buttonsDone = false;
            ButtonGroup.interactable = false;
            _buttonAlphaTimer = ButtonAlphaTime;
        }
    }
}
