using ChessChallenge.API;
using System;

public class Jazz10 : IChessBot //jazz 1.0
{
    public int[] pieceValues = {10, 30, 30, 50, 90 , 0};
    public int getEval(Board board, bool Iswhite)
    {
        int eval = 0;
        PieceList[] pieceslist = board.GetAllPieceLists();
        for(int i = 0;i<pieceslist.Length;i++)
        {
            if (i < pieceslist.Length / 2)
            {
                eval += pieceslist[i].Count * pieceValues[i];
            }
            else
            {
                eval -= pieceslist[i].Count * pieceValues[i-6];
            }
        }
        if(Iswhite) 
        {
            return eval;
        }
        else
        {
            return -eval;
        }
    }
    public Tuple<int,int> getBestMove(Board board, bool iswhite,int depth)
    {
        int curreval = getEval(board, iswhite);
        if (depth <= 0)
        {
            return Tuple.Create(0,curreval);
        }
        if (board.IsInCheckmate())
        {
            return Tuple.Create(0,-9999);
        }
        if (board.IsDraw())
        {
            return Tuple.Create(0,0);
        }
        int bestMove = 0;
        int maxeval = -9999;
        Move[] moves = board.GetLegalMoves();
        for(int i = 0; i<moves.Length;i++)
        {
            bool truesearch = false;
            if (moves[i].IsPromotion || moves[i].IsCapture || board.IsInCheck())
            {
                truesearch = true;
            }
            board.MakeMove(moves[i]);
            if (board.IsInCheck() || truesearch)
            {
                int x = -getBestMove(board, !iswhite, depth - 2).Item2;
                if (x > maxeval)
                {
                    maxeval = x;
                    bestMove = i;
                }
            }
            else
            {
                int x = -getBestMove(board, !iswhite, depth - 4).Item2;
                if (x > maxeval)
                {
                    maxeval = x;
                    bestMove = i;
                }
            }
            board.UndoMove(moves[i]);
        }
        return Tuple.Create(bestMove,maxeval);
    }
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        bool white = board.IsWhiteToMove;
        var (bestmove, eval) = getBestMove(board, white,7);
        //Console.WriteLine(moves[bestmove]);
        return moves[bestmove];
    }
}