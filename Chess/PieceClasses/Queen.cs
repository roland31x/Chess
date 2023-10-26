using System.Collections.Generic;

namespace Chess.PieceClasses
{
    public class Queen : Piece
    {
        public Queen(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Queen[0] : Queen[1];
        }

        public override List<int[]> PieceMoves()
        {
            List<int[]> toreturn = new List<int[]>();

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I + i, J, true);
                bool move = CheckMove(I + i, J, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I + i, J });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I - i, J, true);
                bool move = CheckMove(I - i, J, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I - i, J });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I, J - i, true);
                bool move = CheckMove(I, J - i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I, J - i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I, J + i, true);
                bool move = CheckMove(I, J + i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I, J + i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I + i, J + i, true);
                bool move = CheckMove(I + i, J + i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I + i, J + i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I - i, J - i, true);
                bool move = CheckMove(I - i, J - i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I - i, J - i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I + i, J - i, true);
                bool move = CheckMove(I + i, J - i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I + i, J - i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            for (int i = 1; i < 8; i++)
            {
                bool ok = false;
                bool attack = CheckMove(I - i, J + i, true);
                bool move = CheckMove(I - i, J + i, false);
                if (attack || move)
                {
                    toreturn.Add(new int[] { I - i, J + i });
                    ok = true;
                    if (attack)
                        ok = false;
                }
                if (!ok)
                    break;
            }

            return toreturn;
        }
    }
}
