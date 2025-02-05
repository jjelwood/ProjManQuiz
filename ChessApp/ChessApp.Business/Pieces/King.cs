﻿using ChessApp.Business.Moves;
using ChessApp.Core;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    /// <summary>
    /// King piece class
    /// </summary>
    public class King : BindableBase, IPiece
    {
        public King(PieceColour colour, Tile position)
        {
            Colour = colour;
            Position = position;
        }

        public int Moves { get; set; }

        public PieceColour Colour { get; }

        public Tile Position { get; set; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Config.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}K.svg");

        public char Character => Colour == PieceColour.White ? 'K' : 'k';

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        tiles.Add(Tile.Move(Position, i, j));
                    }
                }
            }
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<StandardMove> GetStandardMoves(ChessBoard board)
        {
            var tiles = MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board),
                this, board);
            return MovementMethods.ConvertToStandardMoves(this, tiles, board);
        }

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            List<IMove> moves = ((IEnumerable<IMove>)GetStandardMoves(board)).ToList();
            for (int i = -1; i <= 1; i += 2)
            {
                if (board.CanCastle(Colour, i))
                {
                    moves.Add(new CastlingMove(Colour, i, board));
                }
            }
            return moves;
        }
    }
}
