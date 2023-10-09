using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameStart gameStart;
    public Gameplay gameplay;
    public XPManager xpManager;
    public DisplayingText displayText;
    public ButtonInput buttonInput;
    public void AlternateRooms()
    {
        RemoveListeners();
        displayText.dialogue = "You come accross a strange person, claiming to grant power for gold. Do you take this offer?\n15 gold per stat increase";
        displayText.bLText = "Yes";
        displayText.bMText = "";
        displayText.bRText = "No";
        buttonInput.buttonLClick.onClick.AddListener(Buy);
        buttonInput.buttonRClick.onClick.AddListener(Leave);

        void Buy()
        {
            RemoveListeners();
            displayText.dialogue = "Which stat would you like to increase by 1?";
            displayText.bLText = "Strength";
            displayText.bMText = "Dexterity";
            displayText.bRText = "Intelligence";
            buttonInput.buttonLClick.onClick.AddListener(StrengthGain);
            buttonInput.buttonMClick.onClick.AddListener(DexGain);
            buttonInput.buttonRClick.onClick.AddListener(IntGain);
            void StrengthGain()
            {
                if (gameStart.gold < 15)
                {
                    displayText.dialogue = "You do not have enough gold, goodbye.";
                    StartCoroutine(LeaveWaiter());
                }
                else
                {
                    gameStart.strength++;
                    gameStart.gold -= 15;
                }
            }
            void DexGain()
            {
                if (gameStart.gold < 15)
                {
                    displayText.dialogue = "You do not have enough gold, goodbye.";
                    StartCoroutine(LeaveWaiter());
                }
                else
                {
                    gameStart.dexterity++;
                    gameStart.gold -= 15;
                }
            }
            void IntGain()
            {
                if (gameStart.gold < 15)
                {
                    displayText.dialogue = "You do not have enough gold, goodbye.";
                    StartCoroutine(LeaveWaiter());
                }
                else
                {
                    gameStart.intelligence++;
                    gameStart.gold -= 15;
                }
            }
        }

        void Leave()
        {
            RemoveListeners();
            displayText.dialogue = "You turn and leave through another door";
            StartCoroutine(GameplayRestartWaiter());
        }
        IEnumerator LeaveWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            Leave();
        }
        IEnumerator GameplayRestartWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            gameplay.GameplayStart();
        }
    }
    public void RemoveListeners()//Removes lingering button functions to reassign them in other functions, probably could of done this easier but oh well
    {
        buttonInput.buttonLClick.onClick.RemoveAllListeners();
        buttonInput.buttonMClick.onClick.RemoveAllListeners();
        buttonInput.buttonRClick.onClick.RemoveAllListeners();
    }
}
