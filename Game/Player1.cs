using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player1 : Player
    {
        static int MaxDepth;
        static Tuple<int, int> BestMove;
        static Stopwatch mytimer;
        static TimeSpan timespan;
        static int MaxPrize;


        public void getPlayers              // players ids
        (
            ref string player1_1,
            ref string player1_2
        )
        {
            player1_1 = "311132971";        // id1
            player1_2 = "036682177";        // id2
        }

        private static char otherPlayer(char player)
        {
            if (player == '1')
                return '2';
            if (player == '2')
                return '1';
            return '0';
        }

   


        static int maxValue(Board board, int alpha, int beta, int depth, char playerChar)
        {

            if (depth >= MaxDepth || !isTimeOk() || board.isTheGameEnded())
            {
                int n = board._n;
                int score = board.gameScore().Item1 - board.gameScore().Item2;
                if (board._boardGame[0,0] == playerChar)
                {
                    score += MaxPrize;
                }
                if (board._boardGame[n-1, 0] == playerChar)
                {
                    score += MaxPrize;
                }
                if (board._boardGame[0, n-1] == playerChar)
                {
                    score += MaxPrize;
                }
                if (board._boardGame[n-1, n-1] == playerChar)
                {
                    score += MaxPrize;
                }

                return score;
            }


            List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);
            Tuple<int, int> localBestMove = null;

            if (legalMoves.Count == 0)
            {
                //pass
                return minValue(board, alpha, beta, depth + 1, otherPlayer(playerChar));
            }


            int bestResult = int.MinValue;

            foreach (Tuple<int, int> legalMove in legalMoves)
            {
                
                Board newBoard = new Board(board);
                newBoard.fillPlayerMove(playerChar, legalMove.Item1, legalMove.Item2);
                int boardScore = newBoard.gameScore().Item1- board.gameScore().Item2;
                int n = board._n;
                if (newBoard._boardGame[0, 0] == playerChar)
                {
                    boardScore += MaxPrize;
                }
                if (newBoard._boardGame[n - 1, 0] == playerChar)
                {
                    boardScore += MaxPrize;
                }
                if (newBoard._boardGame[0, n - 1] == playerChar)
                {
                    boardScore += MaxPrize;
                }
                if (newBoard._boardGame[n - 1, n - 1] == playerChar)
                {
                    boardScore += MaxPrize;
                }
                alpha = minValue(newBoard, alpha, beta, depth + 1, otherPlayer(playerChar));


                if (alpha > bestResult)
                {
                    localBestMove = legalMove;
                    bestResult = alpha;
                }

                if (alpha >= beta)
                {
                    //System.out.println("(II) Beta cut!");
                    break;
                }

            }


            BestMove = new Tuple<int, int>(localBestMove.Item1, localBestMove.Item2);
            return alpha;


        }


        static int minValue(Board board, int alpha, int beta, int depth, char playerChar)
        {

            if (depth >= MaxDepth || !isTimeOk() || board.isTheGameEnded())
            {
                int n = board._n;
                int score = (board.gameScore().Item1 - board.gameScore().Item2);
                if (board._boardGame[0, 0] == playerChar)
                {
                    score -= MaxPrize;
                }
                if (board._boardGame[n-1, 0] == playerChar)
                {
                    score -= MaxPrize;
                }
                if (board._boardGame[0, n-1] == playerChar)
                {
                    score -= MaxPrize;
                }
                if (board._boardGame[n-1, n-1] == playerChar)
                {
                    score -= MaxPrize;
                }

                return score;
            }


            List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);
            Tuple<int, int> localBestMove = null;

            if (legalMoves.Count == 0)
            {
                //pass
                return maxValue(board, alpha, beta, depth + 1, otherPlayer(playerChar));
            }


         
            int bestResult = int.MaxValue;

            foreach (Tuple<int, int> legalMove in legalMoves)
            {

                Board newBoard = new Board(board);
                newBoard.fillPlayerMove(playerChar, legalMove.Item1, legalMove.Item2);
                int boardScore = newBoard.gameScore().Item1- board.gameScore().Item2;
                int n = board._n;
                if (newBoard._boardGame[0, 0] == playerChar)
                {
                    boardScore -= MaxPrize;
                }
                if (newBoard._boardGame[n - 1, 0] == playerChar)
                {
                    boardScore -= MaxPrize;
                }
                if (newBoard._boardGame[0, n - 1] == playerChar)
                {
                    boardScore -= MaxPrize;
                }
                if (newBoard._boardGame[n - 1, n - 1] == playerChar)
                {
                    boardScore -= MaxPrize;
                }
                beta = maxValue(newBoard, alpha, beta, depth + 1, otherPlayer(playerChar));


                if (beta < bestResult)
                {
                    localBestMove = legalMove;
                    bestResult = beta;
                }

                if (beta <= alpha)
                {
                    //System.out.println("(II) Beta cut!");
                    break;
                }

            }


            BestMove = new Tuple<int,int>(localBestMove.Item1,localBestMove.Item2);
            return beta;


        }

        public static bool isTimeOk()
        {
            
            int timeThreshold = 25;
            long maxTime = timespan.Ticks - timeThreshold;

            if (mytimer.Elapsed.Milliseconds > maxTime)
            {
                //System.out.println("(WW) Runnning out of time...");
               


                return false;
            }
            
            return true;
        }

            public static Tuple<int, int> turn(Board board, char playerChar, TimeSpan timesup)
        {
            mytimer = Stopwatch.StartNew();
            timespan = timesup;
            //List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);
            // BestMove = new Tuple<int, int>(legalMoves[0].Item1, legalMoves[0].Item2);
            /*
                        if (legalMoves.Count == 0)
                        {
                            return null;
                        }
                        */
            int n = board._n;
            MaxDepth = 10;

            MaxPrize = n * n;

            int bestScore = maxValue(board, int.MinValue, int.MaxValue, 0, playerChar);


            if (BestMove == null)
            {

            }
            
            return BestMove;
        }

        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup,
            char playerChar          // 1 or 2
        )
        {
            return turn(board, playerChar, timesup);

        }
    }
}
