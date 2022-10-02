using ChessEngine;
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

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board board;
        public MainWindow()
        {
            board = new Board();
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            board.StandardSetup();


            //Draw pieces
            DrawPieces();

        }

        private void DrawPieces()
        {
            RemoveUIElements<Image>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board.Grid[x, y] == null) continue;
                    var typeName = board.Grid[x, y].GetType().Name;
                    Image img07 = new Image { Width = 100, Height = 100 };
                    img07.Margin = new Thickness(5);

                    // Create source.
                    BitmapImage bi = new BitmapImage();
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                    bi.BeginInit();
                    if(board.Grid[x, y].PieceColor == -1)
                    {
                        typeName += "Dark";
                    }
                    bi.UriSource = new Uri(@".\Images\" + typeName + ".png", UriKind.RelativeOrAbsolute);
                    bi.DecodePixelWidth = 100;
                    bi.EndInit();
                    img07.Source = bi;

                    Grid.SetColumn(img07, x);
                    Grid.SetRow(img07, 7 - y);

                    img07.Width = 100;
                    img07.Height = 100;
                    img07.HorizontalAlignment = HorizontalAlignment.Center;
                    img07.VerticalAlignment = VerticalAlignment.Center;
                    img07.Tag = new Point(x, y);
                    grdBoard.Children.Add(img07);
                }
            }
            //RemoveUIElements<Rectangle>();
        }

        private void ShowMoves(int col, int row, bool samePosition)
        {
            if (moving > 0)
            {
                --moving;
                return;
            }
            RemoveUIElements<Rectangle>();


            if (board.Grid[col, row] == null) return;
            int[,,] points = board.GetMoves(col, row);
            if (points == null) return;
            var pieceMoves = board.Grid[col, row].Moves();
            if (pieceMoves == null) return;

            List<Move> movesFlat = board.Flatten(pieceMoves);

            foreach (var move in movesFlat)
            {
                Brush color = move.Take ? Brushes.Gold : Brushes.Silver;
                int column = move.X + move.Piece.X;
                int row1 = move.Y + move.Piece.Y;
                Rectangle rect = CreateRectangle(color, column, row1);
                rect.Tag = move;
                rect.MouseDown += Rect_MouseDown;

            }
        }

        private Rectangle CreateRectangle(Brush color, int column, int row1)
        {
            var rect = new Rectangle();
            rect.Opacity = 0.6;
            Grid.SetColumn(rect, column);
            Grid.SetRow(rect, 7 - row1);
            grdBoard.Children.Add(rect);
            rect.Fill = color;
            return rect;
        }

        private void RemoveUIElements<T>() where T : UIElement
        {
            List<T> eles = new List<T>();
            foreach (var control in grdBoard.Children)
            {
                if (control is T)
                {
                    eles.Add((T)control);
                }
            }
            eles.Reverse();
            foreach (var rect in eles)
            {
                grdBoard.Children.Remove(rect);
            }
        }
        private int moving = 0;
        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool abortMove = false;
            moving = 2;
            Move tag = (Move)((Rectangle)sender).Tag;
            RemoveUIElements<Rectangle>();
            board.MovePiece(tag, (king, kingStatus) => {
                if((kingStatus & KingStatus.Checked) == KingStatus.Checked) CreateRectangle(Brushes.Red, king.X, king.Y);
                if(king.PieceColor == tag.Piece.PieceColor && (kingStatus & KingStatus.UndoMove) == KingStatus.UndoMove)
                {
                    abortMove = true;
                }
            });
            if (abortMove) this.AbortMove();
            DrawPieces();
            --moving;
            Title = board.CreateFEN();
        }

        private void AbortMove()
        {
            //TODO: implement AbortMove by reloading board to the previous position
            //throw new NotImplementedException();
        }

        private void BringImagesToFront()
        {
            List<Image> images = new List<Image>();
            foreach(var control in grdBoard.Children)
            {
                if(control is Image)
                {
                    BringToFront(grdBoard, (Image)control);
                }
            }
        }

        //The rest of the code in this file was mostly copied and pasted from stackoverflow, with few or no changes.
        //It has not been reviewed and may be lacking in quality

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //base.OnMouseDown(e);
            var hit = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (hit == null) { return; }

            Grid grid = hit.VisualHit.Parent<Grid>();
            if (grid == null) 
            {

                return; 
            }

            var gridPosition = grid.GetColumnRow(e.GetPosition(grid));
            //MessageBox.Show(string.Format("Grid location Row: {1} Column: {0}", gridPosition.X, gridPosition.Y));
            if(board != null)
            {
                ShowMoves((int)gridPosition.X, (int)(7.0 - gridPosition.Y), false);
            }
        }

        static public void BringToFront(Grid pParent, UIElement pToMove)
        {
            try
            {
                int currentIndex = Grid.GetZIndex(pToMove);
                int zIndex = 0;
                int maxZ = 0;
                UIElement child;
                for (int i = 0; i < pParent.Children.Count; i++)
                {
                    if (pParent.Children[i] is UIElement &&
                        pParent.Children[i] != pToMove)
                    {
                        child = pParent.Children[i] as UIElement;
                        zIndex = Grid.GetZIndex(child);
                        maxZ = Math.Max(maxZ, zIndex);
                        if (zIndex > currentIndex)
                        {
                            Grid.SetZIndex(child, zIndex - 1);
                        }
                    }
                }
                Grid.SetZIndex(pToMove, maxZ);
            }
            catch (Exception ex)
            {
            }
        }
    }

    public static class GridExtensions
    {
        public static T Parent<T>(this DependencyObject root) where T : class
        {
            if (root is T) { return root as T; }

            DependencyObject parent = VisualTreeHelper.GetParent(root);
            return parent != null ? parent.Parent<T>() : null;
        }

        public static Point GetColumnRow(this Grid obj, Point relativePoint) { return new Point(GetColumn(obj, relativePoint.X), GetRow(obj, relativePoint.Y)); }
        private static int GetRow(Grid obj, double relativeY) { return GetData(obj.RowDefinitions, relativeY); }
        private static int GetColumn(Grid obj, double relativeX) { return GetData(obj.ColumnDefinitions, relativeX); }

        private static int GetData<T>(IEnumerable<T> list, double value) where T : DefinitionBase
        {
            var start = 0.0;
            var result = 0;

            var property = typeof(T).GetProperties().FirstOrDefault(p => p.Name.StartsWith("Actual"));
            if (property == null) { return result; }

            foreach (var definition in list)
            {
                start += (double)property.GetValue(definition);
                if (value < start) { break; }

                result++;
            }

            return result;
        }
    }
}
