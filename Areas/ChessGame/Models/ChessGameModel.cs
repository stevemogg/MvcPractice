using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Media;

namespace MvcPractice.Areas.ChessGame.Models
{
    public class ChessGameModel
    {
        public List<ChessPiece> Player1Pieces { get; set; }
        public List<ChessPiece> Player1DeadPieces { get; set; }
        public List<ChessPiece> Player2Pieces { get; set; }
        public List<ChessPiece> Player2DeadPieces { get; set; }
    }
    public class ChessPiece
    {
        public ChessPiece(bool player1, int x, int y)
        {
            Player1 = player1;
            X = x;
            Y = y;
        }
        public ChessPieceType Type { get; set; }          
                
        public string FaIcon { get; set; }
        private bool Player1 { get; set; }

        //Co-ordinates
        public int StartY { get; set; }
        public int StartX { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
    public class KingPiece : ChessPiece
    {
        public KingPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-king h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"king\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
            this.Type = ChessPieceType.King;
        }
    }
    public class QueenPiece : ChessPiece
    {
        public QueenPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-queen h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"queen\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
            this.Type = ChessPieceType.Queen;
        }
    }
    public class RookPiece : ChessPiece
    {
        public RookPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-rook h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"rook\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
            this.Type = ChessPieceType.Rook;
        }
    }
    public class BishopPiece : ChessPiece
    {
        public BishopPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-bishop h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"bishop\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
            this.Type = ChessPieceType.Bishop;
        }
    }
    public class KnightPiece : ChessPiece
    {
        public KnightPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-knight h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"knight\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\"></i>";
            this.Type = ChessPieceType.Knight;
        }
    }
    public class PawnPiece : ChessPiece
    {
        public PawnPiece(bool player1, int x, int y) : base(player1, x, y)
        {
            this.FaIcon = $"<i class=\"fa-solid fa-chess-pawn h-100 w-100\" style=\"color:{(player1 ? "black" : "white")};\" data-piece=\"pawn\" data-x=\"{x}\" data-y=\"{y}\" data-player=\"{(player1 ? 1 : 2)}\" data-pawn-moved=\"false\" ></i>";
            this.Type = ChessPieceType.Pawn;
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
}