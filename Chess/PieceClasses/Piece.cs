using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess.PieceClasses
{
    public abstract class Piece
    {
        //public static Piece[,] Pieces = new Piece[8, 8];
        public int I { get; set; }
        public int J { get; set; }
        public ImageBrush Body { get; protected set; }
        public PieceColor Color { get; protected set; }
        public bool Moved { get; set; } = false;
        public bool isAlive { get; set; } = true;
        protected Piece(PieceColor color, int i, int j)
        {
            Color = color;
            I = i;
            J = j;
        }
        protected bool CheckMove(int i, int j, bool canattack, Piece[,] Pieces)
        {
            try
            {
                if (Pieces[i, j] == null && !canattack)
                    return true;
                else if (Pieces[i, j] != null && canattack && Pieces[i, j].Color != Color)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public abstract List<int[]> PieceMoves(bool byPlayer, Piece[,] Pieces);
        public virtual void Move(int newI, int newJ, Piece[,] Pieces)
        {
            if(Pieces[I, J].GetType() == typeof(Pawn))
            {
                if ((Pieces[I, J] as Pawn)!.EnpassantPos != null && (Pieces[I, J] as Pawn)!.EnpassantPos![0] == newI && (Pieces[I, J] as Pawn)!.EnpassantPos![1] == newJ)
                {
                    int dir = Color == PieceColor.White ? 1 : -1;
                    Pieces[newI + dir, newJ].isAlive = false;
                    Pieces[newI + dir, newJ] = null;
                }            
            }
            if(Pieces[I, J].GetType() == typeof(King) && !(Pieces[I, J] as King)!.Moved)
            {
                if(Math.Abs(J - newJ) == 2)
                {
                    if(newJ > 3) // king side castle
                        Pieces[I, 7].Move(I, 5, Pieces);
                    else // queen side castle
                        Pieces[I, 0].Move(I, 3, Pieces);
                }
            }
            if (Pieces[newI, newJ] != null)
                Pieces[newI, newJ].isAlive = false;
            Pieces[I, J] = null;
            I = newI;
            J = newJ;
            Pieces[I, J] = this;
        }
        #region IMAGES
        public readonly static ImageBrush[] Queen = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-queen.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-queen.png")))
        };

        public readonly static ImageBrush[] King = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-king.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-king.png")))
        };

        public readonly static ImageBrush[] Pawn = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-pawn.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-pawn.png")))
        };

        public readonly static ImageBrush[] Knight = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-knight.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-knight.png")))
        };

        public readonly static ImageBrush[] Bishop = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-bishop.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-bishop.png")))
        };
        public readonly static ImageBrush[] Rook = new ImageBrush[]
        {
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/white-rook.png"))),
            new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Pieces/black-rook.png")))
        };
        #endregion
    }
}
