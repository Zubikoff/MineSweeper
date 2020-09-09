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

        public int?[,] Field { get; private set; }

        private bool MinesSet;

        public int FieldWidth { get; private set; }

        public int FieldHeight { get; private set; }

        public int Mines { get; private set; }

        public MineSweeperGame(int fieldW, int fieldH, int mines)
        {
            MineField = new bool[fieldW, fieldH];
            Field = new int?[fieldW, fieldH];
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
        protected internal void SetMines(int clickedCol, int clickedRow)
        {
            MineField[clickedCol, clickedRow] = true; // temporarily set user's move as "mine"

            SetMines();

            MineField[clickedCol, clickedRow] = false; // clear user's move from a mine
        }

        /// <summary>
        /// Randomly spitting mines across the field. This method ignores user's move.
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

        /// <summary>
        /// Making user's move.
        /// </summary>
        /// <returns>Returns "true" if demining successful. Returns "false" if you picked a mine</returns>
        public bool DemineCell(int clickedCol, int clickedRow)
        {
            if (clickedRow < 0 || clickedCol < 0 || clickedRow >= FieldHeight || clickedCol >= FieldWidth)
                throw new IndexOutOfRangeException("Переданные координаты находятся за пределами игрового поля.");

            if (MineField[clickedCol, clickedRow])
                return false;

            FieldOpen(clickedCol, clickedRow);

            return true;
        }

        /// <summary>
        /// Open the selected cell
        /// </summary>
        protected void FieldOpen(int clickedCol, int clickedRow)
        {
            // if indexes are out of range we simply return
            if (clickedRow < 0 || clickedCol < 0 || clickedRow >= FieldHeight || clickedCol >= FieldWidth)
                return;

            // if there is a mine in a cell or this cell is already processed we return
            if (MineField[clickedCol, clickedRow] || Field[clickedCol, clickedRow] != null) 
                return;
            else
            {
                int number = 0;

                // counting the surrounding mines
                if (IfTheMine(clickedCol - 1, clickedRow - 1)) number++;
                if (IfTheMine(clickedCol    , clickedRow - 1)) number++;
                if (IfTheMine(clickedCol + 1, clickedRow - 1)) number++;

                if (IfTheMine(clickedCol - 1, clickedRow    )) number++;
                if (IfTheMine(clickedCol + 1, clickedRow    )) number++;

                if (IfTheMine(clickedCol - 1, clickedRow + 1)) number++;
                if (IfTheMine(clickedCol    , clickedRow + 1)) number++;
                if (IfTheMine(clickedCol + 1, clickedRow + 1)) number++;

                if (number == 0)
                {
                    // if there is no mines in surrounding cells we open all cells around us (recursive);
                    FieldOpen(clickedCol - 1, clickedRow - 1);
                    FieldOpen(clickedCol, clickedRow - 1);
                    FieldOpen(clickedCol + 1, clickedRow - 1);

                    FieldOpen(clickedCol - 1, clickedRow);
                    FieldOpen(clickedCol + 1, clickedRow);

                    FieldOpen(clickedCol - 1, clickedRow + 1);
                    FieldOpen(clickedCol, clickedRow + 1);
                    FieldOpen(clickedCol + 1, clickedRow + 1);
                }

                Field[clickedCol, clickedRow] = number;
            }
        }

        /// <summary>
        /// Checking the mine in a current cell. Ignores going out of range.
        /// </summary>
        /// <returns>Returns "true" if there is a mine in a cell. Returns "false" in all other ways</returns>
        protected bool IfTheMine(int clickedCol, int clickedRow)
        {
            if (clickedRow < 0 || clickedCol < 0 || clickedRow >= FieldHeight || clickedCol >= FieldWidth)
                return false;
            else 
                return MineField[clickedCol, clickedRow];
        }
    }
}
