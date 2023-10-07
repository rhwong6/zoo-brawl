using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCardDisplayHandler
{
    public void UpdateHandDisplay(string player)
    {
        // Initalises lists for cards in the hand, its card objects, and its card displays
        List<Card> hand;
        List<GameObject> handObjs;
        List<CardDisplay> handDisplays;

        // Determines which cards and objects to update depending on player in argument
        if (player == "player")
        {
            hand = AIGameState.playerHandCards;
            handObjs = AIGameObjectManager.playerHandCardObjs;
            handDisplays = AIGameObjectManager.playerHandCardDisplays;
        }
        else
        {
            hand = AIGameState.opponentHandCards;
            handObjs = AIGameObjectManager.opponentHandCardObjs;
            handDisplays = AIGameObjectManager.opponentHandCardDisplays;
        }

        // For each hand card object, set its active state to false
        foreach (GameObject handCardObj in handObjs)
        {
            handCardObj.SetActive(false);
        }

        // If there is a card in the hand, set its active state to true and uses the loadCardAttributes method to load its display attributes
        for (int handCard = 0; handCard < hand.Count; handCard++)
        {
            handObjs[handCard].SetActive(true);
            handDisplays[handCard].loadCardAttributes(hand[handCard]);
        }

    }

    public void UpdateZoneDisplay(string currPlayer)
    {
        // Initalises lists for cards in the zone, its card objects, and its card displays
        int[] zone;
        List<GameObject> zoneObjs;
        List<AIGameZoneDisplay> zoneDisplays;

        // Determines which cards and objects to update depending on player in argument
        if (currPlayer == "player")
        {
            zone = AIGameState.playerZoneCards;
            zoneObjs = AIGameObjectManager.playerZoneCardObjs;
            zoneDisplays = AIGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zone = AIGameState.opponentZoneCards;
            zoneObjs = AIGameObjectManager.opponentZoneCardObjs;
            zoneDisplays = AIGameObjectManager.opponentZoneCardDisplays;
        }

        // For each zone card object, set its active state to false
        foreach (GameObject animalZoneObj in zoneObjs)
        {
            animalZoneObj.SetActive(false);
        }

        // If there is a card in the zone, set its active state to true and uses the loadCardAttributes method to load its display attributes
        for (int zoneCard = 0; zoneCard < zone.Length; zoneCard++)
        {
            if (zone[zoneCard] != 0)
            {
                zoneObjs[zoneCard].SetActive(true);
                zoneDisplays[zoneCard].loadCardAttributes(currPlayer, CardCollection.cardCollection[zone[zoneCard] - 1], zoneCard);
            }
        }

    }
}
