using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace Script_App_V1
{
    /// <summary>
    /// Lógica de interacción para UserScript.xaml
    /// </summary>
    public partial class UserScript : System.Windows.Controls.UserControl
    {
        public UserScript()
        {
            InitializeComponent();
            //Combo.DrawMode = DrawMode.OwnerDrawVariable;
            //string selected = Combo.SelectedItem.ToString();


        }
        public StructureSet ss;
        public ScriptContext sc;
        
        public void apply_button(object sender, RoutedEventArgs e)
            
        {
            VMS.TPS.Structures_Creation xapply = new VMS.TPS.Structures_Creation();
            List<VMS.TPS.Structures_Creation> dqm = VMS.TPS.Structures_Creation.Script();//lamo a la clase y la inicializo como es una lista 
            foreach (VMS.TPS.Structures_Creation x in dqm)
            {
                if (x.ID==Select_1.Content.ToString() ) xapply.Start(x.number, sc ,x.approved);
            }
            
            System.Windows.MessageBox.Show("Process Ending ");
        }

        public void close_button(object sender, RoutedEventArgs e)
        {
            System.Windows.Window.GetWindow(this).Close();//cierra la ventana de la interface main
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select_1.Content = Combo.SelectedItem.ToString();//coloca en el boton de select el valor de lo escogido
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Credits cred = new Credits();
            WindowCredit cred = new WindowCredit(); // llamo la ventana de WPF de creditos
            cred.ShowDialog();
        }

    }
}
