using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conways_Game_Of_Life
{
    class CGOLBoard
    {
        int hCellCount, vCellCount;

        public CGOLBoard(int hCellCount, int vCellCount)
        {
            this.hCellCount = hCellCount;
            this.vCellCount = vCellCount;
        }

        private int getBitCount(int n)
        {
            return n;
        }

        private Dictionary<int, Boolean> generateLookUpTable()
        {
            Dictionary<int, Boolean> lookupTable = new Dictionary<int, bool>();

            return lookupTable;
        }
    }
}
