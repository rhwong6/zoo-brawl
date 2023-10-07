using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DeckManager : MonoBehaviour
{
    public static int deckNum;

    // Initialises an array to store deck names, and a list of decks to store decks
    public static string[] deckNames = new string[8];
    public static List<Deck> decks = new List<Deck>();

    // Boolean to check if decks have been initialised
    private static bool decksInit = false;

    private void Awake()
    {
        // If decks have not been initialised, initialises decks
        if (decksInit == false)
        {
            for (int d = 0; d < 8; d++)
            {
                decks.Add(new Deck());
            }

            decksInit = true;
        }

    }

    void Start()
    {
        // Initialises the path for the save file
        string savePath = Application.persistentDataPath + "/decks.txt";

        // If the file exists, reads the contents to load the cards in each created deck
        if (File.Exists(savePath))
        {
            StreamReader reader = new StreamReader(savePath);

            for (int currDeck = 0; currDeck < decks.Count; currDeck++)
            {
                deckNames[currDeck] = reader.ReadLine();

                string[] cardIDs = reader.ReadLine().Split(' ');

                for (int i = 0; i < decks[currDeck].deckCards.Length; i++)
                {
                    decks[currDeck].deckCards[i] = int.Parse(cardIDs[i]);
                }

            }
        }

    }

}
