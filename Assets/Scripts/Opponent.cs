using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public static Opponent instance = null;

    public List<int> opponentHand;
    public List<CardModel> opponentCardModel;
    public CardModel cardModel;

    public GamestateManager gamestateManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        opponentHand = new List<int>();
        opponentCardModel = new List<CardModel>();
    }

    public void SortOpponentHand()
    {
        opponentHand.Sort();
        opponentCardModel.Sort(SortFunc);
        int i = 0;

        for (int n = 0; n < opponentCardModel.Count; n++)
        {
            opponentCardModel[i].transform.position = gamestateManager.opponentDealPosition + (gamestateManager.CardOffset * i);
            opponentCardModel[i].spriteRenderer.sortingOrder = i;
            opponentCardModel[i].cardPosition = i;
            i++;
        }
    }

    public int SortFunc(CardModel a, CardModel b)
    {
        if (a.cardValue < b.cardValue)
        {
            return -1;
        }
        else if (a.cardValue > b.cardValue)
        {
            return 1;
        }
        return 0;
    }
}
