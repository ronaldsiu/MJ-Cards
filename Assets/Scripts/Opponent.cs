using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public static Opponent instance = null;

    public List<int> opponentHand;
    public List<CardModel> opponentCardModel;
    public Dictionary<int, int> opponentHandValues;
    public CardModel cardModel;

    public GamestateManager gamestateManager;
    public UiManager uiManager;
    public DiscardPile discardPile;

    public int lowerSetCardAmount;
    public int higherSetCardAmount;
    

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
        opponentHandValues = new Dictionary<int, int>();
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

    public void resetopponentHandValues()
    {
        opponentHandValues.Clear();

        opponentHandValues[1] = 0;
        opponentHandValues[2] = 0;
        opponentHandValues[3] = 0;
        opponentHandValues[4] = 0;
        opponentHandValues[5] = 0;
        opponentHandValues[6] = 0;
        opponentHandValues[7] = 0;
        opponentHandValues[8] = 0;
        opponentHandValues[9] = 0;
        opponentHandValues[10] = 0;
        opponentHandValues[11] = 0;
        opponentHandValues[12] = 0;
        opponentHandValues[13] = 0;
    }

    public void GetopponentHandValues()
    {
        resetopponentHandValues();
        for (int i = 0; i < opponentCardModel.Count; i++)
        {
            int count = 0;
            opponentHandValues.TryGetValue(opponentCardModel[i].cardValue, out count);
            opponentHandValues.Remove(opponentCardModel[i].cardValue);
            opponentHandValues.Add(opponentCardModel[i].cardValue, count + 1);
        }
    }

   public void OpponentEvaluatePickup()
    {
        GetopponentHandValues();
        lowerSetCardAmount = opponentHandValues[1] + opponentHandValues[2] + opponentHandValues[3] + opponentHandValues[4] + opponentHandValues[5];
        higherSetCardAmount = opponentHandValues[9] + opponentHandValues[10] + opponentHandValues[11] + opponentHandValues[12] + opponentHandValues[13];

    }

    public IEnumerator OpponentDrawCard()
    {
        uiManager.instruction.text = "Opponent Drawing Card";
        yield return new WaitForSeconds(2);
        gamestateManager.DrawCard();
    }

    public void checkOpponentWinningHandLower()
    {
        //Check Matches then Sequences
        GetopponentHandValues();
        for (int i = 1; i < 9; i++)
        {
            if (opponentHandValues[i] >= 3)
            {
                int count = 0;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 3);
            }
        }

        int n = 1;
        while (n < 4)
        {
            if (opponentHandValues[n] > 0 && opponentHandValues[n + 1] > 0 && opponentHandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                opponentHandValues.TryGetValue(n, out count1); opponentHandValues.TryGetValue(n + 1, out count2); opponentHandValues.TryGetValue(n + 2, out count3);
                opponentHandValues.Remove(n); opponentHandValues.Remove(n + 1); opponentHandValues.Remove(n + 2);
                opponentHandValues.Add(n, count1 - 1); opponentHandValues.Add(n + 1, count2 - 1); opponentHandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (opponentHandValues[i] == 2)
            {
                int count;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 2);
                break;
            }
        }

        /*foreach (var pair in opponentHandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }*/

        if (opponentHandValues[1] == 0 &&
            opponentHandValues[2] == 0 &&
            opponentHandValues[3] == 0 &&
            opponentHandValues[4] == 0 &&
            opponentHandValues[5] == 0 &&
            opponentHandValues[6] == 0 &&
            opponentHandValues[7] == 0 &&
            opponentHandValues[8] == 0 &&
            opponentHandValues[9] == 0 &&
            opponentHandValues[10] == 0 &&
            opponentHandValues[11] == 0 &&
            opponentHandValues[12] == 0 &&
            opponentHandValues[13] == 0)
        {
            Debug.Log("Opponent WINS LOWER SET");
            return;
        }
        else
        {
            Debug.Log("Not a Winning Hand");
        }

        //Check in reverse Order
        GetopponentHandValues();

        n = 1;
        while (n < 4)
        {
            if (opponentHandValues[n] > 0 && opponentHandValues[n + 1] > 0 && opponentHandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                opponentHandValues.TryGetValue(n, out count1); opponentHandValues.TryGetValue(n + 1, out count2); opponentHandValues.TryGetValue(n + 2, out count3);
                opponentHandValues.Remove(n); opponentHandValues.Remove(n + 1); opponentHandValues.Remove(n + 2);
                opponentHandValues.Add(n, count1 - 1); opponentHandValues.Add(n + 1, count2 - 1); opponentHandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (opponentHandValues[i] >= 3)
            {
                int count = 0;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 3);
            }
        }

        for (int i = 1; i < 9; i++)
        {
            if (opponentHandValues[i] == 2)
            {
                int count;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 2);
                break;
            }
        }

        /*foreach (var pair in opponentHandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }*/

        if (opponentHandValues[1] == 0 &&
            opponentHandValues[2] == 0 &&
            opponentHandValues[3] == 0 &&
            opponentHandValues[4] == 0 &&
            opponentHandValues[5] == 0 &&
            opponentHandValues[6] == 0 &&
            opponentHandValues[7] == 0 &&
            opponentHandValues[8] == 0 &&
            opponentHandValues[9] == 0 &&
            opponentHandValues[10] == 0 &&
            opponentHandValues[11] == 0 &&
            opponentHandValues[12] == 0 &&
            opponentHandValues[13] == 0)
        {
            Debug.Log("OPPONENT WINS LOWER SET");
            return;
        }
        else
        {
            Debug.Log("Not a Winning Hand");
        }
    }

    public void checkOpponentWinningHandUpper()
    {
        //check matches then sequence
        GetopponentHandValues();
        for (int i = 6; i < 14; i++)
        {
            if (opponentHandValues[i] >= 3)
            {
                int count = 0;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 3);
            }
        }

        int n = 9;
        while (n < 12)
        {
            if (opponentHandValues[n] > 0 && opponentHandValues[n + 1] > 0 && opponentHandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                opponentHandValues.TryGetValue(n, out count1); opponentHandValues.TryGetValue(n + 1, out count2); opponentHandValues.TryGetValue(n + 2, out count3);
                opponentHandValues.Remove(n); opponentHandValues.Remove(n + 1); opponentHandValues.Remove(n + 2);
                opponentHandValues.Add(n, count1 - 1); opponentHandValues.Add(n + 1, count2 - 1); opponentHandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (opponentHandValues[i] == 2)
            {
                int count;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 2);
                break;
            }
        }

        /*foreach (var pair in opponentHandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }*/

        if (opponentHandValues[1] == 0 &&
            opponentHandValues[2] == 0 &&
            opponentHandValues[3] == 0 &&
            opponentHandValues[4] == 0 &&
            opponentHandValues[5] == 0 &&
            opponentHandValues[6] == 0 &&
            opponentHandValues[7] == 0 &&
            opponentHandValues[8] == 0 &&
            opponentHandValues[9] == 0 &&
            opponentHandValues[10] == 0 &&
            opponentHandValues[11] == 0 &&
            opponentHandValues[12] == 0 &&
            opponentHandValues[13] == 0)
        {
            Debug.Log("Opponent WINS UPPER SET");
            return;
        }
        else
        {
            Debug.Log("Not a Winning Hand");
        }
        //Check in reverse Order
        GetopponentHandValues();
        n = 9;
        while (n < 12)
        {
            if (opponentHandValues[n] > 0 && opponentHandValues[n + 1] > 0 && opponentHandValues[n + 2] > 0)
            {
                int count1 = 0; int count2 = 0; int count3 = 0;
                opponentHandValues.TryGetValue(n, out count1); opponentHandValues.TryGetValue(n + 1, out count2); opponentHandValues.TryGetValue(n + 2, out count3);
                opponentHandValues.Remove(n); opponentHandValues.Remove(n + 1); opponentHandValues.Remove(n + 2);
                opponentHandValues.Add(n, count1 - 1); opponentHandValues.Add(n + 1, count2 - 1); opponentHandValues.Add(n + 2, count3 - 1);
            }
            else
            {
                n++;
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (opponentHandValues[i] >= 3)
            {
                int count = 0;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 3);
            }
        }

        for (int i = 6; i < 14; i++)
        {
            if (opponentHandValues[i] == 2)
            {
                int count;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 2);
                break;
            }
        }

        /*foreach (var pair in opponentHandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }*/

        if (opponentHandValues[1] == 0 &&
            opponentHandValues[2] == 0 &&
            opponentHandValues[3] == 0 &&
            opponentHandValues[4] == 0 &&
            opponentHandValues[5] == 0 &&
            opponentHandValues[6] == 0 &&
            opponentHandValues[7] == 0 &&
            opponentHandValues[8] == 0 &&
            opponentHandValues[9] == 0 &&
            opponentHandValues[10] == 0 &&
            opponentHandValues[11] == 0 &&
            opponentHandValues[12] == 0 &&
            opponentHandValues[13] == 0)
        {
            Debug.Log("Opponent WINS UPPER SET");
            return;
        }
        else
        {
            Debug.Log("Not a Winning Hand");
        }


    }

    //Check Sets only
    public void checkOpponentWinningHandSets()
    {
        //Check Matches 1-13
        GetopponentHandValues();
        for (int i = 1; i < 14; i++)
        {
            if (opponentHandValues[i] >= 3)
            {
                int count = 0;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 3);
            }
        }

        for (int i = 1; i < 14; i++)
        {
            if (opponentHandValues[i] == 2)
            {
                int count;
                opponentHandValues.TryGetValue(i, out count);
                opponentHandValues.Remove(i);
                opponentHandValues.Add(i, count - 2);
                break;
            }
        }

        /*foreach (var pair in opponentHandValues)
        {
            Debug.Log("hand Values" + pair.Key.ToString() + ' ' + pair.Value.ToString());
        }*/

        if (opponentHandValues[1] == 0 &&
            opponentHandValues[2] == 0 &&
            opponentHandValues[3] == 0 &&
            opponentHandValues[4] == 0 &&
            opponentHandValues[5] == 0 &&
            opponentHandValues[6] == 0 &&
            opponentHandValues[7] == 0 &&
            opponentHandValues[8] == 0 &&
            opponentHandValues[9] == 0 &&
            opponentHandValues[10] == 0 &&
            opponentHandValues[11] == 0 &&
            opponentHandValues[12] == 0 &&
            opponentHandValues[13] == 0)
        {
            Debug.Log("Opponent WINS LOWER SET");
            return;
        }
        else
        {
            Debug.Log("Not a Winning Hand");
        }
    }
}
