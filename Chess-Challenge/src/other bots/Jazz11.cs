using ChessChallenge.API;
using System;

public class Jazz11 : IChessBot //jazz 1.1
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
    public Tuple<int, int> getBestMoveself(Board board, bool iswhite, int depth, int alpha, int beta)
    {
        if (depth <= 0)
        {
            return Tuple.Create(0, getEval(board, iswhite));
        }
        if (board.IsInCheckmate())
        {
            return Tuple.Create(0, -9999);
        }
        if (board.IsDraw())
        {
            return Tuple.Create(0, -1);
        }
        int max = int.MinValue;
        int bestmove = 0;
        Move[] moves = board.GetLegalMoves();
        for(int i = 0;i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            int score = getBestMoveEnemy(board, iswhite, depth - 1, alpha, beta).Item2;
            board.UndoMove(moves[i]);
            if (score > max)
            {
                max = score;
                bestmove = i;
            }
            alpha = Math.Max(alpha, max);
            if(beta <= alpha)
            {
                break;
            }
        }
        return Tuple.Create(bestmove, max);
    }
    public Tuple<int, int> getBestMoveEnemy(Board board, bool iswhite, int depth, int alpha, int beta)
    {
        if (depth <= 0)
        {
            return Tuple.Create(0, -getEval(board, iswhite));
        }
        if (board.IsInCheckmate())
        {
            return Tuple.Create(0, 9999);
        }
        if (board.IsDraw())
        {
            return Tuple.Create(0, 1);
        }
        int min = int.MaxValue;
        int bestmove = 0;
        Move[] moves = board.GetLegalMoves();
        for (int i = 0; i < moves.Length; i++)
        {
            board.MakeMove(moves[i]);
            int score = getBestMoveself(board, iswhite, depth - 1, alpha, beta).Item2;
            board.UndoMove(moves[i]);
            if (score < min)
            {
                min = score;
                bestmove = i;
            }
            beta = Math.Min(beta, min);
            if (beta <= alpha)
            {
                break;
            }
        }
        return Tuple.Create(bestmove, min);
    }
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        bool white = board.IsWhiteToMove;
        var (bestmove, eval) = getBestMoveself(board, white,4,int.MinValue,int.MaxValue);
        //Console.WriteLine((bestmove, eval));
        return moves[bestmove];
    }
}