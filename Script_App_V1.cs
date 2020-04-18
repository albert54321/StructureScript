////////////////////////////////////////////////////////////////////////////////
// Script_APP.cs
//
//Script that generate Structures
//
//
// Copyright (c) 2018 Alberto Alarcon Paredes
// Mgter. Fisica Medica Instituto Balseiro
// Lic. en Física  Universidad Mayor de San Simon
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Reflection;//da el ensambly
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;//da los colores
using System.Windows.Forms;
using System.IO;

// TODO: uncomment the line below if the script requires write access.
[assembly: ESAPIScript(IsWriteable = true)]
[assembly: AssemblyVersion("1.0.0.252")]
[assembly: AssemblyFileVersion("1.0.0.252")]
[assembly: AssemblyCompany("Instituto Zunino")]
[assembly: AssemblyDescription("Creado por Mgter. Alberto Alarcon Paredes")]



namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        public void Execute(ScriptContext context, System.Windows.Window window/*, ScriptEnvironment environment*/)
        {
            string text = File.ReadAllText(@"U:\14-Scripts Eclipse\Nombres_e_Instructivos\Lic_NET_dot\Licence.txt", Encoding.UTF8);
            Structures_Creation text2 = new Structures_Creation();
            if (text != text2.Key) System.Windows.MessageBox.Show("You dont have a valid key/Contraseña invalida.");

            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();
            if (context.StructureSet == null) { throw new ArgumentNullException("Required input StructureSet is not available"); }
            if (context.StructureSet.Structures == null) { throw new ArgumentNullException("Required input Structures is not available"); }
            
            var MainControl = new Script_App_V1.UserScript();//llamo la ventana del mainwindow
            window.Content = MainControl;//le doy propiedades
            window.Width = MainControl.Width;
            window.Height = MainControl.Height;
            window.Title = "Structure Script";
            MainControl.Patients.Content = context.Patient.Name;
            MainControl.ID.Content = context.Patient.Id;
            MainControl.StructSet.Content = context.StructureSet.Id;
            MainControl.ss = ss;//transfiere el paciente actual
            MainControl.sc = context;

            List<Structures_Creation> dqm = Structures_Creation.Script();//lamo a la clase y la inicializo como es una lista 
            foreach (Structures_Creation x in dqm)//puedo llamar elemento de cada lista
            {             
                MainControl.Combo.Items.Add(x.ID );
            }
        }

    }
}