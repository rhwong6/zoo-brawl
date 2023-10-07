using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGameCardGlowManager
{
    private const string player = "player";
    private const string opponent = "opponent";

    public void UpdateZoneGlow(string currPlayer)
    {
        // Selects the appropriate animal awake array and zone card display list to reference depending on which player was in the argument
        int[] zoneAnimalAwake;
        List<AIGameZoneDisplay> zoneCardDisplays;

        if (currPlayer == player)
        {
            zoneAnimalAwake = AIGameState.playerZoneAnimalAwake;
            zoneCardDisplays = AIGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zoneAnimalAwake = AIGameState.opponentZoneAnimalAwake;
            zoneCardDisplays = AIGameObjectManager.opponentZoneCardDisplays;
        }

        // If the animal can attack (awake set to 2) display the card glow, else disable
        for (int zone = 0; zone < 7; zone++)
        {
            if (zoneAnimalAwake[zone] == 2)
            {
                zoneCardDisplays[zone].SetCardGlow(true);
            }
            else
            {
                zoneCardDisplays[zone].SetCardGlow(false);
            }
        }
    }

    public void DisableZoneGlow(string currPlayer)
    {
        // Selects the appropriate zone card display list to reference depending on which player was in the argument
        List<AIGameZoneDisplay> zoneCardDisplays;

        if (currPlayer == player)
        {
            zoneCardDisplays = AIGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zoneCardDisplays = AIGameObjectManager.opponentZoneCardDisplays;
        }

        // Sets each of the cards glow effect to false
        for (int zone = 0; zone < 7; zone++)
        {
            zoneCardDisplays[zone].SetCardGlow(false);
        }
    }

    public void UpdateHandGlow(string currPlayer)
    {
        // Selects the appropriate hand playable, card display, hand cards list, and player coin amount to reference depending on which player was in the argument
        bool[] handPlayable;
        List<CardDisplay> handCardDisplays;

        List<Card> handCards;
        int coins;

        if (currPlayer == player)
        {
            handPlayable = AIGameState.playerHandPlayable;
            handCardDisplays = AIGameObjectManager.playerHandCardDisplays;

            handCards = AIGameState.playerHandCards;
            coins = AIGameState.playerCoins;
        }
        else
        {
            handPlayable = AIGameState.opponentHandPlayable;
            handCardDisplays = AIGameObjectManager.opponentHandCardDisplays;

            handCards = AIGameState.opponentHandCards;
            coins = AIGameState.opponentCoins;
        }

        for (int i = 0; i < 6; i++)
        {
            handPlayable[i] = false;
        }

        // For each hand card, if it is playable with the current players coin amount, set its glow effect to true else false
        int cardSlot = 0;
        foreach (Card card in handCards)
        {
            if (coins >= card.GetCardCost())
            {
                handPlayable[cardSlot] = true;
            }
            cardSlot++;
        }

        for (int handSlot = 0; handSlot < 6; handSlot++)
        {
            if (handPlayable[handSlot] == true)
            {
                handCardDisplays[handSlot].SetCardGlow(true);
            }
            else
            {
                handCardDisplays[handSlot].SetCardGlow(false);
            }
        }
    }

    public void DisableHandGlow(string currPlayer)
    {
        // Selects the appropriate hand card display to reference depending on which player was in the argument
        List<CardDisplay> handCardDisplays;

        if (currPlayer == player)
        {
            handCardDisplays = AIGameObjectManager.playerHandCardDisplays;
        }
        else
        {
            handCardDisplays = AIGameObjectManager.opponentHandCardDisplays;
        }

        // Disables each of the players hand glow effects
        foreach (CardDisplay display in handCardDisplays)
        {
            display.SetCardGlow(false);
        }
    }
}
