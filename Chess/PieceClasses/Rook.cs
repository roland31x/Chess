﻿using System.Collections.Generic;

namespace Chess.PieceClasses
{
    public class Rook : Piece
    {
        public Rook(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Rook[0] : Rook[1];
        }

        public override List<int[]> PieceMoves(bool byPlayer)
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

            return toreturn;
        }
    }
}
