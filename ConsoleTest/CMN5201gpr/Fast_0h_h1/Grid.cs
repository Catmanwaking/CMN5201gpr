//Author: Dominik Dohmeier
using System;
using System.Collections;

namespace Fast_0h_h1
{
    internal class Grid : IEnumerable
    {
        private readonly int[,,] internalGrid;

        public int SideLength { get; }
        public int Size { get; }
        public int Length { get => internalGrid.Length; }

        public V3Int LastEditPos { get; }

        public int this[int posX, int posY, int posZ]
        {
            get => internalGrid[posX, posY, posZ];
            set
            {
                internalGrid[posX, posY, posZ] = value;
                LastEditPos.SetValues(posX, posY, posZ);
            }
        }

        public int this[V3Int pos]
        {
            get => internalGrid[pos.X, pos.Y, pos.Z];
            set
            {
                internalGrid[pos.X, pos.Y, pos.Z] = value;
                LastEditPos.SetValues(pos.X, pos.Y, pos.Z);
            }
        }

        public Grid(int size)
        {
            if(size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
            SideLength = Size * Rules.ColorAmount;
            internalGrid = new int[SideLength, SideLength, SideLength];

            LastEditPos = new V3Int();
        }

        public Grid(Grid grid)
        {
            internalGrid = grid.ExportGrid();
            SideLength = grid.SideLength;
            Size = grid.Size;
            LastEditPos = new V3Int();
        }

        public Grid(int[,,] grid)
        {
            internalGrid = grid;
            SideLength = grid.GetLength(0);
            Size = SideLength >> 1;
            LastEditPos = new V3Int();
            Rules.RebuildCache(this);
        }

        public bool IsFull()
        {
            foreach (var item in internalGrid)
            {
                if(item == 0)
                    return false;
            }
            return true;
        }

        public int[,,] ExportGrid()
        {
            int[,,] copy = new int[SideLength, SideLength, SideLength];
            Array.Copy(internalGrid, copy, internalGrid.Length);
            return copy;
        }

        public IEnumerator GetEnumerator()
        {
            return internalGrid.GetEnumerator();
        }
    }
}