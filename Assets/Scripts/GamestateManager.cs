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
            //Vector3 drawPosition = playerDealPosition + CardOffset * Player.instance.playerHand.Count;
            GameObject cardCopy = Instantiate(cardPrefab, playerDrawPosition, Quaternion.identity);
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

}
