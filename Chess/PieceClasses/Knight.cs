using System.Collections.Generic;

namespace Chess.PieceClasses
{
    public class Knight : Piece
    {
        static List<int[]> L = new List<int[]>()
        {
            new int[] { 1, 2 },
            new int[] { 1, -2 },
            new int[] { -1, 2 },
            new int[] { -1, -2 },
            new int[] { 2, 1 },
            new int[] { 2, -1 },
            new int[] { -2, 1 },
            new int[] { -2, -1 },
        };
        public Knight(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Knight[0] : Knight[1];
        }

        public override List<int[]> PieceMoves()
        {
            List<int[]> toreturn = new List<int[]>();
            foreach (int[] l in L)
                if (CheckMove(I + l[0], J + l[1], true) || CheckMove(I + l[0], J + l[1], false))
                    toreturn.Add(new int[] { I + l[0], J + l[1] });

            return toreturn;
        }
    }
}
