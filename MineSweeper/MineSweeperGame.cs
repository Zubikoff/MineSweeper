using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class MineSweeperGame
    {
        /// <summary>
        /// Internal mine field. It only contains mines locations.
        /// </summary>
        protected bool[,] MineField;

        public int FieldWidth { get; private set; }

        public int FieldHeight { get; private set; }

        public int Mines { get; private set; }

        public MineSweeperGame(int fieldW, int fieldH, int mines)
        {
            MineField = new bool[fieldW, fieldH];
            Mines = mines;
        }

        public static MineSweeperGame StartNew(int fieldW, int fieldH, int mines)
        {
            return new MineSweeperGame(fieldW, fieldH, mines);
        }

        /// <summary>
        /// Randomly spitting mines across the field. This should happen right after user's first move.
        /// User's move coordinates excluded from available mines loaction.
        /// </summary>
        /// <param name="clickedRow"></param>
        /// <param name="clickedCol"></param>
        protected internal void SetMines(int clickedRow, int clickedCol)
        {
            Random a = new Random();

            MineField[clickedRow, clickedCol] = true;

            int Row, Col;

            int mines = Mines;
            while (mines > 0) // potentially infinite, but simple :3
            {
                Row = a.Next(FieldHeight);
                Col = a.Next(FieldWidth);
                if (!MineField[Row, Col])
                {
                    MineField[Row, Col] = true;
                    mines--;
                }
            }

            MineField[clickedRow, clickedCol] = false;
        }
    }
}
