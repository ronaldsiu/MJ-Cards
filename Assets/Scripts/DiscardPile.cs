using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public static DiscardPile instance = null;

    public List<int> discardPile;
    public List<CardModel> discardCardModel;
    public CardModel cardModel;

    public GamestateManager gamestateManager;
    public int lastDiscardCard;

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

        discardPile = new List<int>();
        discardCardModel = new List<CardModel>();
    }

    void Update()
    {
        lastDiscardCard = discardCardModel.Count - 1;
    }

    public void PickUp()
    {
        if(GamestateManager.instance.gamePhase == GamestateManager.GamePhase.playerTurn)
        {
            Player.instance.playerHand.Add(discardPile[lastDiscardCard]);
            Player.instance.playerCardModel.Add(discardCardModel[lastDiscardCard]);
            discardPile.Remove(discardPile[lastDiscardCard]);
            discardCardModel.Remove(discardCardModel[lastDiscardCard]);
            Player.instance.SortPlayerHand();
            gamestateManager.gamePhase = GamestateManager.GamePhase.playerCheckForWinHand;
            gamestateManager.GameFlow();
            return;
        }
        else if(GamestateManager.instance.gamePhase == GamestateManager.GamePhase.opponentTurn)
        {
            Opponent.instance.opponentHand.Add(discardPile[lastDiscardCard]);
            Opponent.instance.opponentCardModel.Add(discardCardModel[lastDiscardCard]);
            discardPile.Remove(discardPile[lastDiscardCard]);
            discardCardModel.Remove(discardCardModel[lastDiscardCard]);
            Opponent.instance.SortOpponentHand();
            gamestateManager.gamePhase = GamestateManager.GamePhase.opponentCheckForWinHand;
            gamestateManager.GameFlow();
        }
    }

}
