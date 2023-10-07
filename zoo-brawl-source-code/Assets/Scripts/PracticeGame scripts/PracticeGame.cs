using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class PracticeGame : MonoBehaviour
{
    public TextMeshProUGUI logTextObject;
    public TextMeshProUGUI turnTextObject;
    public Button endTurnButton;

    private PracticePlayerDropZone practicePlayerDropZone;
    private PracticeOpponentDropZone practiceOpponentDropZone;

    public TextMeshProUGUI playerCoinAmountObj;
    public TextMeshProUGUI opponentCoinAmountObj;

    public TextMeshProUGUI playerBaseTextObj;
    public TextMeshProUGUI opponentBaseTextObj;

    public GameObject winPopup;
    public TextMeshProUGUI wonPlayerText;

    PracticeGameCardGlow cardGlowManager;
    PracticeGameAbilityProcessor abilityProcessor;

    const string player = "player";
    const string opponent = "opponent";

    private void Awake()
    {
        cardGlowManager = new PracticeGameCardGlow();
        abilityProcessor = new PracticeGameAbilityProcessor();
    }

    void Start()
    {
        practicePlayerDropZone = FindObjectOfType<PracticePlayerDropZone>();
        practicePlayerDropZone.CardAddedToZone += OnCardAddedToPlayerZone;

        practiceOpponentDropZone = FindObjectOfType<PracticeOpponentDropZone>();
        practiceOpponentDropZone.CardAddedToOpponentZone += OnCardAddedToOpponentZone;



        PracticeGameInteractDraggable.CardsBattle += OnCardsBattle;
        PracticeGameInteractDraggable.AttackBase += OnAttackBase;

        StartGameSetup();
    }

    void Update()
    {

    }

    void StartGameSetup()
    {
        winPopup.SetActive(false);

        PracticeGameState.playerHandCards.Clear();
        PracticeGameState.opponentHandCards.Clear();

        for (int i = 0; i < 6; i++)
        {
            PracticeGameState.playerHandPlayable[i] = false;
            PracticeGameState.opponentHandPlayable[i] = false;
        }

        for (int i = 0; i < 7; i++)
        {
            PracticeGameState.playerZoneCards[i] = 0;
            PracticeGameState.opponentZoneCards[i] = 0;

            PracticeGameState.playerZoneAnimalAwake[i] = 0;
            PracticeGameState.opponentZoneAnimalAwake[i] = 0;
        }

        PracticeGameState.turnCounter = 1;

        PracticeGameState.playerBaseHealth = 20;
        PracticeGameState.opponentBaseHealth = 20;

        playerBaseTextObj.text = PracticeGameState.playerBaseHealth.ToString();
        opponentBaseTextObj.text = PracticeGameState.opponentBaseHealth.ToString();

        PracticeGameState.playerCoins = (int)(Math.Ceiling((double)PracticeGameState.turnCounter / 2) + 2);
        PracticeGameState.opponentCoins = (int)(Math.Ceiling((double)PracticeGameState.turnCounter / 2) + 2);

        playerCoinAmountObj.text = PracticeGameState.playerCoins.ToString();
        opponentCoinAmountObj.text = PracticeGameState.opponentCoins.ToString();

        ShuffleDeck(player);
        ShuffleDeck(opponent);

        UpdateHandDisplay(player);
        UpdateHandDisplay(opponent);

        UpdateZoneDisplay(player);
        UpdateZoneDisplay(opponent);

        cardGlowManager.UpdateZoneGlow(player);
        cardGlowManager.UpdateZoneGlow(opponent);

        cardGlowManager.UpdateHandGlow(player);
        cardGlowManager.UpdateHandGlow(opponent);

        for (int i = 0; i < 3; i++)
        {
            DrawCard(player);
            DrawCard(opponent);
        }

        RandomPlayerTurn();
    }

    public void ShuffleDeck(string player)
    {
        List<Card> deckCards;

        if (player == "player")
        {
            deckCards = PracticeGameState.playerDeckCards;
        }
        else
        {
            deckCards = PracticeGameState.opponentDeckCards;
        }

        System.Random rng = new System.Random();

        int cardNum = deckCards.Count;
        while (cardNum > 1)
        {
            cardNum--;
            int randSlot = rng.Next(cardNum + 1);
            Card temp = deckCards[randSlot];
            deckCards[randSlot] = deckCards[cardNum];
            deckCards[cardNum] = temp;
        }

        /*
        string cardList = "DECK AFTER SHUFFLE: ";
        foreach (Card card in deckCards)
        {
            cardList = cardList + ", " + card.getCardName();
        }
        Debug.Log(cardList);
        */
    }

    public void UpdateHandDisplay(string player)
    {
        List<Card> hand;
        List<GameObject> handObjs;
        List<CardDisplay> handDisplays;

        if (player == "player")
        {
            hand = PracticeGameState.playerHandCards;
            handObjs = PracticeGameObjectManager.playerHandCardObjs;
            handDisplays = PracticeGameObjectManager.playerHandCardDisplays;
        }
        else
        {
            hand = PracticeGameState.opponentHandCards;
            handObjs = PracticeGameObjectManager.opponentHandCardObjs;
            handDisplays = PracticeGameObjectManager.opponentHandCardDisplays;
        }

        foreach (GameObject handCardObj in handObjs)
        {
            handCardObj.SetActive(false);
        }

        for (int handCard = 0; handCard < hand.Count; handCard++)
        {
            handObjs[handCard].SetActive(true);
            handDisplays[handCard].loadCardAttributes(hand[handCard]);
        }

        /*
        string cardList = "HAND: ";
        foreach (Card card in hand)
        {
            cardList = cardList + ", " + card.getCardName();
        }
        Debug.Log(cardList);
        */
    }

    public void UpdateZoneDisplay(string currPlayer)
    {
        int[] zone;
        List<GameObject> zoneObjs;
        List<PracticeGameZoneDisplay> zoneDisplays;

        if (currPlayer == player)
        {
            zone = PracticeGameState.playerZoneCards;
            zoneObjs = PracticeGameObjectManager.playerZoneCardObjs;
            zoneDisplays = PracticeGameObjectManager.playerZoneCardDisplays;
        }
        else
        {
            zone = PracticeGameState.opponentZoneCards;
            zoneObjs = PracticeGameObjectManager.opponentZoneCardObjs;
            zoneDisplays = PracticeGameObjectManager.opponentZoneCardDisplays;
        }

        foreach (GameObject animalZoneObj in zoneObjs)
        {
            animalZoneObj.SetActive(false);
        }

        for (int zoneCard = 0; zoneCard < zone.Length; zoneCard++)
        {
            if (zone[zoneCard] != 0)
            {
                zoneObjs[zoneCard].SetActive(true);
                zoneDisplays[zoneCard].loadCardAttributes(currPlayer, CardCollection.cardCollection[zone[zoneCard] - 1], zoneCard);
            }
        }

        /*
        string cardList = "ZONE: ";
        foreach (int cardID in zone)
        {
            if (cardID != 0)
            {
                cardList = cardList + ", " + CardCollection.cardCollection[cardID - 1].getCardID();
            }
            else
            {
                cardList += ", empty";
            }
        }
        Debug.Log(cardList);
        */
    }

    public void DrawCard(string currPlayer)
    {
        List<Card> handCards;
        List<Card> deckCards;

        if (currPlayer == player)
        {
            handCards = PracticeGameState.playerHandCards;
            deckCards = PracticeGameState.playerDeckCards;
        }
        else
        {
            handCards = PracticeGameState.opponentHandCards;
            deckCards = PracticeGameState.opponentDeckCards;
        }

        if (handCards.Count < 6 && deckCards.Count != 0)
        {
            handCards.Add(deckCards[0]);
            deckCards.RemoveAt(0);
        }

        UpdateHandDisplay(currPlayer);
        cardGlowManager.UpdateHandGlow(currPlayer);
    }

    void RandomPlayerTurn()
    {
        System.Random rnd = new System.Random();

        int coin = rnd.Next(2);

        if (coin == 0)
        {
            PracticeGameState.turn = "player";
            logTextObject.text = "Player goes first!";
            turnTextObject.text = "Players turn";
            turnTextObject.color = Color.green;

            cardGlowManager.DisableZoneGlow(opponent);
            cardGlowManager.DisableHandGlow(opponent);
        }
        else
        {
            PracticeGameState.turn = "opponent";
            logTextObject.text = "Opponent goes first!";
            turnTextObject.text = "Opponents turn";
            turnTextObject.color = Color.yellow;

            cardGlowManager.DisableZoneGlow(player);
            cardGlowManager.DisableHandGlow(player);
        }
    }

    public void EndTurn()
    {
        PracticeGameState.turnCounter++;

        if (PracticeGameState.turn == "player")
        {
            PracticeGameState.turn = "opponent";
            turnTextObject.text = "Opponents turn";
            turnTextObject.color = Color.yellow;

            logTextObject.text = "Player ended turn";

            cardGlowManager.DisableZoneGlow(player);
            cardGlowManager.DisableHandGlow(player);
            OnTurnStart(opponent);
        }
        else
        {
            PracticeGameState.turn = "player";
            turnTextObject.text = "Players turn";
            turnTextObject.color = Color.green;

            logTextObject.text = "Opponent ended turn";

            cardGlowManager.DisableZoneGlow(opponent);
            cardGlowManager.DisableHandGlow(opponent);
            OnTurnStart(player);
        }
    }

    public void OnTurnStart(string currPlayer)
    {
        int coinValue;
        int[] zoneAnimalAwake;

        coinValue = (int)(Math.Ceiling((double)PracticeGameState.turnCounter / 2) + 2);

        if (currPlayer == player)
        {
            PracticeGameState.playerCoins = coinValue;
            playerCoinAmountObj.text = coinValue.ToString();
            zoneAnimalAwake = PracticeGameState.playerZoneAnimalAwake;
        }
        else
        {
            PracticeGameState.opponentCoins = coinValue;
            opponentCoinAmountObj.text = coinValue.ToString();
            zoneAnimalAwake = PracticeGameState.opponentZoneAnimalAwake;
        }

        DrawCard(currPlayer);

        for (int zone = 0; zone < 7; zone++)
        {
            if (zoneAnimalAwake[zone] == 1)
            {
                zoneAnimalAwake[zone]++;
            }
            else if (zoneAnimalAwake[zone] == 4)
            {
                zoneAnimalAwake[zone] = 1;
            }
        }

        cardGlowManager.UpdateZoneGlow(currPlayer);
    }

    private void OnCardAddedToPlayerZone(int cardID, int cardZoneNum)
    {
        PracticeGameState.playerCoins -= CardCollection.cardCollection[cardID - 1].GetCardCost();
        playerCoinAmountObj.text = PracticeGameState.playerCoins.ToString();

        abilityProcessor.OnPlayAbility(player, CardCollection.cardAbility[cardID - 1], cardZoneNum);

        UpdateZoneDisplay(player);
        UpdateZoneDisplay(opponent);

        UpdateHandDisplay(player);

        cardGlowManager.UpdateHandGlow(player);
        cardGlowManager.UpdateZoneGlow(player);

        logTextObject.text = "Player played " + CardCollection.cardCollection[cardID - 1].GetCardName();
    }

    private void OnCardAddedToOpponentZone(int cardID, int cardZoneNum)
    {
        PracticeGameState.opponentCoins -= CardCollection.cardCollection[cardID - 1].GetCardCost();
        opponentCoinAmountObj.text = PracticeGameState.opponentCoins.ToString();

        abilityProcessor.OnPlayAbility(opponent, CardCollection.cardAbility[cardID - 1], cardZoneNum);

        UpdateZoneDisplay(opponent);
        UpdateZoneDisplay(player);

        UpdateHandDisplay(opponent);

        cardGlowManager.UpdateHandGlow(opponent);
        cardGlowManager.UpdateZoneGlow(opponent);

        logTextObject.text = "Opponent played " + CardCollection.cardCollection[cardID - 1].GetCardName();
    }

    private void OnCardsBattle(int startCardZone, int endCardZone)
    {
        Card startCard;
        Card endCard;

        if (PracticeGameState.turn == player)
        {
            startCard = CardCollection.cardCollection[PracticeGameState.playerZoneCards[startCardZone] - 1];
            endCard = CardCollection.cardCollection[PracticeGameState.opponentZoneCards[endCardZone] - 1];

            int[] cardDamages = CalculateBattleDamage(player, startCardZone, endCardZone);

            PracticeGameState.playerZoneDamageCounter[startCardZone] += cardDamages[0];
            PracticeGameState.opponentZoneDamageCounter[endCardZone] += cardDamages[1];

            PracticeGameState.playerZoneAnimalAwake[startCardZone] = 1;

            logTextObject.text = "Players's " + startCard.GetCardName() + " attacks opponent's " + endCard.GetCardName();
        }
        else
        {
            startCard = CardCollection.cardCollection[PracticeGameState.opponentZoneCards[startCardZone] - 1];
            endCard = CardCollection.cardCollection[PracticeGameState.playerZoneCards[endCardZone] - 1];

            int[] cardDamages = CalculateBattleDamage(opponent, startCardZone, endCardZone);

            PracticeGameState.opponentZoneDamageCounter[startCardZone] += cardDamages[0];
            PracticeGameState.playerZoneDamageCounter[endCardZone] += cardDamages[1];

            PracticeGameState.opponentZoneAnimalAwake[startCardZone] = 1;

            logTextObject.text = "Opponent's " + startCard.GetCardName() + " attacks player's " + endCard.GetCardName();
        }

        UpdateAfterAttack(startCardZone, endCardZone, startCard, endCard);

        //Debug.Log("OPPONENT DAMAGER COUNTERS: " + string.Join(", ", PracticeGameState.opponentZoneDamageCounter));
        //Debug.Log("PLAYER DAMAGER COUNTERS: " + string.Join(", ", PracticeGameState.playerZoneDamageCounter));
    }

    private int[] CalculateBattleDamage(string currPlayer, int startCardZone, int endCardZone)
    {
        PracticeGameZoneDisplay startCard;
        PracticeGameZoneDisplay endCard;

        if (currPlayer == player)
        {
            startCard = PracticeGameObjectManager.playerZoneCardDisplays[startCardZone];
            endCard = PracticeGameObjectManager.opponentZoneCardDisplays[endCardZone];
        }
        else
        {
            startCard = PracticeGameObjectManager.opponentZoneCardDisplays[startCardZone];
            endCard = PracticeGameObjectManager.playerZoneCardDisplays[endCardZone];
        }

        int[,] attackModifiers = abilityProcessor.OnAttack(currPlayer, startCard, endCard, CardCollection.cardAbility[startCard.id - 1]);

        int startCardAttack = startCard.cardAttack + attackModifiers[0, 0];
        int startCardArmour = startCard.cardArmour;

        int endCardAttack = endCard.cardAttack;
        int endCardArmour = endCard.cardArmour + attackModifiers[1, 1];

        /*
        Debug.Log("STARTCARDATTACK" + startCardAttack);
        Debug.Log("STARTCARDARMOUR" + startCardArmour);

        Debug.Log("ENDCARDATTACK" + endCardAttack);
        Debug.Log("ENDCARDARMOUR" + endCardArmour);
        */

        int startCardDamage = 0;
        int endCardDamage = 0;

        if (startCardArmour < endCardAttack)
        {
            startCardDamage = endCardAttack - startCardArmour;
        }

        if (endCardArmour < startCardAttack)
        {
            endCardDamage = startCardAttack - endCardArmour;
        }

        int[] damage = { startCardDamage, endCardDamage };

        //Debug.Log(startCard.name + " TAKES " + startCardDamage + " DAMAGE");
        //Debug.Log(endCard.name + " TAKES " + endCardDamage + " DAMAGE");

        return damage;
    }

    private void UpdateAfterAttack(int startCardZone, int endCardZone, Card startCard, Card endCard)
    {
        int[] startZoneCards;
        int[] startZoneDamageCounter;

        int[] endZoneCards;
        int[] endZoneDamageCounter;

        List<PracticeGameZoneDisplay> startZoneCardDisplays;
        List<PracticeGameZoneDisplay> endZoneCardDisplays;

        int[] startZoneAnimalAwake;
        int[] endZoneAnimalAwake;

        string startPlayer;
        string endPlayer;

        if (PracticeGameState.turn == player)
        {
            startZoneCards = PracticeGameState.playerZoneCards;
            startZoneDamageCounter = PracticeGameState.playerZoneDamageCounter;

            endZoneCards = PracticeGameState.opponentZoneCards;
            endZoneDamageCounter = PracticeGameState.opponentZoneDamageCounter;

            startZoneCardDisplays = PracticeGameObjectManager.playerZoneCardDisplays;
            endZoneCardDisplays = PracticeGameObjectManager.opponentZoneCardDisplays;

            startZoneAnimalAwake = PracticeGameState.playerZoneAnimalAwake;
            endZoneAnimalAwake = PracticeGameState.opponentZoneAnimalAwake;

            startPlayer = player;
            endPlayer = opponent;
        }
        else
        {
            startZoneCards = PracticeGameState.opponentZoneCards;
            startZoneDamageCounter = PracticeGameState.opponentZoneDamageCounter;

            endZoneCards = PracticeGameState.playerZoneCards;
            endZoneDamageCounter = PracticeGameState.playerZoneDamageCounter;

            startZoneCardDisplays = PracticeGameObjectManager.opponentZoneCardDisplays;
            endZoneCardDisplays = PracticeGameObjectManager.playerZoneCardDisplays;

            startZoneAnimalAwake = PracticeGameState.opponentZoneAnimalAwake;
            endZoneAnimalAwake = PracticeGameState.playerZoneAnimalAwake;

            startPlayer = opponent;
            endPlayer = player;
        }

        if (startZoneDamageCounter[startCardZone] >= startCard.GetCardHealth() + startZoneCardDisplays[startCardZone].cardHealthModifier)//startZoneCardDisplays[startCardZone].cardHealth)
        {
            //Debug.Log("STARTZONEDAMAGERCOUNTER: " + startZoneDamageCounter[startCardZone] + " >= " + startZoneCardDisplays[startCardZone].cardHealth);

            abilityProcessor.OnDeath(startPlayer, CardCollection.cardAbility[startZoneCards[startCardZone] - 1], startCardZone);
            startZoneCards[startCardZone] = 0;
            startZoneDamageCounter[startCardZone] = 0;
            UpdateZoneDisplay(startPlayer);
            UpdateZoneDisplay(endPlayer);

            startZoneAnimalAwake[startCardZone] = 0;
            //cardGlowManager.UpdateZoneGlow(startPlayer);
        }
        else
        {
            startZoneCardDisplays[startCardZone].loadCardAttributes(startPlayer, startCard, startCardZone);
        }

        if (endZoneDamageCounter[endCardZone] >= endCard.GetCardHealth() + endZoneCardDisplays[endCardZone].cardHealthModifier)//endZoneCardDisplays[endCardZone].cardHealth)
        {
            //Debug.Log("ENDZONEDAMAGE: " + endZoneDamageCounter[endCardZone] + " >= " + endZoneCardDisplays[endCardZone].cardHealth);

            abilityProcessor.OnDeath(endPlayer, CardCollection.cardAbility[endZoneCards[endCardZone] - 1], endCardZone);
            endZoneCards[endCardZone] = 0;
            endZoneDamageCounter[endCardZone] = 0;
            UpdateZoneDisplay(endPlayer);
            UpdateZoneDisplay(startPlayer);

            endZoneAnimalAwake[endCardZone] = 0;
            //cardGlowManager.UpdateZoneGlow(endPlayer);
        }
        else
        {
            endZoneCardDisplays[endCardZone].loadCardAttributes(endPlayer, endCard, endCardZone);
        }

        cardGlowManager.UpdateZoneGlow(PracticeGameState.turn);
    }

    private void OnAttackBase(string attackingPlayer, int startCardZone)
    {
        if (attackingPlayer == player)
        {
            PracticeGameState.opponentBaseHealth -= PracticeGameObjectManager.playerZoneCardDisplays[startCardZone].cardAttack; //CardCollection.cardCollection[PracticeGameState.playerZoneCards[startCardZone] - 1].getCardAttack();
            opponentBaseTextObj.text = PracticeGameState.opponentBaseHealth.ToString();

            PracticeGameState.playerZoneAnimalAwake[startCardZone] = 1;
            cardGlowManager.UpdateZoneGlow(player);

            logTextObject.text = "Players " + CardCollection.cardCollection[PracticeGameState.playerZoneCards[startCardZone] - 1].GetCardName() + " attacks opponents base";

            if (PracticeGameState.opponentBaseHealth <= 0)
            {
                PlayerWin(player);
            }
        }
        else
        {
            PracticeGameState.playerBaseHealth -= PracticeGameObjectManager.opponentZoneCardDisplays[startCardZone].cardAttack; //CardCollection.cardCollection[PracticeGameState.opponentZoneCards[startCardZone] - 1].getCardAttack();
            playerBaseTextObj.text = PracticeGameState.playerBaseHealth.ToString();

            PracticeGameState.opponentZoneAnimalAwake[startCardZone] = 1;
            cardGlowManager.UpdateZoneGlow(opponent);

            logTextObject.text = "Opponents " + CardCollection.cardCollection[PracticeGameState.opponentZoneCards[startCardZone] - 1].GetCardName() + " attacks players base";

            if (PracticeGameState.playerBaseHealth <= 0)
            {
                PlayerWin(opponent);
            }
        }
    }

    public void PlayerWin(string currPlayer)
    {
        if (currPlayer == player)
        {
            wonPlayerText.text = "Player wins!";
        }
        else
        {
            wonPlayerText.text = "Opponent wins!";
        }

        winPopup.SetActive(true);
    }

    public void ReturnToMenu()
    {
        PracticeGameState.playerHandCards.Clear();
        PracticeGameState.opponentHandCards.Clear();

        PracticeGameState.playerDeckCards.Clear();
        PracticeGameState.opponentDeckCards.Clear();

        for (int i = 0; i < 7; i++)
        {
            PracticeGameState.playerZoneCards[i] = 0;
            PracticeGameState.opponentZoneCards[i] = 0;
        }

        for (int zone = 0; zone < 7; zone++)
        {
            for (int statModifier = 0; statModifier < 3; statModifier++)
            {
                PracticeGameState.playerUniversalModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentUniversalModifiers[zone, statModifier] = 0;
                PracticeGameState.playerFelineModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentFelineModifiers[zone, statModifier] = 0;
                PracticeGameState.playerCanineModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentCanineModifiers[zone, statModifier] = 0;
                PracticeGameState.playerUrsidaeModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentUrsidaeModifiers[zone, statModifier] = 0;
                PracticeGameState.playerReptiliaModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentReptiliaModifiers[zone, statModifier] = 0;
                PracticeGameState.playerDelphindaeModifiers[zone, statModifier] = 0;
                PracticeGameState.opponentDelphindaeModifiers[zone, statModifier] = 0;
            }
        }

        PracticeGameObjectManager.playerHandCardObjs.Clear();
        PracticeGameObjectManager.playerZoneCardObjs.Clear();
        PracticeGameObjectManager.playerHandCardDisplays.Clear();
        PracticeGameObjectManager.playerZoneCardDisplays.Clear();

        PracticeGameObjectManager.opponentHandCardObjs.Clear();
        PracticeGameObjectManager.opponentZoneCardObjs.Clear();
        PracticeGameObjectManager.opponentHandCardDisplays.Clear();
        PracticeGameObjectManager.opponentZoneCardDisplays.Clear();

        PracticeGameObjectManager.playerZones.Clear();
        PracticeGameObjectManager.opponentZones.Clear();

        PracticeGameInteractDraggable.CardsBattle -= OnCardsBattle;
        PracticeGameInteractDraggable.AttackBase -= OnAttackBase;

        SceneManager.LoadScene("Main Menu");
    }
}
