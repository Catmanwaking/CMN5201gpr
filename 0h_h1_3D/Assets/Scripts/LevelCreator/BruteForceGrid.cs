//Author: Dominik Dohmeier
namespace Optimized_0h_h1_3D
{
    public class BruteForceGrid : GameGrid
    {            
        private readonly ulong[][,] linkedCache;   

        public override int this[int posX, int posY, int posZ]
        {
            get => internalGrid[posX, posY, posZ];
            set
            {
                internalGrid[posX, posY, posZ] = value;
                LastEditPos.SetValues(posX, posY, posZ);
                ManageCache();
            }
        }

        public override int this[V3Int pos]
        {
            get => internalGrid[pos.X, pos.Y, pos.Z];
            set
            {
                internalGrid[pos.X, pos.Y, pos.Z] = value;
                LastEditPos.SetValues(pos.X, pos.Y, pos.Z);
                ManageCache();
            }
        }

        public BruteForceGrid(int size, int colorCount) : base(size, colorCount)
        {
            linkedCache = Rules.GetCache();
        }

        private void ManageCache()
        {
            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                linkedCache[d]
                [
                    LastEditPos[(d + 1) % Rules.DIMENSIONS],
                    LastEditPos[(d + 2) % Rules.DIMENSIONS]
                ] = 0;
            }
        }
    }
}