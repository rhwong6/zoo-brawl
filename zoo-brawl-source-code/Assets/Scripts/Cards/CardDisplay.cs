using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    // Variables to store the attributes of the card being displayed
    public int id;
    string cardName;
    string cardAbility;
    int cardCost;
    int cardAttack;
    int cardArmour;
    int cardHealth;
    string cardType;

    // References the attribute text game objects
    public GameObject cardNameBox;
    public GameObject cardAbilityBox;
    public GameObject cardCostBox;
    public GameObject cardAttackBox;
    public GameObject cardArmourBox;
    public GameObject cardHealthBox;

    // References the gameobjects that displays the card art, background colour, and border
    public GameObject cardArt;
    public Image cardBackgroundColour;
    public Image cardBorder;

    public void loadCardAttributes(Card card)
    {
        // Loads each attribute from the card collection database and sets values of these attributes
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

        TextMeshProUGUI cardAttackBoxText = cardAttackBox.GetComponent<TextMeshProUGUI>();
        cardAttackBoxText.color = Color.black;
        cardAttack = card.GetCardAttack();
        cardAttackBoxText.text = "" + cardAttack;

        TextMeshProUGUI cardArmourBoxText = cardArmourBox.GetComponent<TextMeshProUGUI>();
        cardArmourBoxText.color = Color.black;
        cardArmour = card.GetCardArmour();
        cardArmourBoxText.text = "" + cardArmour;

        TextMeshProUGUI cardHealthBoxText = cardHealthBox.GetComponent<TextMeshProUGUI>();
        cardHealthBoxText.color = Color.black;
        cardHealth = card.GetCardHealth();
        cardHealthBoxText.text = "" + cardHealth;

        // Uses the image file and displays it in the card art image game object
        Image cardArtImage = cardArt.GetComponent<Image>();
        Sprite artSprite = Resources.Load<Sprite>("Card images/" + card.GetCardArtFile());
        cardArtImage.sprite = artSprite;

        // Changes the cards background colour depending on the species
        if (card.GetCardType() == "Feline")
        {
            cardType = "Feline";
            cardBackgroundColour.color = new Color32(255, 180, 0, 255);
        }
        else if (card.GetCardType() == "Canine")
        {
            cardType = "Canine";
            cardBackgroundColour.color = new Color32(90, 110, 140, 255);
        }
        else if (card.GetCardType() == "Ursidae")
        {
            cardType = "Ursidae";
            cardBackgroundColour.color = new Color32(200, 140, 100, 255);
        }
        else if (card.GetCardType() == "Reptilia")
        {
            cardType = "Reptilia";
            cardBackgroundColour.color = new Color32(60, 90, 60, 255);
        }
        else if (card.GetCardType() == "Delphindae")
        {
            cardType = "Delphindae";
            cardBackgroundColour.color = new Color32(0, 100, 200, 255);
        }

        cardBorder.color = Color.black;
    }

    // Sets the card border colour depending on if we want the card to glow or not (cyan for glow, black for default no glow)
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
