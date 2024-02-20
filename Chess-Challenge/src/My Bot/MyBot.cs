using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot //jazz 1.3
{
    public int[] pieceValues = {100, 320, 330, 500, 900 , 0};

    Dictionary<string, int> tableIndex = new Dictionary<string, int>()
    {
        { "Pawn", 0 },
        { "Knight", 1 },
        { "Bishop", 2 },
        { "Rook", 3 },
        { "Queen", 4 },
        { "King", 5 }
    };

    public List<List<int>> pieceTables = new List<List<int>>
    {
        //Pawn
        new List<int>
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5, 5, 10, 25, 25, 10, 5, 5,
            0, 0, 0, 25, 25, 0, 0, 0,
            5, 0, -5, 10, 10, -5, 0, 5,
            5, 10, 10, -20, -20, 10, 10, 5,
            0, 0, 0, 0, 0, 0, 0, 0
        },
        // knight
        new List<int>
        {
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -30, 0, 10, 15, 15, 10, 0, -30,
            -30, 5, 15, 20, 20, 15, 5, -30,
            -30, 0, 15, 20, 20, 15, 0, -30,
            -30, 5, 10, 15, 15, 10, 5, -30,
            -40, -20, 0, 5, 5, 0, -20, -40,
            -50, -25, -30, -30, -30, -30, -25, -50,
        },
        // bishop
        new List<int>
        {
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 10, 10, 5, 0, -10,
            -10, 5, 5, 10, 10, 5, 5, -10,
            -10, 0, 10, 10, 10, 10, 0, -10,
            -10, 10, 10, 10, 10, 10, 10, -10,
            -10, 5, 0, 0, 0, 0, 5, -10,
            -20, -10, -10, -10, -10, -10, -10, -20,
        },
        //rook
        new List<int>
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            5, 10, 10, 10, 10, 10, 10, 5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            0, 0, 0, 5, 5, 0, 0, 0
        },
        //queen
        new List<int>
        {
            -20, -10, -10, -5, -5, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 5, 5, 5, 0, -10,
            -5, 0, 5, 5, 5, 5, 0, -5,
            0, 0, 5, 5, 5, 5, 0, -5,
            -10, 5, 5, 5, 5, 5, 0, -10,
            -10, 0, 5, 0, 0, 0, 0, -10,
            -20, -10, -10, -5, -5, -10, -10, -20
        },
        // king
        new List<int>
        {
            -50, -40, -30, -20, -20, -30, -40, -50,
            -30, -20, -10, -10, -10, -10, -20, -30,
            -30, -10, -30, -30, -30, -30, -10, -30,
            -30, -10, -20, -20, -20, -20, -10, -30,
            -30, -10, -10, -10, -10, -10, -10, -30,
            -30, -10, -10, -10, -10, -10, -10, -30,
            -30, -30, -5, -5, -5, -5, -30, -30,
            50, 30, 0, 0, 0, 0, 30, 50
        }


    };
    public int getEval(Board board, bool Iswhite)
    {
        int eval = 0;
        PieceList[] pieceslist = board.GetAllPieceLists();
        PieceList currentPiece = null;
        int x = 0;
        for(int i = 0;i<pieceslist.Length;i++)
        {
            x = tableIndex[pieceslist[i].TypeOfPieceInList.ToString()];
            if (i < pieceslist.Length / 2)
            {
                currentPiece = board.GetPieceList(pieceslist[i].TypeOfPieceInList,true);
                for (int j = 0; j < currentPiece.Count; j++)
                {
                    eval += pieceValues[x] + pieceTables[x][currentPiece[j].Square.Index];
                }
            }
            else
            {
                currentPiece = board.GetPieceList(pieceslist[i].TypeOfPieceInList, false);
                for (int j = 0; j < currentPiece.Count; j++)
                {
                    eval -= pieceValues[x] + pieceTables[x][63-currentPiece[j].Square.Index];
                }
            }
        }
        if(Iswhite) 
        {
            return eval + board.GetLegalMoves().Length;
        }
        else
        {
            return -eval - board.GetLegalMoves().Length;
        }
    }
    public Move[] mvvlva(Move[] moves)
    {
        int[] movescores = new int[moves.Length];
        for(int i = 0; i < moves.Length; i++)
        {
            movescores[i] = 0;
            if (moves[i].IsCapture)
            {
                movescores[i] = 100 * (int)moves[i].CapturePieceType - (int)moves[i].MovePieceType;
            }
        }
        Array.Sort(movescores, moves);
        Array.Reverse(moves);
        return moves;
    }
    public (Move, int) search(Board board, bool iswhite, int depth, int alpha, int beta)
    {
        if (board.IsInCheckmate())
        {
            return (Move.NullMove, -9999);
        }
        if (board.IsDraw())
        {
            return (Move.NullMove, -1);
        }
        bool qsearch = depth <= 0;
        int maxscore = -100000;
        Move[] moves = board.GetLegalMoves(qsearch);
        if (qsearch)
        {
            int standpat = getEval(board, iswhite);
            if (standpat >= beta)
            {
                return (Move.NullMove, beta);
            }
            if(alpha < standpat)
            {
                alpha = standpat;
            }
        }
        if(moves.Length == 0)
        {
            return((Move.NullMove,getEval(board, iswhite)));
        }
        Move bestmove = moves[0];
        moves = mvvlva(moves);
        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int score = -search(board, !iswhite, depth - 1, -beta, -alpha).Item2;
            board.UndoMove(move);
            if (score > maxscore)
            {
                maxscore = score;
                bestmove = move;
            }
            alpha = Math.Max(alpha, maxscore);
            if(alpha >= beta)
            {
                break;
            }
        }
        return (bestmove, maxscore);
    }
    public Move Think(Board board, Timer timer)
    {
        var (bestmove, eval) = search(board, board.IsWhiteToMove,4,-100000, 100000);
        //Console.WriteLine(eval/100.0);
        return bestmove;
    }
}