using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGameState : MonoBehaviour
{
    // Boolean variable that determines if the AI cover their hand cards
    public static bool aiCoverCards = true;

    // Variables to store cards in the players deck, hand, zone and the damage counters for the cards in the zone
    public static List<Card> playerDeckCards = new List<Card>();
    public static List<Card> playerHandCards = new List<Card>();
    public static int[] playerZoneCards = new int[7];
    public static int[] playerZoneDamageCounter = new int[7];

    // Variables to store cards in the opponents deck, hand, zone and the damage counters for the cards in the zone
    public static List<Card> opponentDeckCards = new List<Card>();
    public static List<Card> opponentHandCards = new List<Card>();
    public static int[] opponentZoneCards = new int[7];
    public static int[] opponentZoneDamageCounter = new int[7];

    // Variable to determine who's turn it is and a counter for how many turns have passed
    public static string turn;
    public static int turnCounter;

    // Variables to store the amount of coins the player and opponent has
    public static int playerCoins;
    public static int opponentCoins;

    // Variable to store if cards in the players and opponents hand are playable
    public static bool[] playerHandPlayable = new bool[6];
    public static bool[] opponentHandPlayable = new bool[6];

    // Variables to store the awake values of the cards on the players and opponents zones
    // (0 means no card is on that zone, 1 means that the card has just been played so cannot attack yet, 2 means ready to attack, 3 means stunned and cannot attack until next turn)
    public static int[] playerZoneAnimalAwake = new int[7];
    public static int[] opponentZoneAnimalAwake = new int[7];

    // 2d arrays to store the current modifiers for cards on the players and opponents zone cards
    public static int[,] playerUniversalModifiers = new int[7, 3];
    public static int[,] opponentUniversalModifiers = new int[7, 3];

    public static int[,] playerFelineModifiers = new int[7, 3];
    public static int[,] opponentFelineModifiers = new int[7, 3];

    public static int[,] playerCanineModifiers = new int[7, 3];
    public static int[,] opponentCanineModifiers = new int[7, 3];

    public static int[,] playerUrsidaeModifiers = new int[7, 3];
    public static int[,] opponentUrsidaeModifiers = new int[7, 3];

    public static int[,] playerReptiliaModifiers = new int[7, 3];
    public static int[,] opponentReptiliaModifiers = new int[7, 3];

    public static int[,] playerDelphindaeModifiers = new int[7, 3];
    public static int[,] opponentDelphindaeModifiers = new int[7, 3];

    // Variables to store the amount of health the players and opponents base has
    public static int playerBaseHealth;
    public static int opponentBaseHealth;
}
