using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Piece> wpieces = new List<Piece>();
        List<Piece> bpieces = new List<Piece>();
        Label[,] labels;
        Label[,] bg;
        Brush LightBlack = new SolidColorBrush(new Color() { R = 50, G = 50, B = 50, A = 255 });
        Piece? selected;
        List<int[]>? legalmoves;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
            InitGame();
            UpdateUI();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void UpdateUI()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (Piece.Pieces[i, j] != null)
                    {
                        labels[i, j].Background = Piece.Pieces[i, j].Body;
                        labels[i, j].Tag = Piece.Pieces[i, j];
                    }

                    else
                    {
                        labels[i, j].Background = Brushes.Transparent;
                        labels[i, j].Tag = null;
                    }
                        
                }
            }
        }
        void InitGame()
        {
            labels = new Label[8, 8];
            bg = new Label[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Label l = new Label();
                    l.Height = 98;
                    l.Width = 98;
                    l.Background = (i + j) % 2 == 0 ? Brushes.Beige : LightBlack;
                    MainCanvas.Children.Add(l);
                    Canvas.SetLeft(l,1 + 100 * j);
                    Canvas.SetTop(l,1 + 100 * i);

                    Label l2 = new Label();
                    l2.Height = 98;
                    l2.Width = 98;
                    MainCanvas.Children.Add(l2);
                    Canvas.SetLeft(l, 1 + 100 * j);
                    Canvas.SetTop(l, 1 + 100 * i);
                    bg[i, j] = l2;

                    Label p = new Label();
                    p.Height = 98;
                    p.Width = 98;
                    p.MouseEnter += P_MouseEnter;
                    p.MouseLeave += P_MouseLeave;
                    p.MouseDown += PieceSelect;
                    MainCanvas.Children.Add(p);
                    Canvas.SetLeft(p, 1 + 100 * j);
                    Canvas.SetTop(p, 1 + 100 * i);
                    labels[i, j] = p;
                }
            }
            for(int i = 0; i < 8; i++)
            {
                Pawn white = new Pawn(PieceColor.White, 6, i);
                Pawn black = new Pawn(PieceColor.Black, 1, i);
                wpieces.Add(white);
                bpieces.Add(black);
            }

            Rook whiter1 = new Rook(PieceColor.White, 7, 0);
            Rook blackr1 = new Rook(PieceColor.Black, 0, 0);
            wpieces.Add(whiter1);
            bpieces.Add(blackr1);

            Knight whitek1 = new Knight(PieceColor.White, 7, 1);
            Knight blackk1 = new Knight(PieceColor.Black, 0, 1);
            wpieces.Add(whitek1);
            bpieces.Add(blackk1);

            Bishop whiteb1 = new Bishop(PieceColor.White, 7, 2);
            Bishop blackb1 = new Bishop(PieceColor.Black, 0, 2);
            wpieces.Add(whiteb1);
            bpieces.Add(blackb1);

            Queen whiteq1 = new Queen(PieceColor.White, 7, 3);
            Queen blackq1 = new Queen(PieceColor.Black, 0, 3);           
            wpieces.Add(whiteq1);
            bpieces.Add(blackq1);

            King whiteking = new King(PieceColor.White, 7, 4);
            King blackking = new King(PieceColor.Black, 0, 4);
            wpieces.Add(whiteking);
            bpieces.Add(blackking);

            Bishop whiteb2 = new Bishop(PieceColor.White, 7, 5);
            Bishop blackb2 = new Bishop(PieceColor.Black, 0, 5);
            wpieces.Add(whiteb2);
            bpieces.Add(blackb2);

            Knight whitek2 = new Knight(PieceColor.White, 7, 6);
            Knight blackk2 = new Knight(PieceColor.Black, 0, 6);
            wpieces.Add(whitek2);
            bpieces.Add(blackk2);

            Rook whiter2 = new Rook(PieceColor.White, 7, 7);
            Rook blackr2 = new Rook(PieceColor.Black, 0, 7);
            wpieces.Add(whiter2);
            bpieces.Add(blackr2);

        }

        private void PieceSelect(object sender, MouseButtonEventArgs e)
        {
            if(selected == null)
            {
                Piece selection = (Piece)((sender as Label).Tag);
                legalmoves = selection.PieceMoves();

            }
            else
            {
                // check move
            }
        }

        private void P_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void P_MouseEnter(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (l.Tag != null)
                Cursor = Cursors.Hand;
        }
    }
    public class Rook : Piece
    {
        public Rook(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Rook[0] : Rook[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public class Queen : Piece
    {
        public Queen(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Queen[0] : Queen[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public class King : Piece
    {
        public King(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? King[0] : King[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public class Bishop : Piece
    {
        public Bishop(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Bishop[0] : Bishop[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public class Knight : Piece
    {
        public Knight(PieceColor c, int i, int j) : base(c, i, j)
        {
            Body = c == PieceColor.White ? Knight[0] : Knight[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public class Pawn : Piece
    {
        public Pawn(PieceColor c, int i, int j) : base(c,i,j)
        {
            Body = c == PieceColor.White ? Pawn[0] : Pawn[1];
        }

        public override List<int[]> PieceMoves()
        {
            throw new NotImplementedException();
        }
    }
    public enum PieceColor
    {
        White = 0,
        Black = 1,
    }
    public abstract class Piece
    {
        public static Piece[,] Pieces = new Piece[8, 8];
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
        protected int I { get; set; }
        protected int J { get; set; }
        public ImageBrush Body { get; protected set; }
        public PieceColor Color { get; protected set; }
        protected Piece(PieceColor color, int i, int j)
        {
            Color = color;
            I = i;
            J = j;
            Pieces[i, j] = this;
        }
        public abstract List<int[]> PieceMoves();
    }
}
