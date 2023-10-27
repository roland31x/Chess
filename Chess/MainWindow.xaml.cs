using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chess.PieceClasses;

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
        Piece[,] Game = Piece.Pieces;
        Piece? selected;
        List<int[]>? legalmoves;
        PieceColor Turn = PieceColor.White;
        bool WhiteEnpassantTurn = false;
        bool BlackEnpassantTurn = false;
        bool whiteincheck = false;
        bool blackincheck = false;
        bool calc = false;
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
                    bg[i, j].Background = Brushes.Transparent;
                    if (Game[i, j] != null)
                    {
                        labels[i, j].Background = Game[i, j].Body;
                        labels[i, j].Tag = Game[i, j];
                    }
                    else
                    {
                        labels[i, j].Background = Brushes.Transparent;
                        labels[i, j].Tag = null;
                    }
                        
                }
            }
            if(selected != null)
                bg[selected.I, selected.J].Background = Brushes.Red;
            if(legalmoves != null)
                foreach (int[] p in legalmoves)
                    bg[p[0], p[1]].Background = Brushes.LimeGreen;
            if (whiteincheck)
                bg[wpieces.OfType<King>().First().I, wpieces.OfType<King>().First().J].Background = Brushes.Orange;
            if (blackincheck)
                bg[bpieces.OfType<King>().First().I, bpieces.OfType<King>().First().J].Background = Brushes.Orange;


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
                    l.Background = (i + j) % 2 == 0 ? Brushes.Beige : Brushes.BurlyWood;
                    MainCanvas.Children.Add(l);
                    Canvas.SetLeft(l,1 + 100 * j);
                    Canvas.SetTop(l,1 + 100 * i);

                    Label l2 = new Label();
                    l2.Height = 98;
                    l2.Width = 98;
                    MainCanvas.Children.Add(l2);
                    Canvas.SetLeft(l2, 1 + 100 * j);
                    Canvas.SetTop(l2, 1 + 100 * i);
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

        private async void PieceSelect(object sender, MouseButtonEventArgs e)
        {
            if (calc)
                return;
            Label l = sender as Label;
            if (selected == null)
            {
                if (l.Tag != null && (l.Tag as Piece).Color == Turn)
                {
                    Piece selection = (Piece)((sender as Label).Tag);
                    selected = selection;
                    legalmoves = selection.PieceMoves(true);
                }                                  
            }
            else
            {
                calc = true;
                bool okmove = false;
                foreach (int[] m in legalmoves)
                {
                    if (l == labels[m[0], m[1]])
                    {
                        int starti = selected.I;
                        int startj = selected.J;

                        Piece[,] savestate = GetState(Piece.Pieces);
                        selected.Move(m[0], m[1]);
                        bool legal = await LegalMove(Turn);
                        if (!legal)
                        {                            
                            Piece.ResetStateTo(savestate);
                            await FlashAnimation();
                        }                            
                        else
                        {
                            okmove = true;
                            selected.Moved = true;
                            InvalidateEnpassantTurns();
                            if(selected.GetType() == typeof(Pawn))
                                CheckEnpassant((Pawn)selected, starti);
                        }

                        await CheckForCheck();

                        Cursor = Cursors.Arrow;
                        break;
                    }
                }
                if (okmove)
                    Turn = Turn == PieceColor.White ? PieceColor.Black : PieceColor.White;
                selected = null;
                legalmoves = null;
            }
            UpdateUI();
            calc = false;
        }
        void CheckEnpassant(Pawn selected, int starti)
        {
            if (Math.Abs(starti - selected.I) == 2)
            {
                int leftj = selected.J - 1;
                int rightj = selected.J + 1;
                if (leftj > 0 && leftj < 8 && Game[selected.I, leftj] != null)
                    if (Game[selected.I, leftj].GetType() == typeof(Pawn) && Game[selected.I, leftj].Color != selected.Color)
                    {
                        (Game[selected.I, leftj] as Pawn)!.EnpassantPos = new int[] { selected.I - (selected.Color == PieceColor.White ? -1 : 1), selected.J };
                        if (Turn == PieceColor.White)
                            BlackEnpassantTurn = true;
                        else
                            WhiteEnpassantTurn = true;
                    }

                if (rightj > 0 && rightj < 8 && Game[selected.I, rightj] != null)
                    if (Game[selected.I, rightj].GetType() == typeof(Pawn) && Game[selected.I, rightj].Color != selected.Color)
                    {
                        (Game[selected.I, rightj] as Pawn)!.EnpassantPos = new int[] { selected.I - (selected.Color == PieceColor.White ? -1 : 1), selected.J };
                        if (Turn == PieceColor.White)
                            BlackEnpassantTurn = true;
                        else
                            WhiteEnpassantTurn = true;
                    }
            }
        }
        void InvalidateEnpassantTurns()
        {
            if (BlackEnpassantTurn)
            {
                foreach (Pawn p in bpieces.OfType<Pawn>())
                    p.EnpassantPos = null;
                BlackEnpassantTurn = false;
            }
            if (WhiteEnpassantTurn)
            {
                foreach (Pawn p in wpieces.OfType<Pawn>())
                    p.EnpassantPos = null;
                WhiteEnpassantTurn = false;
            }
        }
        async Task FlashAnimation()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while(sw.ElapsedMilliseconds < 1000)
            {
                if ((sw.ElapsedMilliseconds / 100) % 2 == 0)
                    MainCanvas.Background = Brushes.Red;
                else
                    MainCanvas.Background = Brushes.Gray;
                await Task.Delay(30);
            }
            MainCanvas.Background = Brushes.Gray;
            sw.Stop();
            sw = null;
        }
        async Task<bool> LegalMove(PieceColor c)
        {
            await Task.Run(CheckForCheck);

            if (c == PieceColor.Black && blackincheck)
                return false;
            if (c == PieceColor.White && whiteincheck)
                return false;
            return true;
        }
        Piece[,] GetState(Piece[,] p)
        {
            Piece[,] tor = new Piece[8, 8];
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    tor[i, j] = p[i, j];
                }
            }
            return tor;
        }
        Task CheckForCheck()
        {
            whiteincheck = false;
            blackincheck = false;
            Piece targetking = bpieces.OfType<King>().First();
            foreach (Piece p in wpieces.Where(x => x.isAlive))
            {
                List<int[]> legal = p.PieceMoves(false);
                foreach (int[] l in legal)
                    if (l[0] == targetking.I && l[1] == targetking.J)
                        blackincheck = true;
            }

            targetking = wpieces.OfType<King>().First();
            foreach (Piece p in bpieces.Where(x => x.isAlive))
            {
                List<int[]> legal = p.PieceMoves(false);
                foreach (int[] l in legal)
                    if (l[0] == targetking.I && l[1] == targetking.J)
                        whiteincheck = true;
            }

            // check for game over in case it's a mate // todo

            return Task.CompletedTask;
        }
        private void P_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void P_MouseEnter(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (selected == null)
            {
                if (l.Tag != null && (l.Tag as Piece).Color == Turn)
                    Cursor = Cursors.Hand;
            }
            else
            {
                foreach (int[] m in legalmoves)
                {
                    if(l == labels[m[0], m[1]])
                    {
                        Cursor = Cursors.Hand;
                        return;
                    }                      
                }
            }               
        }
    }
}
