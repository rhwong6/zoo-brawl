using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using UnityEditor;

public class DeckBuilder : MonoBehaviour
{
    public int deckNum;

    // References to card objects in the scene
    public GameObject collectionCard1;
    public GameObject collectionCard2;
    public GameObject collectionCard3;
    public GameObject collectionCard4;
    public GameObject collectionCard5;

    // Reference to text that showcs card number
    public TextMeshProUGUI cardNumText;

    // List to store card objects and their displays
    List<GameObject> collectionCardObjs = new List<GameObject>();
    List<Card> cards = new List<Card>();
    List<CardDisplay> cardDisplays = new List<CardDisplay>();

    // References the deck name change input field
    public TMP_InputField deckNameInputField;

    int collectionPage;
    int totalCollectionPages;

    // Initialises drop zones (used for when they call events for when player drop cards in their zone)
    private DeckDropZone deckDropZone;
    private CollectionDropZone collectionDropZone;

    private void Awake()
    {
        // Adds card game objects to the list
        collectionCardObjs.Add(collectionCard1);
        collectionCardObjs.Add(collectionCard2);
        collectionCardObjs.Add(collectionCard3);
        collectionCardObjs.Add(collectionCard4);
        collectionCardObjs.Add(collectionCard5);

        // For each card object, initialise a card and display for it
        for (int c = 0; c < 5; c++)
        {
            cards.Add(new Card());
            cardDisplays.Add(collectionCardObjs[c].GetComponent<CardDisplay>());
        }

    }

    void Start()
    {
        deckNum = DeckManager.deckNum;

        // Calculates the amount of pages needed for all cards in the card collection
        totalCollectionPages = (int)Math.Ceiling(((double)CardCollection.cardCollection.Count / 5));
        collectionPage = 1;

        deckNameInputField.text = DeckManager.deckNames[deckNum];

        // Subscribes to the drop zone events
        deckDropZone = FindObjectOfType<DeckDropZone>();
        deckDropZone.CardAddedToDeck += OnCardAddedToDeck;

        collectionDropZone = FindObjectOfType<CollectionDropZone>();
        collectionDropZone.CardRemovedFromDeck += OnCardRemovedFromDeck;

        // Loads cards from card collection database for current collection page
        LoadCards(collectionPage);

        DeckNumText();

    }

    public void LoadCards(int collectionPage)
    {
        int pageStartNum = (collectionPage - 1) * 5;

        // Loads cards from the collection database for the current page that are not already included in the current player deck
        for (int currCard = 0; currCard < 5; currCard++)
        {
            if (CardCollection.cardCollection.Count > pageStartNum + currCard && DeckManager.decks[deckNum].deckCards.Contains(CardCollection.cardCollection[pageStartNum + currCard].GetCardID()) == false)
            {
                collectionCardObjs[currCard].SetActive(true);
                cards[currCard] = CardCollection.cardCollection[pageStartNum + currCard];
                cardDisplays[currCard].loadCardAttributes(cards[currCard]);
            }
            else
            {
                collectionCardObjs[currCard].SetActive(false);
            }
        }
    }

    // Reloads the card collection when a card is added to the current deck
    private void OnCardAddedToDeck(int cardID)
    {
        LoadCards(collectionPage);
        DeckNumText();
    }

    // Reloads the card collection when a card is removed from the current deck
    private void OnCardRemovedFromDeck(int cardID)
    {
        LoadCards(collectionPage);
        DeckNumText();
    }

    // Reloads the card collection for the next page
    public void TurnCollectionPageRight()
    {
        if (collectionPage < totalCollectionPages)
        {
            collectionPage++;
            LoadCards(collectionPage);
        }
    }

    // Reloads the card collection for the previous page
    public void TurnCollectionPageLeft()
    {
        if (collectionPage != 1)
        {
            collectionPage--;
            LoadCards(collectionPage);
        }
    }

    public void saveDeck()
    {
        // Sets the deck name to what the player inputs, otherwise sets it to "Deck + [deck number]"
        string deckName;
        if (deckNameInputField.text == "" || DeckManager.decks[deckNum].deckCards.All(cardID => cardID == 0))
        {
            deckName = "Deck " + (deckNum + 1);
        }
        else
        {
            deckName = deckNameInputField.text;
        }

        DeckManager.deckNames[deckNum] = deckName;

        // Initialises save path and the default lines for the file
        string savePath = Application.persistentDataPath + "/decks.txt";
        string[] deckLines = {"Deck 1", "0 0 0 0 0 0 0 0 0 0", "Deck 2", "0 0 0 0 0 0 0 0 0 0", "Deck 3", "0 0 0 0 0 0 0 0 0 0", "Deck 4", "0 0 0 0 0 0 0 0 0 0", 
                              "Deck 5", "0 0 0 0 0 0 0 0 0 0", "Deck 6", "0 0 0 0 0 0 0 0 0 0", "Deck 7", "0 0 0 0 0 0 0 0 0 0", "Deck 8", "0 0 0 0 0 0 0 0 0 0"};

        // If the save file exists, sets the string to the files contents
        if (File.Exists(savePath))
        {
            deckLines = File.ReadAllLines(savePath);
        }

        // Replaces the players deck name to the appropriate decks name line in the deckLines string
        deckLines[deckNum * 2] = deckName;

        // Stores each card in the deck as a string
        string newDeckLine = "";
        foreach (int cardID in DeckManager.decks[deckNum].deckCards)
        {
            newDeckLine += cardID.ToString();
            newDeckLine += " ";
        }

        // Replaces the string of the line with the cards in the deckLines string
        deckLines[(deckNum * 2) + 1] = newDeckLine;

        // Saves the new string content to the save file
        StreamWriter writer = new StreamWriter(savePath);

        foreach (string deckLine in deckLines)
        {
            writer.WriteLine(deckLine);
        }

        writer.Close();

        Debug.Log("Deck " + deckNum + " saved to file: " + savePath);

    }

    public void DeckNumText()
    {
        int cardCount = 0;
        for (int i = 0; i < 10; i++)
        {
            if (DeckManager.decks[deckNum].deckCards[i] != 0)
            {
                cardCount++;
            }
        }

        cardNumText.text = "Cards: " + cardCount + "/10";
    }

    public void ReturnToDeckMenu()
    {
        // Reloads all decks to decks saved in save file
        string savePath = Application.persistentDataPath + "/decks.txt";

        if (File.Exists(savePath))
        {
            StreamReader reader = new StreamReader(savePath);

            for (int currDeck = 0; currDeck < DeckManager.decks.Count; currDeck++)
            {
                DeckManager.deckNames[currDeck] = reader.ReadLine();

                string[] cardIDs = reader.ReadLine().Split(' ');

                for (int i = 0; i < DeckManager.decks[currDeck].deckCards.Length; i++)
                {
                    DeckManager.decks[currDeck].deckCards[i] = int.Parse(cardIDs[i]);
                }

            }
        }

        // Returns to the deck menu
        SceneManager.LoadScene("Deck Menu");
    }
}
