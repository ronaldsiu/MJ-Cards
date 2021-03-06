﻿using System.Collections;
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
    public UiManager uiManager;

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
        gamePhase = GamePhase.gameStarts;

    }

    void Start()
    {
        GameFlow();
    }

    public enum GamePhase
    {
        gameStarts,
        playerTurn,
        playerCheckForWinHand,
        playerDiscardCard,
        opponentTurn,
        opponentCheckForWinHand,
        opponentDiscardCard
    }

    public void GameFlow()
    {
        switch(gamePhase)
        {
            case GamePhase.gameStarts:
                {
                    Debug.Log("Shuffle and Dealing Hands");
                    StartCoroutine(StartGame());
                    break;
                }
            case GamePhase.playerTurn:
                {
                    Debug.Log("Player's Turn");
                    uiManager.instruction.text = "Draw or Pickup a Card";
                    break;
                    //Wait for Player to draw or pickup card
                }
            case GamePhase.playerCheckForWinHand:
                {
                    Debug.Log("Checking Player's Hand");
                    uiManager.instruction.text = "Checking Player's Hand";
                    //Check if Player has the winning Hand
                    player.checkPlayerWinningHandLower();
                    player.checkPlayerWinningHandUpper();
                    player.checkPlayerWinningHandSets();
                    gamePhase = GamePhase.playerDiscardCard;
                    GameFlow();
                    break;
                }
            case GamePhase.playerDiscardCard:
                {
                    Debug.Log("Waiting for Player to Discard");
                    uiManager.instruction.text = "Waiting for Player to Discard";
                    break;
                    //Waiting for Player to Discard Card
                }
            case GamePhase.opponentTurn:
                {
                    Debug.Log("Opponent's Turn");
                    uiManager.instruction.text = "Opponent's Turn";


                    break;
                    //Wait for Opponent to draw or pickup card
                }
            case GamePhase.opponentCheckForWinHand:
                {
                    Debug.Log("Checking Opponent's Hand");
                    uiManager.instruction.text = "Checking Opponent's Hand";
                    opponent.checkOpponentWinningHandLower();
                    opponent.checkOpponentWinningHandUpper();
                    opponent.checkOpponentWinningHandSets();
                    gamePhase = GamePhase.opponentDiscardCard;
                    GameFlow();
                    break;
                    //Check if Opponent has the winning Hand
                }
            case GamePhase.opponentDiscardCard:
                {
                    Debug.Log("Waiting for Opponent to Discard");
                    uiManager.instruction.text = "Waiting for Opponent to Discard";
                    break;
                    //Waiting for Opponent to Discard Card
                }

        }
    }

    public IEnumerator StartGame()
    {
        uiManager.instruction.text = "Dealing Cards";
        Deck.instance.Shuffle();
        DealCards();
        yield return new WaitForSeconds(3);
        gamePhase = GamePhase.playerTurn;
        player.SortPlayerHand();
        opponent.SortOpponentHand();
        isPlayerTurn = true;
        canDraw = true;
        GameFlow();

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
            canDraw = false;
            gamePhase = GamePhase.playerCheckForWinHand;
            GameFlow();
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
            gamePhase = GamePhase.opponentCheckForWinHand;
            GameFlow();
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
