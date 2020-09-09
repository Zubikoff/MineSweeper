using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class MineSweeperGame
    {
        protected int[,] MineField;

        public int FieldWidth { get; private set; }

        public int FieldHeight { get; private set; }

        public int Mines { get; private set; }

        public MineSweeperGame(int fieldW, int fieldH, int mines)
        {
            MineField = new int[fieldW, fieldH];
            Mines = mines;
        }

        public static MineSweeperGame StartNew(int fieldW, int fieldH, int mines)
        {
            return new MineSweeperGame(fieldW, fieldH, mines);
        }

        protected internal void SetMines()
        {

        }
    }
}
