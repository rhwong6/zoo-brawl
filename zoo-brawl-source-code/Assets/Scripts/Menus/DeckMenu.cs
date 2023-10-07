using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class DeckMenu : MonoBehaviour
{
    // References all the text objects for the deck names in the scene
    public TextMeshProUGUI deckName1;
    public TextMeshProUGUI deckName2;
    public TextMeshProUGUI deckName3;
    public TextMeshProUGUI deckName4;
    public TextMeshProUGUI deckName5;
    public TextMeshProUGUI deckName6;
    public TextMeshProUGUI deckName7;
    public TextMeshProUGUI deckName8;

    // References all the plus display objects in the scene
    public GameObject plus1;
    public GameObject plus2;
    public GameObject plus3;
    public GameObject plus4;
    public GameObject plus5;
    public GameObject plus6;
    public GameObject plus7;
    public GameObject plus8;

    // References all the card objects used to display the first three cards in each deck in the scene
    public GameObject slot1card1Obj;
    public GameObject slot1card2Obj;
    public GameObject slot1card3Obj;

    public GameObject slot2card1Obj;
    public GameObject slot2card2Obj;
    public GameObject slot2card3Obj;

    public GameObject slot3card1Obj;
    public GameObject slot3card2Obj;
    public GameObject slot3card3Obj;

    public GameObject slot4card1Obj;
    public GameObject slot4card2Obj;
    public GameObject slot4card3Obj;

    public GameObject slot5card1Obj;
    public GameObject slot5card2Obj;
    public GameObject slot5card3Obj;

    public GameObject slot6card1Obj;
    public GameObject slot6card2Obj;
    public GameObject slot6card3Obj;

    public GameObject slot7card1Obj;
    public GameObject slot7card2Obj;
    public GameObject slot7card3Obj;

    public GameObject slot8card1Obj;
    public GameObject slot8card2Obj;
    public GameObject slot8card3Obj;

    // Stores all game objects in lists
    List<TextMeshProUGUI> deckNames = new List<TextMeshProUGUI>();
    List<GameObject> pluses = new List<GameObject>();
    List<GameObject> showcaseCardObjs = new List<GameObject>();
    List<Card> showcaseCards = new List<Card>();
    List<CardDisplay> cardDisplays = new List<CardDisplay>();


    private void Awake()
    {
        // Adds all game objects to the list
        deckNames.Add(deckName1);
        deckNames.Add(deckName2);
        deckNames.Add(deckName3);
        deckNames.Add(deckName4);
        deckNames.Add(deckName5);
        deckNames.Add(deckName6);
        deckNames.Add(deckName7);
        deckNames.Add(deckName8);

        pluses.Add(plus1);
        pluses.Add(plus2);
        pluses.Add(plus3);
        pluses.Add(plus4);
        pluses.Add(plus5);
        pluses.Add(plus6);
        pluses.Add(plus7);
        pluses.Add(plus8);


        showcaseCardObjs.Add(slot1card1Obj);
        showcaseCardObjs.Add(slot1card2Obj);
        showcaseCardObjs.Add(slot1card3Obj);

        showcaseCardObjs.Add(slot2card1Obj);
        showcaseCardObjs.Add(slot2card2Obj);
        showcaseCardObjs.Add(slot2card3Obj);

        showcaseCardObjs.Add(slot3card1Obj);
        showcaseCardObjs.Add(slot3card2Obj);
        showcaseCardObjs.Add(slot3card3Obj);

        showcaseCardObjs.Add(slot4card1Obj);
        showcaseCardObjs.Add(slot4card2Obj);
        showcaseCardObjs.Add(slot4card3Obj);

        showcaseCardObjs.Add(slot5card1Obj);
        showcaseCardObjs.Add(slot5card2Obj);
        showcaseCardObjs.Add(slot5card3Obj);

        showcaseCardObjs.Add(slot6card1Obj);
        showcaseCardObjs.Add(slot6card2Obj);
        showcaseCardObjs.Add(slot6card3Obj);

        showcaseCardObjs.Add(slot7card1Obj);
        showcaseCardObjs.Add(slot7card2Obj);
        showcaseCardObjs.Add(slot7card3Obj);

        showcaseCardObjs.Add(slot8card1Obj);
        showcaseCardObjs.Add(slot8card2Obj);
        showcaseCardObjs.Add(slot8card3Obj);

    }

    private void Start()
    {
        // For each deck display appropriate object depending on if there are cards in the deck
        for (int currDeck = 0; currDeck < 8; currDeck++)
        {
            // Initialises the three showcase cards for each deck with a card display component
            for (int c = 0; c < 3; c++)
            {
                showcaseCardObjs[(currDeck * 3) + c].SetActive(false);

                showcaseCards.Add(new Card());
                cardDisplays.Add(showcaseCardObjs[(currDeck*3) + c].GetComponent<CardDisplay>());
            }

            // If the deck is empty, sets the deck name to nothing and sets the plus object of that deck to display
            if (DeckManager.decks[currDeck].deckCards.All(c => c == 0))
            {
                deckNames[currDeck].text = "";
                pluses[currDeck].SetActive(true);
            }
            else
            {
                // Shows the deck name in the text object for that deck
                deckNames[currDeck].text = DeckManager.deckNames[currDeck];
                pluses[currDeck].SetActive(false);

                if (DeckManager.decks[currDeck].deckCards[0] != 0)
                {
                    // If the deck contains 1 card, shows the first showcase card as the first card in the deck
                    showcaseCardObjs[currDeck * 3].SetActive(true);
                    showcaseCards[currDeck * 3] = CardCollection.cardCollection[DeckManager.decks[currDeck].deckCards[0] - 1];
                    cardDisplays[currDeck * 3].loadCardAttributes(showcaseCards[currDeck * 3]);
                }
                if (DeckManager.decks[currDeck].deckCards[1] != 0)
                {
                    // If the deck contains 2 cards, shows the second showcase card as the second card in the deck
                    showcaseCardObjs[(currDeck * 3) + 1].SetActive(true);
                    showcaseCards[(currDeck * 3) + 1] = CardCollection.cardCollection[DeckManager.decks[currDeck].deckCards[1] - 1];
                    cardDisplays[(currDeck * 3) + 1].loadCardAttributes(showcaseCards[(currDeck * 3) + 1]);
                }
                if (DeckManager.decks[currDeck].deckCards[2] != 0)
                {
                    // If the deck contains 3 cards, shows the third showcase card as the third card in the deck
                    showcaseCardObjs[(currDeck * 3) + 2].SetActive(true);
                    showcaseCards[(currDeck * 3) + 2] = CardCollection.cardCollection[DeckManager.decks[currDeck].deckCards[2] - 1];
                    cardDisplays[(currDeck * 3) + 2].loadCardAttributes(showcaseCards[(currDeck * 3) + 2]);
                }
            }
        }

    }

    // Loads the deck builder scene and sets which deck to edit
    public void EditDeck(int deckNum)
    {
        DeckManager.deckNum = deckNum;
        SceneManager.LoadScene("Deck Builder");
    }

    // Load the main menu scene
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
