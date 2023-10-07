using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class DeckDropZone : MonoBehaviour, IDropHandler
{

    int deckNum;

    // References the card game objects in the deck in the scene
    public GameObject deckCard1;
    public GameObject deckCard2;
    public GameObject deckCard3;
    public GameObject deckCard4;
    public GameObject deckCard5;
    public GameObject deckCard6;
    public GameObject deckCard7;
    public GameObject deckCard8;
    public GameObject deckCard9;
    public GameObject deckCard10;

    // Initialises an event that is raised when the card is added to the deck
    public delegate void CardAddedToDeckEventHandler(int cardID);
    public event CardAddedToDeckEventHandler CardAddedToDeck;

    // References the CollectionDropZone class, to use the CardRemovedFromDeck event
    private CollectionDropZone collectionDropZone;

    // Lists to store game objects
    List<GameObject> deckCardObjs = new List<GameObject>();
    List<Card> cards = new List<Card>();
    List<CardDisplay> cardDisplays = new List<CardDisplay>();

    private void Awake()
    {
        // Adds each card game object in the deck to a list
        deckCardObjs.Add(deckCard1);
        deckCardObjs.Add(deckCard2);
        deckCardObjs.Add(deckCard3);
        deckCardObjs.Add(deckCard4);
        deckCardObjs.Add(deckCard5);
        deckCardObjs.Add(deckCard6);
        deckCardObjs.Add(deckCard7);
        deckCardObjs.Add(deckCard8);
        deckCardObjs.Add(deckCard9);
        deckCardObjs.Add(deckCard10);

        // Initialises a card and card display for each card game object
        for (int c = 0; c < 10; c++)
        {
            cards.Add(new Card());
            cardDisplays.Add(deckCardObjs[c].GetComponent<CardDisplay>());
        }
    }

    void Start()
    {
        deckNum = DeckManager.deckNum;

        // Subscribes to the CardRemovedFromDeck event
        collectionDropZone = FindObjectOfType<CollectionDropZone>();
        collectionDropZone.CardRemovedFromDeck += OnCardRemovedFromDeck;

        // Updates the cards in the deck displays
        UpdateDeckCards();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // When a card is dropped from the collection to the deck, add it to the deck and updates the deck displays, and raises the CardAddedToDeckEvent
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
            int cardID = cardDisplay.id;

            DeckManager.decks[deckNum].addCard(cardID);

            UpdateDeckCards();
            
            if (CardAddedToDeck != null)
            {
                StartCoroutine(updateCollection(cardID));
            }
        }
    }

    IEnumerator updateCollection(int cardID)
    {
        // Raises the CardAddedToDeckEvent
        yield return new WaitForSeconds(0.001f);
        CardAddedToDeck(cardID);
    }


    public void UpdateDeckCards()
    {
        // Updates the card displays in the deck to match the cards added to deck from the card collection
        for (int currCard = 0; currCard < 10; currCard++)
        {
            if (DeckManager.decks[deckNum].deckCards[currCard] != 0)
            {
                cards[currCard] = CardCollection.cardCollection[DeckManager.decks[deckNum].deckCards[currCard] - 1];
                cardDisplays[currCard].loadCardAttributes(cards[currCard]);
                deckCardObjs[currCard].SetActive(true);
            }
            else
            {
                deckCardObjs[currCard].SetActive(false);
            }
        }
    }

    private void OnCardRemovedFromDeck(int cardID)
    {
        // Sorts the deck when a card is removed, and updates the cards being displayed in the deck
        SortDeck();
        UpdateDeckCards();
    }

    public void SortDeck()
    {
        // When a card is removed form the deck, sorts the deck so that the cards are displayed at the front
        DeckManager.decks[deckNum].deckCards = DeckManager.decks[deckNum].deckCards.OrderBy(x => x == 0).ToArray();
    }
}
