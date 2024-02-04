using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace RotaCaelum.Utils
{
    internal class CallUserControl
    {

        public static void addControlToGrid( Grid grid, UserControl userControl)
        {
            if(grid.Children.Count > 0)
            {
                if (grid.Children.OfType<UserControl>().FirstOrDefault().Name != userControl.Name)
                {
                    grid.Children.Clear();
                    grid.Children.Add(userControl);
                }
            }
            else
            {
                grid.Children.Add(userControl);
            }
        }

    }
}
