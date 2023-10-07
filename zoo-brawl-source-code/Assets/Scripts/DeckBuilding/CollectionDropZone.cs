using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionDropZone : MonoBehaviour, IDropHandler
{
    int deckNum;

    // Initialises an event for when a card is removed from the deck
    public delegate void CardRemovedFromDeckHandler(int cardID);
    public event CardRemovedFromDeckHandler CardRemovedFromDeck;

    private void Start()
    {
        deckNum = DeckManager.deckNum;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // When a card is dropped from the deck onto the collection, removes the card from the deck and raises the CardRemovedFromDeck event
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
            int cardID = cardDisplay.id;

            DeckManager.decks[deckNum].removeCard(cardID);

            if (CardRemovedFromDeck != null)
            {
                StartCoroutine(updateCollection(cardID));
            }

        }
    }

    // Raises CardRemovedFromDeck event
    IEnumerator updateCollection(int cardID)
    { 
        yield return new WaitForSeconds(0.001f);
        CardRemovedFromDeck(cardID);
    }
}


