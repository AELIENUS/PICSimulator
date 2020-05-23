using System.Text;
using System.Windows;
using System.Windows.Controls;
using Application.ViewModel;

namespace Application
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if(e.Row.GetIndex() < 16)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("0x");
                if (e.Row.GetIndex() < 10)
                {
                    sb.Append(e.Row.GetIndex());
                }
                else if (e.Row.GetIndex() == 10)
                {
                    sb.Append("A");
                }
                else if (e.Row.GetIndex() == 11)
                {
                    sb.Append("B");
                }
                else if (e.Row.GetIndex() == 12)
                {
                    sb.Append("C");
                }
                else if (e.Row.GetIndex() == 13)
                {
                    sb.Append("D");
                }
                else if (e.Row.GetIndex() == 14)
                {
                    sb.Append("E");
                }
                else
                {
                    sb.Append("F");
                }
                sb.Append("C");
                e.Row.Header = sb.ToString();
            }
            else
            {
                e.Row.Header = "";
            }
        }
    }
}
