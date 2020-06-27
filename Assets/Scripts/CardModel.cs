using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite[] faces;
    public Sprite cardback;

    public int cardIndex; // How to identify cards = faces[cardIndex]

    public GameObject card;
    public bool showFace = false;

    public int cardValue;
    public int cardShape;
    public int cardPosition;

    public CardOwner cardOwner;
    public CardSet cardSet;
    public GamestateManager gamestateManager;
    public Player player;
    public Opponent opponent;
    public DiscardPile discardPile;


    // Awake is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardOwner = CardOwner.Deck;
    }

    private void Update()
    {
        if (cardIndex % 4 == 0) { cardShape = 1; }
        else if (cardIndex % 4 == 1) { cardShape = 2; }
        else if (cardIndex % 4 == 2) { cardShape = 3; }
        else if (cardIndex % 4 == 3) { cardShape = 4; }

        if (cardIndex >= 0 && cardIndex <= 3) { cardValue = 1; }
        else if (cardIndex >= 4 && cardIndex <= 7) { cardValue = 2; }
        else if (cardIndex >= 8 && cardIndex <= 11) { cardValue = 3; }
        else if (cardIndex >= 12 && cardIndex <= 15) { cardValue = 4; }
        else if (cardIndex >= 16 && cardIndex <= 19) { cardValue = 5; }
        else if (cardIndex >= 20 && cardIndex <= 23) { cardValue = 9; }
        else if (cardIndex >= 24 && cardIndex <= 27) { cardValue = 10; }
        else if (cardIndex >= 28 && cardIndex <= 31) { cardValue = 11; }
        else if (cardIndex >= 32 && cardIndex <= 35) { cardValue = 12; }
        else if (cardIndex >= 36 && cardIndex <= 39) { cardValue = 13; }
        else if (cardIndex >= 40 && cardIndex <= 43) { cardValue = 6; }
        else if (cardIndex >= 44 && cardIndex <= 47) { cardValue = 7; }
        else if (cardIndex >= 48 && cardIndex <= 51) { cardValue = 8; }

        if (cardValue < 6) { cardSet = CardSet.LowerSet; }
        else if (cardValue > 8) { cardSet = CardSet.UpperSet; }
        else { cardSet = CardSet.Wild; }
    }

    void OnMouseDown()
    {
        DiscardCard();
    }

    public enum CardOwner
    {
        Deck,
        Player,
        Opponent,
        Discard
    }

    public enum CardSet
    {
        LowerSet,
        UpperSet,
        Wild
    }

    public void DiscardCard()
    {

        if (GamestateManager.instance.gamePhase == GamestateManager.GamePhase.playerdDiscardCard && cardOwner == CardOwner.Player)
        {
            DiscardPile.instance.discardPile.Add(Player.instance.playerHand[cardPosition]);
            DiscardPile.instance.discardCardModel.Add(Player.instance.playerCardModel[cardPosition]);
            Player.instance.playerHand.Remove(Player.instance.playerHand[cardPosition]);
            Player.instance.playerCardModel.Remove(Player.instance.playerCardModel[cardPosition]);
            transform.position = GamestateManager.instance.discardPosition + new Vector3(0.5f, 0, 0) * GamestateManager.instance.discardCount;
            spriteRenderer.sortingOrder = GamestateManager.instance.discardCount;
            cardPosition = GamestateManager.instance.discardCount;
            GamestateManager.instance.discardCount++;
            //GamestateManager.instance.gamePhase = GamestateManager.GamePhase.opponentDrawCard;
        }
    }

    public void ToggleFace()
    {
        if(showFace == true)
        {
            //If true show face
            spriteRenderer.sprite = faces[cardIndex];
        }
        else if (showFace == false)
        {
            //If false show back
            spriteRenderer.sprite = cardback;
        }
    }

    public void RandomCard()
    {
        showFace = true;
        cardIndex = Random.Range(0, 52);
    }

}
