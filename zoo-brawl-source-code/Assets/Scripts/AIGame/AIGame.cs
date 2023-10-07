using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class AIGame : MonoBehaviour
{
    // Class that manages player dropping cards to their zone
    private PlayerDropZone playerDropZone;

    // Utility classes
    AIGameCardGlowManager cardGlowManager;
    BattleHandler battleHandler;
    AIGameAbilityProcessor abilityProcessor;
    AIOpponent aiOpponent;
    UpdateCardDisplayHandler cardDisplayHandler;

    const string player = "player";
    const string opponent = "opponent";

    private void Awake()
    {
        // Initialise utility classes
        cardDisplayHandler = new UpdateCardDisplayHandler();
        cardGlowManager = new AIGameCardGlowManager();
        battleHandler = new BattleHandler();
        abilityProcessor = new AIGameAbilityProcessor();
        aiOpponent = GetComponent<AIOpponent>();

    }

    void Start()
    {
        // Subscribes to events that is raised when the player play cards, and attacks with their cards
        playerDropZone = FindObjectOfType<PlayerDropZone>();
        playerDropZone.CardAddedToZone += OnCardAddedToPlayerZone;

        InteractDraggable.CardsBattle += OnCardsBattle;
        InteractDraggable.AttackBase += OnAttackBase;

        // Subscribes to event that is raised when AI ends turn
        aiOpponent.AIEndTurn += AIEndTurn;

        StartGameSetup();
    }

    void Update()
    {

    }

    void StartGameSetup()
    {
        // Sets state of game to starting state
        AIGameObjectManager.winPopupObj.SetActive(false);

        AIGameState.playerHandCards.Clear();
        AIGameState.opponentHandCards.Clear();

        for (int i = 0; i < 6; i++)
        {
            AIGameState.playerHandPlayable[i] = false;
            AIGameState.opponentHandPlayable[i] = false;
        }

        for (int i = 0; i < 7; i++)
        {
            AIGameState.playerZoneCards[i] = 0;
            AIGameState.opponentZoneCards[i] = 0;

            AIGameState.playerZoneAnimalAwake[i] = 0;
            AIGameState.opponentZoneAnimalAwake[i] = 0;
        }

        AIGameState.turnCounter = 1;

        AIGameState.playerBaseHealth = 20;
        AIGameState.opponentBaseHealth = 20;

        AIGameObjectManager.playerBaseTextObj.text = AIGameState.playerBaseHealth.ToString();
        AIGameObjectManager.opponentBaseTextObj.text = AIGameState.opponentBaseHealth.ToString();

        AIGameState.playerCoins = (int)(Math.Ceiling((double)AIGameState.turnCounter / 2) + 2);
        AIGameState.opponentCoins = (int)(Math.Ceiling((double)AIGameState.turnCounter / 2) + 2);

        AIGameObjectManager.playerCoinAmountObj.text = AIGameState.playerCoins.ToString();
        AIGameObjectManager.opponentCoinAmountObj.text = AIGameState.opponentCoins.ToString();

        // Shuffle player and AI's deck
        ShuffleDeck(player);
        ShuffleDeck(opponent);

        // Adds three initial cards to players and AIs hand
        for (int i = 0; i < 3; i++)
        {
            DrawCard(player);
            DrawCard(opponent);
        }

        // Update all card displays and glow effects
        cardDisplayHandler.UpdateHandDisplay(player);
        cardDisplayHandler.UpdateHandDisplay(opponent);

        cardDisplayHandler.UpdateZoneDisplay(player);
        cardDisplayHandler.UpdateZoneDisplay(opponent);

        cardGlowManager.UpdateZoneGlow(player);
        cardGlowManager.UpdateZoneGlow(opponent);

        cardGlowManager.UpdateHandGlow(player);
        cardGlowManager.UpdateHandGlow(opponent);

        // Randomly selects who goes first
        RandomPlayerTurn();
    }

    // Shuffles deck using Fisher-Yates (Knuth) shuffling algorithm
    public void ShuffleDeck(string player)
    {
        List<Card> deckCards;

        if (player == "player")
        {
            deckCards = AIGameState.playerDeckCards;
        }
        else
        {
            deckCards = AIGameState.opponentDeckCards;
        }

        System.Random rng = new System.Random();

        int cardNum = deckCards.Count;
        while (cardNum > 1)
        {
            cardNum--;
            int randSlot = rng.Next(cardNum + 1);
            Card temp = deckCards[randSlot];
            deckCards[randSlot] = deckCards[cardNum];
            deckCards[cardNum] = temp;
        }
    }

    // Method that draws a card for the player or AI
    public void DrawCard(string currPlayer)
    {
        List<Card> handCards;
        List<Card> deckCards;

        if (currPlayer == player)
        {
            handCards = AIGameState.playerHandCards;
            deckCards = AIGameState.playerDeckCards;
        }
        else
        {
            handCards = AIGameState.opponentHandCards;
            deckCards = AIGameState.opponentDeckCards;
        }

        // Only draws if hand is not full and if deck has any cards left
        if (handCards.Count < 6 && deckCards.Count != 0)
        {
            handCards.Add(deckCards[0]);
            deckCards.RemoveAt(0);
        }

        // Updates hand display and glow effect
        cardDisplayHandler.UpdateHandDisplay(currPlayer);
        cardGlowManager.UpdateHandGlow(currPlayer);
    }

    // Method that randomly selects which player goes first
    void RandomPlayerTurn()
    {
        System.Random rnd = new System.Random();

        int coin = rnd.Next(2);

        if (coin == 0)
        {
            AIGameState.turn = "player";
            AIGameObjectManager.logTextObj.text = "Player goes first!";
            AIGameObjectManager.turnTextObj.text = "Players turn";
            AIGameObjectManager.turnTextObj.color = Color.green;

            // Disables glow effect of the AI if player goes first
            cardGlowManager.DisableZoneGlow(opponent);
            cardGlowManager.DisableHandGlow(opponent);

            OnTurnStart(player);
        }
        else
        {
            AIGameState.turn = "opponent";
            AIGameObjectManager.logTextObj.text = "Opponent goes first!";
            AIGameObjectManager.turnTextObj.text = "Opponents turn";
            AIGameObjectManager.turnTextObj.color = Color.yellow;

            // Disables glow effect of the player if AI goes first
            cardGlowManager.DisableZoneGlow(player);
            cardGlowManager.DisableHandGlow(player);

            OnTurnStart(opponent);
        }
    }

    // Setup for when player ends turn
    public void PlayerEndTurn()
    {
        AIGameState.turnCounter++;

        AIGameState.turn = "opponent";
        AIGameObjectManager.turnTextObj.text = "Opponents turn";
        AIGameObjectManager.turnTextObj.color = Color.yellow;

        AIGameObjectManager.logTextObj.text = "Player ended turn";

        // Disable glow effect of player
        cardGlowManager.DisableZoneGlow(player);
        cardGlowManager.DisableHandGlow(player);
        OnTurnStart(opponent);
    }

    // Setup for when AI ends turn
    private void AIEndTurn()
    {
        AIGameState.turnCounter++;

        AIGameState.turn = "player";
        AIGameObjectManager.turnTextObj.text = "Players turn";
        AIGameObjectManager.turnTextObj.color = Color.green;

        AIGameObjectManager.logTextObj.text = "AI ended turn";

        // Disable glow effect of player
        cardGlowManager.DisableZoneGlow(opponent);
        cardGlowManager.DisableHandGlow(opponent);
        OnTurnStart(player);
    }

    // Setup for when a turn starts
    public void OnTurnStart(string currPlayer)
    {
        int coinValue;
        int[] zoneAnimalAwake;

        // The amount of coins starts at 3 for player and AI increasing by 1 each turn (refreshes each turn)
        coinValue = (int)(Math.Ceiling((double)AIGameState.turnCounter / 2) + 2);

        if (currPlayer == player)
        {
            AIGameObjectManager.endTurnButtonObj.interactable = true;

            AIGameState.playerCoins = coinValue;
            AIGameObjectManager.playerCoinAmountObj.text = coinValue.ToString();
            zoneAnimalAwake = AIGameState.playerZoneAnimalAwake;
        }
        else
        {
            AIGameState.opponentCoins = coinValue;
            AIGameObjectManager.opponentCoinAmountObj.text = coinValue.ToString();
            zoneAnimalAwake = AIGameState.opponentZoneAnimalAwake;
        }

        DrawCard(currPlayer);

        // Animals can only attack when their awake value is 2, when they are played the value becomes 1 and is set to 2 on the players next turn
        // Value of 4 means that the animal is stunned by a cards effect, and will be able to attack on the turn after
        for (int zone = 0; zone < 7; zone++)
        {
            if (zoneAnimalAwake[zone] == 1)
            {
                zoneAnimalAwake[zone] = 2;
            }
            else if (zoneAnimalAwake[zone] == 4)
            {
                zoneAnimalAwake[zone] = 1;
            }
        }

        // Updates glow effect
        cardGlowManager.UpdateZoneGlow(currPlayer);

        // If the current turn is the opponent, let the AI make decisions on what to do
        if (currPlayer == "opponent")
        {
            AIGameObjectManager.endTurnButtonObj.interactable = false;

            StartCoroutine(aiOpponent.AIStartTurn());
        }
    }

    // When the CardAddedToPlayerZone event is raised, add card to zone
    private void OnCardAddedToPlayerZone(int cardID, int cardZoneNum)
    {
        // Reduce the amount of coins the player has by the cost of card played
        AIGameState.playerCoins -= CardCollection.cardCollection[cardID - 1].GetCardCost();
        AIGameObjectManager.playerCoinAmountObj.text = AIGameState.playerCoins.ToString();

        // Activates the on play ability of the card
        abilityProcessor.OnPlayAbility(player, CardCollection.cardAbility[cardID - 1], cardZoneNum);

        // Updates zone and hand displays
        cardDisplayHandler.UpdateZoneDisplay(player);
        cardDisplayHandler.UpdateZoneDisplay(opponent);

        cardDisplayHandler.UpdateHandDisplay(player);

        //Update card glow effects
        cardGlowManager.UpdateHandGlow(player);
        cardGlowManager.UpdateZoneGlow(player);

        // Displays which card the player played in the log
        AIGameObjectManager.logTextObj.text = "Player played " + CardCollection.cardCollection[cardID - 1].GetCardName();
    }

    // When OnCardsBattle event is raised
    private void OnCardsBattle(int startCardZone, int endCardZone)
    {
        // Uses battlehandler class to handle the battle results
        battleHandler.CardsBattle(startCardZone, endCardZone);
    }

    // When OnAttackBase event is raised
    private void OnAttackBase(string attackingPlayer, int startCardZone)
    {
        // Uses battlehandler class to handle the base attack results
        battleHandler.AttackBase(attackingPlayer, startCardZone);
    }

    // Returns to the main menu, resetting all variables and game objects
    public void ReturnToMenu()
    {
        AIGameState.playerHandCards.Clear();
        AIGameState.opponentHandCards.Clear();

        AIGameState.playerDeckCards.Clear();
        AIGameState.opponentDeckCards.Clear();

        // Clears all stat modifiers
        for (int zone = 0; zone < 7; zone++)
        {
            for (int statModifier = 0; statModifier < 3; statModifier++)
            {
                AIGameState.playerUniversalModifiers[zone, statModifier] = 0;
                AIGameState.opponentUniversalModifiers[zone, statModifier] = 0;
                AIGameState.playerFelineModifiers[zone, statModifier] = 0;
                AIGameState.opponentFelineModifiers[zone, statModifier] = 0;
                AIGameState.playerCanineModifiers[zone, statModifier] = 0;
                AIGameState.opponentCanineModifiers[zone, statModifier] = 0;
                AIGameState.playerUrsidaeModifiers[zone, statModifier] = 0;
                AIGameState.opponentUrsidaeModifiers[zone, statModifier] = 0;
                AIGameState.playerReptiliaModifiers[zone, statModifier] = 0;
                AIGameState.opponentReptiliaModifiers[zone, statModifier] = 0;
                AIGameState.playerDelphindaeModifiers[zone, statModifier] = 0;
                AIGameState.opponentDelphindaeModifiers[zone, statModifier] = 0;
            }
        }

        // Clears all player and AI hand and zone cards
        for (int i = 0; i < 7; i++)
        {
            AIGameState.playerZoneCards[i] = 0;
            AIGameState.opponentZoneCards[i] = 0;

            AIGameState.playerZoneDamageCounter[i] = 0;
            AIGameState.opponentZoneDamageCounter[i] = 0;
        }

        // Removes all objects loaded
        AIGameObjectManager.playerHandCardObjs.Clear();
        AIGameObjectManager.playerZoneCardObjs.Clear();
        AIGameObjectManager.playerHandCardDisplays.Clear();
        AIGameObjectManager.playerZoneCardDisplays.Clear();

        AIGameObjectManager.opponentHandCardObjs.Clear();
        AIGameObjectManager.opponentZoneCardObjs.Clear();
        AIGameObjectManager.opponentHandCardDisplays.Clear();
        AIGameObjectManager.opponentZoneCardDisplays.Clear();

        AIGameObjectManager.playerZones.Clear();
        AIGameObjectManager.opponentZones.Clear();

        // Unsubscribes from events
        InteractDraggable.CardsBattle -= OnCardsBattle;
        InteractDraggable.AttackBase -= OnAttackBase;
        aiOpponent.AIEndTurn -= AIEndTurn;

        // Loads main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
