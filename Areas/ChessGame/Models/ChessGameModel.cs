using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Media;
using System.Net;

namespace MvcPractice.Areas.ChessGame.Models
{    
    public class ChessGameModel
    {
        public ChessBoard ChessBoard { get; set; }
        public PieceToPotentialMove PieceToPotentialMove { get; set; }
        public int TurnCount { get; set; }
        public bool Turn { get; set; }
    }
    #region Chess functionality models
    public class PieceToPotentialMove
    {
        public int x { get; set; }
        public int y { get; set; }
        public ChessPieceType pieceType { get; set; }
    }
    public class MovePieceModel
    {
        public int x { get; set; }
        public int y { get; set; }
    }
    #endregion

    #region Chess Board
    public class ChessBoard
    {
        public List<ChessRow> Rows { get; set; }
        public List<ChessPiece> Player1DeadPieces { get; set; }
        public List<ChessPiece> Player2DeadPieces { get; set; }
    }
    public class ChessRow 
    {
        public List<ChessColumn> Columns { get; set; }
        public int X { get; set; }
    }
    public class ChessColumn
    {
        public ChessPiece? Piece { get; set; }
        //Co-ordinates        
        public int X { get; set; }
        public int Y { get; set; }
        public bool HighlightMove { get; set; }
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
        public bool HasMoved { get; set; }
        public bool Alive { get; set; }
    }
    public class KingPiece : ChessPiece
    {
        public KingPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.King;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-king\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class QueenPiece : ChessPiece
    {
        public QueenPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Queen;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-queen\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class RookPiece : ChessPiece
    {
        public RookPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Rook;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-rook\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class BishopPiece : ChessPiece
    {
        public BishopPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Bishop;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-bishop\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class KnightPiece : ChessPiece
    {
        public KnightPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Knight;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-knight\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
        }
    }
    public class PawnPiece : ChessPiece
    {
        public PawnPiece(bool player1) : base(player1)
        {
            this.Type = ChessPieceType.Pawn;
            this.FaIcon = $"<i class=\"fa-solid fa-chess-pawn\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"{this.Type}\" data-player=\"{(player1 ? 1 : 2)}\" data-pawn-moved=\"false\" ></i>";
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