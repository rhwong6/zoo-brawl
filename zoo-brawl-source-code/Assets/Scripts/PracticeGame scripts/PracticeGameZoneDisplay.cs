using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PracticeGameZoneDisplay : MonoBehaviour
{
    public int id;
    public string cardName;
    public string cardAbility;
    public int cardCost;
    public int cardAttack;
    public int cardArmour;
    public int cardHealth;
    public string cardType;

    public int cardAttackModifier;
    public int cardArmourModifier;
    public int cardHealthModifier;

    public GameObject cardNameBox;
    public GameObject cardAbilityBox;
    public GameObject cardCostBox;
    public GameObject cardAttackBox;
    public GameObject cardArmourBox;
    public GameObject cardHealthBox;

    public GameObject cardArt;
    public Image cardBackgroundColour;
    public Image cardBorder;

    const string player = "player";
    const string opponent = "opponent";

    public void loadCardAttributes(string currPlayer, Card card, int zoneNum)
    {
        this.id = card.GetCardID();

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
        cardCost = card.GetCardCost();
        cardCostBoxText.text = "" + cardCost;

        int[] cardModifiedAttributes = CardModifiedAttributes(currPlayer, zoneNum);

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

        int[] zoneDamageCounter;

        if (currPlayer == player)
        {
            zoneDamageCounter = PracticeGameState.playerZoneDamageCounter;
        }
        else
        {
            zoneDamageCounter = PracticeGameState.opponentZoneDamageCounter;
        }

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

        Image cardArtImage = cardArt.GetComponent<Image>();
        Sprite artSprite = Resources.Load<Sprite>("Card images/" + card.GetCardArtFile());
        cardArtImage.sprite = artSprite;

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
        Card card;
        int[,] modifier = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        int[,] universalModifier;

        if (currPlayer == player)
        {
            card = CardCollection.cardCollection[PracticeGameState.playerZoneCards[cardZone] - 1];
            universalModifier = PracticeGameState.playerUniversalModifiers;
        }
        else
        {
            card = CardCollection.cardCollection[PracticeGameState.opponentZoneCards[cardZone] - 1];
            universalModifier = PracticeGameState.opponentUniversalModifiers;
        }

        int cardAttack = card.GetCardAttack();
        int cardArmour = card.GetCardArmour();
        int cardHealth = card.GetCardHealth();

        switch (card.GetCardType())
        {
            case "Feline":
                if (currPlayer == player)
                {
                    modifier = PracticeGameState.playerFelineModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentFelineModifiers;
                }
                break;

            case "Canine":
                if (currPlayer == player)
                {
                    modifier = PracticeGameState.playerCanineModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentCanineModifiers;
                }
                break;

            case "Ursidae":
                if (currPlayer == player)
                {
                    modifier = PracticeGameState.playerUrsidaeModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentUrsidaeModifiers;
                }
                break;

            case "Reptilia":
                if (currPlayer == player)
                {
                    modifier = PracticeGameState.playerReptiliaModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentReptiliaModifiers;
                }
                break;

            case "Delphindae":
                if (currPlayer == player)
                {
                    modifier = PracticeGameState.playerDelphindaeModifiers;
                }
                else
                {
                    modifier = PracticeGameState.opponentDelphindaeModifiers;
                }
                break;

        }

        cardAttackModifier = modifier[cardZone, 0] + universalModifier[cardZone, 0];
        cardArmourModifier = modifier[cardZone, 1] + universalModifier[cardZone, 1];
        cardHealthModifier = modifier[cardZone, 2] + universalModifier[cardZone, 2];

        cardAttack += modifier[cardZone, 0] + universalModifier[cardZone, 0];
        cardArmour += modifier[cardZone, 1] + universalModifier[cardZone, 1];
        cardHealth += modifier[cardZone, 2] + universalModifier[cardZone, 2];

        int[] modifiedAttributes = { cardAttack, cardArmour, cardHealth };

        return modifiedAttributes;
    }

    public void SetCardGlow(bool glow)
    {
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
