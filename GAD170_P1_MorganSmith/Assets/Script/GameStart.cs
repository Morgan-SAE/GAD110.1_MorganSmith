using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public Gameplay gameplay;
    public XPManager xpManager;
    public RoomManager roomManager;
    public DisplayingText displayText;
    public ButtonInput buttonInput;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int statPool;
    public int playerLevel;
    public int playerMaxExperience;
    public int playerExperience;
    public int maxHealth;
    public int currentHealth;
    public int gold;
    // Start is called before the first frame update
    void Start()
    {
        RemoveListeners();
        //Rolls stats based on stat pool remaining
        statPool = 20;

        strength = Random.Range(1, statPool);
        statPool -= strength;

        dexterity = Random.Range(0, statPool);
        statPool -= dexterity;

        intelligence = Random.Range(0, statPool);
        statPool -= intelligence;
        maxHealth = strength * 2;
        currentHealth = maxHealth;
        playerLevel = 1;
        playerMaxExperience = 10;
        playerExperience = 0;
        gold = 0;
        if (statPool > 0)//Lets player choose to use remaining stat points
        {
            displayText.dialogue = "Would you like to distribute more stats with remaining Stat points?\n" + statPool + " remaining Stat points";
            displayText.bLText = "Yes";
            displayText.bMText = "";
            displayText.bRText = "No";
            buttonInput.buttonLClick.onClick.AddListener(StatAllocation);
        }
        else
        {
            buttonInput.buttonMClick.onClick.AddListener(gameplay.GameplayStart);
        }
    }
    // Update is called once per frame
    void Update()
    {
        maxHealth = strength * 2;
        if (statPool > 0)//Checks to see if there are stat points left and displays how many are left
        {
            displayText.playerStats = "Level: " + playerLevel + "\nEXP: " + playerExperience + "/" + playerMaxExperience + "\nStrength: " + strength + "\nDexterity: " + dexterity + "\nIntelligence: " + intelligence + "\nHealth: " + currentHealth + "/" + maxHealth + "\nGold: " + gold + "\nStat points: " + statPool;
        }
        else
        {
            displayText.playerStats = "Level: " + playerLevel + "\nHealth: " + currentHealth + "/" + maxHealth + "\nEXP: " + playerExperience + "/" + playerMaxExperience + "\nStrength: " + strength + "\nDexterity: " + dexterity + "\nIntelligence: " + intelligence + "\nGold: " + gold;
        }

        if (currentHealth <= 0)//Makes sure you lose fi you have no health left
        {
            GameOver();
        }

        if(playerExperience > playerMaxExperience)//Makes sure that your current exp doesn't exceed max exp
        {
            playerExperience = playerMaxExperience;
        }
    }



    #region
    public void StatAllocation()
    {
        //Adding stats from left over stat pool
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
            strength++;
            statPool--;
            if (statPool < 1)
            {
                currentHealth = maxHealth;
                gameplay.GameplayStart();
            }
        }
        void DexGain()
        {
            dexterity++;
            statPool--;
            if (statPool < 1)
            {
                currentHealth = maxHealth;
                gameplay.GameplayStart();
            }
        }
        void IntGain()
        {
            intelligence++;
            statPool--;
            if (statPool < 1)
            {
                currentHealth = maxHealth;
                gameplay.GameplayStart();
            }
        }
    }
    #endregion


    public void RemoveListeners()//Removes lingering button functions to reassign them in other functions, probably could of done this easier but oh well
    {
        buttonInput.buttonLClick.onClick.RemoveAllListeners();
        buttonInput.buttonMClick.onClick.RemoveAllListeners();
        buttonInput.buttonRClick.onClick.RemoveAllListeners();
    }

    
    public void GameOver()
    {
        RemoveListeners();
        displayText.dialogue = "You Have died.\n Would you like to restart?";
        displayText.bLText = "";
        displayText.bMText = "Yes";
        displayText.bRText = "";
        buttonInput.buttonMClick.onClick.AddListener(Start);
    }

    public void Victory()
    {
        displayText.dialogue = "Congratulations!\nYou have hit level 5\nWhich means you win :)\nWould you like to restart?";
        displayText.bLText = "";
        displayText.bMText = "Yes";
        displayText.bRText = "";
        buttonInput.buttonMClick.onClick.AddListener(Start);
    }
}
