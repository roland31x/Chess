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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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

        Label[,] labels = new Label[8, 8];
        Label[,] plabels = new Label[8, 8];
        Ellipse[,] ebg = new Ellipse[8, 8];
        Label[,] bg = new Label[8, 8];

        Piece[,] Pieces = new Piece[8, 8];
        Piece? selected;
        List<int[]>? legalmoves;

        PieceColor _t = PieceColor.White;
        PieceColor Turn { get { return _t; } set { _t = value; if (_t == PieceColor.White) { WhiteTimerBG.Fill = Brushes.Goldenrod; BlackTimerBG.Fill = Brushes.White; whitetimer.Start(); blacktimer.Stop(); } else { BlackTimerBG.Fill = Brushes.Goldenrod; WhiteTimerBG.Fill = Brushes.White; blacktimer.Start(); whitetimer.Stop(); } } }
        
        bool WhiteEnpassantTurn = false;
        bool BlackEnpassantTurn = false;

        bool whiteincheck = false;
        bool blackincheck = false;
        bool calc = false;

        Stopwatch whitetimer = new Stopwatch();
        Stopwatch blacktimer = new Stopwatch();
        DispatcherTimer timer = new DispatcherTimer();

        Piece? promotion;

        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
            InitGame();
            UpdateUI();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            BlackTimerLabel.Content = blacktimer.Elapsed.ToString(@"mm\:ss");
            WhiteTimerLabel.Content = whitetimer.Elapsed.ToString(@"mm\:ss");
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void UpdateUI()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ebg[i, j].Fill = Brushes.Transparent;
                    bg[i, j].Background = Brushes.Transparent;
                    if (Pieces[i, j] != null)
                    {
                        plabels[i, j].Background = Pieces[i, j].Body;
                        labels[i, j].Tag = Pieces[i, j];
                    }
                    else
                    {
                        plabels[i, j].Background = Brushes.Transparent;
                        labels[i, j].Tag = null;
                    }

                }
            }
            if (selected != null)
                bg[selected.I, selected.J].Background = Brushes.LimeGreen;
            if (legalmoves != null)
                foreach (int[] p in legalmoves)
                    ebg[p[0], p[1]].Fill = Brushes.LimeGreen;
            if (whiteincheck)
                bg[wpieces.OfType<King>().First().I, wpieces.OfType<King>().First().J].Background = Brushes.Orange;
            if (blackincheck)
                bg[bpieces.OfType<King>().First().I, bpieces.OfType<King>().First().J].Background = Brushes.Orange;
            BlackCaptures.Children.Clear();
            foreach (Piece p in bpieces.Where(x => !x.isAlive))
                BlackCaptures.Children.Add(new Label() { Height = 32, Width = 32, Background = p.Body });
            WhiteCaptures.Children.Clear();
            foreach (Piece p in wpieces.Where(x => !x.isAlive))
                WhiteCaptures.Children.Add(new Label() { Height = 32, Width = 32, Background = p.Body });
            
        }
        public void ResetStateTo(Piece[,] state)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pieces[i, j] = state[i, j];
                    if (state[i, j] != null)
                    {
                        Pieces[i, j].I = i;
                        Pieces[i, j].J = j;
                        Pieces[i, j].isAlive = true;
                    }
                }
            }
        }
        void ResetGame()
        {
            Turn = PieceColor.White;
            WhiteEnpassantTurn = false;
            BlackEnpassantTurn = false;
            whiteincheck = false;
            blackincheck = false;
            calc = false;
            selected = null;
            legalmoves = null;
            whitetimer.Reset();
            blacktimer.Reset();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Pieces[i, j] = null;
            wpieces.Clear();
            bpieces.Clear();
            LoadDefaultPositionPieces();
            UpdateUI();
        }
        void InitGame()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Label l = new Label();
                    l.Height = 100;
                    l.Width = 100;
                    l.Background = (i + j) % 2 == 0 ? Brushes.Beige : Brushes.BurlyWood;
                    MainCanvas.Children.Add(l);
                    Canvas.SetLeft(l, 100 * j);
                    Canvas.SetTop(l, 100 * i);

                    Label l3 = new Label();
                    l3.Height = 100;
                    l3.Width = 100;
                    l3.Opacity = 0.66;
                    MainCanvas.Children.Add(l3);
                    Canvas.SetLeft(l3, 100 * j);
                    Canvas.SetTop(l3, 100 * i);
                    bg[i, j] = l3;

                    Label p = new Label();
                    p.Height = 100;
                    p.Width = 100;
                    MainCanvas.Children.Add(p);
                    Canvas.SetLeft(p, 100 * j);
                    Canvas.SetTop(p, 100 * i);
                    plabels[i, j] = p;

                    Ellipse l2 = new Ellipse();
                    l2.Height = 33;
                    l2.Width = 33;
                    l2.Opacity = 0.66;
                    MainCanvas.Children.Add(l2);
                    Canvas.SetLeft(l2, 100 * j + 50 - l2.Width / 2);
                    Canvas.SetTop(l2, 100 * i + 50 - l2.Height / 2);
                    ebg[i, j] = l2;

                    Label p2 = new Label();
                    p2.Height = 100;
                    p2.Width = 100;
                    p2.MouseEnter += P_MouseEnter;
                    p2.MouseLeave += P_MouseLeave;
                    p2.MouseDown += PieceSelect;
                    MainCanvas.Children.Add(p2);
                    Canvas.SetLeft(p2, 100 * j);
                    Canvas.SetTop(p2, 100 * i);
                    labels[i, j] = p2;
                }
            }
            LoadDefaultPositionPieces();

        }
        void LoadDefaultPositionPieces()
        {
            Queen whiteq = new Queen(PieceColor.White, 7, 3);
            Queen blackq = new Queen(PieceColor.Black, 0, 3);
            wpieces.Add(whiteq);
            bpieces.Add(blackq);

            Rook whiter1 = new Rook(PieceColor.White, 7, 0);
            Rook blackr1 = new Rook(PieceColor.Black, 0, 0);
            wpieces.Add(whiter1);
            bpieces.Add(blackr1);

            Rook whiter2 = new Rook(PieceColor.White, 7, 7);
            Rook blackr2 = new Rook(PieceColor.Black, 0, 7);
            wpieces.Add(whiter2);
            bpieces.Add(blackr2);

            Bishop whiteb1 = new Bishop(PieceColor.White, 7, 2);
            Bishop blackb1 = new Bishop(PieceColor.Black, 0, 2);
            wpieces.Add(whiteb1);
            bpieces.Add(blackb1);

            Bishop whiteb2 = new Bishop(PieceColor.White, 7, 5);
            Bishop blackb2 = new Bishop(PieceColor.Black, 0, 5);
            wpieces.Add(whiteb2);
            bpieces.Add(blackb2);

            King whiteking = new King(PieceColor.White, 7, 4);
            King blackking = new King(PieceColor.Black, 0, 4);
            wpieces.Add(whiteking);
            bpieces.Add(blackking);

            Knight whitek1 = new Knight(PieceColor.White, 7, 1);
            Knight blackk1 = new Knight(PieceColor.Black, 0, 1);
            wpieces.Add(whitek1);
            bpieces.Add(blackk1);

            Knight whitek2 = new Knight(PieceColor.White, 7, 6);
            Knight blackk2 = new Knight(PieceColor.Black, 0, 6);
            wpieces.Add(whitek2);
            bpieces.Add(blackk2);

            for (int i = 0; i < 8; i++)
            {
                Pawn white = new Pawn(PieceColor.White, 6, i);
                Pawn black = new Pawn(PieceColor.Black, 1, i);
                wpieces.Add(white);
                bpieces.Add(black);
            }
            foreach (Piece p in wpieces.Union(bpieces))
                Pieces[p.I, p.J] = p;
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
                    legalmoves = selection.PieceMoves(true, Pieces);
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

                        Piece[,] savestate = GetState(Pieces);
                        selected.Move(m[0], m[1], Pieces);
                        bool legal = await LegalMove(Turn);
                        if (!legal)
                        {
                            ResetStateTo(savestate);
                            await FlashAnimation();
                        }
                        else
                        {
                            okmove = true;
                            selected.Moved = true;
                            InvalidateEnpassantTurns();
                            if (selected.GetType() == typeof(Pawn))
                            {
                                CheckEnpassant((Pawn)selected, starti);
                                await CheckPromotion(selected);
                            }

                        }


                        await CheckForCheck();
                        if (whiteincheck)
                           await Task.Run(() => CheckForMate(PieceColor.White));
                        if (blackincheck)
                           await Task.Run(() => CheckForMate(PieceColor.Black));

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
        async Task FlashAnimation()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 1000)
            {
                if ((sw.ElapsedMilliseconds / 100) % 2 == 0)
                    bg[selected.I,selected.J].Background = Brushes.Red;
                else
                    bg[selected.I, selected.J].Background = Brushes.Orange;
                await Task.Delay(30);
            }
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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tor[i, j] = p[i, j];
                }
            }
            return tor;
        }
        async Task CheckForMate(PieceColor c)
        {
            Piece[,] save = GetState(Pieces);
            bool mate = true;
            foreach (Piece p in wpieces.Union(bpieces).Where(x => x.isAlive && x.Color == c))
            {
                List<int[]> legal = p.PieceMoves(false, Pieces);
                foreach (int[] l in legal)
                {
                    p.Move(l[0], l[1], Pieces);
                    await CheckForCheck();
                    if ((whiteincheck && c == PieceColor.White) || (blackincheck && c == PieceColor.Black))
                    {
                        ResetStateTo(save);
                        continue;
                    }
                    else
                    {
                        mate = false;
                        break;
                    }

                }
                if (!mate)
                    break;
            }
            if (mate)
                MessageBox.Show("CHECK MATE");
            ResetStateTo(save);

        }
        Task CheckForCheck()
        {
            whiteincheck = false;
            blackincheck = false;
            Piece targetking = bpieces.OfType<King>().First();
            foreach (Piece p in wpieces.Where(x => x.isAlive))
            {
                List<int[]> legal = p.PieceMoves(false, Pieces);
                foreach (int[] l in legal)
                    if (l[0] == targetking.I && l[1] == targetking.J)
                        blackincheck = true;
            }

            targetking = wpieces.OfType<King>().First();
            foreach (Piece p in bpieces.Where(x => x.isAlive))
            {
                List<int[]> legal = p.PieceMoves(false, Pieces);
                foreach (int[] l in legal)
                    if (l[0] == targetking.I && l[1] == targetking.J)
                        whiteincheck = true;
            }

            // check for game over in case it's a mate // todo

            return Task.CompletedTask;
        }
        void CheckEnpassant(Pawn selected, int starti)
        {
            if (Math.Abs(starti - selected.I) == 2)
            {
                int leftj = selected.J - 1;
                int rightj = selected.J + 1;
                if (leftj > 0 && leftj < 8 && Pieces[selected.I, leftj] != null)
                    if (Pieces[selected.I, leftj].GetType() == typeof(Pawn) && Pieces[selected.I, leftj].Color != selected.Color)
                    {
                        (Pieces[selected.I, leftj] as Pawn)!.EnpassantPos = new int[] { selected.I - (selected.Color == PieceColor.White ? -1 : 1), selected.J };
                        if (Turn == PieceColor.White)
                            BlackEnpassantTurn = true;
                        else
                            WhiteEnpassantTurn = true;
                    }

                if (rightj > 0 && rightj < 8 && Pieces[selected.I, rightj] != null)
                    if (Pieces[selected.I, rightj].GetType() == typeof(Pawn) && Pieces[selected.I, rightj].Color != selected.Color)
                    {
                        (Pieces[selected.I, rightj] as Pawn)!.EnpassantPos = new int[] { selected.I - (selected.Color == PieceColor.White ? -1 : 1), selected.J };
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
        private async Task CheckPromotion(Piece topromote)
        {
            if (topromote.Color == PieceColor.White && topromote.I != 0)
                return;
            if (topromote.Color == PieceColor.Black && topromote.I != 7)
                return;

            Piece promoted = await PromotionBoxResult(topromote.Color);
            promoted.I = topromote.I;
            promoted.J = topromote.J;
            if (topromote.Color == PieceColor.White)
            {
                wpieces.Remove(topromote);
                wpieces.Add(promoted);
            }
            else
            {
                bpieces.Remove(topromote);
                bpieces.Add(promoted);
            }
            Pieces[topromote.I, topromote.J] = promoted;
        }
        async Task<Piece> PromotionBoxResult(PieceColor color)
        {
            Label overlay = new Label() { Height = 5000, Width = 5000, Background = Brushes.White, Opacity = 0.33 };
            BaseCanvas.Children.Add(overlay);
            Panel.SetZIndex(overlay, 10);

            PromotionCanvas.Visibility = Visibility.Visible;
            Panel.SetZIndex(PromotionCanvas, 100);

            int c = (int)color;
            QueenPromotion.Background = Piece.Queen[c];
            KnightPromotion.Background = Piece.Knight[c];
            BishopPromotion.Background = Piece.Bishop[c];
            RookPromotion.Background = Piece.Rook[c];

            while (promotion == null)
                await Task.Delay(100);

            Piece toreturn = promotion;

            promotion = null;

            BaseCanvas.Children.Remove(overlay);
            PromotionCanvas.Visibility = Visibility.Hidden;
            return toreturn;
        }
        private void RookPromotion_Click(object sender, MouseEventArgs e) => promotion = new Rook(Turn, 0, 0);
        private void QueenPromotion_Click(object sender, MouseEventArgs e) => promotion = new Queen(Turn, 0, 0);
        private void KnightPromotion_Click(object sender, MouseEventArgs e) => promotion = new Knight(Turn, 0, 0);
        private void BishopPromotion_Click(object sender, MouseEventArgs e) => promotion = new Bishop(Turn, 0, 0);
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
                    if (l == labels[m[0], m[1]])
                    {
                        Cursor = Cursors.Hand;
                        return;
                    }
                }
            }
        }

        private void ResetGame_Click(object sender, MouseButtonEventArgs e) => ResetGame();
        private void ExitButton_Click(object sender, MouseButtonEventArgs e) => Close();

        private void Label_MouseEnter(object sender, MouseEventArgs e) => Cursor = Cursors.Hand;

        private void Label_MouseLeave(object sender, MouseEventArgs e) => Cursor = Cursors.Arrow;

    }
}
