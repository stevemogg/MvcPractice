using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Media;
using System.Net;

namespace MvcPractice.Areas.ChessGame.Models
{
    public class GetMovesModel
    {
        public int x { get; set; }
        public int y { get; set; }
        public ChessPieceType pieceType { get; set; }
    }
    public class ChessGameModel
    {
        public ChessBoard ChessBoard = new ChessBoard();
        public int TurnCount { get; set; }
        public bool Turn { get; set; }
        public string GameJsonString { get; set; }
        public GetMovesModel MovesModel { get; set; }
    }

    #region Chess Board
    public class ChessBoard
    {
        public ChessBoard()
        {
            List<ChessPiece?> line1 = new List<ChessPiece?>() { new RookPiece(true), new KnightPiece(true), new BishopPiece(true), new QueenPiece(true), new KingPiece(true), new BishopPiece(true), new KnightPiece(true), new RookPiece(true) };
            List<ChessPiece?> line2 = new List<ChessPiece?>() { new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true) };

            List<ChessPiece?> line7 = new List<ChessPiece?>() { new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false) };
            List<ChessPiece?> line8 = new List<ChessPiece?>() { new RookPiece(false), new KnightPiece(false), new BishopPiece(false), new QueenPiece(false), new KingPiece(false), new BishopPiece(false), new KnightPiece(false), new RookPiece(false) };

            this.Board = new List<ChessRow>() { };

            for (int x = 0; x < 8; x++)
            {
                ChessRow row = new ChessRow() { };
                row.X = x+1;

                for (int y = 0; y < 8; y++)
                {
                    ChessColumn col = new ChessColumn();
                    col.X = x + 1;
                    col.Y = y + 1;

                    if (x == 0)
                        col.Piece = line1[y];
                    else if (x == 1)
                        col.Piece = line2[y];
                    else if (x == 6)
                        col.Piece = line7[y];
                    else if (x == 7)
                        col.Piece = line8[y];

                    row.Columns.Add(col);
                }
                this.Board.Add(row);
            }
        }

        public List<ChessRow> Board { get; set; }
        public List<ChessPiece> Player1DeadPieces { get; set; }
        public List<ChessPiece> Player2DeadPieces { get; set; }
    }
    public class ChessRow 
    {
        public List<ChessColumn> Columns = new List<ChessColumn>();
        public int X { get; set; }
    }
    public class ChessColumn
    {
        public ChessPiece? Piece = null;
        //Co-ordinates        
        public int X { get; set; }
        public int Y { get; set; }
        public bool HighlightMove = false;
    }
    #endregion

    #region Chess Pieces
    public class ChessPiece
    {
        public ChessPiece(bool player1)
        {
            Player1 = player1;
        }
        public ChessPieceType Type { get; set; }
        public string FaIcon { get; set; }
        public bool Player1 { get; set; }
        public bool HasMoved = false;
        public bool Alive = true;
    }
    public class KingPiece : ChessPiece
    {
        public KingPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.King;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-king h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class QueenPiece : ChessPiece
    {
        public QueenPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Queen;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-queen h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class RookPiece : ChessPiece
    {
        public RookPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Rook;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-rook h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class BishopPiece : ChessPiece
    {
        public BishopPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Bishop;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-bishop h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class KnightPiece : ChessPiece
    {
        public KnightPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Knight;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-knight h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class PawnPiece : ChessPiece
    {
        public PawnPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Pawn;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-pawn h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\" data-pawn-moved=\"false\" ></i>";
        }
    }
    public enum ChessPieceType
    {
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }
    #endregion    
}