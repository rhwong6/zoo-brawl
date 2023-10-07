using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class AIDeckSelection : MonoBehaviour
{
    // References all player UI game objects in the scene
    public GameObject playerPlus;

    public Button playerDeckButton1;
    public Button playerDeckButton2;
    public Button playerDeckButton3;
    public Button playerDeckButton4;
    public Button playerDeckButton5;
    public Button playerDeckButton6;
    public Button playerDeckButton7;
    public Button playerDeckButton8;

    public TextMeshProUGUI playerDeckName1;
    public TextMeshProUGUI playerDeckName2;
    public TextMeshProUGUI playerDeckName3;
    public TextMeshProUGUI playerDeckName4;
    public TextMeshProUGUI playerDeckName5;
    public TextMeshProUGUI playerDeckName6;
    public TextMeshProUGUI playerDeckName7;
    public TextMeshProUGUI playerDeckName8;

    public TextMeshProUGUI playerDeckCardCount1;
    public TextMeshProUGUI playerDeckCardCount2;
    public TextMeshProUGUI playerDeckCardCount3;
    public TextMeshProUGUI playerDeckCardCount4;
    public TextMeshProUGUI playerDeckCardCount5;
    public TextMeshProUGUI playerDeckCardCount6;
    public TextMeshProUGUI playerDeckCardCount7;
    public TextMeshProUGUI playerDeckCardCount8;

    public GameObject playerShowcaseCardObj1;
    public GameObject playerShowcaseCardObj2;
    public GameObject playerShowcaseCardObj3;

    public TextMeshProUGUI playerSelectedDeckName;

    // List to store these player UI game objects
    List<Button> playerDeckButtonObjs = new List<Button>();
    List<TextMeshProUGUI> playerDeckNames = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> playerDeckCardCounts = new List<TextMeshProUGUI>();
    List<GameObject> playerShowcaseCardObjs = new List<GameObject>();
    List<CardDisplay> playerCardDisplays = new List<CardDisplay>();

    // References all opponent UI game objects in the scene
    public GameObject opponentPlus;

    public Button opponentDeckButton1;
    public Button opponentDeckButton2;
    public Button opponentDeckButton3;
    public Button opponentDeckButton4;
    public Button opponentDeckButton5;
    public Button opponentDeckButton6;
    public Button opponentDeckButton7;
    public Button opponentDeckButton8;

    public TextMeshProUGUI opponentDeckName1;
    public TextMeshProUGUI opponentDeckName2;
    public TextMeshProUGUI opponentDeckName3;
    public TextMeshProUGUI opponentDeckName4;
    public TextMeshProUGUI opponentDeckName5;
    public TextMeshProUGUI opponentDeckName6;
    public TextMeshProUGUI opponentDeckName7;
    public TextMeshProUGUI opponentDeckName8;

    public TextMeshProUGUI opponentDeckCardCount1;
    public TextMeshProUGUI opponentDeckCardCount2;
    public TextMeshProUGUI opponentDeckCardCount3;
    public TextMeshProUGUI opponentDeckCardCount4;
    public TextMeshProUGUI opponentDeckCardCount5;
    public TextMeshProUGUI opponentDeckCardCount6;
    public TextMeshProUGUI opponentDeckCardCount7;
    public TextMeshProUGUI opponentDeckCardCount8;

    public GameObject opponentShowcaseCardObj1;
    public GameObject opponentShowcaseCardObj2;
    public GameObject opponentShowcaseCardObj3;

    public TextMeshProUGUI opponentSelectedDeckName;

    // List to store opponent UI objects
    List<Button> opponentDeckButtonObjs = new List<Button>();
    List<TextMeshProUGUI> opponentDeckNames = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> opponentDeckCardCounts = new List<TextMeshProUGUI>();
    List<GameObject> opponentShowcaseCardObjs = new List<GameObject>();
    List<CardDisplay> opponentCardDisplays = new List<CardDisplay>();

    // Variable to store which players deck to select
    public string currPlayer;

    // Reference game object button that toggles if the AI hand is visible or not
    public Image toggleAIHandVisibleBackground;

    private void Awake()
    {
        // Adds game objects to their respective lists
        playerDeckButtonObjs.Add(playerDeckButton1);
        playerDeckButtonObjs.Add(playerDeckButton2);
        playerDeckButtonObjs.Add(playerDeckButton3);
        playerDeckButtonObjs.Add(playerDeckButton4);
        playerDeckButtonObjs.Add(playerDeckButton5);
        playerDeckButtonObjs.Add(playerDeckButton6);
        playerDeckButtonObjs.Add(playerDeckButton7);
        playerDeckButtonObjs.Add(playerDeckButton8);

        playerDeckNames.Add(playerDeckName1);
        playerDeckNames.Add(playerDeckName2);
        playerDeckNames.Add(playerDeckName3);
        playerDeckNames.Add(playerDeckName4);
        playerDeckNames.Add(playerDeckName5);
        playerDeckNames.Add(playerDeckName6);
        playerDeckNames.Add(playerDeckName7);
        playerDeckNames.Add(playerDeckName8);

        playerDeckCardCounts.Add(playerDeckCardCount1);
        playerDeckCardCounts.Add(playerDeckCardCount2);
        playerDeckCardCounts.Add(playerDeckCardCount3);
        playerDeckCardCounts.Add(playerDeckCardCount4);
        playerDeckCardCounts.Add(playerDeckCardCount5);
        playerDeckCardCounts.Add(playerDeckCardCount6);
        playerDeckCardCounts.Add(playerDeckCardCount7);
        playerDeckCardCounts.Add(playerDeckCardCount8);

        playerShowcaseCardObjs.Add(playerShowcaseCardObj1);
        playerShowcaseCardObjs.Add(playerShowcaseCardObj2);
        playerShowcaseCardObjs.Add(playerShowcaseCardObj3);

        opponentDeckButtonObjs.Add(opponentDeckButton1);
        opponentDeckButtonObjs.Add(opponentDeckButton2);
        opponentDeckButtonObjs.Add(opponentDeckButton3);
        opponentDeckButtonObjs.Add(opponentDeckButton4);
        opponentDeckButtonObjs.Add(opponentDeckButton5);
        opponentDeckButtonObjs.Add(opponentDeckButton6);
        opponentDeckButtonObjs.Add(opponentDeckButton7);
        opponentDeckButtonObjs.Add(opponentDeckButton8);

        opponentDeckNames.Add(opponentDeckName1);
        opponentDeckNames.Add(opponentDeckName2);
        opponentDeckNames.Add(opponentDeckName3);
        opponentDeckNames.Add(opponentDeckName4);
        opponentDeckNames.Add(opponentDeckName5);
        opponentDeckNames.Add(opponentDeckName6);
        opponentDeckNames.Add(opponentDeckName7);
        opponentDeckNames.Add(opponentDeckName8);

        opponentDeckCardCounts.Add(opponentDeckCardCount1);
        opponentDeckCardCounts.Add(opponentDeckCardCount2);
        opponentDeckCardCounts.Add(opponentDeckCardCount3);
        opponentDeckCardCounts.Add(opponentDeckCardCount4);
        opponentDeckCardCounts.Add(opponentDeckCardCount5);
        opponentDeckCardCounts.Add(opponentDeckCardCount6);
        opponentDeckCardCounts.Add(opponentDeckCardCount7);
        opponentDeckCardCounts.Add(opponentDeckCardCount8);

        opponentShowcaseCardObjs.Add(opponentShowcaseCardObj1);
        opponentShowcaseCardObjs.Add(opponentShowcaseCardObj2);
        opponentShowcaseCardObjs.Add(opponentShowcaseCardObj3);

        // For each card showcase, initialise a card and card display for it
        for (int cardShowcase = 0; cardShowcase < 3; cardShowcase++)
        {
            playerCardDisplays.Add(playerShowcaseCardObjs[cardShowcase].GetComponent<CardDisplay>());
            opponentCardDisplays.Add(opponentShowcaseCardObjs[cardShowcase].GetComponent<CardDisplay>());
        }
    }

    void Start()
    {
        // When the scene loads, displays the plus game object and disable the showcase card objects
        playerPlus.SetActive(true);

        for (int deckNum = 0; deckNum < 8; deckNum++)
        {
            playerDeckButtonObjs[deckNum].gameObject.SetActive(false);
            playerDeckCardCounts[deckNum].gameObject.SetActive(false);

            opponentDeckButtonObjs[deckNum].gameObject.SetActive(false);
            opponentDeckCardCounts[deckNum].gameObject.SetActive(false);
        }
    }

    public void DisplayDecks(string player)
    {
        GameObject plus;
        List<Button> deckButtonObjs;
        List<TextMeshProUGUI> deckNames;
        List<TextMeshProUGUI> deckCardCounts;
        List<GameObject> showcaseCardObjs;

        // Depending of if the player selects to change the player or opponent deck, selects the appropriate game objects to modify
        currPlayer = player;

        if (currPlayer == "player")
        {
            plus = playerPlus;
            deckButtonObjs = playerDeckButtonObjs;
            deckNames = playerDeckNames;
            deckCardCounts = playerDeckCardCounts;
            showcaseCardObjs = playerShowcaseCardObjs;
        }
        else
        {
            plus = opponentPlus;
            deckButtonObjs = opponentDeckButtonObjs;
            deckNames = opponentDeckNames;
            deckCardCounts = opponentDeckCardCounts;
            showcaseCardObjs = opponentShowcaseCardObjs;
        }

        // Disables the plus game object for the player or opponent
        plus.SetActive(false);

        // Displays each deck the player can select and displays/does not display appropriate objects
        for (int deckNum = 0; deckNum < 8; deckNum++)
        {
            deckButtonObjs[deckNum].gameObject.SetActive(true);
            // If the player deck does not have any empty deck slots, allow the player to select that deck to play with
            if (DeckManager.decks[deckNum].deckCards.Contains(0) == false)
            {
                deckButtonObjs[deckNum].interactable = true;
                deckNames[deckNum].text = DeckManager.deckNames[deckNum];
            }
            else
            {
                // Else, does not allow the player to select deck, and shows text telling them how many cards are in the deck
                if (DeckManager.decks[deckNum].deckCards.All(x => x == 0) == false)
                {
                    Debug.Log("TEST");
                    deckNames[deckNum].text = DeckManager.deckNames[deckNum];
                    deckCardCounts[deckNum].text = DeckManager.decks[deckNum].deckCards.Count(cardID => cardID != 0) + "/10";
                    deckCardCounts[deckNum].gameObject.SetActive(true);
                }

                deckButtonObjs[deckNum].interactable = false;
            }
        }

        // Does not display showcase card objects
        for (int showcaseCards = 0; showcaseCards < 3; showcaseCards++)
        {
            showcaseCardObjs[showcaseCards].SetActive(false);
        }

    }

    public void SelectDeck(int deckSelected)
    {
        TextMeshProUGUI selectedDeckName;
        List<Card> deckCards;
        List<Button> deckButtonObjs;
        List<CardDisplay> cardDisplays;
        List<GameObject> showcaseCardObjs;
        List<TextMeshProUGUI> deckCardCounts;

        // Depending of if the player selects to change the player or opponent deck, selects the appropriate game objects to modify
        if (currPlayer == "player")
        {
            selectedDeckName = playerSelectedDeckName;
            deckCards = AIGameState.playerDeckCards;
            deckButtonObjs = playerDeckButtonObjs;
            cardDisplays = playerCardDisplays;
            showcaseCardObjs = playerShowcaseCardObjs;
            deckCardCounts = playerDeckCardCounts;
        }
        else
        {
            selectedDeckName = opponentSelectedDeckName;
            deckCards = AIGameState.opponentDeckCards;
            deckButtonObjs = opponentDeckButtonObjs;
            cardDisplays = opponentCardDisplays;
            showcaseCardObjs = opponentShowcaseCardObjs;
            deckCardCounts = opponentDeckCardCounts;
        }

        // Clears current deck
        deckCards.Clear();

        // Sets text UI to show which deck was selected
        selectedDeckName.text = DeckManager.deckNames[deckSelected];

        // Adds each card from the deck selected to the current players deck in the game
        for (int cardID = 0; cardID < 10; cardID++)
        {
            deckCards.Add(CardCollection.cardCollection[DeckManager.decks[deckSelected].deckCards[cardID] - 1]);
        }

        // Disables game object deck buttons
        for (int deckNum = 0; deckNum < 8; deckNum++)
        {
            deckButtonObjs[deckNum].gameObject.SetActive(false);
            deckCardCounts[deckNum].gameObject.SetActive(false);
        }

        // Updates the showcase cards and displays them
        for (int showcaseCard = 0; showcaseCard < 3; showcaseCard++)
        {
            cardDisplays[showcaseCard].loadCardAttributes(deckCards[showcaseCard]);
            showcaseCardObjs[showcaseCard].SetActive(true);
        }
    }

    public void ToggleAIHandVisible()
    {
        // When the toggle AI hand button is clicked, changes the AIGameState boolean variable and changes the colour appropriately
        if (AIGameState.aiCoverCards)
        {
            AIGameState.aiCoverCards = false;
            toggleAIHandVisibleBackground.color = Color.green;
        }
        else
        {
            AIGameState.aiCoverCards = true;
            toggleAIHandVisibleBackground.color = Color.red;
        }
    }

    public void StartGame()
    {
        // If both decks are full, start the game by loading the AI game scene
        if (AIGameState.playerDeckCards.Count == 10 && AIGameState.opponentDeckCards.Count == 10)
        {
            SceneManager.LoadScene("AI Game");
        }
    }

    // Loads the main menu scene
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
