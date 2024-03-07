using Microsoft.AspNetCore.Mvc;
using MvcPractice.Areas.ChessGame.Models;

namespace MvcPractice.Areas.ChessGame.Controllers
{
    [Area("ChessGame")]
    public class ChessGameController : Controller
    {
        public IActionResult Index()
        {
            ChessGameModel model = SetUpNewGameModel();

            return View(model);
        }
        public ChessGameModel SetUpNewGameModel() 
        {
            ChessGameModel newGame = new ChessGameModel();

            //Player1
            RookPiece OneRook1 = new RookPiece(true,1,1);
            KnightPiece OneKnight1 = new KnightPiece(true,1,2);
            BishopPiece OneBishop1 = new BishopPiece(true,1,3);
            QueenPiece OneQueen1 = new QueenPiece(true,1,4);
            KingPiece OneKing1 = new KingPiece(true,1,5);
            BishopPiece OneBishop2 = new BishopPiece(true,1,6);
            KnightPiece OneKnight2 = new KnightPiece(true,1,7);
            RookPiece OneRook2 = new RookPiece(true,1,8);
            newGame.Player1Pieces = new List<ChessPiece>();
            newGame.Player1Pieces.Add(OneRook1);
            newGame.Player1Pieces.Add(OneKnight1);
            newGame.Player1Pieces.Add(OneBishop1);
            newGame.Player1Pieces.Add(OneQueen1);
            newGame.Player1Pieces.Add(OneKing1);
            newGame.Player1Pieces.Add(OneBishop2);
            newGame.Player1Pieces.Add(OneKnight2);
            newGame.Player1Pieces.Add(OneRook2);
            for (int i = 1; i < 9; i++)
            {
                PawnPiece pawn = new PawnPiece(true,2,i);
                newGame.Player1Pieces.Add(pawn);
            }

            //Player2
            RookPiece TwoRook1 = new RookPiece(false, 8, 1);
            KnightPiece TwoKnight1 = new KnightPiece(false, 8, 2);
            BishopPiece TwoBishop1 = new BishopPiece(false, 8, 3);
            QueenPiece TwoQueen1 = new QueenPiece(false, 8, 4);
            KingPiece TwoKing1 = new KingPiece(false, 8, 5);
            BishopPiece TwoBishop2 = new BishopPiece(false, 8, 6);
            KnightPiece TwoKnight2 = new KnightPiece(false, 8, 7);
            RookPiece TwoRook2 = new RookPiece(false, 8, 8);
            newGame.Player2Pieces = new List<ChessPiece>();
            newGame.Player2Pieces.Add(TwoRook1);
            newGame.Player2Pieces.Add(TwoKnight1);
            newGame.Player2Pieces.Add(TwoBishop1);
            newGame.Player2Pieces.Add(TwoQueen1);
            newGame.Player2Pieces.Add(TwoKing1);
            newGame.Player2Pieces.Add(TwoBishop2);
            newGame.Player2Pieces.Add(TwoKnight2);
            newGame.Player2Pieces.Add(TwoRook2);
            for (int i = 1; i < 9; i++)
            {
                PawnPiece pawn = new PawnPiece(false, 7, i);
                newGame.Player2Pieces.Add(pawn);
            }

            return newGame;
        }
    }
}