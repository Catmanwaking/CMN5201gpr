//Author: Dominik Dohmeier
using System;
using System.Collections;

namespace Fast_0h_h1
{
    internal class Grid
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
            SideLength = Size * Rules.COLOR_AMOUNT;
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
    }
}