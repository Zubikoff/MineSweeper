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

        public int[,] Field { get; private set; }

        private bool MinesSet;

        public int FieldWidth { get; private set; }

        public int FieldHeight { get; private set; }

        public int Mines { get; private set; }

        public MineSweeperGame(int fieldW, int fieldH, int mines)
        {
            MineField = new bool[fieldW, fieldH];
            Mines = mines;
            MinesSet = false;
        }

        public static MineSweeperGame StartNew(int fieldW, int fieldH, int mines)
        {
            return new MineSweeperGame(fieldW, fieldH, mines);
        }

        /// <summary>
        /// Randomly spitting mines across the field. This should happen right after user's first move.
        /// User's move coordinates excluded from available mines location.
        /// </summary>
        /// <param name="clickedRow"></param>
        /// <param name="clickedCol"></param>
        protected internal void SetMines(int clickedRow, int clickedCol)
        {
            MineField[clickedRow, clickedCol] = true; // temporarily set user's move as "mine"

            SetMines();

            MineField[clickedRow, clickedCol] = false; // clear user's move from a mine
        }

        /// <summary>
        /// Randomly spitting mines across the field.
        /// </summary>
        protected internal void SetMines()
        {
            if (MinesSet) return;
            Random a = new Random();

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
            MinesSet = true;
        }
    }
}
