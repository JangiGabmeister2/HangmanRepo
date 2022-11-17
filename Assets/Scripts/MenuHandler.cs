using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelStates { MenuPanel, GamePanel, PausePanel }

public enum GameStates { MenuState, GameState }

public class MenuHandler : MonoBehaviour
{
    private static MenuHandler _menuHandler;

    public MenuHandler menuHandlerInstance
    {
        get => _menuHandler;
        private set
        {
            if (_menuHandler == null)
            {
                _menuHandler = value;
            }
            else
            {
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        menuHandlerInstance = this;
    }

    public GameObject[] panels;

    public PanelStates panelStates;
    public GameStates gameStates;

    public void ChangePanel(int value)
    {
        panelStates = (PanelStates)value;

        switch (panelStates)
        {
            case PanelStates.MenuPanel:
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }
                panels[0].SetActive(true);
                break;
            case PanelStates.GamePanel:
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }
                panels[1].SetActive(true);
                break;
            case PanelStates.PausePanel:
                panels[2].SetActive(true);
                break;
            default:
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }
                panels[1].SetActive(true);
                break;
        }
    }

    public void NextState()
    {
        switch (gameStates)
        {
            case GameStates.MenuState:
                StartCoroutine(MenuState());
                break;
            case GameStates.GameState:
                StartCoroutine(GameState());
                break;
            default:
                StartCoroutine(MenuState());
                break;
        }
    }

    public void PlayButton()
    {
        gameStates = GameStates.GameState;

        NextState();
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ResumeButton()
    {
        PlayButton();
    }

    public void RestartButton()
    {
        //NewGame();
    }

    private void Start()
    {
        gameStates = GameStates.MenuState;

        ChangePanel(0);
        NextState();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (gameStates == GameStates.GameState)
            {
                gameStates = GameStates.MenuState;

                ChangePanel(2);
                NextState(); 
            }
        }
    }

    IEnumerator MenuState()
    {
        yield return null;
    }

    IEnumerator GameState()
    {
        ChangePanel(1);

        yield return null;
    }

}
