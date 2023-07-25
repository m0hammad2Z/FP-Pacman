using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera pacmanCamera;
    [SerializeField] PacmanManager pacmanManager;

    VisualElement menuC;
    VisualElement gameC;
    VisualElement lostC;
    VisualElement winC;

    Button startButton;
    Button[] restartButtons = new Button[2];
    Button[] quitButtons = new Button[3];

    Label score;
    Label hightScore;
    Label lives;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        menuC = root.Q<VisualElement>("MenuContainer");
        gameC = root.Q<VisualElement>("GameContainer");
        lostC = root.Q<VisualElement>("LoseContainer");
        winC = root.Q<VisualElement>("WinContainer");

        score = gameC.Q<VisualElement>("TOP_ELEMENTS").Q<Label>("Score");
        hightScore = gameC.Q<VisualElement>("TOP_ELEMENTS").Q<Label>("HightScore");
        lives = gameC.Q<VisualElement>("BOTTOM_ELEMENTS").Q<VisualElement>("PacmanIcon").Q<Label>("Lives");

        startButton = menuC.Q<VisualElement>("Buttons").Q<Button>("PlayButton");

        restartButtons[0] = lostC.Q<VisualElement>("Buttons").Q<Button>("RestartButton");
        restartButtons[1] = winC.Q<VisualElement>("Buttons").Q<Button>("RestartButton");

        quitButtons[0] = menuC.Q<VisualElement>("Buttons").Q<Button>("QuitButton");
        quitButtons[1] = lostC.Q<VisualElement>("Buttons").Q<Button>("QuitButton");
        quitButtons[2] = winC.Q<VisualElement>("Buttons").Q<Button>("QuitButton");

        HideOrShow(menuC, true);
        HideOrShow(gameC, false);
        HideOrShow(lostC, false);
        HideOrShow(winC, false);
       
        pacmanCamera.Priority = 9;

        startButton.clicked += () => 
        {
            HideOrShow(gameC, true);
            HideOrShow(menuC, false);
            HideOrShow(lostC, false);
            HideOrShow(winC, false);
           
            pacmanCamera.Priority = 11;

            PacmanManager.isDie = false;
            PacmanManager.isWin = false;
            PacmanManager.lives = 100;
            PacmanManager.score = 0;
            PacmanManager.dashes = 6;
            pacmanManager.SetActiveGhosts();
        };

        foreach(Button button in restartButtons)
        {
            button.clicked += () =>
            {
                HideOrShow(menuC, true);
                HideOrShow(gameC, false);
                HideOrShow(lostC, false);
                HideOrShow(winC, false);

                pacmanCamera.Priority = 9;
                pacmanCamera.m_Lens.Dutch = 0;
            };
        }

        foreach (Button button in quitButtons)
        {
            button.clicked += () => { Application.Quit(); };
        }


    }

    private void Update()
    {

        score.text = "score: " + PacmanManager.score.ToString();
        hightScore.text = "hight score: " + PacmanManager.hightScore.ToString();
        lives.text = PacmanManager.lives.ToString();

        if(PacmanManager.isDie && menuC.style.display == DisplayStyle.None)
        {
            pacmanCamera.m_Lens.Dutch = Mathf.Lerp(pacmanCamera.m_Lens.Dutch, 60, Time.deltaTime * 2.5f);
            if(pacmanCamera.m_Lens.Dutch > 20)
            {
                HideOrShow(menuC, false);
                HideOrShow(gameC, false);
                HideOrShow(lostC, true);
                HideOrShow(winC, false);
            }
        }

        if (PacmanManager.isWin && menuC.style.display == DisplayStyle.None && lostC.style.display == DisplayStyle.None)
        {
            //Replace this with win menu.
            HideOrShow(menuC, false);
            HideOrShow(gameC, false);
            HideOrShow(lostC, false);
            HideOrShow(winC, true);
        }
    }

    void HideOrShow(VisualElement element, bool show)
    {
        if (show)
        {
            element.style.display = DisplayStyle.Flex;
        }
        else
        {
            element.style.display = DisplayStyle.None;
        }
    }

}
