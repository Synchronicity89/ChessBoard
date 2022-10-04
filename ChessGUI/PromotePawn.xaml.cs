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
using System.Windows.Shapes;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for PromotePawn.xaml
    /// </summary>
    public partial class PromotePawn : Window
    {
        public char Promote { get; set; }
        public PromotePawn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "Queen") Promote = 'Q';
            if (((Button)sender).Content.ToString() == "Rook") Promote = 'R';
            if (((Button)sender).Content.ToString() == "Bishop") Promote = 'B';
            if (((Button)sender).Content.ToString() == "Knight") Promote = 'N';
            Close();
        }
    }
}
