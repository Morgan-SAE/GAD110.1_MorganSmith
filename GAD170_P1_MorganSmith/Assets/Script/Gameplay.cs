using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public GameStart gameStart;
    public XPManager xpManager;
    public RoomManager roomManager;
    public DisplayingText displayText;
    public ButtonInput buttonInput;
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public int enemyStrength;
    public int enemyDexterity;
    public int enemyDamage;
    public int enemyMinDamage;
    public int enemyMaxDamage;
    public int enemyChanceToHit;
    public int enemyLevel;
    public int enemyLevelRange;
    public int playerDamage;
    public int playerMinDamage;
    public int playerMaxDamage;
    public int playerHeal;
    public int playerMinHeal;
    public int playerMaxHeal;
    public int playerChanceToHit;
    // Start is called before the first frame update
    public void GameplayStart()
    {
        RemoveListeners();
        enemyLevelRange = Mathf.CeilToInt(gameStart.playerLevel * 2.5f);
        enemyLevel = Random.Range(gameStart.playerLevel, enemyLevelRange);
        enemyStrength = 4 * enemyLevel;
        enemyDexterity = 3 * enemyLevel / 2;                            //<---Rolling of enemy stats
        enemyMaxHealth = (int)(enemyStrength * 1.2f);
        enemyCurrentHealth = enemyMaxHealth;
        Debug.Log("Enemy Dex: " + enemyDexterity);
        Debug.Log("Enemy Strength: " + enemyStrength);
        Debug.Log("Enemy Health: " + enemyMaxHealth);


        displayText.dialogue = "You are standing in a room, with a door infront of you\nDo you wish to enter?";
        displayText.bLText = "Yes";
        displayText.bMText = "";
        displayText.bRText = "No";
        buttonInput.buttonLClick.onClick.AddListener(StartCombat);
        buttonInput.buttonRClick.onClick.AddListener(Wait);

        void StartCombat()
        {
            RemoveListeners();

            displayText.enemyStats = "Goblin" + "\nLevel: " + enemyLevel + "\nHealth: " + enemyCurrentHealth + "/" + enemyMaxHealth + "\nStrength: " + enemyStrength + "\nDexterity: " + enemyDexterity;

            Debug.Log("A goblin has appeared!");
            displayText.dialogue = "You find a goblin, looking to attack you, what do you do?";
            displayText.bLText = "Attack";
            displayText.bMText = "Fireblast";
            displayText.bRText = "Heal";
            buttonInput.buttonLClick.onClick.AddListener(PlayerAttack);
            buttonInput.buttonMClick.onClick.AddListener(PlayerSpell);
            buttonInput.buttonRClick.onClick.AddListener(PlayerHeal);
        }

        void Wait()
        {

        }

        void PlayerAttack()
        {
            RemoveListeners();
            playerChanceToHit = Random.Range(0, 50);//Rolls number to hit goblin
            if (playerChanceToHit < enemyDexterity)//Checks if the number rolled is lower than enemy dex, if so, the attack misses
            {
                displayText.dialogue = "You missed the goblin.";
                StartCoroutine(EnemyAttackWaiter());
            }
            else
            {
                playerMinDamage = Mathf.FloorToInt(gameStart.strength * 0.5f);//Rolls damage number, damage based on strength
                playerMaxDamage = Mathf.CeilToInt(gameStart.strength * 1.5f);
                playerDamage = Random.Range(playerMinDamage, playerMaxDamage);
                displayText.dialogue = "You deal " + playerDamage + " damage to the goblin.";
                enemyCurrentHealth -= playerDamage;
                if (enemyCurrentHealth < 0)
                {
                    enemyCurrentHealth = 0;
                }
                displayText.enemyStats = "Goblin" + "\nLevel: " + enemyLevel + "\nHealth: " + enemyCurrentHealth + "/" + enemyMaxHealth + "\nStrength: " + enemyStrength + "\nDexterity: " + enemyDexterity;
                Debug.Log("Player Damage: " + playerDamage);
                StartCoroutine(EnemyHealthCheckWaiter());
            }

        }

        void PlayerSpell()
        {
            RemoveListeners();
            playerChanceToHit = Random.Range(0, 50);//Rolls number to hit goblin
            if (playerChanceToHit < enemyDexterity)//Checks if the number rolled is lower than enemy dex, if so, the attack misses
            {
                displayText.dialogue = "You missed the goblin.";
                StartCoroutine(EnemyAttackWaiter());
            }
            else
            {
                playerMinDamage = Mathf.FloorToInt(gameStart.intelligence * 0.5f);//Rolls damage number, damage based off intelligence
                playerMaxDamage = Mathf.CeilToInt(gameStart.intelligence * 2);
                playerDamage = Random.Range(playerMinDamage, playerMaxDamage);
                displayText.dialogue = "You deal " + playerDamage + " damage to the goblin.";
                enemyCurrentHealth -= playerDamage;
                displayText.enemyStats = "Goblin" + "\nLevel: " + enemyLevel + "\nHealth: " + enemyCurrentHealth + "/" + enemyMaxHealth + "\nStrength: " + enemyStrength + "\nDexterity: " + enemyDexterity;
                StartCoroutine(EnemyHealthCheckWaiter());
            }
        }


        void EnemyAttack()
        {
            enemyChanceToHit = Random.Range(0, 50);//Rolls number to hit player
            if (enemyChanceToHit < gameStart.dexterity)//Checks if the number rolled is lower than player dex, if so, the attack misses
            {
                displayText.dialogue = "The goblin missed you.";
            }
            else
            {
                enemyMinDamage = Mathf.FloorToInt(enemyStrength * 0.5f);//Rolls damage number, damage based of enemy strength
                enemyMaxDamage = Mathf.CeilToInt(enemyStrength * 1.5f);
                enemyDamage = Random.Range(enemyMinDamage, enemyMaxDamage);
                displayText.dialogue = "The goblin deals " + enemyDamage + " damage to you.";
                gameStart.currentHealth -= enemyDamage;//Reduces player's current health
            }
            buttonInput.buttonLClick.onClick.AddListener(PlayerAttack);
            buttonInput.buttonMClick.onClick.AddListener(PlayerSpell);
            buttonInput.buttonRClick.onClick.AddListener(PlayerHeal);
        }

        void PlayerHeal()
        {
            playerMinHeal = Mathf.FloorToInt(gameStart.intelligence * 1.5f);//Rolls to heal player health
            playerMaxHeal = Mathf.CeilToInt(gameStart.intelligence * 3);
            playerHeal = Random.Range(playerMinHeal, playerMaxHeal);
            displayText.dialogue = "You gain " + playerHeal + " health.";
            gameStart.currentHealth += playerHeal;
            if (gameStart.currentHealth > gameStart.maxHealth)//Makes sure current health is not more than max health
            {
                gameStart.currentHealth = gameStart.maxHealth;
            }
            StartCoroutine(EnemyAttackWaiter());
        }

        void EnemyHealthCheck()
        {
            if (enemyCurrentHealth <= 0)//Checks to see if enemy is dead to give exp or continue combat
            {
                int goldGain = Random.Range(5, 20);
                gameStart.gold += goldGain;
                displayText.dialogue = "You killed the goblin!\nYou gained " + goldGain + " gold!";
                StartCoroutine(GivePlayerXPWaiter());
            }
            else
            {
                StartCoroutine(EnemyAttackWaiter());
            }
        }
        IEnumerator EnemyAttackWaiter()//These Ienumerators are for delayed text because I couldn't figure out how else to do them  and they wouldn't work properly outside this function :(
        {
            yield return new WaitForSecondsRealtime(2);
            EnemyAttack();
        }

        IEnumerator EnemyHealthCheckWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            EnemyHealthCheck();
        }

        IEnumerator GivePlayerXPWaiter()
        {
            yield return new WaitForSecondsRealtime(2);
            xpManager.GivePlayerEXP();
        }
    }
    public void Wait()
    {
        displayText.dialogue = "You stand around, waiting for nothing";
        displayText.bLText = "Move";
        displayText.bMText = "";
        displayText.bRText = "Wait";
    }

    public void RemoveListeners()//Removes lingering button functions to reassign them in other functions, probably could of done this easier but oh well
    {
        buttonInput.buttonLClick.onClick.RemoveAllListeners();
        buttonInput.buttonMClick.onClick.RemoveAllListeners();
        buttonInput.buttonRClick.onClick.RemoveAllListeners();
    }
}
