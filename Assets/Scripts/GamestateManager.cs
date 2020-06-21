using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamestateManager : MonoBehaviour
{
    public static GamestateManager instance = null;

    public Deck deck;
    public Player player;
    public Opponent opponent;
    public GameObject cardPrefab;

    public Vector3 playerDealPosition;
    public Vector3 playerDrawPosition;

    public Vector3 opponentDealPosition;
    public Vector3 opponentDrawPosition;

    public Vector3 CardOffset;

    public Vector3 discardPosition;
    public int discardCount;

    public bool isPlayerTurn;
    public bool canDraw;

    bool matchSet1;
    bool matchSet2;
    bool matchSet3;
    bool matchPair4;

    public GamePhase gamePhase;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        discardCount = 0;

    }

    void Start()
    {
        Deck.instance.Shuffle();
        DealCards();
    }

    public enum GamePhase
    {
        gameStarts,
        playerDrawCard,
        playerCheckForWinHand,
        playerdDiscardCard,
        opponentDrawCard,
        opponentCheckForWinHand,
        opponentDiscardCard
    }

    public void DrawCard()
    {
        
        if (isPlayerTurn == true && canDraw == true)
        {
            Vector3 drawPosition = playerDealPosition + CardOffset * Player.instance.playerHand.Count;
            GameObject cardCopy = Instantiate(cardPrefab, drawPosition, Quaternion.identity);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            cardModel.cardIndex = deck.cards[0];
            cardModel.cardPosition = Player.instance.playerHand.Count;
            cardModel.showFace = true;
            cardModel.ToggleFace();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();

            player.playerHand.Add(deck.cards[0]);
            deck.cards.Remove(deck.cards[0]);
            player.playerCardModel.Add(cardModel);
            cardModel.cardOwner = CardModel.CardOwner.Player;
            //canDraw = false;
            //gamePhase = GamePhase.playerCheckForWinHand;
        }
        else if (isPlayerTurn == false && canDraw == true)
        {
            GameObject cardCopy = Instantiate(cardPrefab, playerDrawPosition, Quaternion.identity);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            cardModel.cardIndex = deck.cards[0];
            cardModel.cardPosition = Opponent.instance.opponentHand.Count;
            cardModel.showFace = true;
            cardModel.ToggleFace();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();

            opponent.opponentHand.Add(deck.cards[0]);
            deck.cards.Remove(deck.cards[0]);
            opponent.opponentCardModel.Add(cardModel);
            cardModel.cardOwner = CardModel.CardOwner.Opponent;
            canDraw = true;
            gamePhase = GamePhase.opponentCheckForWinHand;
        }
    }

    public void DealCards()
    {
        int cardCountX = 0;
        //Deal 10 cards to Player
        for (int i = 0; i < 10; i++)
        {

            GameObject cardCopy = Instantiate(cardPrefab, playerDealPosition + CardOffset * cardCountX, Quaternion.identity);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            cardModel.cardIndex = deck.cards[0];
            cardModel.showFace = true;
            cardModel.ToggleFace();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = cardCountX;

            player.playerHand.Add(deck.cards[0]);
            deck.cards.Remove(deck.cards[0]);
            player.playerCardModel.Add(cardModel);

            cardModel.cardOwner = CardModel.CardOwner.Player;
            cardModel.cardPosition = cardCountX;

            cardCountX++;
        }

        int cardCountY = 0;
        //Deal 10 cards to Opponent
        for (int i = 0; i < 10; i++)
        {

            GameObject cardCopy = Instantiate(cardPrefab, opponentDealPosition + CardOffset * cardCountY, Quaternion.identity);
            CardModel cardModel = cardCopy.GetComponent<CardModel>();
            cardModel.cardIndex = deck.cards[0];
            cardModel.showFace = true;
            cardModel.ToggleFace();
            SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = cardCountY;

            opponent.opponentHand.Add(deck.cards[0]);
            deck.cards.Remove(deck.cards[0]);
            opponent.opponentCardModel.Add(cardModel);

            cardModel.cardOwner = CardModel.CardOwner.Opponent;
            cardModel.cardPosition = cardCountY;

            cardCountY++;
        }

    }

    public void CheckForWin()
    {

        // Check First way to Win SetSetSetPair

        if(player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue
            || player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue - 1 && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue - 2)
        {
            matchSet1 = true;
            Debug.Log("MatchSet1");
        }
        if (player.playerCardModel[3].cardValue == player.playerCardModel[4].cardValue && player.playerCardModel[3].cardValue == player.playerCardModel[5].cardValue
    || player.playerCardModel[3].cardValue == player.playerCardModel[4].cardValue - 1 && player.playerCardModel[3].cardValue == player.playerCardModel[5].cardValue - 2)
        {
            matchSet2 = true;
            Debug.Log("MatchSet2");
        }
        if (player.playerCardModel[6].cardValue == player.playerCardModel[7].cardValue && player.playerCardModel[6].cardValue == player.playerCardModel[8].cardValue
    || player.playerCardModel[6].cardValue == player.playerCardModel[7].cardValue - 1 && player.playerCardModel[6].cardValue == player.playerCardModel[8].cardValue - 2)
        {
            matchSet3 = true;
            Debug.Log("MatchSet3");
        }
        if (player.playerCardModel[9].cardValue == player.playerCardModel[10].cardValue)
        {
            matchPair4 = true;
            Debug.Log("MatchPair4");
        }
        if (matchSet1 == true && matchSet2 == true && matchSet3 == true && matchPair4 == true)
        {
            if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[9].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[9].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else { Debug.Log("Not a consistent Set"); }


            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;

        }
        else
        {
            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }

        // Check 2nd way to Win SetSetPairSet

        if (player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue
            || player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue - 1 && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue - 2)
        {
            matchSet1 = true;
            Debug.Log("MatchSet1");
        }
        if (player.playerCardModel[3].cardValue == player.playerCardModel[4].cardValue && player.playerCardModel[3].cardValue == player.playerCardModel[5].cardValue
    || player.playerCardModel[3].cardValue == player.playerCardModel[4].cardValue - 1 && player.playerCardModel[3].cardValue == player.playerCardModel[5].cardValue - 2)
        {
            matchSet2 = true;
            Debug.Log("MatchSet2");
        }
        if (player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue
    || player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue - 1 && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue - 2)
        {
            matchSet3 = true;
            Debug.Log("MatchSet3");
        }
        if (player.playerCardModel[6].cardValue == player.playerCardModel[7].cardValue)
        {
            matchPair4 = true;
            Debug.Log("MatchPair4");
        }
        if (matchSet1 == true && matchSet2 == true && matchSet3 == true && matchPair4 == true)
        {
            if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                            && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[6].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else { Debug.Log("Not a consistent Set"); }

            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }
        else
        {
            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }

        // Check 3rd way to Win SetPairSetSet

        if (player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue
            || player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue - 1 && player.playerCardModel[0].cardValue == player.playerCardModel[2].cardValue - 2)
        {
            matchSet1 = true;
            Debug.Log("MatchSet1");
        }
        if (player.playerCardModel[5].cardValue == player.playerCardModel[6].cardValue && player.playerCardModel[5].cardValue == player.playerCardModel[7].cardValue
    || player.playerCardModel[5].cardValue == player.playerCardModel[6].cardValue - 1 && player.playerCardModel[5].cardValue == player.playerCardModel[7].cardValue - 2)
        {
            matchSet2 = true;
            Debug.Log("MatchSet2");
        }
        if (player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue
    || player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue - 1 && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue - 2)
        {
            matchSet3 = true;
            Debug.Log("MatchSet3");
        }
        if (player.playerCardModel[3].cardValue == player.playerCardModel[4].cardValue)
        {
            matchPair4 = true;
            Debug.Log("MatchPair4");
        }
        if (matchSet1 == true && matchSet2 == true && matchSet3 == true && matchPair4 == true)
        {
            if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                            && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[3].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else { Debug.Log("Not a consistent Set"); }

            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }
        else
        {
            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }
        // Check 4th way to Win PairSetSetSet

        if (player.playerCardModel[2].cardValue == player.playerCardModel[3].cardValue && player.playerCardModel[2].cardValue == player.playerCardModel[4].cardValue
            || player.playerCardModel[2].cardValue == player.playerCardModel[3].cardValue - 1 && player.playerCardModel[2].cardValue == player.playerCardModel[4].cardValue - 2)
        {
            matchSet1 = true;
            Debug.Log("MatchSet1");
        }
        if (player.playerCardModel[5].cardValue == player.playerCardModel[6].cardValue && player.playerCardModel[5].cardValue == player.playerCardModel[7].cardValue
    || player.playerCardModel[5].cardValue == player.playerCardModel[7].cardValue - 1 && player.playerCardModel[5].cardValue == player.playerCardModel[7].cardValue - 2)
        {
            matchSet2 = true;
            Debug.Log("MatchSet2");
        }
        if (player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue
    || player.playerCardModel[8].cardValue == player.playerCardModel[9].cardValue - 1 && player.playerCardModel[8].cardValue == player.playerCardModel[10].cardValue - 2)
        {
            matchSet3 = true;
            Debug.Log("MatchSet3");
        }
        if (player.playerCardModel[0].cardValue == player.playerCardModel[1].cardValue)
        {
            matchPair4 = true;
            Debug.Log("MatchPair4");
        }
        if (matchSet1 == true && matchSet2 == true && matchSet3 == true && matchPair4 == true)
        {
            if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                             && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.LowerSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.LowerSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.LowerSet)
            { Debug.Log("WIN Lower"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.UpperSet
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.UpperSet && player.playerCardModel[8].cardSet == CardModel.CardSet.Wild)
            { Debug.Log("WIN Upper"); }
            else if (player.playerCardModel[0].cardSet == CardModel.CardSet.Wild && player.playerCardModel[2].cardSet == CardModel.CardSet.Wild
                && player.playerCardModel[5].cardSet == CardModel.CardSet.Wild && player.playerCardModel[8].cardSet == CardModel.CardSet.UpperSet)
            { Debug.Log("WIN Upper"); }
            else { Debug.Log("Not a consistent Set"); }

            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }
        else
        {
            matchSet1 = false;
            matchSet2 = false;
            matchSet3 = false;
            matchPair4 = false;
        }

    }

}
