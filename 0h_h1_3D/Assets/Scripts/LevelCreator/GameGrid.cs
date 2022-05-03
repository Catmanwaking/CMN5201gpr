//Author: Dominik Dohmeier
using System;
using System.Collections;

namespace Optimized_0h_h1_3D
{
    public class GameGrid : IEnumerable
    {        
        protected int[,,] internalGrid;

        public int SideLength { get; }
        public int Size { get; }
        public int Length { get => internalGrid.Length; }

        public V3Int LastEditPos { get; }

        public virtual int this[int posX, int posY, int posZ]
        {
            get => internalGrid[posX, posY, posZ];
            set
            {
                internalGrid[posX, posY, posZ] = value;
                LastEditPos.SetValues(posX, posY, posZ);
            }
        }

        public virtual int this[V3Int pos]
        {
            get => internalGrid[pos.X, pos.Y, pos.Z];
            set
            {
                internalGrid[pos.X, pos.Y, pos.Z] = value;
                LastEditPos.SetValues(pos.X, pos.Y, pos.Z);
            }
        }

        public GameGrid(int size, int colorCount)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Rules.SetupRules(size, colorCount);

            Size = size;
            SideLength = Size * Rules.ColorCount;
            internalGrid = new int[SideLength, SideLength, SideLength];

            LastEditPos = new V3Int();            
        }

        public int[,,] ExportGrid()
        {
            int[,,] copy = new int[SideLength, SideLength, SideLength];
            for (int x = 0; x < SideLength; x++)
            {
                for (int y = 0; y < SideLength; y++)
                {
                    for (int z = 0; z < SideLength; z++)
                    {
                        copy[x, y, z] = internalGrid[x, y, z];
                    }
                }
            }
            return copy;
        }

        public IEnumerator GetEnumerator()
        {
            return internalGrid.GetEnumerator();
        }
    }
}