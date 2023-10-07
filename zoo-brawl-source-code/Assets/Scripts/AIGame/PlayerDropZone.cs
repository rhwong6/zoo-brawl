using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerDropZone : MonoBehaviour, IDropHandler
{
    // References the player zones game objects
    public GameObject playerZone1;
    public GameObject playerZone2;
    public GameObject playerZone3;
    public GameObject playerZone4;
    public GameObject playerZone5;
    public GameObject playerZone6;
    public GameObject playerZone7;

    // References the player outer zone object
    public GameObject outerZone;

    // List for storing the player zones
    List<GameObject> playerZones = new List<GameObject>();

    // Variable for storing the players hand cards, zone cards, and animal awake values
    List<Card> handCards = AIGameState.playerHandCards;
    int[] zoneCards = AIGameState.playerZoneCards;
    int[] zoneAnimalAwake = AIGameState.playerZoneAnimalAwake;

    // Initialises an event that is raised whena card is dropped onto the players zone
    public delegate void CardAddedToZoneHandler(int cardID, int cardZoneNum);
    public event CardAddedToZoneHandler CardAddedToZone;

    private void Awake()
    {
        // Adds each of the player zones to the list
        playerZones.Add(playerZone1);
        playerZones.Add(playerZone2);
        playerZones.Add(playerZone3);
        playerZones.Add(playerZone4);
        playerZones.Add(playerZone5);
        playerZones.Add(playerZone6);
        playerZones.Add(playerZone7);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // When a card is dropped on the game object on the players turn
        if (AIGameState.turn == "player")
        {
            HandDraggable hd = eventData.pointerDrag.GetComponent<HandDraggable>();
            if (hd != null)
            {
                // Gets the ID of the card dropped
                int cardID = handCards[hd.handPos].GetCardID();

                // Uses the zone objects in the list to determine which zone it is dropped on
                GameObject dropZone = eventData.pointerEnter;
                int dropZoneIndex = playerZones.IndexOf(dropZone);

                // Adds the card to the players zone if it is not the outer zone it is dropped on, if the zone does not already have a card, if the player has enough coins
                if (dropZone != outerZone && zoneCards[dropZoneIndex] == 0 && AIGameState.playerCoins >= CardCollection.cardCollection[cardID - 1].GetCardCost())
                {
                    // Removes that card from the hand and sets its awake value to 1
                    handCards.RemoveAt(hd.handPos);
                    zoneCards[dropZoneIndex] = cardID;
                    zoneAnimalAwake[dropZoneIndex] = 1;

                    if (CardAddedToZone != null)
                    {
                        // Raises the CardAddedToPlayerZone event
                        StartCoroutine(UpdateHandAndZone(cardID, dropZoneIndex));
                    }
                }
            }
        }
    }

    IEnumerator UpdateHandAndZone(int cardID, int cardZoneNum)
    {
        yield return new WaitForSeconds(0.001f);
        CardAddedToZone(cardID, cardZoneNum);
    }
}
