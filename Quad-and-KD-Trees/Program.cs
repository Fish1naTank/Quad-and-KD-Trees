using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quad_and_KD_Trees
{
    class Program
    {
        public static void Main(string[] args)
        {
            //uncomment for game
            /**/
            TreesGame treesGame = new TreesGame();
            treesGame.Run();
            /**/

            //uncomment for tests
            /**
            TreesTester treeTester = new TreesTester();
            treeTester.Run();
            /**/
        }
    }
}
