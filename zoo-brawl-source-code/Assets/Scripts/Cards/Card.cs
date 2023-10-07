using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // Variables for the various attributes for the card and the file for the card art
    private int id;
    private string cardName;
    private string cardAbility;
    private int cardCost;
    private int cardAttack;
    private int cardArmour;
    private int cardHealth;
    private string cardType;
    private string cardArt;

    // Default constructor of a card object
    public Card()
    {
        id = 0;
        cardName = "";
        cardAbility = "";
        cardCost = 0;
        cardAttack = 0;
        cardArmour = 0;
        cardHealth = 0;
        cardType = "";
        cardArt = "";
    }

    // Constructor for a card object with specified values
    public Card(int id, string cardName, string cardAbility, int cardCost, int cardAttack, int cardArmour, int cardHealth, string cardType, string cardArt)
    {
        this.id = id;
        this.cardName = cardName;
        this.cardAbility = cardAbility;
        this.cardCost = cardCost;
        this.cardAttack = cardAttack;
        this.cardArmour = cardArmour;
        this.cardHealth = cardHealth;
        this.cardType = cardType;
        this.cardArt = cardArt;
    }

    // Below are all getter functions for each of the attributes of the card and card art file
    public int GetCardID()
    {
        return id;
    }

    public string GetCardName()
    {
        return cardName;
    }

    public string GetCardAbility()
    {
        return cardAbility;
    }

    public int GetCardCost()
    {
        return cardCost;
    }

    public int GetCardAttack()
    {
        return cardAttack;
    }

    public int GetCardArmour()
    {
        return cardArmour;
    }

    public int GetCardHealth()
    {
        return cardHealth;
    }

    public string GetCardType()
    {
        return cardType;
    }

    public string GetCardArtFile()
    {
        return cardArt;
    }
}
