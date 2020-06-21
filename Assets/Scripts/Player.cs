using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public List<int> playerHand;
    public List<CardModel> playerCardModel;
    public CardModel cardModel;

    public GamestateManager gamestateManager;

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

        playerHand = new List<int>();
        playerCardModel = new List<CardModel>();
    }

    public void SortPlayerHand()
    {
        playerHand.Sort();
        playerCardModel.Sort(SortFunc);
        int i = 0;

        for (int n = 0; n < playerCardModel.Count; n++)
        {
            playerCardModel[i].transform.position = gamestateManager.playerDealPosition + gamestateManager.CardOffset * i;
            playerCardModel[i].spriteRenderer.sortingOrder = i;
            playerCardModel[i].cardPosition = i;
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
