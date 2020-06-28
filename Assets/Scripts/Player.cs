using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public List<int> playerHand;
    public List<CardModel> playerCardModel;
    public Dictionary<int, int> playerhandValues;
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
        playerhandValues = new Dictionary<int, int>();
    }

    public void resetHandValues()
    {
        playerhandValues.Clear();

        playerhandValues[1] = 0;
        playerhandValues[2] = 0;
        playerhandValues[3] = 0;
        playerhandValues[4] = 0;
        playerhandValues[5] = 0;
        playerhandValues[6] = 0;
        playerhandValues[7] = 0;
        playerhandValues[8] = 0;
        playerhandValues[9] = 0;
        playerhandValues[10] = 0;
        playerhandValues[11] = 0;
        playerhandValues[12] = 0;
        playerhandValues[13] = 0;
    }

    public void GetPlayerHandValues()
    {
        resetHandValues();
        for (int i = 0; i < playerCardModel.Count; i++)
        {
                int count = 0;
            playerhandValues.TryGetValue(playerCardModel[i].cardValue, out count);
            playerhandValues.Remove(playerCardModel[i].cardValue);
            playerhandValues.Add(playerCardModel[i].cardValue, count + 1);
        }
    }

    public void checkPlayerWinningHandLower()
    {
        //Check Matches then Sequences
        GetPlayerHandValues();
        for (int i = 1; i < 9; i++)
        {
            if (playerhandValues[i] >= 3)
            {
                int count = 0;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 3);
            }
        }

        int n = 1;
        while(n < 4)
        {
            if (playerhandValues[n] > 0 && playerhandValues[n+1] > 0 && playerhandValues[n+2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                playerhandValues.TryGetValue(n, out count1); playerhandValues.TryGetValue(n+1, out count2); playerhandValues.TryGetValue(n+2, out count3);
                playerhandValues.Remove(n); playerhandValues.Remove(n+1); playerhandValues.Remove(n+2);
                playerhandValues.Add(n, count1 - 1); playerhandValues.Add(n+1, count2 - 1); playerhandValues.Add(n+2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (playerhandValues[i] == 2)
            {
                int count;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in playerhandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (playerhandValues[1] == 0 &&
            playerhandValues[2] == 0 &&
            playerhandValues[3] == 0 &&
            playerhandValues[4] == 0 &&
            playerhandValues[5] == 0 &&
            playerhandValues[6] == 0 &&
            playerhandValues[7] == 0 &&
            playerhandValues[8] == 0 &&
            playerhandValues[9] == 0 &&
            playerhandValues[10] == 0 &&
            playerhandValues[11] == 0 &&
            playerhandValues[12] == 0 &&
            playerhandValues[13] == 0)
        { Debug.Log("PLAYER WINS LOWER SET");
            return;
        }

        //Check in reverse Order
        GetPlayerHandValues();

        n = 1;
        while (n < 4)
        {
            if (playerhandValues[n] > 0 && playerhandValues[n + 1] > 0 && playerhandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                playerhandValues.TryGetValue(n, out count1); playerhandValues.TryGetValue(n + 1, out count2); playerhandValues.TryGetValue(n + 2, out count3);
                playerhandValues.Remove(n); playerhandValues.Remove(n + 1); playerhandValues.Remove(n + 2);
                playerhandValues.Add(n, count1 - 1); playerhandValues.Add(n + 1, count2 - 1); playerhandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (playerhandValues[i] >= 3)
            {
                int count = 0;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 3);
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (playerhandValues[i] == 2)
            {
                int count;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in playerhandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (playerhandValues[1] == 0 &&
            playerhandValues[2] == 0 &&
            playerhandValues[3] == 0 &&
            playerhandValues[4] == 0 &&
            playerhandValues[5] == 0 &&
            playerhandValues[6] == 0 &&
            playerhandValues[7] == 0 &&
            playerhandValues[8] == 0 &&
            playerhandValues[9] == 0 &&
            playerhandValues[10] == 0 &&
            playerhandValues[11] == 0 &&
            playerhandValues[12] == 0 &&
            playerhandValues[13] == 0)
        {
            Debug.Log("PLAYER WINS LOWER SET");
            return;
        }
    }

    public void checkPlayerWinningHandUpper()
    {
        //check matches then sequence
        GetPlayerHandValues();
        for (int i = 6; i < 14; i++)
        {
            if (playerhandValues[i] >= 3)
            {
                int count = 0;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 3);
            }
        }

        int n = 9;
        while (n < 14)
        {
            if (playerhandValues[n] > 0 && playerhandValues[n + 1] > 0 && playerhandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                playerhandValues.TryGetValue(n, out count1); playerhandValues.TryGetValue(n + 1, out count2); playerhandValues.TryGetValue(n + 2, out count3);
                playerhandValues.Remove(n); playerhandValues.Remove(n + 1); playerhandValues.Remove(n + 2);
                playerhandValues.Add(n, count1 - 1); playerhandValues.Add(n + 1, count2 - 1); playerhandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (playerhandValues[i] == 2)
            {
                int count;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in playerhandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (playerhandValues[1] == 0 &&
            playerhandValues[2] == 0 &&
            playerhandValues[3] == 0 &&
            playerhandValues[4] == 0 &&
            playerhandValues[5] == 0 &&
            playerhandValues[6] == 0 &&
            playerhandValues[7] == 0 &&
            playerhandValues[8] == 0 &&
            playerhandValues[9] == 0 &&
            playerhandValues[10] == 0 &&
            playerhandValues[11] == 0 &&
            playerhandValues[12] == 0 &&
            playerhandValues[13] == 0)
        { Debug.Log("PLAYER WINS UPPER SET");
            return;
        }
        //Check in reverse Order
        GetPlayerHandValues();
        n = 9;
        while (n < 14)
        {
            if (playerhandValues[n] > 0 && playerhandValues[n + 1] > 0 && playerhandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                playerhandValues.TryGetValue(n, out count1); playerhandValues.TryGetValue(n + 1, out count2); playerhandValues.TryGetValue(n + 2, out count3);
                playerhandValues.Remove(n); playerhandValues.Remove(n + 1); playerhandValues.Remove(n + 2);
                playerhandValues.Add(n, count1 - 1); playerhandValues.Add(n + 1, count2 - 1); playerhandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (playerhandValues[i] >= 3)
            {
                int count = 0;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 3);
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (playerhandValues[i] == 2)
            {
                int count;
                playerhandValues.TryGetValue(i, out count);
                playerhandValues.Remove(i);
                playerhandValues.Add(i, count - 2);
                break;
            }
        }

        foreach (var pair in playerhandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }

        if (playerhandValues[1] == 0 &&
            playerhandValues[2] == 0 &&
            playerhandValues[3] == 0 &&
            playerhandValues[4] == 0 &&
            playerhandValues[5] == 0 &&
            playerhandValues[6] == 0 &&
            playerhandValues[7] == 0 &&
            playerhandValues[8] == 0 &&
            playerhandValues[9] == 0 &&
            playerhandValues[10] == 0 &&
            playerhandValues[11] == 0 &&
            playerhandValues[12] == 0 &&
            playerhandValues[13] == 0)
        {
            Debug.Log("PLAYER WINS UPPER SET");
            return;
        }


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
