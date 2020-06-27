using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public List<int> playerHand;
    public List<CardModel> playerCardModel;
    public Dictionary<int, int> handValues;
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
        handValues = new Dictionary<int, int>();
    }

    public void resetHandValues()
    {
        handValues.Clear();

        handValues[1] = 0;
        handValues[2] = 0;
        handValues[3] = 0;
        handValues[4] = 0;
        handValues[5] = 0;
        handValues[6] = 0;
        handValues[7] = 0;
        handValues[8] = 0;
        handValues[9] = 0;
        handValues[10] = 0;
        handValues[11] = 0;
        handValues[12] = 0;
        handValues[13] = 0;
    }

    public void GetPlayerHandValues()
    {
        resetHandValues();
        for (int i = 0; i < playerCardModel.Count; i++)
        {
                int count = 0;
                handValues.TryGetValue(playerCardModel[i].cardValue, out count);
                handValues.Remove(playerCardModel[i].cardValue);
                handValues.Add(playerCardModel[i].cardValue, count + 1);
        }
    }

    public void checkPlayerWinningHandLower()
    {
        GetPlayerHandValues();
        for (int i = 6; i < 9; i++)
        {
            if (handValues[i] >= 3)
            {
                int count = 0;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 3);
            }
        }

        for (int i = 1; i < 6; i++)
        {
            if (handValues[i] >= 3)
            {
                int count = 0;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 3);
            }
        }

        int n = 1;
        while(n < 4)
        {
            if (handValues[n] > 0 && handValues[n+1] > 0 && handValues[n+2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                handValues.TryGetValue(n, out count1); handValues.TryGetValue(n+1, out count2); handValues.TryGetValue(n+2, out count3);
                handValues.Remove(n); handValues.Remove(n+1); handValues.Remove(n+2);
                handValues.Add(n, count1 - 1); handValues.Add(n+1, count2 - 1); handValues.Add(n+2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (handValues[i] == 2)
            {
                int count;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in handValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (handValues[1] == 0 &&
            handValues[2] == 0 &&
            handValues[3] == 0 &&
            handValues[4] == 0 &&
            handValues[5] == 0 &&
            handValues[6] == 0 &&
            handValues[7] == 0 &&
            handValues[8] == 0 &&
            handValues[9] == 0 &&
            handValues[10] == 0 &&
            handValues[11] == 0 &&
            handValues[12] == 0 &&
            handValues[13] == 0)
        { Debug.Log("PLAYER WINS LOWER SET"); }

    }

    public void checkPlayerWinningHandUpper()
    {
        GetPlayerHandValues();
        for (int i = 6; i < 9; i++)
        {
            if (handValues[i] >= 3)
            {
                int count = 0;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 3);
            }
        }

        for (int i = 9; i < 14; i++)
        {
            if (handValues[i] >= 3)
            {
                int count = 0;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 3);
            }
        }

        int n = 9;
        while (n < 14)
        {
            if (handValues[n] > 0 && handValues[n + 1] > 0 && handValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                handValues.TryGetValue(n, out count1); handValues.TryGetValue(n + 1, out count2); handValues.TryGetValue(n + 2, out count3);
                handValues.Remove(n); handValues.Remove(n + 1); handValues.Remove(n + 2);
                handValues.Add(n, count1 - 1); handValues.Add(n + 1, count2 - 1); handValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (handValues[i] == 2)
            {
                int count;
                handValues.TryGetValue(i, out count);
                handValues.Remove(i);
                handValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in handValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (handValues[1] == 0 &&
            handValues[2] == 0 &&
            handValues[3] == 0 &&
            handValues[4] == 0 &&
            handValues[5] == 0 &&
            handValues[6] == 0 &&
            handValues[7] == 0 &&
            handValues[8] == 0 &&
            handValues[9] == 0 &&
            handValues[10] == 0 &&
            handValues[11] == 0 &&
            handValues[12] == 0 &&
            handValues[13] == 0)
        { Debug.Log("PLAYER WINS UPPER SET"); }

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
