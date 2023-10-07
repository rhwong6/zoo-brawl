using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleHandler : MonoBehaviour
{
    // Initialises utility classes
    AIGameAbilityProcessor abilityProcessor = new AIGameAbilityProcessor();
    UpdateCardDisplayHandler cardDisplayHandler = new UpdateCardDisplayHandler();
    AIGameCardGlowManager cardGlowManager = new AIGameCardGlowManager();

    const string player = "player";
    const string opponent = "opponent";

    public void CardsBattle(int startCardZone, int endCardZone)
    {
        // Initialises variables to store the card starting the battle, and card receiving from the battle
        Card startCard;
        Card endCard;

        // Stores the appropriate starting and ending cards using the collection database
        if (AIGameState.turn == player)
        {
            startCard = CardCollection.cardCollection[AIGameState.playerZoneCards[startCardZone] - 1];
            endCard = CardCollection.cardCollection[AIGameState.opponentZoneCards[endCardZone] - 1];

            // Uses the CalculateBattleDamage method to calculate the damage counters from the battle and sets the damage to the appropriate damage counter variables
            int[] cardDamages = CalculateBattleDamage(player, startCardZone, endCardZone);

            AIGameState.playerZoneDamageCounter[startCardZone] += cardDamages[0];
            AIGameState.opponentZoneDamageCounter[endCardZone] += cardDamages[1];

            // Sets the attacking cards awake value to 1
            AIGameState.playerZoneAnimalAwake[startCardZone] = 1;

            // Displays the battle in the log
            AIGameObjectManager.logTextObj.text = "Players's " + startCard.GetCardName() + " attacks opponent's " + endCard.GetCardName();
        }
        else
        {
            startCard = CardCollection.cardCollection[AIGameState.opponentZoneCards[startCardZone] - 1];
            endCard = CardCollection.cardCollection[AIGameState.playerZoneCards[endCardZone] - 1];

            // Uses the CalculateBattleDamage method to calculate the damage counters from the battle and sets the damage to the appropriate damage counter variables
            int[] cardDamages = CalculateBattleDamage(opponent, startCardZone, endCardZone);

            AIGameState.opponentZoneDamageCounter[startCardZone] += cardDamages[0];
            AIGameState.playerZoneDamageCounter[endCardZone] += cardDamages[1];

            // Sets the attacking cards awake value to 1
            AIGameState.opponentZoneAnimalAwake[startCardZone] = 1;
        }

        // Uses the UpdateAfterAttack method to remove any cards that are destroyed after the battle
        UpdateAfterAttack(startCardZone, endCardZone, startCard, endCard);
    }

    private int[] CalculateBattleDamage(string currPlayer, int startCardZone, int endCardZone)
    {
        // Initialises variables to store the card display starting the battle, and card display receiving from the battle
        AIGameZoneDisplay startCard;
        AIGameZoneDisplay endCard;

        // Stores the appropriate starting and ending card displays
        if (currPlayer == player)
        {
            startCard = AIGameObjectManager.playerZoneCardDisplays[startCardZone];
            endCard = AIGameObjectManager.opponentZoneCardDisplays[endCardZone];
        }
        else
        {
            startCard = AIGameObjectManager.opponentZoneCardDisplays[startCardZone];
            endCard = AIGameObjectManager.playerZoneCardDisplays[endCardZone];
        }

        // Uses the abilityprocessor class to store any attack modifiers if the card has an on attack ability
        Tuple<int[,], int[,]> attackModifiers = abilityProcessor.OnAttack(currPlayer, startCard, endCard, CardCollection.cardAbility[startCard.id - 1]);

        // Stores the attacker and defender modifiers
        int[,] attackerModifiers = attackModifiers.Item1;
        int[,] defenderModifiers = attackModifiers.Item2;

        // Stores the attack and armour values of the attacking card including the attack modifiers
        int startCardAttack = startCard.cardAttack + attackerModifiers[0, 0];
        int startCardArmour = startCard.cardArmour + attackerModifiers[1, 1];

        // Stores the attack and armour values of the defending card including the defender modifiers
        int endCardAttack = endCard.cardAttack;
        int endCardArmour = endCard.cardArmour + defenderModifiers[1, 1];

        // Initialises the damage each card receives from the battle
        int startCardDamage = 0;
        int endCardDamage = 0;

        // Calculates the damage each card takes taking into account armour
        if (startCardArmour < endCardAttack)
        {
            startCardDamage = endCardAttack - startCardArmour;
        }

        if (endCardArmour < startCardAttack)
        {
            endCardDamage = startCardAttack - endCardArmour;
        }

        // Returns the damage each card takes
        int[] damage = { startCardDamage, endCardDamage };
        return damage;
    }

    private void UpdateAfterAttack(int startCardZone, int endCardZone, Card startCard, Card endCard)
    {
        // Initialises variables to store the zone cards, damage counters, card displays, awake array, and player starting the battle and player receiving the battle
        int[] startZoneCards;
        int[] startZoneDamageCounter;

        int[] endZoneCards;
        int[] endZoneDamageCounter;

        List<AIGameZoneDisplay> startZoneCardDisplays;
        List<AIGameZoneDisplay> endZoneCardDisplays;

        int[] startZoneAnimalAwake;
        int[] endZoneAnimalAwake;

        string startPlayer;
        string endPlayer;

        // Appropriately sets these variables
        if (AIGameState.turn == player)
        {
            startZoneCards = AIGameState.playerZoneCards;
            startZoneDamageCounter = AIGameState.playerZoneDamageCounter;

            endZoneCards = AIGameState.opponentZoneCards;
            endZoneDamageCounter = AIGameState.opponentZoneDamageCounter;

            startZoneCardDisplays = AIGameObjectManager.playerZoneCardDisplays;
            endZoneCardDisplays = AIGameObjectManager.opponentZoneCardDisplays;

            startZoneAnimalAwake = AIGameState.playerZoneAnimalAwake;
            endZoneAnimalAwake = AIGameState.opponentZoneAnimalAwake;

            startPlayer = player;
            endPlayer = opponent;
        }
        else
        {
            startZoneCards = AIGameState.opponentZoneCards;
            startZoneDamageCounter = AIGameState.opponentZoneDamageCounter;

            endZoneCards = AIGameState.playerZoneCards;
            endZoneDamageCounter = AIGameState.playerZoneDamageCounter;

            startZoneCardDisplays = AIGameObjectManager.opponentZoneCardDisplays;
            endZoneCardDisplays = AIGameObjectManager.playerZoneCardDisplays;

            startZoneAnimalAwake = AIGameState.opponentZoneAnimalAwake;
            endZoneAnimalAwake = AIGameState.playerZoneAnimalAwake;

            startPlayer = opponent;
            endPlayer = player;
        }

        // If the damage counter of the card starting the attack is higher than that cards health (including modifiers), remove that card from the zone, activate its on death effect, and update the zone displays
        if (startZoneDamageCounter[startCardZone] >= startCard.GetCardHealth() + startZoneCardDisplays[startCardZone].cardHealthModifier)
        {
            abilityProcessor.OnDeath(startPlayer, CardCollection.cardAbility[startZoneCards[startCardZone] - 1], startCardZone);
            startZoneCards[startCardZone] = 0;
            startZoneDamageCounter[startCardZone] = 0;
            cardDisplayHandler.UpdateZoneDisplay(startPlayer);
            cardDisplayHandler.UpdateZoneDisplay(endPlayer);

            startZoneAnimalAwake[startCardZone] = 0;
        }
        else
        {
            // Else, if the card starting the attack is not destroyed, displays the new card attributes
            startZoneCardDisplays[startCardZone].loadCardAttributes(startPlayer, startCard, startCardZone);
        }

        // If the damage counter of the card receiving the attack is higher than that cards health (including modifiers), remove that card from the zone, activate its on death effect, and update the zone displays
        if (endZoneDamageCounter[endCardZone] >= endCard.GetCardHealth() + endZoneCardDisplays[endCardZone].cardHealthModifier)
        {
            abilityProcessor.OnDeath(endPlayer, CardCollection.cardAbility[endZoneCards[endCardZone] - 1], endCardZone);
            endZoneCards[endCardZone] = 0;
            endZoneDamageCounter[endCardZone] = 0;
            cardDisplayHandler.UpdateZoneDisplay(endPlayer);
            cardDisplayHandler.UpdateZoneDisplay(startPlayer);

            endZoneAnimalAwake[endCardZone] = 0;
        }
        else
        {
            // Else, if the card receiving the attack is not destroyed, displays the new card attributes
            endZoneCardDisplays[endCardZone].loadCardAttributes(endPlayer, endCard, endCardZone);
        }

        // Updates the card glow effects
        cardGlowManager.UpdateZoneGlow(AIGameState.turn);
    }

    public void AttackBase(string attackingPlayer, int startCardZone)
    {
        // Changes the appropriate base health variables depending on which player is attacking using the cards attack, displays it in the log, and if the players base is destroyed, calls the PlayerWin method
        if (attackingPlayer == player)
        {
            AIGameState.opponentBaseHealth -= AIGameObjectManager.playerZoneCardDisplays[startCardZone].cardAttack;
            AIGameObjectManager.opponentBaseTextObj.text = AIGameState.opponentBaseHealth.ToString();

            AIGameState.playerZoneAnimalAwake[startCardZone] = 1;
            cardGlowManager.UpdateZoneGlow(player);

            AIGameObjectManager.logTextObj.text = "Players " + CardCollection.cardCollection[AIGameState.playerZoneCards[startCardZone] - 1].GetCardName() + " attacks opponents base";

            if (AIGameState.opponentBaseHealth <= 0)
            {
                PlayerWin(attackingPlayer);
            }
        }
        else
        {
            AIGameState.playerBaseHealth -= AIGameObjectManager.opponentZoneCardDisplays[startCardZone].cardAttack;
            AIGameObjectManager.playerBaseTextObj.text = AIGameState.playerBaseHealth.ToString();

            AIGameState.opponentZoneAnimalAwake[startCardZone] = 1;
            cardGlowManager.UpdateZoneGlow(opponent);

            if (AIGameState.playerBaseHealth <= 0)
            {
                PlayerWin(attackingPlayer);
            }
        }
    }

    private void PlayerWin(string currPlayer)
    {
        // Changes the text for which player wins, and displays the win popup object
        if (currPlayer == player)
        {
            AIGameObjectManager.wonPlayerTextObj.text = "Player wins!";
        }
        else
        {
            AIGameObjectManager.wonPlayerTextObj.text = "AI wins!";
        }
        AIGameObjectManager.winPopupObj.SetActive(true);
    }
}
