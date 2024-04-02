using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Dashboards.WebApi;//
using Microsoft.TeamFoundation.Work.WebApi;
using MvcPractice.Areas.ChessGame.Models;
using MvcPractice.Helpers;
using Newtonsoft.Json;
using Ninject.Activation.Caching;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;//

namespace MvcPractice.Areas.ChessGame.Controllers
{
    [Area("ChessGame")]
    public class ChessGameController : Controller
    {
        #region Dependancy injection && game setup/get
        private readonly IViewRenderService _viewRenderService;
        public ChessGameModel _game { get; set; }
        public ChessGameController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }
        #endregion

        public void SetUpGameModel()
        {            
        }      
        
        public IActionResult Index()
        {
            ChessGameModel model = new ChessGameModel();
            model.GameJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            return View(model);
        }

        public ActionResult GetMoves(ChessGameModel model)
        {
            try
            {
                ////Get the column
                //var col = _game.ChessBoard.Board[model.x].Columns[model.y];

                //List<Tuple<int, int>> pawnMoves = new List<Tuple<int, int>>() { };
                //pawnMoves.Add(new Tuple<int, int>(0, 1));
                //pawnMoves.Add(new Tuple<int, int>(0, 2));

                //foreach (var move in pawnMoves)
                //{
                //    _game.ChessBoard.Board[model.x + move.Item1].Columns[model.y + move.Item2].HighlightMove = true;
                //}

                //string html = _viewRenderService.RenderToStringAsync("_Board", _game).Result;

                var col = model.ChessBoard.Board[model.MovesModel.x].Columns[model.MovesModel.y];
                var piece = col.Piece;
                List<Tuple<int, int>> moves = new List<Tuple<int, int>>() { };
                                
                if (model.MovesModel.pieceType == ChessPieceType.Pawn)
                {
                    //piece moved?
                    //player1?
                    //forwards move - factor in double forwards
                    //diagonal take moves
                    //First forwards move

                    //FirstMove
                    Tuple<int, int> firstMove = new Tuple<int, int>(
                            ((col.X + 1) * (piece.Player1 == true ? 1 : -1)),
                            (col.Y)
                        );
                    if (model.ChessBoard.Board[firstMove.Item1].Columns[firstMove.Item2].Piece == null)                                          
                        moves.Add(firstMove);
                    //SecondMove
                    Tuple<int, int> secondMove = new Tuple<int, int>(
                            ((col.X + 2) * (piece.Player1 == true ? 1 : -1)),
                            (col.Y)
                        );
                    if (piece.HasMoved == false && model.ChessBoard.Board[model.MovesModel.x].Columns[model.MovesModel.y].Piece == null)                                                                   
                        moves.Add(secondMove);
                    //DiagonalMoves
                    Tuple<int, int> diagonalRightMove = new Tuple<int, int>(
                            ((col.X + 1) * (piece.Player1 == true ? 1 : -1)),
                            (col.Y + 1)
                        );
                    if (piece.HasMoved == false && model.ChessBoard.Board[diagonalRightMove.Item1].Columns[diagonalRightMove.Item2].Piece == null)
                        moves.Add(diagonalRightMove);
                    
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}