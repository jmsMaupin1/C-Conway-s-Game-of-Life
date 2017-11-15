using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conways_Game_Of_Life
{
    class CGOLBoard
    {
        int hCellCount, vCellCount;
        bool[] displayBoardState, mutableBoardState;
        bool[] lookupTable;

        public CGOLBoard(int hCellCount, int vCellCount) : this(hCellCount, vCellCount, new bool[hCellCount * vCellCount]) { }

        public CGOLBoard(int hCellCount, int vCellCount, bool[] seed)
        {
            this.hCellCount = hCellCount;
            this.vCellCount = vCellCount;

            displayBoardState = seed;
            mutableBoardState = seed;


            lookupTable = GenerateLookupTable();

            Console.WriteLine(lookupTable[24]);  // False
            Console.WriteLine(lookupTable[25]);  // True
            Console.WriteLine(lookupTable[41]);  // True
            Console.WriteLine(lookupTable[57]);  // True
            Console.WriteLine(lookupTable[59]);  // False 
            Console.WriteLine(lookupTable[137]); // True
            Console.WriteLine(lookupTable[56]);  // True

            mutableBoardState = NextGeneration();
        }

        /*
         * Courtesy Of: 
         * 
         * https://en.wikipedia.org/wiki/Hamming_weight
         * http://aggregate.ee.engr.uky.edu/MAGIC/#Population%20Count%20(Ones%20Count)
         */

        private int GetBitCount(int n)
        {
            n = n - ((n >> 1) & (int)0x55555555);
            n = (n & (int)0x33333333) + ((n >> 2) & (int)0x33333333);
            return (((n + (n >> 4)) & (int)0x0F0F0F0F) * (int)0x01010101) >> 24;
        }

        /*
         * For each individual cell its status in the next generation only relies on 
         * the cell's status and that of its 8 neighbors.
         * 
         * Each cell has a total of two states, on or off. This fact allows us to store the state of
         * each individual cells state with 1 bit. 
         * 
         * Since a cell's state in the next generation depends on its neighbors, plus its own state
         * you can essentially precompute every possible state by generating every possible 9 bit number
         * and applying conway's rules to the middle cell.
         * 
         * so, we can create a 2^9(512) boolean array, use the index of a cell and its neighbors to translate
         * the configuration into the cell's next state.
         * 
         * The bit representation can be thought of like this (bottom right to top left, left to right, bottom to top)
         * ---------------------------------------------------------------------------------------------------------
         * 
         * number = binary rep = alive next gen?
         * 
         * Representation of
         * the bit board
         * representation
         * 
         * ---------------------------------------------------------------------------------------------------------
         * 
         * 24 = 0b11000 = False
         * 
         * 0 0 0
         * 0 1 1
         * 0 0 0
         * 
         * 25 = 0b11001 = True
         * 
         * 0 0 0
         * 0 1 1
         * 0 0 1
         * 
         * 
         * 41 = 0b101001 = True
         * 
         * 0 0 0
         * 1 0 1
         * 0 0 1
         * 
         * 57 = 0b111001 = true
         * 
         * 0 0 0
         * 1 1 1
         * 0 0 1
         * 
         * 59 = 0b111011 = false
         * 
         * 0 0 0
         * 1 1 1
         * 0 1 1
         * 
         * 137  = 0b10001001 = true
         * 
         * 1 0 0
         * 0 1 0
         * 0 0 1
         * 
         * 56 = 0b111000 = true
         * 
         * 0 0 0
         * 1 1 1
         * 0 0 0
         * 
         */

        private bool[] GenerateLookupTable()
        {
            bool[] lookup = new bool[512];

            for(int i = 0; i < 512; ++i)
            {
                int curCell = (1 << 4) & i;
                int neighborCount = GetBitCount(i & ~(1 << 4));

                if((curCell == 16 && ((neighborCount >= 2) && (neighborCount <= 3)))
                    || (curCell == 0 && neighborCount == 3))
                {
                    lookup[i] = true;
                }
                else
                {
                    lookup[i] = false;
                }
            }
            return lookup;
        }

        private bool GetCellState(int x, int y)
        {
            if (x >= 0 && x < hCellCount && y >= 0 && y < vCellCount)
                return mutableBoardState[y * hCellCount + x];

            return false;
        }
        

        /*
         * PUBLIC API:
         * TODO: Document
         */

        public void UpdateDisplay()
        {
            displayBoardState = mutableBoardState;
            mutableBoardState = NextGeneration();
        }

        /*
         * 256 128 64    ^
         * 32  16  8     |
         * 4   2   1     y
         * 
         * <   -   x
         */

        /// <summary>
        ///     Generate The next grid state using a lookup table
        /// </summary>
        /// <returns>
        ///     A bool array representing the next state
        /// </returns>
        public bool[] NextGeneration()
        { 
            int cellCount = hCellCount * vCellCount;
            bool[] nextState = new bool[cellCount];

            for(int i = 0; i < cellCount; ++i)
            {
                    int x = i % hCellCount;
                    int y = i / hCellCount;

                    int indexKey =
                        (GetCellState(x + 1, y + 1) ? 1 << 8 : 0) | (GetCellState(x, y + 1) ? 1 << 7 : 0) | (GetCellState(x - 1, y + 1) ? 1 << 6 : 0) |
                        (GetCellState(x + 1, y) ? 1 << 5 : 0)     | (GetCellState(x, y) ? 1 << 4 : 0)     | (GetCellState(x - 1, y) ? 1 << 3 : 0)     |
                        (GetCellState(x + 1, y - 1) ? 1 << 2 : 0) | (GetCellState(x, y - 1) ? 1 << 1 : 0) | (GetCellState(x - 1, y - 1) ? 1 : 0);


                    nextState[i] = lookupTable[indexKey];
            }

            return nextState;
            
        }

        /// <summary>
        /// Returns the display state of cell i
        /// </summary>
        /// <param name="i">The bit index of cell i's display state</param>
        /// <returns>The display state of cell i</returns>
        public bool GetDispCellState(int i)
        {
            return displayBoardState[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Total number of cells in the grid</returns>
        public int GetCellCount()
        {
            return hCellCount * vCellCount;
        }

    }
}

