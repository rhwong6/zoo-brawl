using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;


public class AIOpponent : MonoBehaviour
{
    // A memoisation table used by the knapsack algorithm for cards to play
    int[,] cardPlayDynamicTable;
    
    // Utility classes
    AIGameAbilityProcessor abilityProcessor;
    UpdateCardDisplayHandler cardDisplayHandler;
    BattleHandler battleHandler;

    // Linerenderer to display AI actions
    public GameObject aiLine;
    private LineRenderer lineRenderer;

    // Event to notify when the AI ends its turn
    public delegate void AIEndTurnHandler();
    public event AIEndTurnHandler AIEndTurn;

    private void Awake()
    {
        // Initialises utility classes
        abilityProcessor = new AIGameAbilityProcessor();
        cardDisplayHandler = new UpdateCardDisplayHandler();
        battleHandler = GetComponent<BattleHandler>();
        lineRenderer = aiLine.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.enabled = false;
    }

    public IEnumerator AIStartTurn()
    {
        // Plays cards then attacks player
        yield return StartCoroutine(PlayCards());
        yield return StartCoroutine(AttackPlayer());

        // Raises AI end turn event
        if (AIEndTurn != null)
        {
            AIEndTurn();
        }
    }

    private IEnumerator PlayCards()
    {
        int AIHandCards = AIGameState.opponentHandCards.Count;
        int AICoins = AIGameState.opponentCoins;

        // Initialises dynamic table for knapsack algorithm
        cardPlayDynamicTable = new int[AIHandCards + 1, AICoins + 1];
        for (int card = 0; card <= AIHandCards; card++)
        {
            for (int coins = 0; coins <= AICoins; coins++)
            {
                cardPlayDynamicTable[card, coins] = -1;
            }
        }

        // Stores the maximum total stats the AI can play usign the knapsack algorithm
        int totalStats = PlayCardKnapsack(AIHandCards, AICoins);
        int tempCoin = AICoins;

        List<Card> cardsToPlay = new List<Card>();

        // Backtracks the dynamic table the knapsack algorithm used to find which cards are in the knapsack with the maximum stat value (stores these cards in cardsToPlay list)
        for (int i = AIHandCards; i > 0 && totalStats > 0; i--)
        {
            if (totalStats == cardPlayDynamicTable[i - 1, tempCoin])
            {
                continue;
            }
            else
            {
                cardsToPlay.Add(AIGameState.opponentHandCards[i - 1]);

                totalStats = totalStats - AIGameState.opponentHandCards[i - 1].GetCardAttack() -
                                          AIGameState.opponentHandCards[i - 1].GetCardArmour() -
                                          AIGameState.opponentHandCards[i - 1].GetCardHealth();

                tempCoin = tempCoin - AIGameState.opponentHandCards[i - 1].GetCardCost();
            }
        }

        // Plays each card in the maximum knapsack
        foreach (Card playCard in cardsToPlay)
        {
            List<int> idealZone = IdealZone(playCard);

            foreach (int zoneNum in idealZone)
            {
                if (AIGameState.opponentZoneCards[zoneNum] == 0)
                {
                    yield return StartCoroutine(AIPlayCard(playCard, zoneNum));

                    break;
                }
            }
        }
    }

    // Recursive 0 1 knapsack algorithm that maximises the stats the AI can play with their current given amount of coins
    private int PlayCardKnapsack(int cardNum, int coins)
    {
        int result;

        if (cardPlayDynamicTable[cardNum, coins] != -1)
        {
            // If current value has been computed before, gets the value from the memoisation table
            result = cardPlayDynamicTable[cardNum, coins];
        }
        else if (cardNum == 0 || coins == 0)
        {
            // Base case
            result = 0;
        }
        else if (AIGameState.opponentHandCards[cardNum - 1].GetCardCost() > coins)
        {
            // If card cannot fit in knapsack (cost too much)
            result = PlayCardKnapsack(cardNum - 1, coins);
        }
        else
        {
            // Computes the value if the current card is included vs if the current card is ignored
            int withoutCard = PlayCardKnapsack(cardNum - 1, coins);
            int withCard = AIGameState.opponentHandCards[cardNum - 1].GetCardAttack() +
                           AIGameState.opponentHandCards[cardNum - 1].GetCardArmour() +
                           AIGameState.opponentHandCards[cardNum - 1].GetCardHealth() +
                           PlayCardKnapsack(cardNum - 1, coins - AIGameState.opponentHandCards[cardNum - 1].GetCardCost());

            // Selects the option with the higher stat value
            if (withCard > withoutCard)
            {
                result = withCard;
            }
            else
            {
                result = withoutCard;
            }
        }

        // Stores in memoisation table
        cardPlayDynamicTable[cardNum, coins] = result;

        return result;
    }

    // Takes a card to be played by the AI and returns the ideal zones (depending on their ability)
    private List<int> IdealZone(Card playCard)
    {
        List<int> idealZone = new List<int>();

        switch (CardCollection.cardAbility[playCard.GetCardID() - 1].actionTarget)
        {
            // If the card to be played has an ability that affects the facing enemy, adds the AI zone facing it to an ideal zone
            case CardAbility.Target.facingEnemy:
                int highestStatTarget = -1;
                int highestStatTargetValue = 0;
                for (int playerZone = 0; playerZone < 7; playerZone++)
                {
                    if (AIGameState.playerZoneCards[playerZone] != 0 && AIGameState.opponentZoneCards[playerZone] == 0)
                    {
                        // Prioritises the target with the highest stats
                        int targetStats = AIGameObjectManager.playerZoneCardDisplays[playerZone].cardAttack +
                                          AIGameObjectManager.playerZoneCardDisplays[playerZone].cardArmour +
                                          AIGameObjectManager.playerZoneCardDisplays[playerZone].cardHealth;

                        if (targetStats > highestStatTargetValue)
                        {
                            highestStatTargetValue = targetStats;
                            highestStatTarget = playerZone;
                        }
                    }
                }

                if (highestStatTarget != -1 && idealZone.Contains(highestStatTarget) == false)
                {
                    idealZone.Add(highestStatTarget);
                }

                break;
        }

        // If the card has an ability that modifies allies, selects ideal zone depending on the target they modify
        switch (CardCollection.cardAbility[playCard.GetCardID() - 1].modificationTarget)
        {
            case CardAbility.Target.adjacentAllies:

                for (int aiZone = 1; aiZone < 6; aiZone++)
                {
                    if (AIGameState.opponentZoneCards[aiZone - 1] != 0 && AIGameState.opponentZoneCards[aiZone + 1] != 0)
                    {
                        if (idealZone.Contains(aiZone) == false)
                        {
                            idealZone.Add(aiZone);
                        }
                    }
                }

                for (int aiZone = 1; aiZone < 6; aiZone++)
                {
                    if (AIGameState.opponentZoneCards[aiZone - 1] != 0 || AIGameState.opponentZoneCards[aiZone + 1] != 0)
                    {
                        if (idealZone.Contains(aiZone) == false)
                        {
                            idealZone.Add(aiZone);
                        }
                    }
                }

                for (int aiZone = 0; aiZone < 7; aiZone++)
                {
                    if (aiZone != 0)
                    {
                        if (AIGameState.opponentZoneCards[aiZone - 1] != 0)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }

                    if (aiZone != 6)
                    {
                        if (AIGameState.opponentZoneCards[aiZone + 1] != 0)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }
                }

                for (int aiZone = 1; aiZone < 6; aiZone++)
                {
                    if (idealZone.Contains(aiZone) == false)
                    {
                        idealZone.Add(aiZone);
                    }
                }

                break;

            default:

                for (int aiZone = 1; aiZone < 6; aiZone++)
                {
                    if (AIGameState.opponentZoneCards[aiZone - 1] != 0 && AIGameState.opponentZoneCards[aiZone + 1] != 0)
                    {
                        if (CardCollection.cardAbility[AIGameState.opponentZoneCards[aiZone - 1] - 1].modificationTarget == CardAbility.Target.adjacentAllies && CardCollection.cardAbility[AIGameState.opponentZoneCards[aiZone + 1] - 1].modificationTarget == CardAbility.Target.adjacentAllies)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }
                }

                for (int aiZone = 0; aiZone < 7; aiZone++)
                {
                    if (aiZone != 0)
                    {

                        if (AIGameState.opponentZoneCards[aiZone - 1] != 0 && CardCollection.cardAbility[AIGameState.opponentZoneCards[aiZone - 1] - 1].modificationTarget == CardAbility.Target.adjacentAllies)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }

                    if (aiZone != 6)
                    {
                        if (AIGameState.opponentZoneCards[aiZone + 1] != 0 && CardCollection.cardAbility[AIGameState.opponentZoneCards[aiZone + 1] - 1].modificationTarget == CardAbility.Target.adjacentAllies)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }
                }

                if (AIGameState.opponentZoneCards[1] == 0)
                {
                    if (idealZone.Contains(0) == false)
                    {
                        idealZone.Add(0);
                    }
                }

                if (AIGameState.opponentZoneCards[5] == 0)
                {
                    if (idealZone.Contains(6) == false)
                    {
                        idealZone.Add(6);
                    }
                }

                for (int aiZone = 1; aiZone < 6; aiZone++)
                {
                    if (AIGameState.opponentZoneCards[aiZone - 1] == 0 && AIGameState.opponentZoneCards[aiZone + 1] == 0)
                    {
                        if (idealZone.Contains(aiZone) == false)
                        {
                            idealZone.Add(aiZone);
                        }
                    }
                }

                for (int aiZone = 0; aiZone < 7; aiZone++)
                {
                    if (aiZone != 0)
                    {
                        if (AIGameState.opponentZoneCards[aiZone - 1] == 0)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }

                    if (aiZone != 6)
                    {
                        if (AIGameState.opponentZoneCards[aiZone + 1] == 0)
                        {
                            if (idealZone.Contains(aiZone) == false)
                            {
                                idealZone.Add(aiZone);
                            }
                        }
                    }
                }

                break;
        }

        // Adds the rest of the zones
        for (int otherZone = 0; otherZone < 7; otherZone++)
        {
            if (idealZone.Contains(otherZone) == false)
            {
                idealZone.Add(otherZone);
            }
        }

        return idealZone;
    }

    private IEnumerator AIPlayCard(Card card, int zoneNum)
    {
        // When AI plays a card, draw a line from where they play to the zone the are playing it to
        lineRenderer.enabled = true;

        Vector3 pos1 = AIGameObjectManager.opponentHandCardObjs[AIGameState.opponentHandCards.IndexOf(card)].transform.position;
        Vector3 pos2 = AIGameObjectManager.opponentZoneCardObjs[zoneNum].transform.position;

        pos1.z = 0;
        pos2.z = 0;

        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);

        // Displays what the AI is playing in the log
        AIGameObjectManager.logTextObj.text = "AI playing " + card.GetCardName();

        // Waits a second before playing the card (updates the displays as well)
        yield return new WaitForSeconds(1f);

        AIGameState.opponentZoneCards[zoneNum] = card.GetCardID();
        AIGameState.opponentZoneAnimalAwake[zoneNum] = 1;
        abilityProcessor.OnPlayAbility("opponent", CardCollection.cardAbility[card.GetCardID() - 1], zoneNum);
        AIGameState.opponentHandCards.Remove(card);

        cardDisplayHandler.UpdateZoneDisplay("opponent");
        cardDisplayHandler.UpdateHandDisplay("opponent");

        lineRenderer.enabled = false;
    }

    private IEnumerator AttackPlayer()
    {
        // Stores the list of cards on the AI and player zones
        List<AIGameZoneDisplay> AIZoneCards = new List<AIGameZoneDisplay>();
        List<AIGameZoneDisplay> playerZoneCards = new List<AIGameZoneDisplay>();

        Tuple<List<AIGameZoneDisplay>, List<AIGameZoneDisplay>> zoneCards = GetZoneCards();
        AIZoneCards = zoneCards.Item1;
        playerZoneCards = zoneCards.Item2;

        int AIZoneCardNum = AIZoneCards.Count;
        int playerZoneCardNum = playerZoneCards.Count;

        // Variables that show how much damage the current state for the AI and player can deal
        int AIZoneAttackPotential = 0;
        int playerZoneAttackPotential = 0;

        for (int zone = 0; zone < 7; zone++)
        {
            if (AIGameState.opponentZoneCards[zone] != 0 && AIGameState.opponentZoneAnimalAwake[zone] == 2)
            {
                AIZoneAttackPotential += AIGameObjectManager.opponentZoneCardDisplays[zone].cardAttack;
            }

            if (AIGameState.playerZoneCards[zone] != 0)
            {
                playerZoneAttackPotential += AIGameObjectManager.playerZoneCardDisplays[zone].cardAttack;
            }
        }

        // Depth tells the AI how many attackers they should consider on one of the players card, needDefend tells the AI the current game state requires them to defend aggressively, as it is in a losing position
        int AIAttackDepth = 3;
        bool needDefend = false;

        if (AIZoneAttackPotential >= AIGameState.playerBaseHealth)
        {
            // If the AI zones total attack can destroy the opponent base, it will do so to win the game
            yield return StartCoroutine(AIBaseAttack());
        }
        else
        {
            // If the players total attack can destroy the AI's base, increases the depth to consider more attacks, and sets needDefend to true (used in BestAttacker method to defend aggressively)
            if (playerZoneAttackPotential >= AIGameState.opponentBaseHealth)
            {
                AIAttackDepth = 6;
                needDefend = true;
            }

            for (int depth = 0; depth < AIAttackDepth; depth++)
            {
                if (AIZoneCardNum != 0 && playerZoneCardNum != 0)
                {
                    // Uses Permute function to get all permutations of the players current zone cards (so that each order to attack is considered to find the optimal one)
                    List<List<AIGameZoneDisplay>> playerCardPermutations = Permute(playerZoneCards);
                    List<AIGameZoneDisplay> bestPlayerZoneOrder = new List<AIGameZoneDisplay>();

                    int[] bestOverallAttackers = new int[playerZoneCardNum];
                    int[] bestOverallAttackerValues = new int[playerZoneCardNum];
                    bool[] bestOverallAttacked = new bool[AIZoneCardNum];

                    int bestTotalValue = int.MinValue;

                    // Initalises attackers to -1
                    for (int attackers = 0; attackers < playerZoneCardNum; attackers++)
                    {
                        bestOverallAttackers[attackers] = -1;
                    }

                    // For each permutation, compute the best attackers using the AI's current zone cards using the BestAttacker function
                    foreach (List<AIGameZoneDisplay> playerCardPermutation in playerCardPermutations)
                    {
                        Tuple<int[], int[], bool[]> bestAttacker = BestAttacker(AIZoneCards, playerCardPermutation, needDefend);

                        // If current permutation gives the best value in terms of stats, sets it to the best
                        if (bestAttacker.Item2.Sum() >= bestTotalValue)
                        {
                            bestTotalValue = bestAttacker.Item2.Sum();

                            bestOverallAttackers = bestAttacker.Item1;
                            bestOverallAttackerValues = bestAttacker.Item2;
                            bestOverallAttacked = bestAttacker.Item3;

                            bestPlayerZoneOrder = playerCardPermutation.ConvertAll(zoneCard => zoneCard);
                        }
                    }

                    // Parses results of each permutation and attacks the apponent zone cards using the best attacker values
                    int[] parsedAttacks = ParseAttacks(playerZoneCards, bestPlayerZoneOrder, bestOverallAttackers);
                    yield return StartCoroutine(PerformZoneAttacks(parsedAttacks));

                    // Process is repeated for each depth, so use new zone cards after attacks
                    AIZoneCards.Clear();
                    AIZoneCards.Clear();

                    Tuple<List<AIGameZoneDisplay>, List<AIGameZoneDisplay>> newZoneCards = GetZoneCards();

                    AIZoneCards = newZoneCards.Item1;
                    playerZoneCards = newZoneCards.Item2;

                    AIZoneCardNum = AIZoneCards.Count();
                    playerZoneCardNum = playerZoneCards.Count();
                }
            }

            // Attacks the opponents base with the rest of the cards
            if (AIZoneCardNum != 0)
            {
                yield return StartCoroutine(AIBaseAttack());
            }
        }
    }

    // Function that returns list of AI and player zone cards
    private Tuple<List<AIGameZoneDisplay>, List<AIGameZoneDisplay>> GetZoneCards()
    {
        List<AIGameZoneDisplay> AIZoneCards = new List<AIGameZoneDisplay>();
        List<AIGameZoneDisplay> playerZoneCards = new List<AIGameZoneDisplay>();

        for (int zone = 0; zone < 7; zone++)
        {
            if (AIGameState.opponentZoneCards[zone] != 0)
            {
                AIZoneCards.Add(AIGameObjectManager.opponentZoneCardDisplays[zone]);
            }

            if (AIGameState.playerZoneCards[zone] != 0)
            {
                playerZoneCards.Add(AIGameObjectManager.playerZoneCardDisplays[zone]);
            }
        }

        return Tuple.Create(AIZoneCards, playerZoneCards);
    }

    // Function that computes the best attackers for the players current zone cards
    private Tuple<int[], int[], bool[]> BestAttacker(List<AIGameZoneDisplay> AIZoneCards, List<AIGameZoneDisplay> playerZoneCards, bool needDefend)
    {
        int AIZoneCardNum = AIZoneCards.Count;
        int playerZoneCardNum = playerZoneCards.Count;

        bool[] AIZoneAttacked = new bool[AIZoneCardNum];

        int[] bestAttackers = new int[playerZoneCardNum];
        int[] bestAttackerValues = new int[playerZoneCardNum];

        for (int attacker = 0; attacker < playerZoneCardNum; attacker++)
        {
            bestAttackers[attacker] = -1;
        }

        // For each players card, finds the best AI zone card attacker (best means outputs the best AI card stat minus player card stat after battle)
        for (int playerCard = 0; playerCard < playerZoneCardNum; playerCard++)
        {
            bool hasAttacker = false;

            bestAttackerValues[playerCard] = -1000;
            for (int aiCard = 0; aiCard < AIZoneCardNum; aiCard++)
            {
                // Finds the zone of the current AI card in the list
                int currAICardZone = 0;
                for (int aiZone = 0; aiZone < 7; aiZone++)
                {
                    if (AIGameState.opponentZoneCards[aiZone] == AIZoneCards[aiCard].id)
                    {
                        currAICardZone = aiZone;
                    }
                }

                // Will only consider the AI card if it can attack so not already attacked or just played (animals need to wait a turn before attacking in the games rules)
                if (AIZoneAttacked[aiCard] == false && AIGameState.opponentZoneAnimalAwake[currAICardZone] == 2)
                {
                    // Calculate the total stat value of the battle (AI card total stats minus player card total stats after battle)
                    int newAttackValue = CalculateAfterBattleValue(AIZoneCards[aiCard], playerZoneCards[playerCard]);

                    // Will only consider attacking if the value is not negative, and better than all other cards that can attack
                    // Or if the AI is in a game losing state, just considers the best cards for attack even if attacks are generally unfavourable
                    if (newAttackValue > bestAttackerValues[playerCard] && newAttackValue >= 0 || needDefend && newAttackValue > bestAttackerValues[playerCard])
                    {
                        bestAttackerValues[playerCard] = newAttackValue;
                        bestAttackers[playerCard] = aiCard;
                    }

                    hasAttacker = true;
                }
            }
            
            // Sets the best card to attack's attacked variable (to prevent attacking again)
            if (hasAttacker && bestAttackers[playerCard] != -1)
            {
                AIZoneAttacked[bestAttackers[playerCard]] = true;
            }

        }

        return Tuple.Create(bestAttackers, bestAttackerValues, AIZoneAttacked);
    }

    // Function that computes the stat value of an attack
    private int CalculateAfterBattleValue(AIGameZoneDisplay startCard, AIGameZoneDisplay endCard)
    {
        // Consider attack modifiers from abilities
        Tuple<int[,], int[,]> attackModifiers = abilityProcessor.OnAttack("opponent", startCard, endCard, CardCollection.cardAbility[startCard.id - 1]);

        int[,] attackerModifers = attackModifiers.Item1;
        int[,] defenderModifiers = attackModifiers.Item2;

        int startCardAttack = startCard.cardAttack + attackerModifers[0, 0];
        int startCardArmour = startCard.cardArmour + attackerModifers[1, 1];

        int endCardAttack = endCard.cardAttack;
        int endCardArmour = endCard.cardArmour + defenderModifiers[1, 1];

        int startCardDamage = 0;
        int endCardDamage = 0;

        // Computes damage each card takes taking into consideration the cards current armour
        if (startCardArmour < endCardAttack)
        {
            startCardDamage = endCardAttack - startCardArmour;
        }

        if (endCardArmour < startCardAttack)
        {
            endCardDamage = startCardAttack - endCardArmour;
        }

        int[] damage = { startCardDamage, endCardDamage };

        // Computes the battling cards total stats after the battle
        int aiCardValue = 0;
        int playerCardValue = 0;

        if (damage[0] < (CardCollection.cardCollection[startCard.id - 1].GetCardHealth() + startCard.cardHealthModifier))
        {
            aiCardValue = startCard.cardAttack + startCard.cardArmour + CardCollection.cardCollection[startCard.id - 1].GetCardHealth() + startCard.cardHealthModifier - damage[0];
        }

        if (damage[1] < (CardCollection.cardCollection[endCard.id - 1].GetCardHealth() + endCard.cardHealthModifier))
        {
            playerCardValue = endCard.cardAttack + endCard.cardArmour + CardCollection.cardCollection[endCard.id - 1].GetCardHealth() + endCard.cardHealthModifier - damage[1];
        }

        // The total value is then the outcome AI card stats minus the players outcome stats
        int battleValue = aiCardValue - playerCardValue;

        return battleValue;
    }

    // Permute, DoPermute, and Swap is a modified version of:
    // www.chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp by Chad (June 21st 2019) which recursively finds all permutations of an array of integers
    // This modified version returns the list of all permutation of a list of cards
    private static List<List<AIGameZoneDisplay>> Permute(List<AIGameZoneDisplay> cards)
    {
        var list = new List<List<AIGameZoneDisplay>>();
        return DoPermute(cards, 0, cards.Count - 1, list);
    }

    // Recursive helper method
    private static List<List<AIGameZoneDisplay>> DoPermute(List<AIGameZoneDisplay> cards, int start, int end, List<List<AIGameZoneDisplay>> list)
    {
        if (start == end)
        {
            // All possible combinations have been added
            list.Add(new List<AIGameZoneDisplay>(cards));
        }
        else
        {
            // Recusively swaps elements in range with the first element
            for (var i = start; i <= end; i++)
            {
                Swap(cards, start, i);
                DoPermute(cards, start + 1, end, list);
                Swap(cards, start, i);
            }
        }

        return list;
    }

    // Helper method that swaps two elements in a list
    private static void Swap(List<AIGameZoneDisplay> cards, int i, int j)
    {
        var temp = cards[i];
        cards[i] = cards[j];
        cards[j] = temp;
    }

    // Function that parses the best cards to attack to match the original permutation of player cards
    private int[] ParseAttacks(List<AIGameZoneDisplay> originalPlayerZoneOrder, List<AIGameZoneDisplay> bestPlayerZoneOrder, int[] bestOverallAttackers)
    {
        int[] opponentZoneAttacks = new int[bestOverallAttackers.Length];

        for (int i = 0; i < bestOverallAttackers.Length; i++)
        {
            opponentZoneAttacks[i] = -1;
        }

        // Sets the current set of best attacks to match the original permutation of player cards
        for (int bestAttacker = 0; bestAttacker < bestOverallAttackers.Length; bestAttacker++)
        {
            if (bestOverallAttackers[bestAttacker] != -1)
            {
                opponentZoneAttacks[originalPlayerZoneOrder.IndexOf(bestPlayerZoneOrder[bestAttacker])] = bestOverallAttackers[bestAttacker];
            }
        }

        // Parses the player zone attacks into an array that shows cards on the AI's zone and which cards they will attack
        int[] aiAttacks = new int[7];

        for (int zone = 0; zone < 7; zone++)
        {
            aiAttacks[zone] = -1;
        }

        for (int attackPlayer = 0; attackPlayer < opponentZoneAttacks.Length; attackPlayer++)
        {
            if (opponentZoneAttacks[attackPlayer] != -1)
            {
                int aiCard = opponentZoneAttacks[attackPlayer];
                int aiZone = 0;

                for (int zone = 0; zone < 7; zone++)
                {
                    if (AIGameState.opponentZoneCards[zone] != 0)
                    {
                        if (aiCard == aiZone)
                        {
                            aiAttacks[zone] = attackPlayer;
                            break;
                        }
                        else
                        {
                            aiZone++;
                        }
                    }
                }
            }
        }

        return aiAttacks;
    }

    private IEnumerator PerformZoneAttacks(int[] aiAttacks)
    {
        int[] tempZone = (int[])AIGameState.playerZoneCards.Clone();

        // Goes through each zone to check if the AI card needs to attack
        for (int aiAttackZone = 0; aiAttackZone < 7; aiAttackZone++)
        {
            if (aiAttacks[aiAttackZone] != -1)
            {
                // If it does need to attack, go through the player zones to find the actual zone of the card and attacks it
                int playerCard = 0;
                for (int playerZone = 0; playerZone < 7; playerZone++)
                {
                    if (tempZone[playerZone] != 0)
                    {
                        if (playerCard == aiAttacks[aiAttackZone])
                        {
                            yield return StartCoroutine(AIAttackPlayerZone(aiAttackZone, playerZone));
                            break;
                        }
                        else
                        {
                            playerCard++;
                        }
                    }
                }
            }
        }

    }

    private IEnumerator AIAttackPlayerZone(int aiCardZone, int playerCardZone)
    {
        // Uses the linerenderer to display a line going from the card AI is using to attack to the players card it is attacking
        Vector3 pos1 = AIGameObjectManager.opponentZoneCardObjs[aiCardZone].transform.position;
        Vector3 pos2 = AIGameObjectManager.playerZoneCardObjs[playerCardZone].transform.position;

        pos1.z = 0;
        pos2.z = 0;

        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);

        lineRenderer.enabled = true;

        // Displays what the AI is attacking with which card in the log
        AIGameObjectManager.logTextObj.text = "AI " +  CardCollection.cardCollection[AIGameState.opponentZoneCards[aiCardZone] - 1].GetCardName() + " attacking " + CardCollection.cardCollection[AIGameState.playerZoneCards[playerCardZone] - 1].GetCardName();

        // After waiting a second, attacks the players card
        yield return new WaitForSeconds(1f);

        battleHandler.CardsBattle(aiCardZone, playerCardZone);
        cardDisplayHandler.UpdateZoneDisplay("player");
        cardDisplayHandler.UpdateZoneDisplay("opponent");

        lineRenderer.enabled = false;
    }

    private IEnumerator AIBaseAttack()
    {
        //Checks which cards the AI can use to attack the players base and attacks

        for (int aiZone = 0; aiZone < 7; aiZone++)
        {
            if (AIGameState.opponentZoneCards[aiZone] != 0 && AIGameState.opponentZoneAnimalAwake[aiZone] == 2)
            {
                yield return StartCoroutine(AttackPlayerBase(aiZone));
            }
        }
    }

    private IEnumerator AttackPlayerBase(int aiZone)
    {
        // Uses the linerenderer to display a line going from the card AI is using to attack to the players base
        Vector3 pos1 = AIGameObjectManager.opponentZoneCardObjs[aiZone].transform.position;
        Vector3 pos2 = AIGameObjectManager.playerBaseObj.transform.position;

        pos1.z = 0;
        pos2.z = 0;

        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);

        lineRenderer.enabled = true;

        // Displays which card AI is using to attack your base
        AIGameObjectManager.logTextObj.text = "AI " + CardCollection.cardCollection[AIGameState.opponentZoneCards[aiZone] - 1].GetCardName() + " attacking your base!";

        // After a second, attack the players base with AI card in current zone
        yield return new WaitForSeconds(1f);
        battleHandler.AttackBase("opponent", aiZone);

        lineRenderer.enabled = false;
    }
}
