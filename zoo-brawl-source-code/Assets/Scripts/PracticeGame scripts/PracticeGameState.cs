using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeGameState : MonoBehaviour
{
    public static List<Card> playerDeckCards = new List<Card>();
    public static List<Card> playerHandCards = new List<Card>();
    public static int[] playerZoneCards = new int[7];
    public static int[] playerZoneDamageCounter = new int[7];

    public static List<Card> opponentDeckCards = new List<Card>();
    public static List<Card> opponentHandCards = new List<Card>();
    public static int[] opponentZoneCards = new int[7];
    public static int[] opponentZoneDamageCounter = new int[7];

    public static string turn;
    public static int turnCounter;

    public static int playerCoins;
    public static int opponentCoins;

    public static bool[] playerHandPlayable = new bool[6];
    public static bool[] opponentHandPlayable = new bool[6];

    public static int[] playerZoneAnimalAwake = new int[7];
    public static int[] opponentZoneAnimalAwake = new int[7];

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

    public static int playerBaseHealth;
    public static int opponentBaseHealth;
}
