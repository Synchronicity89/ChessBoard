﻿using ChessEngine;
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
            //for (int i = 0; i < 8; i++)
            //{
            //    if (i < 3 || i >= 5)
            //    {
            //        board.Grid[i, 1] = new Pawn(board, 1, i, 1);
            //        board.Grid[i, 6] = new Pawn(board, -1, i, 6);
            //    }
            //    else
            //    {
            //        board.Grid[i, 3] = new Pawn(board, 1, i, 3);
            //        board.Grid[i, 4] = new Pawn(board, -1, i, 4);
            //    }
            //    if (i == 0 || i == 7)
            //    {
            //        board.Grid[i, 2] = new Rook(board, 1, i, 2);
            //        board.Grid[i, 5] = new Rook(board, -1, i, 5);
            //    }
            //    if (i == 1 || i == 6)
            //    {
            //        board.Grid[i, 0] = new Knight(board, 1, i, 0);
            //        board.Grid[i, 7] = new Knight(board, -1, i, 7);
            //    }
            //    if (i == 2 || i == 5)
            //    {
            //        board.Grid[i, 0] = new Bishop(board, 1, i, 0);
            //        board.Grid[i, 7] = new Bishop(board, -1, i, 7);
            //    }
            //    if (i == 2 || i == 5)
            //    {
            //        board.Grid[i, 4] = new Queen(board, 1, i, 4);
            //        board.Grid[i, 3] = new Queen(board, -1, i, 3);
            //    }
            //    if (i == 4)
            //    {
            //        board.Grid[i, 1] = new King(board, 1, i, 1);
            //        board.Grid[i, 6] = new King(board, -1, i, 6);
            //    }
            //}

            //Draw pieces
            DrawPieces();


            //int col = 1;
            //int row = 0;

            //ShowMoves(col, row);
            //var pawnMoves1 = board.Grid[3, 3].Moves();
            //var knightMoves1 = board.Grid[6, 0].Moves(Constants.KnightMoves);
            //var pawnMoves2 = board.Grid[3, 3].TakeMoves();
            //var knightMoves2 = board.Grid[1, 7].Moves();
            //var pawnMoves3 = board.Grid[1, 4].Moves();
            //var knightMoves3 = board.Grid[6, 7].Moves();
            //var pawnMoves4 = board.Grid[3, 4].TakeMoves();
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
            RemoveUIElements<Rectangle>();
        }

        private void ShowMoves(int col, int row, bool samePosition)
        {
            if (moving > 0)
            {
                --moving;
                return;
            }
            RemoveUIElements<Rectangle>();

            //if (samePosition)
            //{
            //    this.lastHit = new Point(-1, -1);
            //    return;
            //}

            if (board.Grid[col, row] == null) return;
            int[,,] points = board.GetMoves(col, row);
            if(points == null) return;
            var pieceMoves = board.Grid[col, row].Moves(points);
            if (pieceMoves == null) return;

            var xLen = pieceMoves.GetLength(0);
            var yLen = pieceMoves.GetLength(1);
            var zLen = pieceMoves.GetLength(2);
            var knightMovesFlat = new List<Move>();
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    if (pieceMoves[x, y, 0] != null)
                    {
                        knightMovesFlat.Add(pieceMoves[x, y, 0]);
                    }
                }
            }

            foreach (var move in knightMovesFlat)
            {
                var rect = new Rectangle(); rect.Fill = move.Take ? Brushes.Gold : Brushes.Silver;

                Grid.SetColumn(rect, move.X + move.Piece.X);
                Grid.SetRow(rect, 7 - (move.Y + move.Piece.Y));
                rect.Tag = move;
                rect.MouseDown += Rect_MouseDown;
                grdBoard.Children.Add(rect);


            }
            //Not working yet
            //BringImagesToFront();

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
            moving = 2;
            Move tag = (Move)((Rectangle)sender).Tag;
            board.MovePiece(tag);
            RemoveUIElements<Rectangle>();
            DrawPieces();
            --moving;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

        }

        //The rest of the code in this file was copied and pasted from stackoverflow, with few or no changes.
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
            //if((lastHit.X == -1 && lastHit.Y == -1) == false)
            //{ 
            //    lastHit = gridPosition;
            //}
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

        private void grdBoard_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void grdBoard_DragLeave(object sender, DragEventArgs e)
        {

        }
    }

    public static class GridExtentions
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
