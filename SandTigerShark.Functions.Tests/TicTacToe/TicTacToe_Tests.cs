using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SandTigerShark.Functions.TicTacToe;
using System;
using System.Linq;

namespace SandTigerShark.Functions.Tests.TicTacToe
{
    public class TicTacToe_Tests
    {
        public class initialization_should
        {
            [TestClass]
            public class raise_an_argument_null_exception
            {
                [TestMethod, ExpectedException(typeof(ArgumentNullException))]
                public void when_the_passed_board_is_null()
                {
                    new Game(null);
                }
            }

            [TestClass]
            public class raise_an_argument_exception
            {
                [TestMethod, ExpectedException(typeof(ArgumentException))]
                public void when_the_passed_board_is_too_big()
                {
                    new Game(new int[10]);
                }

                [TestMethod, ExpectedException(typeof(ArgumentException))]
                public void when_the_passed_board_is_too_small()
                {
                    new Game(new int[1]);
                }

                [TestMethod, ExpectedException(typeof(ArgumentException))]
                public void when_the_passed_board_is_invalid()
                {
                    new Game(new int[] { 1, 1, 2, 1, 1, 2, 1, 1, 2 });
                }
            }
        }

        public class play_should
        {
            protected void Play(Game game, Play command)
            {
                game.Play(command.Player, command.Position);
            }

            [TestClass]
            public class raise_an_argument_exeption : play_should
            {
                [TestMethod, ExpectedException(typeof(ArgumentException))]
                public void when_the_passed_player_is_not_1_or_2()
                {
                    var game = new Game();
                    var command = PlayCommand.New()
                                            .ForAnInvalidPlayer();

                    game.Play(command.Player, command.Position);
                }

                [TestMethod, ExpectedException(typeof(ArgumentException))]
                public void when_the_position_is_not_on_the_board()
                {
                    var game = new Game();
                    var command = PlayCommand.New()
                                            .AtAnInvalidPosition();

                    game.Play(command.Player, command.Position);
                }
            }

            [TestClass]
            public class update_the_status_of_an_empty_game : play_should
            {
                private Game game;

                [TestInitialize]
                public void Init()
                {
                    game = new Game();
                }

                [TestMethod]
                public void by_putting_the_player_on_position()
                {
                    var command = PlayCommand.New()
                                           .ForPlayer(1)
                                           .AtPosition(9);

                    Play(game, command);

                    game.Board[8].Should().Be(command.Player);
                    game.Board.Where(v => v == 0).Should().HaveCount(8);
                }
            }

            [TestClass]
            public class notupdate_the_status_of_an_in_progress_game : play_should
            {
                private int[] initialBoard;
                private Game inProgressGame;

                [TestInitialize]
                public void Init()
                {
                    initialBoard = new int[] { 1, 0, 0, 0, 1, 0, 0, 0, 2 };
                    inProgressGame = new Game(initialBoard);
                }


                [TestMethod]
                public void when_a_player_try_to_play_on_an_occupied_value()
                {
                    var command = PlayCommand.New()
                                           .ForPlayer(1)
                                           .AtPosition(5);
                    
                    var inProgressBoard = new int[] { 1, 0, 0, 0, 2, 0, 0, 0, 2 };
                    var game = new Game(inProgressBoard);

                    Play(game, command);
                    game.Board.Should().BeEquivalentTo(inProgressBoard);
                }
            }

            [TestClass]
            public class on_an_in_progress_game : play_should
            {
                [TestMethod]
                public void update_the_status_to_gameOver_when_a_player_wins()
                {
                    var command = PlayCommand.New()
                                           .ForPlayer(2)
                                           .AtPosition(3);

                    var game = new Game(new int[] { 1, 2, 0, 1, 2, 1, 2, 1, 2 });

                    Play(game, command);

                    game.IsGameOver.Should().BeTrue();
                    game.Winner.Should().Be(2);
                    game.IsDraw.Should().BeFalse();
                }

                [TestMethod]
                public void update_the_status_to_gameOver_when_draw()
                {
                    var command = PlayCommand.New()
                                           .ForPlayer(2)
                                           .AtPosition(9);

                    var game = new Game(new int[] { 1, 2, 1, 2, 1, 2, 2, 1, 0 });

                    Play(game, command);

                    game.IsGameOver.Should().BeTrue();
                    game.Winner.Should().BeNull();
                    game.IsDraw.Should().BeTrue();
                }

                [TestMethod]
                public void set_the_player_on_the_position()
                {
                    var command = PlayCommand.New()
                                           .ForPlayer(1)
                                           .AtPosition(8);

                    var game = new Game(new int[] { 1,0,0,0,0,1,2,0,2 });

                    Play(game, command);

                    game.Board.Select((value, index) => new { index, value })
                        .Where(kvp => kvp.index != 7)
                        .ToList()
                        .ForEach(kvp => kvp.value.Should().Be(game.Board[kvp.index]));

                    game.Board[7].Should().Be(1);
                    game.IsGameOver.Should().BeFalse();
                    game.Winner.Should().BeNull();
                    game.IsDraw.Should().BeFalse();
                }
            }
        }
    }
}
