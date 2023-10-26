using System.Collections.Generic;

namespace Chess.PieceClasses
{
    public class King : Piece
    {
        public King(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? King[0] : King[1];
        }

        public override List<int[]> PieceMoves()
        {
            List<int[]> toreturn = new List<int[]>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (CheckMove(I + i, J + j, true) || CheckMove(I + i, J + j, false))
                    {
                        toreturn.Add(new int[] { I + i, J + j });
                    }
                }
            }

            return toreturn;
        }
    }
}
