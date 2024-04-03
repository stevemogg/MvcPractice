using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Dashboards.WebApi;//
using Microsoft.TeamFoundation.Work.WebApi;
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
        #region Dependancy Injection && Game Setup/Get
        private readonly IViewRenderService _viewRenderService;
        public ChessGameModel _game { get; set; }
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
                model.ChessBoard = new ChessBoard();
                List<ChessPiece?> line1 = new List<ChessPiece?>() { new RookPiece(true), new KnightPiece(true), new BishopPiece(true), new QueenPiece(true), new KingPiece(true), new BishopPiece(true), new KnightPiece(true), new RookPiece(true) };
                List<ChessPiece?> line2 = new List<ChessPiece?>() { new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true), new PawnPiece(true) };

                List<ChessPiece?> line7 = new List<ChessPiece?>() { new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false), new PawnPiece(false) };
                List<ChessPiece?> line8 = new List<ChessPiece?>() { new RookPiece(false), new KnightPiece(false), new BishopPiece(false), new QueenPiece(false), new KingPiece(false), new BishopPiece(false), new KnightPiece(false), new RookPiece(false) };

                model.ChessBoard.Rows = new List<ChessRow>() { };

                for (int x = 0; x < 8; x++)
                {
                    ChessRow row = new ChessRow() { };
                    row.Columns = new List<ChessColumn>() { };
                    row.X = x + 1;

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
                    model.ChessBoard.Rows.Add(row);
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
        public ChessGameModel ResetPotentialMoves(ChessGameModel model)
        {
            foreach (var row in model.ChessBoard.Rows)
            {
                foreach (var col in row.Columns)
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
            ChessGameModel model = GetGame();
            
            SetGame(model);
            return model;
        }
        #endregion

        #region Main Game Functions
        [HttpPost]
        public ActionResult GetMoves(PieceToPotentialMove MoveModel)
        {
            try
            {
                ChessGameModel model = ResetPotentialMovesAndGetGame();
                model.PieceToPotentialMove = MoveModel;
            
                var col = model.ChessBoard.Rows[MoveModel.x - 1].Columns[MoveModel.y - 1];
                var piece = col.Piece;
                List<Tuple<int, int>> moves = new List<Tuple<int, int>>() { };

                // #########
                // - PAWN
                //##########
                if (MoveModel.pieceType == ChessPieceType.Pawn)
                {
                    //FirstMove
                    try
                    {
                        Tuple<int, int> firstMove = new Tuple<int, int>(
                                ((col.X - 1) + (1 * PlayerMoveDirection(piece.Player1))),
                                (col.Y - 1)
                            );
                        if (model.ChessBoard.Rows[firstMove.Item1].Columns[firstMove.Item2].Piece == null)
                            moves.Add(firstMove);
                    }
                    catch (Exception ex) { }
                    //SecondMove
                    try
                    {
                        Tuple<int, int> secondMove = new Tuple<int, int>(
                                ((col.X - 1) + (2 * PlayerMoveDirection(piece.Player1))),
                                (col.Y - 1)
                            );
                        if (piece.HasMoved == false && model.ChessBoard.Rows[secondMove.Item1].Columns[secondMove.Item2].Piece == null)
                            moves.Add(secondMove);
                    }
                    catch (Exception ex) { }
                    //DiagonalRightMove
                    try
                    {
                        Tuple<int, int> diagonalRightMove = new Tuple<int, int>(
                                ((col.X - 1) + (1 * PlayerMoveDirection(piece.Player1))),
                                ((col.Y - 1) + 1)
                            );
                        if ((model.ChessBoard.Rows[diagonalRightMove.Item1].Columns[diagonalRightMove.Item2].Piece != null)
                            &&
                            (model.ChessBoard.Rows[diagonalRightMove.Item1].Columns[diagonalRightMove.Item2].Piece.Player1 != piece.Player1))
                            moves.Add(diagonalRightMove);
                    }
                    catch (Exception ex) { }
                    //DiagonalLeftMove
                    try
                    {
                        Tuple<int, int> diagonalLeftMove = new Tuple<int, int>(
                                ((col.X - 1) + (1 * PlayerMoveDirection(piece.Player1))),
                                ((col.Y - 1) - 1)
                            );
                        if ((model.ChessBoard.Rows[diagonalLeftMove.Item1].Columns[diagonalLeftMove.Item2].Piece != null)
                            &&
                            (model.ChessBoard.Rows[diagonalLeftMove.Item1].Columns[diagonalLeftMove.Item2].Piece.Player1 != piece.Player1))
                            moves.Add(diagonalLeftMove);
                    }
                    catch (Exception ex) { }
                }
                // ##########
                // - ROOK
                //###########
                else if (MoveModel.pieceType == ChessPieceType.Rook)
                {

                }

                //Highlight the moves
                foreach (var item in moves)
                {
                    model.ChessBoard.Rows[item.Item1].Columns[item.Item2].HighlightMove = true;
                }

                //Build board up
                string html = _viewRenderService.RenderToStringAsync("_Board", model).Result;
                //Set the new model in the session with the highlighted moves
                SetGame(model);
                return Json(new { success = true, html });
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
                return Json( new { success = true, html});
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

                //find and remove
                var piece = model.ChessBoard.Rows[model.PieceToPotentialMove.x-1].Columns[model.PieceToPotentialMove.y-1].Piece;
                model.ChessBoard.Rows[model.PieceToPotentialMove.x-1].Columns[model.PieceToPotentialMove.y-1].Piece = null;

                //find and insert
                model.ChessBoard.Rows[moveModel.x-1].Columns[moveModel.y-1].Piece = piece;

                //reset game potentialmove
                model.PieceToPotentialMove = null;

                string html = _viewRenderService.RenderToStringAsync("_Board", model).Result;
                return Json(new { success = true, html});
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        #endregion
    }
}