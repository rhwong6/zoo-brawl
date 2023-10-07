using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIGameZoneDisplay : MonoBehaviour
{
    // Variables to store the attributes of the cards
    public int id;
    public string cardName;
    public string cardAbility;
    public int cardCost;
    public int cardAttack;
    public int cardArmour;
    public int cardHealth;
    public string cardType;

    // Variables to store the current modifiers affecting the card
    public int cardAttackModifier;
    public int cardArmourModifier;
    public int cardHealthModifier;

    // References to attribute text game objects within the cards
    public GameObject cardNameBox;
    public GameObject cardAbilityBox;
    public GameObject cardCostBox;
    public GameObject cardAttackBox;
    public GameObject cardArmourBox;
    public GameObject cardHealthBox;

    // References to visual game objects within the card
    public GameObject cardArt;
    public Image cardBackgroundColour;
    public Image cardBorder;

    const string player = "player";
    const string opponent = "opponent";

    public void loadCardAttributes(string currPlayer, Card card, int zoneNum)
    {
        // Loads each attribute from the card collection database and sets values of these attributes
        id = card.GetCardID();

        TextMeshProUGUI cardNameBoxText = cardNameBox.GetComponent<TextMeshProUGUI>();
        cardNameBoxText.color = Color.white;
        cardName = card.GetCardName();
        cardNameBoxText.text = cardName;

        TextMeshProUGUI cardAbilityBoxText = cardAbilityBox.GetComponent<TextMeshProUGUI>();
        cardAbilityBoxText.color = Color.black;
        cardAbility = card.GetCardAbility();
        cardAbilityBoxText.text = cardAbility;

        TextMeshProUGUI cardCostBoxText = cardCostBox.GetComponent<TextMeshProUGUI>();
        cardCostBoxText.color = Color.black;
        cardCost= card.GetCardCost();
        cardCostBoxText.text = "" + cardCost;

        int[] cardModifiedAttributes = CardModifiedAttributes(currPlayer, zoneNum);

        // If the cards attack value has been modified, show it as cyan (if higher), yellow (if lower), or black if same as base value
        TextMeshProUGUI cardAttackBoxText = cardAttackBox.GetComponent<TextMeshProUGUI>();
        cardAttack = cardModifiedAttributes[0];
        cardAttackBoxText.text = "" + cardAttack;
        if (cardAttack > card.GetCardAttack())
        {
            cardAttackBoxText.color = new Color(0, 226, 255);
        }
        else if (cardAttack < card.GetCardAttack())
        {
            cardAttackBoxText.color = Color.yellow;
        }
        else
        {
            cardAttackBoxText.color = Color.black;
        }

        // If the cards armour value has been modified, show it as cyan (if higher), yellow (if lower), or black if same as base value
        TextMeshProUGUI cardArmourBoxText = cardArmourBox.GetComponent<TextMeshProUGUI>();
        cardArmour = cardModifiedAttributes[1];
        cardArmourBoxText.text = "" + cardArmour;
        if (cardArmour > card.GetCardArmour())
        {
            cardArmourBoxText.color = new Color(0, 226, 255);
        }
        else if (cardArmour < card.GetCardArmour())
        {
            cardArmourBoxText.color = Color.yellow;
        }
        else
        {
            cardArmourBoxText.color = Color.black;
        }

        // Selects appropriate damage counter depending on if it is a players card or opponents
        int[] zoneDamageCounter;
        if (currPlayer == player)
        {
            zoneDamageCounter = AIGameState.playerZoneDamageCounter;
        }
        else
        {
            zoneDamageCounter = AIGameState.opponentZoneDamageCounter;
        }

        // If the cards health value has been modified, show it as cyan (if higher), yellow (if lower), or black if same as base value
        TextMeshProUGUI cardHealthBoxText = cardHealthBox.GetComponent<TextMeshProUGUI>();
        cardHealth = cardModifiedAttributes[2] - zoneDamageCounter[zoneNum];
        cardHealthBoxText.text = "" + cardHealth;
        if (cardHealth > card.GetCardHealth())
        {
            cardHealthBoxText.color = new Color(0, 226, 255);
        }
        else if (cardHealth < card.GetCardHealth())
        {
            cardHealthBoxText.color = Color.yellow;
        }
        else
        {
            cardHealthBoxText.color = Color.black;
        }

        // Uses the image file and displays it in the card art image game object
        Image cardArtImage = cardArt.GetComponent<Image>();
        Sprite artSprite = Resources.Load<Sprite>("Card images/" + card.GetCardArtFile());
        cardArtImage.sprite = artSprite;

        // Changes the cards background colour depending on the species
        cardType = card.GetCardType();

        if (cardType == "Feline")
        {
            cardBackgroundColour.color = new Color32(255, 180, 0, 255);
        }
        else if (cardType == "Canine")
        {
            cardBackgroundColour.color = new Color32(90, 110, 140, 255);
        }
        else if (cardType == "Ursidae")
        {
            cardBackgroundColour.color = new Color32(200, 140, 100, 255);
        }
        else if (cardType == "Reptilia")
        {
            cardBackgroundColour.color = new Color32(60, 90, 60, 255);
        }
        else if (cardType == "Delphindae")
        {
            cardBackgroundColour.color = new Color32(0, 100, 200, 255);
        }

        cardBorder.color = Color.black;
    }

    private int[] CardModifiedAttributes(string currPlayer, int cardZone)
    {
        // Initialises a card, 2d list for the modifiers, and 2d list for universal modifiers
        Card card;
        int[,] modifier = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int[,] universalModifier;

        // Selects the card using the zone the card is currently in
        if (currPlayer == player)
        {
            card = CardCollection.cardCollection[AIGameState.playerZoneCards[cardZone] - 1];
            universalModifier = AIGameState.playerUniversalModifiers;
        }
        else
        {
            card = CardCollection.cardCollection[AIGameState.opponentZoneCards[cardZone] - 1];
            universalModifier = AIGameState.opponentUniversalModifiers;
        }

        // Gets the original attack, armour, and health values of the card from the collection database
        int cardAttack = card.GetCardAttack();
        int cardArmour = card.GetCardArmour();
        int cardHealth = card.GetCardHealth();

        // Selects the correct modifier for the species the card is
        switch (card.GetCardType())
        {
            case "Feline":
                if (currPlayer == player)
                {
                    modifier = AIGameState.playerFelineModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentFelineModifiers;
                }
                break;

            case "Canine":
                if (currPlayer == player)
                {
                    modifier = AIGameState.playerCanineModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentCanineModifiers;
                }
                break;

            case "Ursidae":
                if (currPlayer == player)
                {
                    modifier = AIGameState.playerUrsidaeModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentUrsidaeModifiers;
                }
                break;

            case "Reptilia":
                if (currPlayer == player)
                {
                    modifier = AIGameState.playerReptiliaModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentReptiliaModifiers;
                }
                break;

            case "Delphindae":
                if (currPlayer == player)
                {
                    modifier = AIGameState.playerDelphindaeModifiers;
                }
                else
                {
                    modifier = AIGameState.opponentDelphindaeModifiers;
                }
                break;

        }

        // Sets the attribute modifiers appropriately using the cards species modifier plus the universal modifier
        cardAttackModifier = modifier[cardZone, 0] + universalModifier[cardZone, 0];
        cardArmourModifier = modifier[cardZone, 1] + universalModifier[cardZone, 1];
        cardHealthModifier = modifier[cardZone, 2] + universalModifier[cardZone, 2];

        // Adds the modifiers values to the cards attack, armour, and health
        cardAttack += modifier[cardZone, 0] + universalModifier[cardZone, 0];
        cardArmour += modifier[cardZone, 1] + universalModifier[cardZone, 1];
        cardHealth += modifier[cardZone, 2] + universalModifier[cardZone, 2];

        // Returns the modified attributes
        int[] modifiedAttributes = { cardAttack, cardArmour, cardHealth };
        return modifiedAttributes;
    }

    public void SetCardGlow(bool glow)
    {
        // Sets the card border colour depending on if we want the card to glow or not (cyan for glow, black for default no glow)
        if (glow == true)
        {
            cardBorder.color = new Color(0, 226, 255);
        }
        else
        {
            cardBorder.color = Color.black;
        }
    }
}
