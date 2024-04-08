using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Dashboards.WebApi;//
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Process.WebApi.Models;
using Microsoft.VisualStudio.Services.CircuitBreaker;
using Microsoft.VisualStudio.Services.Profile;
using Microsoft.VisualStudio.Services.TestResults.WebApi;
using MvcPractice.Areas.ChessGame.Models;
using MvcPractice.Helpers;
using Newtonsoft.Json;
using Ninject.Activation.Caching;
using System.IO.Pipelines;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;//

namespace MvcPractice.Areas.ChessGame.Controllers
{
    [Area("ChessGame")]
    public class ChessGameController : Controller
    {
        #region Dependancy Injection && Game Setup / Get / ResetPotentialMoves
        private readonly IViewRenderService _viewRenderService;
        public ChessGameController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }
        public IActionResult Index()
        {
            ChessGameModel model = CreateGame();
            return View(model);
        }
        [NonAction]
        public ChessGameModel CreateGame()
        {
            try
            {
                ChessGameModel model = new ChessGameModel() { };

                model.PromotionPieces = new List<ChessPiece>() { new KnightPiece(true), new BishopPiece(true), new RookPiece(true), new QueenPiece(true) };
                
                model.ChessBoard = new ChessBoard();

                model.ChessBoard.Player1DeadPieces = new List<ChessPiece> { };
                model.ChessBoard.Player2DeadPieces = new List<ChessPiece> { };

                List<ChessPiece?> line1 = new List<ChessPiece?>() { new RookPiece(true), new KnightPiece(true), new BishopPiece(true), new QueenPiece(true), new KingPiece(true), new BishopPiece(true), new KnightPiece(true), new RookPiece(true) };
                List<ChessPiece?> line2 = new List<ChessPiece?>() { new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true) };

                List<ChessPiece?> line7 = new List<ChessPiece?>() { new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false) };
                List<ChessPiece?> line8 = new List<ChessPiece?>() { new RookPiece(false), new KnightPiece(false), new BishopPiece(false), new QueenPiece(false), new KingPiece(false), new BishopPiece(false), new KnightPiece(false), new RookPiece(false) };

                model.ChessBoard.Board = new List<List<ChessColumn>>() { };

                for (int x = 0; x < 8; x++)
                {
                    List<ChessColumn> row = new List<ChessColumn>() { };
                    for (int y = 0; y < 8; y++)
                    {
                        ChessColumn col = new ChessColumn();
                        col.X = x;
                        col.Y = y;

                        if (x == 0)
                            col.Piece = line1[y];
                        else if (x == 1)
                            col.Piece = line2[y];
                        else if (x == 6)
                            col.Piece = line7[y];
                        else if (x == 7)
                            col.Piece = line8[y];

                        row.Add(col);
                    }
                    model.ChessBoard.Board.Add(row);
                }
                HttpContext.Session.SetString("UserData", Newtonsoft.Json.JsonConvert.SerializeObject(model));
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [NonAction]
        public ChessGameModel GetGame()
        {
            var userData = HttpContext.Session.GetString("UserData");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ChessGameModel>(userData);
        }
        [NonAction]
        public void SetGame(ChessGameModel model)
        {
            HttpContext.Session.SetString("UserData", Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }
        [NonAction]
        public ChessGameModel ResetPotentialMovesFromGame(ChessGameModel model)
        {
            foreach (var row in model.ChessBoard.Board)
            {
                foreach (var col in row)
                {
                    col.HighlightMove = false;
                }
            }
            model.PieceToPotentialMove = null;
            return model;
        }
        [NonAction]
        public ChessGameModel ResetPotentialMovesFromSession()
        {
            ChessGameModel model = GetGame();
            foreach (var row in model.ChessBoard.Board)
            {
                foreach (var col in row)
                {
                    col.HighlightMove = false;
                }
            }
            model.PieceToPotentialMove = null;
            return model;
        }
        #endregion

        #region Game Functions
        [NonAction]
        public int PlayerMoveDirection(bool player1)
        {
            //Function that returns 1 or -1 dependant on true or false
            //So that when we create moves it will go up or down the board dependant on the player
            return player1 == true ? 1 : -1;
        }
        [NonAction]
        public ChessGameModel ResetPotentialMovesAndGetGame()
        {
            ChessGameModel model = ResetPotentialMovesFromSession();
            SetGame(model);

            return model;
        }
        [NonAction]
        public ChessGameModel ResetPawnSpecialMovesFromGame(ChessGameModel model)
        {
            foreach (var row in model.ChessBoard.Board)
            {
                foreach (var col in row)
                {
                    if(col.Piece != null)
                        col.Piece.PawnSpecialMove = false;
                }
            }
            return model;
        }
        [NonAction]
        public ChessGameModel PawnSpecialMoveExists(ChessGameModel model)
        {
            foreach (var row in model.ChessBoard.Board)
                foreach (var col in row)
                    if(col.Piece != null)
                        col.Piece.PawnSpecialMove = false;  
            return model;            
        }
        #endregion

        #region Piece Moves
        //PAWN
        [NonAction]
        public List<Tuple<int, int>> GetPawnMoves(ChessGameModel model, ChessColumn col, ChessPiece piece)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>() { };
            //FirstMove
            try
            {
                Tuple<int, int> firstMove = new Tuple<int, int>(
                        (col.X + (1 * PlayerMoveDirection(piece.Player1))),
                        (col.Y)
                    );
                if (model.ChessBoard.Board[firstMove.Item1][firstMove.Item2].Piece == null)
                    moves.Add(firstMove);
            }
            catch (Exception ex) { }
            //SecondMove - Can only do when firstmove is available
            if (moves.Count != 0)
            {
                try
                {
                    Tuple<int, int> secondMove = new Tuple<int, int>(
                            (col.X + (2 * PlayerMoveDirection(piece.Player1))),
                            (col.Y)
                        );
                    if (piece.HasMoved == false && model.ChessBoard.Board[secondMove.Item1][secondMove.Item2].Piece == null)
                        moves.Add(secondMove);
                }
                catch (Exception ex) { }
            }
            //DiagonalRightMove
            try
            {
                Tuple<int, int> diagonalRightMove = new Tuple<int, int>(
                        (col.X + (1 * PlayerMoveDirection(piece.Player1))),
                        (col.Y + 1)
                    );
                if ((model.ChessBoard.Board[diagonalRightMove.Item1][diagonalRightMove.Item2].Piece != null)
                    &&
                    (model.ChessBoard.Board[diagonalRightMove.Item1][diagonalRightMove.Item2].Piece.Player1 != piece.Player1))
                    moves.Add(diagonalRightMove);
            }
            catch (Exception ex) { }
            //DiagonalLeftMove
            try
            {
                Tuple<int, int> diagonalLeftMove = new Tuple<int, int>(
                        (col.X + (1 * PlayerMoveDirection(piece.Player1))),
                        (col.Y - 1)
                    );
                if ((model.ChessBoard.Board[diagonalLeftMove.Item1][diagonalLeftMove.Item2].Piece != null)
                    &&
                    (model.ChessBoard.Board[diagonalLeftMove.Item1][diagonalLeftMove.Item2].Piece.Player1 != piece.Player1))
                    moves.Add(diagonalLeftMove);
            }
            catch (Exception ex) { }
            //Special pawn left move
            try
            {
                Tuple<int, int> specialLeftMove = new Tuple<int, int>(
                        (col.X),
                        (col.Y - 1)
                    );
                if (model.ChessBoard.Board[specialLeftMove.Item1][specialLeftMove.Item2].Piece != null
                    &&
                    model.ChessBoard.Board[specialLeftMove.Item1][specialLeftMove.Item2].Piece.Type == ChessPieceType.Pawn
                    &&
                    model.ChessBoard.Board[specialLeftMove.Item1][specialLeftMove.Item2].Piece.PawnSpecialMove == true
                    )
                {
                    moves.Add(specialLeftMove);
                }
            }
            catch (Exception) { }
            //Special pawn right move
            try
            {
                Tuple<int, int> specialRightMove = new Tuple<int, int>(
                        (col.X),
                        (col.Y + 1)
                    );
                if (model.ChessBoard.Board[specialRightMove.Item1][specialRightMove.Item2].Piece != null
                    &&
                    model.ChessBoard.Board[specialRightMove.Item1][specialRightMove.Item2].Piece.Type == ChessPieceType.Pawn
                    &&
                    model.ChessBoard.Board[specialRightMove.Item1][specialRightMove.Item2].Piece.PawnSpecialMove == true
                    )
                {
                    moves.Add(specialRightMove);
                }
            }
            catch (Exception) { }

            return moves;
        }
        //[NonAction]
        //public List<Tuple<int, int>> GetKnightMoves(ChessGameModel model, ChessColumn col, ChessPiece piece)
        //{
        //    List<Tuple<int, int>> moves = new List<Tuple<int, int>>() { };
        //    List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>() { };

        //    //Add Moves
        //    moves.Add(new Tuple<int, int>((col.X + 2) -1, (col.Y + 1) - 1));
        //    moves.Add(new Tuple<int, int>((col.X + 2) -1, (col.Y + -1) - 1));

        //    moves.Add(new Tuple<int, int>((col.X + -2) -1, (col.Y + 1) - 1));
        //    moves.Add(new Tuple<int, int>((col.X + -2) -1, (col.Y + -1) - 1));

        //    moves.Add(new Tuple<int, int>((col.X + 1) -1, (col.Y + 2) - 1));
        //    moves.Add(new Tuple<int, int>((col.X + -1) -1, (col.Y + 2) - 1));

        //    moves.Add(new Tuple<int, int>((col.X + 1) -1, (col.Y + -2) - 1));
        //    moves.Add(new Tuple<int, int>((col.X + -1) -1, (col.Y + -2) - 1));

        //    //Remove moves if col is empty or col contains same team piece
        //    //else move can stay in list
        //    foreach (var move in moves)
        //    {
        //        try
        //        {
        //            var moveCol = model.ChessBoard.Rows[move.Item1 - 1].Columns[move.Item2 - 1];
        //            if (moveCol.Piece == null || moveCol.Piece.Player1 == piece.Player1)
        //            {
        //                //Do Nothing
        //            }
        //            else
        //            {
        //                availableMoves.Add(move);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }                
        //    }
        //    return availableMoves;
        //}
        #endregion

        #region Main Game Functions
        [HttpPost]
        public ActionResult GetMoves(PieceToPotentialMove MoveModel)
        {
            try
            {
                ChessGameModel model = ResetPotentialMovesAndGetGame();
                model.PieceToPotentialMove = MoveModel;

                var col = model.ChessBoard.Board[MoveModel.x][MoveModel.y];
                var piece = col.Piece;
                List<Tuple<int, int>> moves = new List<Tuple<int, int>>() { };

                if (MoveModel.pieceType == ChessPieceType.Pawn)
                {
                    moves = GetPawnMoves(model, col, piece);
                }
                //else if (MoveModel.pieceType == ChessPieceType.Knight)
                //{
                //    moves = GetKnightMoves(model, col, piece);
                //}

                //Highlight the moves
                foreach (var item in moves)
                {
                    model.ChessBoard.Board[item.Item1][item.Item2].HighlightMove = true;
                }

                //Build board up
                string html = _viewRenderService.RenderToStringAsync("_Board", model).Result;
                //Set the new model in the session with the highlighted moves
                SetGame(model);
                return Json(new { success = true, html, potentialMove = Newtonsoft.Json.JsonConvert.SerializeObject(MoveModel) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public ActionResult RemovePotentialMoves()
        {
            try
            {
                ChessGameModel model = ResetPotentialMovesAndGetGame();
                string html = _viewRenderService.RenderToStringAsync("_Board", model).Result;
                return Json(new { success = true, html });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult MovePiece(MovePieceModel moveModel)
        {
            try
            {
                ChessGameModel model = GetGame();                

                //find the pieces 
                ChessPiece? pieceToMove = model.ChessBoard.Board[model.PieceToPotentialMove.x][model.PieceToPotentialMove.y].Piece;

                //if pawn and second move and piece next to pawn will be a pawn set pawnspecialmove to true.
                if (model.PieceToPotentialMove.pieceType == ChessPieceType.Pawn)
                {
                    if (pieceToMove.HasMoved == false && Math.Abs(model.PieceToPotentialMove.x - moveModel.x) == 2)
                    {
                        try
                        {
                            if(model.ChessBoard.Board[moveModel.x][moveModel.y + 1].Piece?.Type == ChessPieceType.Pawn)
                                pieceToMove.PawnSpecialMove = true;
                        }
                        catch (Exception ex) {}

                        try
                        {
                            if(model.ChessBoard.Board[moveModel.x][moveModel.y - 1].Piece?.Type == ChessPieceType.Pawn)
                                pieceToMove.PawnSpecialMove = true;
                        }
                        catch (Exception ex) { }
                    }
                }

                pieceToMove.HasMoved = true;
                model.ChessBoard.Board[model.PieceToPotentialMove.x][model.PieceToPotentialMove.y].Piece = null;

                ChessPiece pieceToReplace = null;
                //find pieceToReplace || Or Empty column and insert pieceToMove               
                if (model.ChessBoard.Board[moveModel.x][moveModel.y].Piece != null)
                {
                    pieceToReplace = model.ChessBoard.Board[moveModel.x][moveModel.y].Piece;
                    if (pieceToReplace.Player1 == true)
                        model.ChessBoard.Player1DeadPieces.Add(pieceToReplace);
                    else if (pieceToReplace.Player1 == false)
                        model.ChessBoard.Player2DeadPieces.Add(pieceToReplace);
                    
                    model.ChessBoard.Board[moveModel.x][moveModel.y].Piece = null;
                }

                //logic for en passant pawn taking the piece but the square above
                if (pieceToReplace != null && pieceToReplace.PawnSpecialMove == true)
                {
                    model.ChessBoard.Board[moveModel.x + (1 * PlayerMoveDirection(pieceToMove.Player1))][moveModel.y].Piece = pieceToMove;
                    //model = ResetPawnSpecialMovesFromGame(model);
                }
                else
                {
                    if (moveModel.promotionPiece != null)
                    {
                        ChessPiece promoPiece =
                            moveModel.promotionPiece == ChessPieceType.Knight ? new KnightPiece(pieceToMove.Player1) :
                            moveModel.promotionPiece == ChessPieceType.Bishop ? new BishopPiece(pieceToMove.Player1) :
                            moveModel.promotionPiece == ChessPieceType.Rook ? new RookPiece(pieceToMove.Player1) :
                            new QueenPiece(pieceToMove.Player1);

                        model.ChessBoard.Board[moveModel.x][moveModel.y].Piece = promoPiece;
                    }
                    else
                    {
                        model.ChessBoard.Board[moveModel.x][moveModel.y].Piece = pieceToMove;
                    }
                }

                if (pieceToMove.PawnSpecialMove != true && pieceToReplace?.PawnSpecialMove != true)                
                    model = PawnSpecialMoveExists(model);
                
                //reset game potentialmove
                model.PieceToPotentialMove = null;
                ChessGameModel newModel = ResetPotentialMovesFromGame(model);
                SetGame(newModel);

                string html = _viewRenderService.RenderToStringAsync("_Board", model).Result;
                return Json(new { success = true, html });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        #endregion
    }
}