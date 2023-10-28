using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess.PieceClasses
{
    public abstract class Piece
    {
        public static Piece[,] Pieces = new Piece[8, 8];
        public int I { get; protected set; }
        public int J { get; protected set; }
        public ImageBrush Body { get; protected set; }
        public PieceColor Color { get; protected set; }
        public bool Moved { get; set; } = false;
        public bool isAlive { get; protected set; } = true;
        public delegate void PromotionEventHandler (object sender);
        public event PromotionEventHandler? PromotionEvent;
        protected Piece(PieceColor color, int i, int j)
        {
            Color = color;
            I = i;
            J = j;
            Pieces[i, j] = this;
        }
        protected bool CheckMove(int i, int j, bool canattack)
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
        public static void ResetStateTo(Piece[,] state)
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Pieces[i,j] = state[i,j];
                    if (state[i, j] != null)
                    {
                        Pieces[i, j].I = i;
                        Pieces[i, j].J = j;
                        Pieces[i, j].isAlive = true;
                    }                      
                }
            }
        }
        public abstract List<int[]> PieceMoves(bool byPlayer);
        public virtual void Move(int newI, int newJ)
        {
            if(Pieces[I, J].GetType() == typeof(Pawn))
            {
                if ((Color == PieceColor.White && newI == 0) || (Color == PieceColor.Black && newI == 7))
                    PromotionEvent?.Invoke(this);
                else if ((Pieces[I, J] as Pawn)!.EnpassantPos != null && (Pieces[I, J] as Pawn)!.EnpassantPos![0] == newI && (Pieces[I, J] as Pawn)!.EnpassantPos![1] == newJ)
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
                        Pieces[I, 7].Move(I, 5);
                    else // queen side castle
                        Pieces[I, 0].Move(I, 3);
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
