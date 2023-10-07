using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck
{
    public int[] deckCards = new int[10];
    int deckLength = 0;

    public int addCard(int cardID)
    {
        // Adds card to deck if not already in deck, or if deck length is not 10
        if (deckCards.Contains(cardID) == false && deckLength != 10)
        {
            // Checks the first empty deck slot for the card and adds the card to that slot
            int emptySpace = 0;
            while (deckCards[emptySpace] != 0)
            {
                emptySpace++;
            }

            deckCards[emptySpace] = cardID;
            deckLength++;

            // Returns 1 if successful, and 0 otherwise
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int removeCard(int cardID)
    {
        // If card is in the deck, removes the card
        if (deckCards.Contains(cardID))
        {
            int cardLocation = 0;
            while (deckCards[cardLocation] != cardID)
            {
                cardLocation++;
            }
            deckCards[cardLocation] = 0;
            deckLength--;

            // Returns 1 if successful, and 0 otherwise
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // Returns the decks length
    public int getDeckLength()
    {
        return deckLength;
    }

    // Returns the cards in the deck
    public int[] getDeckCards()
    {
        return deckCards;
    }
}
