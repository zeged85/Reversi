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
  

        public void getPlayers              // players ids
        (
            ref string player1_1,
            ref string player1_2
        )
        {
            player1_1 = "311132971";        // id1
            player1_2 = "036682177";        // id2
        }

        private char otherPlayer(char player)
        {
            if (player == '1')
                return '2';
            if (player == '2')
                return '1';
            return '0';
        }

   


        Tuple<int, Tuple<int, int>> maxValue(Board board, int alpha, int beta, int depth, char playerChar, long quantom)
        {

            if (quantom < 0)
            {
                Console.WriteLine("quantom");
               // int score = board.gameScore().Item1 - board.gameScore().Item2;
                return new Tuple<int, Tuple<int, int>>(int.MinValue, null); ;
            }

            Stopwatch mytimer = Stopwatch.StartNew();
            List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);

            int _n = board._n;
            int myChips = 1;
            int enemyChips = 1;
            for (int i = 0; i < _n; i++)
                for (int j = 0; j < _n; j++)
                {
                    if (board._boardGame[i, j] == otherPlayer(playerChar))
                        enemyChips += 1;
                    else if (board._boardGame[i, j] == playerChar)
                        myChips += 1;
                }

            int noZero = board.gameScore().Item2;
            if (noZero == 0)
            {
                noZero++;
            }

            int score = /*(board.gameScore().Item1 / noZero) * (legalMoves.Count + 1) * */ (myChips / enemyChips) * 100;

            if (depth ==0 || board.isTheGameEnded())
            {
                
                if (depth == 0)
                {
                //    Console.WriteLine("depth");
                }
                return new Tuple<int, Tuple<int, int>>(score, null);
            }


            
            Tuple<int, int> localBestMove = null;

            if (legalMoves.Count == 0)
            {
                //pass
                return minValue(board, alpha, beta, depth - 1, otherPlayer(playerChar), quantom - mytimer.ElapsedMilliseconds);
            }


            int bestResult = int.MinValue;

            foreach (Tuple<int, int> legalMove in legalMoves)
            {
                if (quantom - mytimer.ElapsedMilliseconds < 0)
                {
                 //   Console.WriteLine("break");
                    break;
                }
                Board newBoard = new Board(board);
                newBoard.fillPlayerMove(playerChar, legalMove.Item1, legalMove.Item2);
                int boardScore = newBoard.gameScore().Item1- board.gameScore().Item2;

                alpha = minValue(newBoard, alpha, beta, depth - 1, otherPlayer(playerChar), quantom-mytimer.ElapsedMilliseconds).Item1;


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


            return new Tuple<int,Tuple<int,int>>(alpha,localBestMove);


        }







        Tuple<int, Tuple<int, int>> minValue(Board board, int alpha, int beta, int depth, char playerChar, long quantom)
        {
            if (quantom < 0)
            {
                Console.WriteLine("quantom");
             //   int score = board.gameScore().Item1 - board.gameScore().Item2;
                return new Tuple<int, Tuple<int, int>>(int.MaxValue, null);
            }


            Stopwatch mytimer = Stopwatch.StartNew();
            List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);

            int _n = board._n;
            int myChips = 1;
            int enemyChips = 1;
            for (int i = 0; i < _n; i++)
                for (int j = 0; j < _n; j++)
                {
                    if (board._boardGame[i, j] == otherPlayer(playerChar))
                        enemyChips += 1;
                    else if (board._boardGame[i, j] == playerChar)
                        myChips += 1;
                }

            int noZero = board.gameScore().Item2;
            if (noZero == 0)
            {
                noZero++;
            }

                    int score = /*(board.gameScore().Item1 / noZero) * (legalMoves.Count + 1) * */ ( myChips / enemyChips ) * 100; //chips count


            if (depth == 0 || board.isTheGameEnded())
            {
                int n = board._n;
                
                if (depth == 0)
                {
                  //  Console.WriteLine("depth");
                }
                return new Tuple<int,Tuple<int,int>> (score, null);
            }

 
            
            Tuple<int, int> localBestMove = null;

            if (legalMoves.Count == 0)
            {
                //pass
                
                return maxValue(board, alpha, beta, depth - 1, otherPlayer(playerChar), quantom- mytimer.ElapsedMilliseconds);
            }


         
            int bestResult = int.MaxValue;

            foreach (Tuple<int, int> legalMove in legalMoves)
            {
                if (quantom - mytimer.ElapsedMilliseconds < 0)
                {
                //    Console.WriteLine("break");
                    break;
                }
                Board newBoard = new Board(board);
                newBoard.fillPlayerMove(playerChar, legalMove.Item1, legalMove.Item2);
                int boardScore = newBoard.gameScore().Item1- board.gameScore().Item2;
                int n = board._n;

                beta = maxValue(newBoard, alpha, beta, depth - 1, otherPlayer(playerChar), quantom- mytimer.ElapsedMilliseconds).Item1;


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

            return new Tuple < int,Tuple < int,int>> (beta, localBestMove);


        }



        Tuple<int, int> turn(Board board, char playerChar, TimeSpan timesup)
        {
            Stopwatch mytimer = Stopwatch.StartNew();
            TimeSpan timespan = timesup;
            long timeThreshold = 10;
            long quantom = timespan.Ticks - mytimer.Elapsed.Milliseconds - timeThreshold;

            int n = board._n;
           // MaxDepth = 10;
            
            Tuple<int,Tuple<int,int>> best = maxValue(board, int.MinValue, int.MaxValue, n*n/2, playerChar, quantom);
            int bestScore = best.Item1;
            Tuple<int, int> BestMove = best.Item2;


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
