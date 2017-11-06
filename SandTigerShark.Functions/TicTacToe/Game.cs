using System;
using System.Collections.Generic;
using System.Linq;

namespace SandTigerShark.Functions.TicTacToe
{
    public sealed class Game
    {
        private const int empty = 0;
        private const int player1 = 1;
        private const int player2 = 2;
        private int[] acceptableValues = new int[] { empty, player1, player2 };

        public int[] Board { get; }
        public bool IsGameOver { get { return IsDraw || Winner.HasValue; } }
        public bool IsDraw { get; private set; }
        public int? Winner { get; private set; }

        private readonly IReadOnlyList<int[]> winningCombinations = new List<int[]>
        {
            //Horizontal
            new int[] { 0,1,2 },
            new int[] { 3,4,5 },
            new int[] { 6,7,8 },

            //Vertical
            new int[] { 0,3,6 },
            new int[] { 1,4,7 },
            new int[] { 2,5,8 },

            //Cross
            new int[] { 0,4,8},
            new int[] { 2,4,6 },
        };

        public Game()
        {
            Board = new int[9];

            for (var i = 0; i < Board.Length; i++)
            {
                Board[i] = empty;
            }
        }

        public Game(int[] board)
        {
            CheckIfValid(board);
            Board = board;
            UpdateStatus();
        }

        public bool CanPlay(int player)
        {
            if (!IsGameOver)
            {
                var otherPlayer = player == player1 ? player2 : player1;
                return Board.Count(v => v == player) <= Board.Count(v => v == otherPlayer);
            }
            return false;
        }

        public void Play(int player, int position)
        {
            CheckPlayer(player);
            int playPosition = position - 1;
            CheckPosition(playPosition);

            if (!CanPlay(player, playPosition))
            {
                return;
            }

            Board[playPosition] = player;
            UpdateStatus();
        }

        private bool CanPlay(int player, int position)
        {
            return CanPlay(player) &&
                    IsEmpty(position);
        }

        private bool IsEmpty(int position)
        {
            return Board[position] == empty;
        }

        private void UpdateStatus()
        {
            if (IsAUserPlayed3Times())
            {
                CheckWinner();

                if (!IsGameOver)
                {
                    IsDraw = !ContainsEmptyValue();
                }
            }
        }

        private bool IsAUserPlayed3Times() => Board.Count(c => c == player1) >= 3
                   || Board.Count(c => c == player2) >= 3;

        private void CheckWinner()
        {
            foreach (var combination in winningCombinations)
            {
                var valuesInCombination = ExtractCombination(combination);
                var distinctPlayersInCombination = valuesInCombination.Distinct();

                if (distinctPlayersInCombination.Count() == 1 &&
                    distinctPlayersInCombination.First() != empty)
                {
                    Winner = distinctPlayersInCombination.First();
                    break;
                }
            }
        }

        private bool ContainsEmptyValue() => Board.Contains(empty);

        private int[] ExtractCombination(int[] positions)
        {
            int[] extractedValues = new int[3];

            for (int i = 0; i < positions.Length; i++)
            {
                extractedValues[i] = Board[positions[i]];
            }
            return extractedValues;
        }

        #region Checks

        private void CheckIfValid(int[] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }

            if (board.Length != 9 || board.Except(acceptableValues).Any())
            {
                throw new ArgumentException("Invalid game");
            }

            if (Math.Abs(board.Count(v => v == player1) - board.Count(v => v == player2)) > 1)
            {
                throw new ArgumentException("Invalid game : one player has played too much.");
            }
        }
        
        private void CheckPosition(int position)
        {
            if (position < 0 || position > 8)
            {
                throw new ArgumentException("Invalid position supplied");
            }
        }

        private static void CheckPlayer(int player)
        {
            if (player != player1 && player != player2)
            {
                throw new ArgumentException("Invalid player supplied");
            }
        }

        #endregion  
    }
}
