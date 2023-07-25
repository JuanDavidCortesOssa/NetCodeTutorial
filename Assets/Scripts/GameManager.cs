using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action<TurnManager.PlayerTurn> OnPlayerWin;
    public event Action OnGameDraw;

    [SerializeField] private TextMeshProUGUI[] boardState;

    // Winning combinations (indexes of the cells)
    private int[][] winCombinations = new int[][]
    {
        new int[] { 0, 1, 2 }, // Row 1
        new int[] { 3, 4, 5 }, // Row 2
        new int[] { 6, 7, 8 }, // Row 3
        new int[] { 0, 3, 6 }, // Column 1
        new int[] { 1, 4, 7 }, // Column 2
        new int[] { 2, 5, 8 }, // Column 3
        new int[] { 0, 4, 8 }, // Diagonal 1
        new int[] { 2, 4, 6 } // Diagonal 2
    };

    private bool gameEnded = false;

    public void CheckGameEnd()
    {
        Debug.Log("Check");
        // Check for a win
        for (int i = 0; i < winCombinations.Length; i++)
        {
            int a = winCombinations[i][0];
            int b = winCombinations[i][1];
            int c = winCombinations[i][2];

            string first = boardState[a].text;
            string second = boardState[b].text;
            string third = boardState[c].text;

            if (first != "" && first == second && second == third)
            {
                gameEnded = true;
                OnPlayerWin?.Invoke(first == "X" ? TurnManager.PlayerTurn.Server : TurnManager.PlayerTurn.Client);
                Debug.Log("Player " + boardState[a].text + " wins!");
                return;
            }
        }

        // Check for a draw
        if (!gameEnded)
        {
            bool isDraw = true;
            foreach (TextMeshProUGUI cellState in boardState)
            {
                if (cellState.text == "")
                {
                    isDraw = false;
                    break;
                }
            }

            if (isDraw)
            {
                gameEnded = true;
                Debug.Log("It's a draw!");
                OnGameDraw?.Invoke();
            }
        }
    }
}