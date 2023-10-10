using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public GameStart gameStart;
    public Gameplay gameplay;
    public RoomManager roomManager;
    public DisplayingText displayText;
    public ButtonInput buttonInput;
    public void GivePlayerEXP()
    {
        displayText.enemyStats = "";
        int alternateRoomCheck = Random.Range(0, 20);
        int gainedXP = Random.Range(1, (int)(gameStart.playerMaxExperience / 1.3f));//Rolls to give player exp
        gameStart.playerExperience += gainedXP;//Adds the gained xp to player's current xp count
        displayText.dialogue = "You gained " + gainedXP + " EXP!";
        Debug.Log("You gained " + gainedXP + " EXP!");
        if (gameStart.playerExperience >= gameStart.playerMaxExperience)//Checks to see if player's current xp is greater or equal to the required xp to level
        {
            StartCoroutine(PlayerLevelUpWaiter());
        }
        else if (alternateRoomCheck > 12)
        {
            StartCoroutine(AlternateRoomWaiter());
        }
        else
        {
            StartCoroutine(GameplayRestartWaiter());
        }
        IEnumerator PlayerLevelUpWaiter()//These Ienumerators are for delayed text because I couldn't figure out how else to do them :(
        {
            yield return new WaitForSecondsRealtime(2);
            PlayerLevelUp();
        }
        IEnumerator GameplayRestartWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            gameplay.GameplayStart();
        }
        IEnumerator AlternateRoomWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            roomManager.AlternateRooms();
        }
    }
    public void PlayerLevelUp()
    {
        RemoveListeners();
        gameStart.playerExperience = 0;//Sets player's current xp back to zero
        gameStart.playerLevel++;//Increases level by 1
        gameStart.playerMaxExperience = (int)(gameStart.playerMaxExperience * 1.4f);//Increases max exp so it takes longer to level
        int strIncrease;
        int dexIncrease;
        int intIncrease;
        if (gameStart.playerLevel == 5)//If the player is level 5, they win the game
        {
            gameStart.Victory();
            return;
        }
        else
        {
            strIncrease = Mathf.CeilToInt(gameStart.strength * 1.25f);//Multiplies strength/dex/int to increase the stat
            dexIncrease = Mathf.CeilToInt(gameStart.dexterity * 1.3f);
            intIncrease = Mathf.CeilToInt(gameStart.intelligence * 1.35f);
            gameStart.strength = strIncrease;
            gameStart.dexterity = dexIncrease;
            gameStart.intelligence = intIncrease;
            Debug.Log("You leveled up!\nStrength increased to " + strIncrease + "\nDexterity increased to " + dexIncrease + "\nIntelligence increases to " + intIncrease);
            displayText.dialogue = "You leveled up!" + "\nStrength increased to " + strIncrease + "\nDexterity increased to " + dexIncrease + "\nIntelligence increases to " + intIncrease;
            displayText.bLText = "";
            displayText.bMText = "Ok";
            displayText.bRText = "";
            buttonInput.buttonMClick.onClick.AddListener(Next);
        }

        void Next()
        {
            int alternateRoomCheck = Random.Range(0, 20);
            gameStart.currentHealth = gameStart.maxHealth;//heals the player to full after a level for QoL sake
            if (alternateRoomCheck > 12)
            {
                roomManager.AlternateRooms();
            }
            else
            {
                gameplay.GameplayStart();
            }
        }
    }
    public void RemoveListeners()//Removes lingering button functions to reassign them in other functions, probably could of done this easier but oh well
    {
        buttonInput.buttonLClick.onClick.RemoveAllListeners();
        buttonInput.buttonMClick.onClick.RemoveAllListeners();
        buttonInput.buttonRClick.onClick.RemoveAllListeners();
    }
}
