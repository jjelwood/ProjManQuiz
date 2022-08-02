﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class Knight : BindableBase, IPiece
    {
        public Knight(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public int Moves { get; set; } = 0;
        public bool IsWhite { get; }

        public char Character => IsWhite ? 'N' : 'n';

        public Tile Position { get; set; }

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            // Iterates through -1 and 1
            for (int i = -1; i < 2; i += 2)
            {
                // Iterates through -2 and 2
                for (int j = -2; j < 3; j += 4)
                {
                    tiles.Add(Tile.Move(Position, i, j));
                    tiles.Add(Tile.Move(Position, j, i));
                }
            }
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board), this, board);
        }
    }
}
