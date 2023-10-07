using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class OpponentDropZone : MonoBehaviour, IDropHandler
{
    // References the opponent zones game objects
    public GameObject opponentZone1;
    public GameObject opponentZone2;
    public GameObject opponentZone3;
    public GameObject opponentZone4;
    public GameObject opponentZone5;
    public GameObject opponentZone6;
    public GameObject opponentZone7;

    // References the opponents outer zone object
    public GameObject opponentOuterZone;

    // List for storing the opponent zones
    List<GameObject> opponentZones = new List<GameObject>();

    // Variable for storing the hand cards, zone cards, and animal awake values
    List<Card> handCards = AIGameState.opponentHandCards;
    int[] zoneCards = AIGameState.opponentZoneCards;
    int[] zoneAnimalAwake = AIGameState.opponentZoneAnimalAwake;

    // Initialises an event that is raised whena card is dropped onto the opponents zone
    public delegate void CardAddedToOpponentZoneHandler(int cardID, int cardZoneNum);
    public event CardAddedToOpponentZoneHandler CardAddedToOpponentZone;

    private void Awake()
    {
        // Adds each of the opponent zones to the list
        opponentZones.Add(opponentZone1);
        opponentZones.Add(opponentZone2);
        opponentZones.Add(opponentZone3);
        opponentZones.Add(opponentZone4);
        opponentZones.Add(opponentZone5);
        opponentZones.Add(opponentZone6);
        opponentZones.Add(opponentZone7);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // When a card is dropped on the game object on the opponents turn
        if (AIGameState.turn == "opponent")
        {
            HandDraggable hd = eventData.pointerDrag.GetComponent<HandDraggable>();
            if (hd != null)
            {
                // Gets the ID of the card dropped
                int cardID = handCards[hd.handPos].GetCardID();

                // Uses the zone objects in the list to determine which zone it is dropped on
                GameObject dropZone = eventData.pointerEnter;
                int dropZoneIndex = opponentZones.IndexOf(dropZone);

                // Adds the card to the opponents zone if it is not the outer zone it is dropped on, if the zone does not already have a card, if the opponent has enough coins
                if (dropZone != opponentOuterZone && zoneCards[dropZoneIndex] == 0 && AIGameState.opponentCoins >= CardCollection.cardCollection[cardID - 1].GetCardCost())
                {
                    // Removes that card from the hand and sets its awake value to 1
                    handCards.RemoveAt(hd.handPos);
                    zoneCards[dropZoneIndex] = cardID;
                    zoneAnimalAwake[dropZoneIndex] = 1;

                    if (CardAddedToOpponentZone != null)
                    {
                        // Raises the CardAddedToOpponentZone event
                        StartCoroutine(UpdateOpponentHandAndZone(cardID, dropZoneIndex));
                    }
                }
            }
        }
    }

    IEnumerator UpdateOpponentHandAndZone(int cardID, int cardZoneNum)
    {
        yield return new WaitForSeconds(0.001f);
        CardAddedToOpponentZone(cardID, cardZoneNum);
    }

}
