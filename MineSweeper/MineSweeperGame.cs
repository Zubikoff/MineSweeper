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

        /// <summary>
        /// A mine field representation to a player. Number 0 to 8 represents number of mines in the surrounding cells. 
        /// <see langword="null"/> means this cell was never opened before. -1 means that user put a flag on that cell.
        /// </summary>
        public int?[,] Field { get; private set; }

        private bool MinesSet { get; set; }

        public bool GameOver { get; private set; }

        public int FieldWidth { get; private set; }

        public int FieldHeight { get; private set; }

        public int Mines { get; private set; }

        private int Flags { get; set; }

        /// <summary>
        /// Represents a session of the MineSweeper game.
        /// </summary>
        /// <param name="fieldW">Column number of the game field.</param>
        /// <param name="fieldH">Row number of the game field.</param>
        /// <param name="mines">Quantity of mines that will be set on the field.</param>
        public MineSweeperGame(int fieldW, int fieldH, int mines)
        {
            if (mines > fieldW*fieldH)
            {
                throw new ArgumentOutOfRangeException("mines", "Perameter mines can't be bigger then multiplication of fieldW and fieldH.");
            }
            FieldWidth = fieldW;
            FieldHeight = fieldH;
            MineField = new bool[fieldW, fieldH];
            Field = new int?[fieldW, fieldH];
            Mines = mines;
            Flags = 0;
            MinesSet = false;

        }

        public static MineSweeperGame StartNew(int fieldW, int fieldH, int mines)
        {
            return new MineSweeperGame(fieldW, fieldH, mines);
        }

        /// <summary>
        /// Randomly spitting mines across the field. This should happen right after player's first move.
        /// player's move coordinates excluded from available mines location.
        /// </summary>
        /// <param name="clickedRow"></param>
        /// <param name="clickedCol"></param>
        protected internal void SetMines(int clickedCol, int clickedRow)
        {
            MineField[clickedCol, clickedRow] = true; // temporarily set player's move as "mine"

            SetMines();

            MineField[clickedCol, clickedRow] = false; // clear player's move from a mine
        }

        /// <summary>
        /// Randomly spitting mines across the field. This method ignores player's move.
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
                if (!MineField[Col, Row])
                {
                    MineField[Col, Row] = true;
                    mines--;
                }
            }
            MinesSet = true;
        }

        /// <summary>
        /// Opening a cell.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if demining successful. Returns <see langword="false"/> if you picked a mine</returns>
        public bool DemineCell(int clickedCol, int clickedRow)
        {
            if (clickedRow < 0 || clickedCol < 0 || clickedRow >= FieldHeight || clickedCol >= FieldWidth)
                throw new IndexOutOfRangeException("Переданные координаты находятся за пределами игрового поля.");

            if (!MinesSet) SetMines(clickedCol, clickedRow);

            if (MineField[clickedCol, clickedRow])
                return false;

            if (GameOver) return true;

            FieldOpen(clickedCol, clickedRow);

            if (Mines == Flags) VictoryCheck();

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

            // if there is a mine in a cell or this cell is already processed we return (also if game is already over)
            if (MineField[clickedCol, clickedRow] || Field[clickedCol, clickedRow] != null || GameOver) 
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

                Field[clickedCol, clickedRow] = number;

                if (number == 0)
                {
                    // if there is no mines in surrounding cells we open all cells around us (recursively);
                    FieldOpen(clickedCol - 1, clickedRow - 1);
                    FieldOpen(clickedCol, clickedRow - 1);
                    FieldOpen(clickedCol + 1, clickedRow - 1);

                    FieldOpen(clickedCol - 1, clickedRow);
                    FieldOpen(clickedCol + 1, clickedRow);

                    FieldOpen(clickedCol - 1, clickedRow + 1);
                    FieldOpen(clickedCol, clickedRow + 1);
                    FieldOpen(clickedCol + 1, clickedRow + 1);
                }
            }
        }

        /// <summary>
        /// Checking the existence of a mine in a current cell. Ignores going out of range.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if there is a mine in a cell. Returns <see langword="false"/> in all other ways</returns>
        protected bool IfTheMine(int clickedCol, int clickedRow)
        {
            if (clickedRow < 0 || clickedCol < 0 || clickedRow >= FieldHeight || clickedCol >= FieldWidth)
                return false;
            else 
                return MineField[clickedCol, clickedRow];
        }

        /// <summary>
        /// Puts or removes a flag from a cell.
        /// Works only if the game started and this field was never opened.
        /// </summary>
        public void PutAFlag(int clickedCol, int clickedRow)
        {
            if (!MinesSet || GameOver) return; 
            if (Field[clickedCol, clickedRow] == null)
            {
                Field[clickedCol, clickedRow] = -1;
                Flags++;
            }
            else
                if (Field[clickedCol, clickedRow] == -1)
                {
                    Field[clickedCol, clickedRow] = null;
                    Flags--;
                }
            if (Mines == Flags) VictoryCheck();
        }

        /// <summary>
        /// Checking out if player made a final move and won the game
        /// </summary>
        /// <returns><see langword="True"/> if the game is over and player won. <see langword="False"/> if there are still unopened cells or mistakenly displayed flags</returns>
        protected void VictoryCheck()
        {
            for (int i = 0; i < FieldWidth; i++)
            {
                for (int j = 0; j < FieldHeight; j++)
                {
                    if (((Field[i, j] != -1) & (MineField[i, j])) || ((Field[i, j] == -1) & (MineField[i, j])) || (Field[i, j] == null)) 
                        GameOver = false;
                }
            }
            GameOver = true;
        } 
    }
}
