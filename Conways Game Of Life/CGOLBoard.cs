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

            bool[] table = generateLookupTable();

            Console.WriteLine(table[24]); // False
            Console.WriteLine(table[25]); // True
            Console.WriteLine(table[41]); // True
            Console.WriteLine(table[57]); // True
            Console.WriteLine(table[59]); // False
        }

        /*
         * Courtesy Of: 
         * 
         * https://en.wikipedia.org/wiki/Hamming_weight
         * http://aggregate.ee.engr.uky.edu/MAGIC/#Population%20Count%20(Ones%20Count)
         */

        private int getBitCount(int n)
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
         */

        private bool[] generateLookupTable()
        {
            bool[] lookup = new bool[512];

            for(int i = 0; i < 512; ++i)
            {
                int curCell = (1 << 4) & i;
                int neighborCount = getBitCount(i & ~(1 << 4));

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
    }
}

