using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeGameCardGlow
{
    private const string player = "player";
    private const string opponent = "opponent";

    public void UpdateZoneGlow(string currPlayer)
    {

        int[] zoneAnimalAwake;
        List<PracticeGameZoneDisplay> zoneCardDisplays;

        if (currPlayer == player)
        {
            zoneAnimalAwake = PracticeGameState.playerZoneAnimalAwake;
            zoneCardDisplays = PracticeGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zoneAnimalAwake = PracticeGameState.opponentZoneAnimalAwake;
            zoneCardDisplays = PracticeGameObjectManager.opponentZoneCardDisplays;
        }

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
        List<PracticeGameZoneDisplay> zoneCardDisplays;

        if (currPlayer == player)
        {
            zoneCardDisplays = PracticeGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zoneCardDisplays = PracticeGameObjectManager.opponentZoneCardDisplays;
        }

        for (int zone = 0; zone < 7; zone++)
        {
            zoneCardDisplays[zone].SetCardGlow(false);
        }
    }

    public void UpdateHandGlow(string currPlayer)
    {
        bool[] handPlayable;
        List<CardDisplay> handCardDisplays;

        List<Card> handCards;
        int coins;

        if (currPlayer == player)
        {
            handPlayable = PracticeGameState.playerHandPlayable;
            handCardDisplays = PracticeGameObjectManager.playerHandCardDisplays;

            handCards = PracticeGameState.playerHandCards;
            coins = PracticeGameState.playerCoins;
        }
        else
        {
            handPlayable = PracticeGameState.opponentHandPlayable;
            handCardDisplays = PracticeGameObjectManager.opponentHandCardDisplays;

            handCards = PracticeGameState.opponentHandCards;
            coins = PracticeGameState.opponentCoins;
        }

        for (int i = 0; i < 6; i++)
        {
            handPlayable[i] = false;
        }

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
        List<CardDisplay> handCardDisplays;

        if (currPlayer == player)
        {
            handCardDisplays = PracticeGameObjectManager.playerHandCardDisplays;
        }
        else
        {
            handCardDisplays = PracticeGameObjectManager.opponentHandCardDisplays;
        }

        foreach (CardDisplay display in handCardDisplays)
        {
            display.SetCardGlow(false);
        }
    }
}
