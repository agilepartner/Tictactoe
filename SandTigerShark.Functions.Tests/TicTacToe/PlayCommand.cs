using SandTigerShark.Functions.TicTacToe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandTigerShark.Functions.Tests.TicTacToe
{
    internal static class PlayCommand
    {
        public static Play New(
            int[] board = null, 
            int? player = null,
            int? position = null)
        {
            return new Play
            {
                Board = board ?? new Game().Board,
                Player = player ?? 1,
                Position = position ?? 1
            };
        }

        public static Play ForAnInvalidPlayer(this Play play)
        {
            return play.ForPlayer(-1);
        }

        public static Play ForPlayer(this Play play, int player)
        {
            play.Player = player;
            return play;
        }

        public static Play AtAnInvalidPosition(this Play play)
        {
            return play.AtPosition(10);
        }

        public static Play AtPosition(this Play play, int position)
        {
            play.Position = position;
            return play;
        }
    }
}
