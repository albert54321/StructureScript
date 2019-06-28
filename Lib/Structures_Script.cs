////////////////////////////////////////////////////////////////////////////////
// Prostate
//
//  A ESAPI v15.1+ script that demonstrates optimization structure creation.
//
// Applies to:
//      Eclipse Scripting API
//          15.1.1
//          15.5
//
// Copyright (c) 2018 Alberto Alarcon Paredes
// Mgter. Fisica Medica Instituto Balseiro
// Lic. en Física  Universidad Mayor de San Simon
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
/**
* @author $AlbertoAlarcon$
*
* @date - $2018$ 
*/
namespace VMS.TPS//tiene que ser igual que el main
{
    public class Structures_Creation
    {
        public Structures_Creation() { } //esto es para poder invocar a la clase
        public string ID { get; set; }//nombre del script a ejecutar
        public bool approved { get; set; }// si esta aprobado
        public string SCRIPT_NAME { get; set; }//el nombre que muestra en la applicacion
        public int number { get; set; }
        public void VerifSt( Structure st1, bool need,string name)//cambia el nombre y convierte en alta resolucion
        {
            if (st1 == null || st1.IsEmpty)
            {
                System.Windows.MessageBox.Show(string.Format("'{0}' not found!", name), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (need == true)
                {
                    System.Windows.MessageBox.Show("Script doesnt execute");
                }
            }
            else {
                if(name != "Body" && name != "Outer Contour" && st1.CanConvertToHighResolution()) st1.ConvertToHighResolution();
                try
                {
                    if (st1.Id != name) st1.Id = name;
                }
                catch (Exception e)
                {
                    st1.Id = name + ".";
                }
                //if (st1.Id != name) st1.Id = name;              
            }
        }
        public void VerifStLow(Structure st1, bool need, string name)//cambia el nombre y NO convierte en alta resolucion
        {
            if (st1 == null || st1.IsEmpty)
            {
                System.Windows.MessageBox.Show(string.Format("'{0}' not found!", name), SCRIPT_NAME, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (need == true)
                {
                    System.Windows.MessageBox.Show("Script doesnt execute");
                }
            }
            else
            {
                try
                {
                    if (st1.Id != name) st1.Id = name;
                }
                catch (Exception e)
                {
                    st1.Id = name+".";
                }
                
            }
        }
        public bool HighResol(Structure s)//verifica su una estructura es alta resol devuelve a= false si es alta resol
        {
            bool a;
            if (s == null || s.IsEmpty) a = true;
            else
            {
                if (s.IsHighResolution) a = false;
                else a = true;
            }
            return a;
        }
        //La lista de script es aqui
        public static List<Structures_Creation> Script()//ScriptContext sc) //hay que aprobar el script aqui sino no va correr
        {
            List<Structures_Creation> list = new List<Structures_Creation>();
            list.Add(new Structures_Creation//esto es una lista de clases dentro
            {
                ID = "Script_Breast_ChestWall(Mama/Pared/ParedExpansor)",//nombre que aparece en la lista de la interfaz grafica
                approved = true,//si el script esta aprobado o no
                SCRIPT_NAME = "Breast_ChestWall_Structures",//nombre de las advertencias 
                number = 1//nombre del script se enlaza con Start.
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_SBRT_Prostate(SBRT_prostata)",
                approved = true,
                SCRIPT_NAME = "Prostate_Structures",
                number = 0
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_Rectum20Fx(Recto)",
                approved = true,
                SCRIPT_NAME = "Rectum_Structures",
                number = 2
        });
            list.Add(new Structures_Creation
            {
                ID = "Script_Head_Neck25Fx(CYC)",
                approved = true,
                SCRIPT_NAME = "Head_Neck_Structures",
                number = 3
        });
            list.Add(new Structures_Creation
            {
                ID = "Script_Cervix20Fx(CuelloUtero)",
                approved = true,
                SCRIPT_NAME = "Cervix_Structures",
                number = 4
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_Prostate_Combo_HDR_Fx15(Braqui)",
                approved = true,
                SCRIPT_NAME = "Prostate_ComboHDR_Structures",
                number = 5
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_ProstateBed_Fx20(Lecho)",
                approved = true,
                SCRIPT_NAME = "ProstateBed_Structures",
                number = 6
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_Bladder_Fx20(Vejiga)",//saint fausto
                approved = true,
                SCRIPT_NAME = "Bladder_Structures",
                number = 7
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_Endometrium_Fx20(Endometrio)",//saint fausto
                approved = true,
                SCRIPT_NAME = "Bladder_Structures",
                number = 8
            });
            list.Add(new Structures_Creation
            {
                ID = "Script_Liver_Fx3(Higado)",//saint fausto
                approved = true,
                SCRIPT_NAME = "Bladder_Structures",
                number = 9
            });
            return list;
        } //ESTAN LIGADOS EL NUMBER AQUI
        //comienzo del script strutures
        public void start(int template, ScriptContext context, bool appr) {//ejecuta el scrit seleccionado en User script.c   LIGADO CON VALOR DE TEMPLATE
            if (template == 0 && appr) St_Prostate(context);
            else if (template == 1 && appr) St_Breast_ChestWall(context);
            //else if (template == 2 && appr) St_Breast_ChestWall_RA(context);
            else if (template == 2 && appr) St_Rectum20fx(context);
            else if (template == 3 && appr) St_CYC_25fx(context);
            else if (template == 4 && appr) St_Cervix_25fx(context);
            else if (template == 5 && appr) St_HDR_15fx(context);//
            else if (template == 6 && appr) St_Lecho_20fx(context);
            else if (template == 7 && appr) St_Bladder_20fx(context);//
            else if (template == 8 && appr) St_Endometrium_20fx(context);//endometrium
            else if (template == 9 && appr) St_Higado_3fx(context);//endometrium
            else System.Windows.MessageBox.Show("The Script does not approved for clinical use ");
        }// Aqui hay que implementar el nuevo script de estructuras y correlacionar los numeros sino no se ejecuta
        public void Cropbody(Structure st1, Structure body1)
        {
            if (st1 != null && body1 != null && !st1.IsEmpty)   /// Esto es para que 
            {
                st1.SegmentVolume = body1.And(st1);
            }
        }//crop 0.5cm del body

        public void St_Prostate(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_SBRT_Prostate";
            string[] N_Prostate = { "CTV_Prostate",     "CTV_Prostata", "1-Prostata" };
            string[] N_LN = {       "CTV_LN_Obturator", "ganglio","ganglios","obturador"};
            string[] N_VS = {       "CTV_SeminalVes",   "VSem", "Vesiculas", "Vesiculas Sem", "VS+1cm" };
            string[] N_SIB = {      "GTV_SIB",          "_SIB",  "nodulo" };
            string[] N_GP = {       "CTV_LN_Pelvic",    "Pelviano", "Pelvico", "ganglios pelvicos", "RegGanglionares" };
            string[] N_Urethra = {  "Urethra",          "Uretra", "uretra"};
            string[] N_Trigone = {  "Trigone",          "trigono", "Trigono" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino" };
            string[] N_Body = {     "Body",             "Outer Contour", "body" };
            //cambiar nombres
            string[] N_HJL = {      "FemoralJoint_L",   "Hip Joint, Left", "Hip Joint Left", "CFI" };//hip joint left
            string[] N_HJR = {      "FemoralJoint_R",   "Hip Joint, Right", "Hip Joint Right", "CFD" };
            string[] N_Penile = {   "PenileBulb",       "Penile Bulb", "Pene B","penile bulb", "B Pene", "Bulbo" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation); 
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(uretra) || !HighResol(trigono) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifSt(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifSt(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifSt(uretra, true, N_Urethra[0]);//es necesario true
                if (uretra == null) return;
                VerifSt(trigono, true, N_Trigone[0]);//es necesario true
                if (trigono == null) return;
                VerifSt(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifSt(colon, false, N_Colon[0]);//es necesario true
                VerifSt(bowel, false, N_Bowel[0]);//es necesario true
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifStLow(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifStLow(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifStLow(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifStLow(uretra, true, N_Urethra[0]);//es necesario true
                if (uretra == null) return;
                VerifStLow(trigono, true, N_Trigone[0]);//es necesario true
                if (trigono == null) return;
                VerifStLow(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifStLow(colon, false, N_Colon[0]);//es necesario true
                VerifStLow(bowel, false, N_Bowel[0]);//es necesario true
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre voy a crear structuras y cambiar id
            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure penile = ss.Structures.FirstOrDefault(s => N_Penile.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjl, false, N_HJL[0]);
            VerifStLow(hjr, false, N_HJR[0]);
            VerifStLow(penile, false, N_Penile[0]);

            //comienza las estrucuras
            //New Structures 
            const string PTV_ID12 = "PTV_Prostate";
            const string PTV_ID13 = "PTV_LN_Obturator";
            const string PTV_ID14 = "PTV_Urethra!";
            const string PTV_ID15 = "PTV_Trigone!";
            const string PTV_ID16 = "PTV_RectumPRV05!";
            const string PTV_ID17 = "PTV-PRVs!";
            const string PTV_ID18 = "PTV_SeminalVes";//
            const string PTV_ID19 = "PTV_Total!";
            const string PTV_ID20 = "PTV_High_3625";
            const string PTV_ID20_ = "PTV_High_4000";
            const string PTV_ID21 = "PTV_Low_2500";
            const string PTV_ID22 = "PTV_Mid_2750";
            const string PTV_ID23 = "PTV_SIB";
            const string PTV_ID24 = "PTV_LN_Pelvic";
            const string PRV_Rectum = "Rectum_PRV05";
            const string Rect_ant = "Rectum_A";
            const string Rect_post = "Rectum_P";
            const string PRV_colon = "Colon_PRV05";//
            const string PRV_bowel = "Bowel_PRV05";//

            DialogResult result = System.Windows.Forms.MessageBox.Show("Dose prescription is 36.25Gy?"+ "\n" +"If Yes, dose prescription is 36.25Gy" + "\n" +"If No, dose prescription is 40Gy" + "\n" + "If Cancel, End aplication", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Cancel) return;
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);
            if (result == DialogResult.No) ptv_ID20.Id = PTV_ID20_;
            
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures
            
            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);//ptv20 arriba
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);
            Structure ptv_ID24 = ss.AddStructure("PTV", PTV_ID24);
            Structure prv_rectum = ss.AddStructure("CONTROL", PRV_Rectum);
            Structure rect_ant = ss.AddStructure("CONTROL", Rect_ant);
            Structure rect_post = ss.AddStructure("CONTROL", Rect_post);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_colon);
            Structure prv_bowel = ss.AddStructure("CONTROL", PRV_bowel);
            //Structure uretra2 = ss.AddStructure("PTV", "Buffer_U");
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(prv_rectum); St.Add(rect_ant); St.Add(rect_post); St.Add(prv_colon); St.Add(prv_bowel);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            ptv_ID12.SegmentVolume = ctv_ID2.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5));// PTV Prostate asymmetry
            /*uretra2.SegmentVolume = uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 0, 10, 0, 0, 10));//expande urethra 1 cm sup and inf   z1 pies z2 cab pero lo hace mal porque es chico
            uretra2.SegmentVolume = uretra2.Sub(uretra);
            uretra2.SegmentVolume = uretra2.Sub(uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 7, 0, 0, 7, 0)));
            uretra2.SegmentVolume = uretra2.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 4, 0));//quite esto porque dibujaba la uretra mal
            uretra.SegmentVolume = uretra.Or(uretra2);*///era para que la uretra se salga del ptv pero lo hace mal
            ptv_ID20.SegmentVolume = ptv_ID12;///PTV4000 o 3625
            // expand PTV
            if (ctv_ID4 != null)
            {
                ptv_ID18.SegmentVolume = ctv_ID4.AsymmetricMargin(new AxisAlignedMargins(0, 9, 9, 9, 9, 5, 9));// ves sem 9&5
                ptv_ID22.SegmentVolume = ptv_ID18.Sub(ptv_ID12); //PTV2750-3625
            }
            if (ctv_ID3 != null)   /// Evite mistake in structure empty el orden es importante 3 debe estar desp 4
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV reg gang   obturator
                ptv_ID21.SegmentVolume = ptv_ID13.Sub(ptv_ID12); //PTV2500-3625
                if (ctv_ID4 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID18); //PTV2500-27500
            }
            if (ctv_ID6 != null)   /// Evite mistake in structure empty el orden es importante 3 debe estar desp 4
            {
                ptv_ID24.SegmentVolume = ctv_ID6.Margin(6.0); //PTV reg gang   pelvic
                ptv_ID21.SegmentVolume = ptv_ID24.Sub(ptv_ID12); //PTV2500-3625
                if (ctv_ID4 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID18); //PTV2500-27500
            }
            if (ctv_ID5 != null)
            {
                ptv_ID23.SegmentVolume = ctv_ID5.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5)); //PTV sib
            }
            if (colon != null)
            {
                prv_colon.SegmentVolume = colon.Margin(5.0);// PRV colon
            }
            if (bowel != null)
            {
                prv_bowel.SegmentVolume = bowel.Margin(5.0);// PRV colon
            }
            //Structure aux                 = ss.AddStructure("CONTROL", "auxilary");//axilary
            prv_rectum.SegmentVolume = rectum.Margin(5.0);// PRV Rectum
            rect_post.SegmentVolume = rectum.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 17, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            rect_ant.SegmentVolume = rectum.Sub(rect_post);

            ptv_ID14.SegmentVolume = ptv_ID12.And(uretra);//PTV*U
            ptv_ID15.SegmentVolume = ptv_ID12.And(trigono);//PTV*T
            ptv_ID16.SegmentVolume = ptv_ID12.And(prv_rectum);//PTV*PRVV re
            ptv_ID17.SegmentVolume = uretra.Or(trigono);//U+T
            ptv_ID17.SegmentVolume = ptv_ID17.Or(prv_rectum);//U+T+PrvRe
            ptv_ID17.SegmentVolume = ptv_ID12.Sub(ptv_ID17);//PTV pros-(U+T+PrvRe)
            ptv_ID19.SegmentVolume = ptv_ID12.Or(ptv_ID21);///PTV_total           
            ptv_ID19.SegmentVolume = ptv_ID19.Or(ptv_ID18);///PTV_total

            //ss.RemoveStructure(uretra2);
            //Remove strutures null   
            if (ctv_ID6 == null && ctv_ID3 == null) ss.RemoveStructure(ptv_ID21);
            if (ctv_ID3 == null)
            {
                ss.RemoveStructure(ctv_ID3);
                ss.RemoveStructure(ptv_ID13);
                //ss.RemoveStructure(ptv_ID18);
            }
            if (ctv_ID6 == null)
            {
                ss.RemoveStructure(ctv_ID6);
                ss.RemoveStructure(ptv_ID24);
                //ss.RemoveStructure(ptv_ID18);
            }
            if (ctv_ID4 == null)
            {
                ss.RemoveStructure(ctv_ID4);
                ss.RemoveStructure(ptv_ID18);
                ss.RemoveStructure(ptv_ID22);
            }
            if (ctv_ID5 == null)
            {
                ss.RemoveStructure(ctv_ID5);
                ss.RemoveStructure(ptv_ID23);
            }
            if (bowel == null)
            {
                ss.RemoveStructure(bowel);
                ss.RemoveStructure(prv_bowel);
            }
            if (colon == null)
            {
                ss.RemoveStructure(colon);
                ss.RemoveStructure(prv_colon);
            }
        }
        
        public void St_Breast_ChestWall(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_Breast_ChestWall";
            string[] N_Breast = { "zCTV_Mama",      "1-Mama",   "CTV_Breast",       "zCTV_Mama_I", "zCTV_Mama_D" };
            string[] N_LNI =    { "CTV_GL_Axila_1", "2-Ax I",   "CTV_LN_Ax_L1",     "Axila I" };
            string[] N_LNII =   { "CTV_GL_Axila_2", "3-Ax II",  "CTV_LN_Ax_L2",     "Axila II" };
            string[] N_LNIII =  { "CTV_GL_Axila_3", "4-Ax III", "CTV_LN_Ax_L3",     "Axila III" };
            string[] N_Rotter = { "CTV_GL_Rotter",  "5-Rotter", "CTV_LN_Rotter",    "7 Rotter" };
            string[] N_Sclav =  { "CTV_GL_Supra",   "6-Supra",  "CTV_LN_Sclav",     "6-SUPRA" };
            string[] N_IMN =    { "CTV_GL_CMI",     "7-CMI",    "CTV_LN_IMN" };
            string[] N_Dist =   { "CTV_Mama_Dist",  "8-MDISTAL", "CTV_Breast_Dist",  "Distal", "10- CTV distal", "zCTV_Mama_Dist" };
            string[] N_Prox =   { "CTV_Mama_Prox",  "9-MPROX",   "CTV_Breast_Prox",  "Proximal", "9 CTV proximal", "zCTV_Mama_Prox" };
            string[] N_SIB =    { "GTV_SIB",        "10-SIB",    "8 SIB" };
            string[] N_Chest =  { "CTV_Pared",      "1-Pared",   "CTV_Chestwall",    "Pared", "zCTV_Pared" };
            //bad names
            string[] N_Body = { "Body",     "Outer Contour", "body" };
            string[] N_SC = { "MedulaEspinal", "SpinalCord", "Spinal Cord", "Spinal, Cord","medula" };
            string[] N_LL = { "Pulmon_I", "Lung_L",     "Lung Left", "Lung, Left" };
            string[] N_LR = { "Pulmon_D", "Lung_R",     "Lung Right", "Lung, Right" };
            string[] N_Es = { "Esofago","Esophagus" };
            string[] N_BR = { "Mama_Der","Breast_R",   "MDer", "MD" };
            string[] N_BL = { "Mama_I", "Breast_L",   "MIzq", };
            string[] N_Tr = { "Traquea", "Trachea",  "traquea" };//aumentar corazon
            string[] N_Cor = { "Corazon", "Heart", "corazon" };//aumentar corazon
            string[] N_Intes = { "Intestino", "Bowel", "corazon" };//aumentar corazon
            /*//names for original
            string NPROX    = "CTV_Mama_Prox-05";
            string NDIST    = "CTV_Mama_Dist-05";
            string NSIB     = "GTV_SIB-05";*/
            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            //bool resol=ss.Structures. Any(x => x.IsHighResolution); //encuentra un elemeto de alta resolucion el problema esta que te encuentra de todo
            
            DialogResult result = System.Windows.Forms.MessageBox.Show("Breast or Chest wall or Prosthesis?" + "\n" + "If Yes, the volume is Breast(Mama)." + "\n" + "If No, the volume is Chest wall(Pared)." + "\n" + "If Cancel, the volume is chestwall with expander(Expansor).", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            
            Structure ctv_ID11 = ss.Structures.FirstOrDefault(s => N_Chest.Any(x => s.Id.Contains(x)));
            if (result == DialogResult.No || result == DialogResult.Cancel) //esto no lo convierto en alta resolucion para que haga la superficie, despues si
            {
                if (ctv_ID11 == null)
                {
                    System.Windows.MessageBox.Show(string.Format("'{0}' not found!", N_Chest[0]), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;//es la unica por el momento para terminar la aplicacion
                }
                else ctv_ID11.Id = N_Chest[0];
            }
            //Crea las estructuras necesarias
            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_LNI.Any(x => s.Id.Contains(x)));
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LNII.Any(x => s.Id.Contains(x)));
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_LNIII.Any(x => s.Id.Contains(x)));
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_Rotter.Any(x => s.Id.Contains(x)));
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_Sclav.Any(x => s.Id.Contains(x)));
            Structure ctv_ID7 = ss.Structures.FirstOrDefault(s => N_IMN.Any(x => s.Id.Contains(x)));
            Structure ctv_ID8 = ss.Structures.FirstOrDefault(s => N_Dist.Any(x => s.Id.Contains(x)));
            Structure ctv_ID9 = ss.Structures.FirstOrDefault(s => N_Prox.Any(x => s.Id.Contains(x)));
            Structure ctv_ID10 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));
            
            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2)|| !HighResol(ctv_ID3)|| !HighResol(ctv_ID4)|| !HighResol(ctv_ID5)|| !HighResol(ctv_ID6)|| !HighResol(ctv_ID7)|| !HighResol(ctv_ID8)|| !HighResol(ctv_ID9)|| !HighResol(ctv_ID10) || !HighResol(ctv_ID11))
            {
                Low = false;
                //VerifSt(ctv_ID1, true, N_Breast[0]);
                VerifSt(ctv_ID2, false, N_LNI[0]);
                VerifSt(ctv_ID3, false, N_LNII[0]);
                VerifSt(ctv_ID4, false, N_LNIII[0]);
                VerifSt(ctv_ID5, false, N_Rotter[0]);
                VerifSt(ctv_ID6, false, N_Sclav[0]);
                VerifSt(ctv_ID7, false, N_IMN[0]);
                if (result == DialogResult.Yes) VerifSt(ctv_ID8, true, N_Dist[0]);
                if (result == DialogResult.Yes && ctv_ID8 == null) return;

                if (result == DialogResult.Yes) VerifSt(ctv_ID9, true, N_Prox[0]);
                if (result == DialogResult.Yes && ctv_ID9 == null) return;

                if (result == DialogResult.Yes) VerifSt(ctv_ID10, true, N_SIB[0]);
                if (result == DialogResult.Yes && ctv_ID10 == null) return;

                //if (result == DialogResult.No || result==DialogResult.Cancel) VerifSt(ctv_ID11, true, N_Chest[0]);
                //if (result == DialogResult.No && ctv_ID11 == null) return;
            }
            else
            {
                Low = true;
                //VerifStLow(ctv_ID1, true, N_Breast[0]);
                VerifStLow(ctv_ID2, false, N_LNI[0]);
                VerifStLow(ctv_ID3, false, N_LNII[0]);
                VerifStLow(ctv_ID4, false, N_LNIII[0]);
                VerifStLow(ctv_ID5, false, N_Rotter[0]);
                VerifStLow(ctv_ID6, false, N_Sclav[0]);
                VerifStLow(ctv_ID7, false, N_IMN[0]);
                if (result == DialogResult.Yes) VerifStLow(ctv_ID8, true, N_Dist[0]);
                if (result == DialogResult.Yes && ctv_ID8 == null) return;

                if (result == DialogResult.Yes) VerifStLow(ctv_ID9, true, N_Prox[0]);
                if (result == DialogResult.Yes && ctv_ID9 == null) return;

                if (result == DialogResult.Yes) VerifStLow(ctv_ID10, true, N_SIB[0]);
                if (result == DialogResult.Yes && ctv_ID10 == null) return;

                //if (result == DialogResult.No || result==DialogResult.Cancel) VerifSt(ctv_ID11, true, N_Chest[0]);
                //if (result == DialogResult.No && ctv_ID11 == null) return;
                if (result == DialogResult.No || result == DialogResult.Cancel) //esto no lo convierto en alta resolucion para que haga la superficie, despues si
                {
                    VerifStLow(ctv_ID11, true, N_Chest[0]);
                    if (ctv_ID11 == null) return;
                }

            }
            Structure ctv_ID1 = ss.Structures.FirstOrDefault(s => N_Breast.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //if (result == DialogResult.Yes) VerifSt(ctv_ID1, true, N_Breast[0]);//es necesario true
            if (result == DialogResult.Yes && ctv_ID1 == null)// esto no lo convierto en alta resolucion para que haga la superficie, despues si
            {
                System.Windows.MessageBox.Show(string.Format("'{0}' not found!", N_Breast[0]), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;//es la unica por el momento para terminar la aplicacion
            }
            else if (result == DialogResult.Yes && ctv_ID1 != null) ctv_ID1.Id = N_Breast[0];

            //if (result == DialogResult.Cancel && ctv_ID11 == null) return;
            Structure body0 = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));
            VerifSt(body0, true, N_Body[0]);

            //solo cambia nombre
            Structure sc = ss.Structures.FirstOrDefault(s => N_SC.Any(x => s.Id.Contains(x)));
            VerifStLow(sc, false, N_SC[0]);
            Structure ll = ss.Structures.FirstOrDefault(s => N_LL.Any(x => s.Id.Contains(x)));
            VerifStLow(ll, false, N_LL[0]);
            Structure lr = ss.Structures.FirstOrDefault(s => N_LR.Any(x => s.Id.Contains(x)));
            VerifStLow(lr, false, N_LR[0]);
            Structure es = ss.Structures.FirstOrDefault(s => N_Es.Any(x => s.Id.Contains(x)));
            VerifStLow(es, false, N_Es[0]);
            Structure br = ss.Structures.FirstOrDefault(s => N_BR.Any(x => s.Id.Contains(x)));
            VerifStLow(br, false, N_BR[0]);
            Structure bl = ss.Structures.FirstOrDefault(s => N_BL.Any(x => s.Id.Contains(x)));
            VerifStLow(bl, false, N_BL[0]);
            Structure tr = ss.Structures.FirstOrDefault(s => N_Tr.Any(x => s.Id.Contains(x)));
            VerifStLow(tr, false, N_Tr[0]);
            Structure cor = ss.Structures.FirstOrDefault(s => N_Cor.Any(x => s.Id.Contains(x)));
            VerifStLow(tr, false, N_Cor[0]);

            DialogResult result2 = System.Windows.Forms.MessageBox.Show("Fraction: 16Fx or 20Fx?" + "\n" + "If Yes, the volume is 16Fx." + "\n" + "If No, the volume is 20Fx." + "\n" + "If Cancel, Stop Script", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Cancel) return;

            DialogResult result3 = System.Windows.Forms.MessageBox.Show("Breast ChestWall Start/RA?" + "\n" + " If Yes is Start (Mama/Pared Inicio)" + "\n" + "If No, is RA (Mama/Pared RA)" + "\n" + "If Cancel, Stop Script", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result3 == DialogResult.Cancel) return;
            /*else if (result3 == DialogResult.No && result == DialogResult.Yes) esto era para colocar -05 pero no fue viable
            {
                ctv_ID8.Id  = NDIST;//coloca los nombres -5
                ctv_ID9.Id  = NPROX;
                ctv_ID10.Id = NSIB;
            }*/
            //New Structures Breast 16fx
            const string PTV_ID12 = "PTV_GL_Axila_1";//"PTV_LN_Ax_L1";     //"PTV_Ax I"
            const string PTV_ID13 = "PTV_GL_Axila_2";//"PTV_LN_Ax_L2";     //"PTV_Ax II"
            const string PTV_ID14 = "PTV_GL_Axila_3";//"PTV_LN_Ax_L3";     //"PTV_Ax III"
            const string PTV_ID15 = "PTV_GL_Rotter";//"PTV_LN_Rotter";    //"PTV_Rotter"
            const string PTV_ID16 = "PTV_GL_Supra";//"PTV_LN_Supra";//"PTV_LN_Sclav";     //"PTV_Supra"
            const string PTV_ID17 = "PTV_GL_CMI";//"PTV_LN_CMI"; //"PTV_LN_IMN";       //"PTV_CMI"
            const string PTV_ID18 = "PTV_Mama_Dist";//"PTV_Breast_Dist";  //"PTV_MDISTAL"
            const string PTV_ID19 = "PTV_Mama_Prox";//"PTV_Breast_Prox";  //"PTV_MPROX"
            const string PTV_ID_20 = "PTV_GTV_SIB";//"PTV_GTV_SIB";     //"PTV_SIB"
            const string PTV_ID20 = "zPTV_High_5200!";  //"PTV_52Gy"
            const string PTV_ID21 = "zPTV_Low_4000!";   //PTV_40Gy
            const string PTV_ID22 = "zPTV_Mid_4100!";   //PTV_41Gy
            const string PTV_ID23 = "zPTV_Mid_4320!";   //PTV_43.2Gy
            const string PTV_ID24 = "zPTV_Total!";      //PTV_Total
            const string Ring = "zAnillo";//"zRing";                //Anillo
            const string Surface = "zSuperficie";//"zSurface";          //Superficie

            //20fx Breast
            const string PTV_ID20_ = "zPTV_High_5640!"; //PTV_56.4Gy//tengo un problema con los IDS por eso le quito el signo de admiracion
            const string PTV_ID21_ = "zPTV_Low_4300!";  //PTV_43Gy
            const string PTV_ID22_ = "zPTV_Mid_4600!";  //PTV_46Gy
            const string PTV_ID23_ = "zPTV_Mid_4540!";  //PTV_45.4Gy

            //Chest wall 16fx
            const string PTV_ID25 = "zPTV_High_4400!";  //PTV_44Gy

            //Chest wall 20fx
            const string PTV_ID25_ = "zPTV_High_4700"; //PTV_47Gy//problema con el id
            //const string PTV_ID26_ = "zPTV_Mid_4600!";  //PTV_46Gy
            const string PTV_ID27 = "PTV_Pared";//"PTV_Chestwall";  //PTV pared


            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);
            Structure ptv_ID_20 = ss.AddStructure("PTV", PTV_ID_20);
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);
            Structure ptv_ID24 = ss.AddStructure("PTV", PTV_ID24);
            Structure ptv_ID25 = ss.AddStructure("PTV", PTV_ID25);
            Structure ptv_ID27 = ss.AddStructure("PTV", PTV_ID27);
            Structure ring = ss.AddStructure("CONTROL", Ring);
            Structure surface = ss.AddStructure("AVOIDANCE", Surface);
            Structure buffered_outer = ss.AddStructure("CONTROL", "outer-5");
            Structure ctv_ID_1 = ss.AddStructure("CTV", "Breast");//Mama MD o MI
            Structure body = ss.AddStructure("CONTROL", "High_Body");
            body.SegmentVolume = body0;
            int is_high = 0;// esto  me sirve para hacer la superficie sino es alta resolucion es 0 si es es 1
            if (!(ctv_ID1 == null))
            {
                if (result3 == DialogResult.Yes && !ctv_ID1.IsHighResolution)
                {
                    surface.SegmentVolume = ctv_ID1.Margin(12.0);
                    surface.SegmentVolume = surface.Sub(body0);
                    surface.SetAssignedHU(0.0);  //asigno UH de agua
                    body0.SegmentVolume = body0.Or(surface);//Union con el body de la superficie
                }
                else if (ctv_ID1.IsHighResolution)
                {
                    is_high = 1;
                    ctv_ID1.Id = N_Breast[0];
                }
            }
            else if (!(ctv_ID11 == null))
            {
                if (result3 == DialogResult.Yes && !ctv_ID11.IsHighResolution)
                {
                    surface.SegmentVolume = ctv_ID11.Margin(12.0);
                    surface.SegmentVolume = surface.Sub(body0);
                    surface.SetAssignedHU(0.0);  //asigno UH de agua
                    body0.SegmentVolume = body0.Or(surface);//Union con el body de la superficie
                }
                else if (ctv_ID11.IsHighResolution) is_high = 2;
            }

            if (!Low)//si no es alta resolucion crea todo esto
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20); St.Add(ptv_ID_20); St.Add(ptv_ID25); St.Add(ptv_ID27);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(surface); St.Add(ctv_ID_1); St.Add(body); //St.Add(buffered_outer); St.Add(ring); 
                if (ctv_ID1 != null && !ctv_ID1.IsHighResolution) St.Add(ctv_ID1);// necesito colocar el null primero porque si no me da error cuando aplico ishihresolution a una estructura vacia
                if (ctv_ID11 != null && !ctv_ID11.IsHighResolution) St.Add(ctv_ID11);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            

            buffered_outer.SegmentVolume = body.Margin(-5.0);//para cropp de los elementos con el body -0.5

            if (result == DialogResult.Yes)
            {
                ctv_ID_1.SegmentVolume = ctv_ID_1.Or(ctv_ID8);
                ctv_ID_1.SegmentVolume = ctv_ID_1.Or(ctv_ID9);
                ctv_ID_1.SegmentVolume = ctv_ID_1.Or(ctv_ID10);
                if (Math.Abs(ctv_ID_1.Volume - ctv_ID1.Volume) > 20.0)
                {
                    System.Windows.MessageBox.Show(string.Format("'{0}' is different to union of Distal + Prox + SIB. Fix them! Script doesnt execute", ctv_ID1.Id), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            // expand PTV
            if (ctv_ID2 != null) ptv_ID12.SegmentVolume = ctv_ID2.Margin(5.0); /// Esto es para que no haga ptv 
            if (ctv_ID3 != null) ptv_ID13.SegmentVolume = ctv_ID3.Margin(5.0);
            if (ctv_ID4 != null) ptv_ID14.SegmentVolume = ctv_ID4.Margin(5.0);
            if (ctv_ID5 != null) ptv_ID15.SegmentVolume = ctv_ID5.Margin(5.0);
            if (ctv_ID6 != null) ptv_ID16.SegmentVolume = ctv_ID6.Margin(5.0);
            if (ctv_ID7 != null) ptv_ID17.SegmentVolume = ctv_ID7.Margin(5.0);
            if (ctv_ID8 != null) ptv_ID18.SegmentVolume = ctv_ID8.Margin(5.0);//40Gy
            if (ctv_ID9 != null) ptv_ID19.SegmentVolume = ctv_ID9.Margin(5.0);//43.2Gy
            if (ctv_ID10 != null)
            {
                ptv_ID20.SegmentVolume = ctv_ID10.Margin(5.0); ptv_ID_20.SegmentVolume = ctv_ID10.Margin(5.0);//52Gy//52Gy
            }
            if (ctv_ID11 != null)
            {
                ptv_ID25.SegmentVolume = ctv_ID11.Margin(5.0); ptv_ID27.SegmentVolume = ctv_ID11.Margin(5.0);//pared//pared
            } 
            
            //Union lymph nodes
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID12);
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID13);
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14);//41Gy46
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID15);//41Gy46
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID16);//41Gy46
            ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID17);//41Gy46
            //if (!ptv_ID22.IsEmpty && !ptv_ID20.IsEmpty &&) ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID20);//41Gy-52Gy
                                                                                                        /* string message = string.Format("{0} volume = {1}\n{2} volume = {3}",
                                                                                                         ptv_ID17.Id, ptv_ID17.Volume, ptv_ID22.Id, ptv_ID22.Volume);
                                                                                                         MessageBox.Show(message);
                                                                                                         */
            //Non overlapping
            if (result == DialogResult.Yes)
            {
                ptv_ID21.SegmentVolume = ptv_ID18.Sub(ptv_ID19);//40Gy-43.2Gy 43-45.4
                ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID20);//40Gy-52Gy 43-56.4
                ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID22);//40Gy-41Gy 43-46
                ptv_ID23.SegmentVolume = ptv_ID19.Sub(ptv_ID20);//43.2Gy-52Gy 45.4-56.4
            }

            if (result == DialogResult.Yes && !ptv_ID22.IsEmpty)
            {
                if (result2==DialogResult.Yes) ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23);//41-43.2 46-45.4///
                else if(result2 == DialogResult.No) ptv_ID23.SegmentVolume = ptv_ID23.Sub(ptv_ID22);//45.4-46///
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID20);//41-52 46-56.4

            }
            else if (result == DialogResult.No && !ptv_ID22.IsEmpty)
            {
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID25);//41-44  46-47
                
            }
            else if (result == DialogResult.Cancel  && !ptv_ID22.IsEmpty)
            {
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID25);//41-44
            }
            //PTV Total
            if (result==DialogResult.Yes) //no se puede unir dos elementos vacios en alta resolucion??
            {
                ptv_ID24.SegmentVolume = ptv_ID20.Or(ptv_ID21);
                ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID22);
                ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID23);
            }
            else if (result == DialogResult.No || result==DialogResult.Cancel)
            {
                ptv_ID24.SegmentVolume = ptv_ID22.Or(ptv_ID25);//pared
            }
            
            Structure buffered_ring = ss.AddStructure("AVOIDANCE", "b_ring");//luego se quita
            if (result3 == DialogResult.Yes)
            {
                if (is_high == 1 || is_high == 2)// solo entra aqui cuando no pudo hacerlo porque breast, chestwall eran alta resulcuion
                {
                    if (result == DialogResult.Yes) surface.SegmentVolume = ctv_ID1.Margin(12.0);
                    else surface.SegmentVolume = ctv_ID11.Margin(12.0);
                    surface.SegmentVolume = surface.Sub(body);
                    surface.SetAssignedHU(0.0);  //asigno UH de agua
                    body.SegmentVolume = body.Or(surface);//Union con el body de la superficie*/
                }

                //anillo
                ring.SegmentVolume = ptv_ID24.Margin(30.0);

                buffered_ring.SegmentVolume = ptv_ID24.Margin(15.0);

                ring.SegmentVolume = ring.Sub(buffered_ring);
                ring.SegmentVolume = ring.And(body);
            }
            else if (result3 == DialogResult.No)
            {

                Cropbody(ctv_ID1, buffered_outer);//no importa que sean nullo
                Cropbody(ctv_ID2, buffered_outer);
                Cropbody(ctv_ID3, buffered_outer);
                Cropbody(ctv_ID4, buffered_outer);
                Cropbody(ctv_ID5, buffered_outer);
                Cropbody(ctv_ID6, buffered_outer);
                Cropbody(ctv_ID7, buffered_outer);
                Cropbody(ctv_ID8, buffered_outer);
                Cropbody(ctv_ID9, buffered_outer);
                Cropbody(ctv_ID10, buffered_outer);
                //Cropbody(ctv_ID11, buffered_outer);

                Cropbody(ptv_ID12, buffered_outer);
                Cropbody(ptv_ID13, buffered_outer);
                Cropbody(ptv_ID14, buffered_outer);
                Cropbody(ptv_ID15, buffered_outer);
                Cropbody(ptv_ID16, buffered_outer);
                Cropbody(ptv_ID17, buffered_outer);
                Cropbody(ptv_ID18, buffered_outer);
                Cropbody(ptv_ID19, buffered_outer);
                Cropbody(ptv_ID20, buffered_outer);
                Cropbody(ptv_ID_20, buffered_outer);
                Cropbody(ptv_ID21, buffered_outer);
                Cropbody(ptv_ID22, buffered_outer);
                Cropbody(ptv_ID23, buffered_outer);
                if (result == DialogResult.Yes) Cropbody(ptv_ID24, buffered_outer);
                else if (result == DialogResult.No)
                {
                    ptv_ID25.SegmentVolume = body.And(ptv_ID25);// no falta el ptv27
                    ptv_ID27.SegmentVolume = body.And(ptv_ID27);
                    ptv_ID24.SegmentVolume = ptv_ID22.Or(ptv_ID25);//pared

                }
                else if (result == DialogResult.Cancel)
                {
                    Cropbody(ptv_ID25, buffered_outer);
                    Cropbody(ptv_ID27, buffered_outer);
                    ptv_ID24.SegmentVolume = ptv_ID22.Or(ptv_ID25);//pared
                }
            }

            //Remove Auxilary Structure      
            ss.RemoveStructure(buffered_ring);
            ss.RemoveStructure(ctv_ID_1); 
            ss.RemoveStructure(buffered_outer);
            ss.RemoveStructure(body);
            if (result3 == DialogResult.No)
            {
                ss.RemoveStructure(ring);
                ss.RemoveStructure(surface);
            }

            //ver si es de 16 o 20 FX
            if (result2 == DialogResult.No)
            {
                if (result3 == DialogResult.Yes)
                {
                    ptv_ID20.Id = PTV_ID20_;
                    ptv_ID21.Id = PTV_ID21_;
                    ptv_ID22.Id = PTV_ID22_;//por defecto esta en fx16
                    ptv_ID23.Id = PTV_ID23_;
                    ptv_ID25.Id = PTV_ID25_;
                }
                else if(result3==DialogResult.No)//lo coloco asi porque sino me sale que ya existe el id
                {
                    ptv_ID20.Id = PTV_ID20_+".";
                    ptv_ID21.Id = PTV_ID21_ + ".";
                    ptv_ID22.Id = PTV_ID22_ + ".";//por defecto esta en fx16
                    ptv_ID23.Id = PTV_ID23_ + ".";
                    ptv_ID25.Id = PTV_ID25_ + ".";
                }
                /*try
                {
                    ptv_ID20.Id = PTV_ID20_;
                    ptv_ID21.Id = PTV_ID21_;
                    ptv_ID22.Id = PTV_ID22_;//por defecto esta en fx16
                    ptv_ID23.Id = PTV_ID23_;
                    ptv_ID25.Id = PTV_ID25_;
                }
                catch {  }
                */
            }


            //Remove strutures null     
            if (ctv_ID2 == null)      /// Esto es para que 
            {
                ss.RemoveStructure(ptv_ID12);
            }
            if (ctv_ID3 == null)
            {
                ss.RemoveStructure(ptv_ID13);
            }
            if (ctv_ID4 == null)
            {
                ss.RemoveStructure(ptv_ID14);
            }
            if (ctv_ID5 == null)
            {
                ss.RemoveStructure(ptv_ID15);
            }
            if (ctv_ID6 == null)
            {
                ss.RemoveStructure(ptv_ID16);
            }
            if (ctv_ID7 == null)
            {
                ss.RemoveStructure(ptv_ID17);
            }
            //retirar si es mama
            if (ctv_ID11 == null)
            {
                ss.RemoveStructure(ptv_ID25);
                ss.RemoveStructure(ptv_ID27);
            }
            if (ctv_ID8 == null)
            {
                ss.RemoveStructure(ptv_ID18);
                ss.RemoveStructure(ptv_ID21);

            }
            if (ctv_ID9 == null)
            {
                ss.RemoveStructure(ptv_ID19);
                ss.RemoveStructure(ptv_ID23);
            }
            if (ctv_ID10 == null)
            {
                ss.RemoveStructure(ptv_ID20);
                ss.RemoveStructure(ptv_ID_20);
            }

            if ((ctv_ID2 == null) && (ctv_ID3 == null) && (ctv_ID4 == null) && (ctv_ID5 == null) && (ctv_ID6 == null) && (ctv_ID7 == null))
            {
                ss.RemoveStructure(ptv_ID22);
            }
            if (result3 == DialogResult.Yes)
            {
                if (is_high == 1 || is_high == 2)// solo entra aqui cuando no pudo hacerlo porque breast, chestwall eran alta resulcuion
                {
                    System.Windows.MessageBox.Show(string.Format("Dont forget to join body with surface!\n (Unir Contorno externo con la superficie)"), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            if (result==DialogResult.Yes && result3 == DialogResult.No) ctv_ID1.Id = "CTV_Mama";

        }

        public void St_Rectum20fx(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_Rectum20Fx";
            //ctv
            string[] N_SIB = {  "GTV_SIB",          "_SIB"};
            string[] N_Lat = {  "CTV_LN_Lateral",   "Laterales", "laterales" };
            string[] N_Meso = { "CTV_Mesorectum",   "Mesorecto", "mesorecto" };
            string[] N_Inf = {  "CTV_Inf_Rectum",   "Inferior", "inferior" };
            string[] N_Pre = {  "CTV_LN_Presacra",  "Presacro", "presacro" };
            string[] N_Ing = {  "CTV_LN_Inguinal",  "Inguinal" };
            //oar
            string[] N_Colon = {"Colon",            "colon","sigma" };
            string[] N_Bladder={"Bladder",          "Vejiga","vejiga" };
            string[] N_Bowel = {"Bowel",            "Intestino", "intestino","intestinos" };
            string[] N_Body = { "Body",             "Outer Contour" };
            //cambiar nombre
            string[] N_Prostate = {"Prostate",      "Prostata","prostata" };
            string[] N_Penile = { "PenileBulb",     "Penile Bulb", "Pene B", "penile bulb", "B Pene", "Bulbo", "bulbo peneano" };
            string[] N_GM = { "Gluteus_Maximus",    "Gluteo Mayor", "gluteos" };
            string[] N_GS = { "Gluteal_Skin",       "Piel Glutea", "pielG", "Piel glutea","piel" };
            string[] N_HJL = { "FemoralJoint_L",    "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR = { "FemoralJoint_R",    "Hip Joint, Right", "Hip Joint Right",  "CFD" };
            string[] N_SC = { "SpinalCord",         "Spinal Cord", "Spinal, Cord" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_Lat.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_Meso.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_Inf.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_Pre.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.FirstOrDefault(s => N_Ing.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OAR
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bladder = ss.Structures.FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver        
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            


            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(ctv_ID7) || !HighResol(colon) || !HighResol(bladder) || !HighResol(bowel) )
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_SIB[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_Lat[0]);//es necesario true
                VerifSt(ctv_ID4, false, N_Meso[0]);//es necesario true
                VerifSt(ctv_ID5, false, N_Inf[0]);//es necesario true
                VerifSt(ctv_ID6, false, N_Pre[0]);//es necesario true
                VerifSt(ctv_ID7, false, N_Ing[0]);//es necesario true
                VerifSt(colon, false, N_Colon[0]);
                VerifSt(bladder, false, N_Bladder[0]);//es necesario true
                VerifSt(bowel, false, N_Bowel[0]);//es necesario true
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_SIB[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_Lat[0]);//es necesario true
                VerifStLow(ctv_ID4, false, N_Meso[0]);//es necesario true
                VerifStLow(ctv_ID5, false, N_Inf[0]);//es necesario true
                VerifStLow(ctv_ID6, false, N_Pre[0]);//es necesario true
                VerifStLow(ctv_ID7, false, N_Ing[0]);//es necesario true
                VerifStLow(colon, false, N_Colon[0]);
                VerifStLow(bladder, false, N_Bladder[0]);//es necesario true
                VerifStLow(bowel, false, N_Bowel[0]);//es necesario true
            }

            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(body, false, N_Body[0]);//es necesario true
            
            //solo cambia nombre
            Structure pros = ss.Structures.FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));
            VerifStLow(pros, false, N_Prostate[0]);

            Structure penile = ss.Structures.FirstOrDefault(s => N_Penile.Any(x => s.Id.Contains(x)));
            VerifStLow(penile, false, N_Penile[0]);

            Structure gm = ss.Structures.FirstOrDefault(s => N_GM.Any(x => s.Id.Contains(x)));
            VerifStLow(gm, false, N_GM[0]);

            Structure gs = ss.Structures.FirstOrDefault(s => N_GS.Any(x => s.Id.Contains(x)));
            VerifStLow(gs, false, N_GS[0]);

            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));
            VerifStLow(hjl, false, N_HJL[0]);

            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));
            VerifStLow(hjr, false, N_HJR[0]);

            Structure sc = ss.Structures.FirstOrDefault(s => N_SC.Any(x => s.Id.Contains(x)));
            VerifStLow(sc, false, N_SC[0]);

            const string PTV_ID12 = "PTV_SIB";
            const string PTV_ID13 = "PTV_LN_Lateral";
            const string PTV_ID14 = "PTV_Mesorectum";
            const string PTV_ID15 = "PTV_InfRectum";
            const string PTV_ID16 = "PTV_LN_Presacral";
            const string PTV_ID17 = "PTV_LN_Inguinal";
            const string PTV_ID21 = "zPTV_Low_4600!";
            const string PTV_ID22 = "zPTV_Mid_4800!";
            const string PTV_ID22_ = "zPTV_Low_4800!";
            const string PTV_ID23 = "zPTV_High_5200!";
            const string PTV_ID24 = "zPTV_High_5400!";
            const string PTV_ID25 = "zPTV_High_5900!";
            //const string PTV_ID26 = "zPTV_Low_4900!";
            const string PTV_ID27 = "zPTV_Total!";
            const string PRV_Colon = "Colon_PRV05";//
            const string PRV_Bowel = "Bowel_PRV05";//

            DialogResult result = System.Windows.Forms.MessageBox.Show("OPtions:" + "\n" + "1.-If Yes, SIB of 52Gy" + "\n" +
    "2.-If No, SIB of 54Gy" + "\n" + "3.-If Cancel, SIB of 59Gy", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            DialogResult result2 = System.Windows.Forms.MessageBox.Show("Do you continue?", SCRIPT_NAME0, MessageBoxButtons.YesNo);
            if (result2 == DialogResult.No) return;
            //============================
            // GENERATE  expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);//ptv sib todos
            Structure ptv_ID27 = ss.AddStructure("PTV", PTV_ID27);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_Colon);
            Structure prv_intestino = ss.AddStructure("CONTROL", PRV_Bowel);
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID27); St.Add(prv_colon); St.Add(prv_intestino);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }

            ptv_ID12.SegmentVolume = ctv_ID2.Margin(9.0); //PTV sib
            ptv_ID23.SegmentVolume = ptv_ID12;//ptv 52gy
            if (ctv_ID3 != null) ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV latera
            //else ss.RemoveStructure(ctv_ID3); ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 != null) ptv_ID14.SegmentVolume = ctv_ID4.Margin(9.0); //PTV mesorecto
            //else ss.RemoveStructure(ctv_ID4); ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 != null) ptv_ID15.SegmentVolume = ctv_ID5.Margin(9.0); //PTV inf
            if (ctv_ID6 != null) ptv_ID16.SegmentVolume = ctv_ID6.Margin(6.0); //PTV presacral
            if (ctv_ID7 != null) ptv_ID17.SegmentVolume = ctv_ID7.Margin(6.0); //PTV inguinal

            if (!ptv_ID13.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID13);                //union ptv 48   22=48
            if (!ptv_ID14.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14);
            if (!ptv_ID15.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID15);
            if (!ptv_ID16.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID16);
            ptv_ID21.SegmentVolume = ptv_ID17;//ptv46gy
            if (!ptv_ID22.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23);//48-52
            if (!ptv_ID21.IsEmpty) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID22);//46-48

            //PTV total
            ptv_ID27.SegmentVolume = ptv_ID23;// ptv total = sib
            if (!ptv_ID22.IsEmpty) ptv_ID27.SegmentVolume = ptv_ID27.Or(ptv_ID22);//58.4+48
            if (!ptv_ID21.IsEmpty) ptv_ID27.SegmentVolume = ptv_ID27.Or(ptv_ID21);//58.4+43

            if (colon != null) prv_colon.SegmentVolume = colon.Margin(5.0);// PRV 
            if (bowel != null) prv_intestino.SegmentVolume = bowel.Margin(5.0);// PRV 

            if (result == DialogResult.No)
            {
                ptv_ID23.Id = PTV_ID24;
            }
            else if (result == DialogResult.Cancel)
            {
                ptv_ID23.Id = PTV_ID25;
            }

            if (ctv_ID3 == null) ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 == null) ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 == null) ss.RemoveStructure(ptv_ID15);
            if (ctv_ID6 == null) ss.RemoveStructure(ptv_ID16);
            if (ctv_ID3 == null && ctv_ID4 == null && ctv_ID5 == null && ctv_ID6 == null) ss.RemoveStructure(ptv_ID22);
            if (ctv_ID7 == null)
            {
                ss.RemoveStructure(ptv_ID17);
                ss.RemoveStructure(ptv_ID21);
                ptv_ID22.Id = PTV_ID22_;

            }

        }

        public void St_CYC_25fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "CYC";
            //gtv-ctvs
            string[] N_GTV = {  "GTV_HeadNeck",     "GTV_SIB", "SIB", "sib base leng" };
            string[] N_CTV = {  "CTV_High_Risk",    "CTV_Tumor", "CTV_Peritumor" };
            string[] N_NL = {   "CTV_LN_Neck_L",    "cuello izq", "Cuello izq", "cuello izq" };
            string[] N_NR = {   "CTV_LN_Neck_R",    "cuello der", "Cuello d", "cuello derech" };
            string[] N_ADPR = { "GTV_ADP_R",        "adp der", "ADP D", "sib adp izq" };
            string[] N_ADPL = { "GTV_ADP_L",        "adp izq", "ADP I" };
            string[] N_N1 = {   "CTV_LN_NI_A",      "Nivel Ia" };//50gy=cuello
            //oars
            string[] N_Brainst = {  "Brainstem",     "tronco", "Tronco" };
            string[] N_Parotid_L = { "Parotid_L",   "parotida izq", "parotid i", "Parotid Gland, L", "Parotida Izq" };
            string[] N_Parotid_R = { "Parotid_R",   "parotida der", "parotid d", "Parotid Gland, R", "Parotida D" };
            string[] N_Body = {      "Body",        "Outer Contour", "body" };
            string[] N_SC = {       "SpinalCord",   "Spinal Cord", "Sc", "sc", "ME" };
            string[] N_OpticNR = {  "OpticNrv_R",   "NOD" };//falta
            string[] N_OpticNL = {  "OpticNrv_L",   "NOI" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.
            DialogResult result = System.Windows.Forms.MessageBox.Show("Unio los huecos de aire del contorno externo? Recuerde que el script retrae con respecto a esta estructura" , SCRIPT_NAME0, MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;// mensaje para no perder los CTVs

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_GTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_CTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_NL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_NR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_ADPR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.FirstOrDefault(s => N_ADPL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID8 = ss.Structures.FirstOrDefault(s => N_N1.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure brainstem = ss.Structures.FirstOrDefault(s => N_Brainst.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure sc = ss.Structures.FirstOrDefault(s => N_SC.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure parotidL = ss.Structures.FirstOrDefault(s => N_Parotid_L.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure parotidR = ss.Structures.FirstOrDefault(s => N_Parotid_R.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(ctv_ID7) || !HighResol(ctv_ID8) || !HighResol(brainstem) || !HighResol(sc) || !HighResol(parotidL)|| !HighResol(parotidR))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_GTV[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, true, N_CTV[0]);//es necesario true
                if (ctv_ID3 == null) return;
                VerifSt(ctv_ID4, false, N_NL[0]);//
                VerifSt(ctv_ID5, false, N_NR[0]);//
                VerifSt(ctv_ID6, false, N_ADPR[0]);//
                VerifSt(ctv_ID7, false, N_ADPL[0]);//
                VerifSt(ctv_ID8, false, N_N1[0]);//
                //OARS
                VerifSt(brainstem, false, N_Brainst[0]);//
                VerifSt(sc, true, N_SC[0]);//
                if (sc == null) return;
                VerifSt(parotidL, true, N_Parotid_L[0]);//
                if (parotidL == null) return;
                VerifSt(parotidR, true, N_Parotid_R[0]);//
                if (parotidR == null) return;//
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_GTV[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, true, N_CTV[0]);//es necesario true
                if (ctv_ID3 == null) return;
                VerifStLow(ctv_ID4, false, N_NL[0]);//
                VerifStLow(ctv_ID5, false, N_NR[0]);//
                VerifStLow(ctv_ID6, false, N_ADPR[0]);//
                VerifStLow(ctv_ID7, false, N_ADPL[0]);//
                VerifStLow(ctv_ID8, false, N_N1[0]);//
                //OARS
                VerifStLow(brainstem, false, N_Brainst[0]);//
                VerifStLow(sc, true, N_SC[0]);//
                if (sc == null) return;
                VerifStLow(parotidL, true, N_Parotid_L[0]);//
                if (parotidL == null) return;
                VerifStLow(parotidR, true, N_Parotid_R[0]);//
                if (parotidR == null) return;//
            }
            Structure optic_r = ss.Structures.FirstOrDefault(s => N_OpticNR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(optic_r, false, N_OpticNR[0]);//
            Structure optic_l = ss.Structures.FirstOrDefault(s => N_OpticNL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(optic_l, false, N_OpticNL[0]);//

            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(body, true, N_Body[0]);//

            Structure body0 = ss.AddStructure("CONTROL", "Body0");
            body0.SegmentVolume = body;

            const string Dientes = "zTeeth";
            const string Inf = "zInferior_Region";
            const string MR = "zMouth_Region";
            const string Af = "zArtifact";
            const string Lips = "zLips";
            const string PTV_ID12 = "PTV_GTV_HeadNeck";
            const string PTV_ID13 = "PTV_High_Risk";
            const string PTV_ID14 = "PTV_LN_Neck_L";
            const string PTV_ID15 = "PTV_LN_Neck_R";
            const string PTV_ID16 = "PTV_GTV_ADP_R";
            const string PTV_ID17 = "PTV_GTV_ADP_L";
            const string PTV_ID18 = "PTV_LN_NI_A";
            const string PTV_ID21 = "zPTV_Low_5000!";
            const string PTV_ID22 = "zPTV_Mid_5800!";
            const string PTV_ID23 = "zPTV_High_6700!";
            const string PTV_ID25 = "zPTV_Total!";
            const string PRV_Brainstem = "Brainstem_PRV05";
            const string PRV_SC = "SpinalCord_PRV05";//


            //============================
            // GENERATE  expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);//n1
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);
            Structure ptv_ID25 = ss.AddStructure("PTV", PTV_ID25);

            Structure prv_brainstem = ss.AddStructure("CONTROL", PRV_Brainstem);
            Structure prv_sc = ss.AddStructure("CONTROL", PRV_SC);
            Structure dientes = ss.AddStructure("CONTROL", Dientes);
            Structure mr = ss.AddStructure("CONTROL", MR);
            Structure af = ss.AddStructure("CONTROL", Af);
            Structure inf = ss.AddStructure("CONTROL", Inf);
            Structure lips = ss.AddStructure("CONTROL", Lips);

            Structure auxi = ss.AddStructure("CONTROL", "Auxi");//auxiliar 
            Structure auxi2 = ss.AddStructure("CONTROL", "auxi2");//auxiliar 
            Structure auxi3 = ss.AddStructure("CONTROL", "auxi3");//auxiliar 
            Structure buff_body = ss.AddStructure("CONTROL", "buff_body");//auxiliar
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID25); St.Add(prv_brainstem);
                St.Add(prv_sc); St.Add(mr); St.Add(auxi); St.Add(auxi2); St.Add(auxi3); St.Add(buff_body); St.Add(body0); St.Add(ptv_ID18);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            
            //---------------------------------------
            //comienza la manipulacion de estructuras
            //----------------------------------------
            buff_body.SegmentVolume = body0.Margin(-5.0);
            ptv_ID12.SegmentVolume = ctv_ID2.Margin(5.0); //PTV GTV
            Cropbody(ctv_ID2, buff_body);
            Cropbody(ptv_ID12, buff_body);

            ptv_ID23.SegmentVolume = ptv_ID12; //PTV 6700
            if (ctv_ID6 != null)
            {
                ptv_ID16.SegmentVolume = ctv_ID6.Margin(5.0);
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID16); //PTV 6700
                Cropbody(ctv_ID6, buff_body);
                Cropbody(ptv_ID16, buff_body);
            }
            if (ctv_ID7 != null)
            {
                ptv_ID17.SegmentVolume = ctv_ID7.Margin(5.0);
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID17); //PTV 6700
                Cropbody(ctv_ID7, buff_body);
                Cropbody(ptv_ID17, buff_body);
            }

            ptv_ID13.SegmentVolume = ctv_ID3.Margin(5.0); //PTV CTV
            Cropbody(ctv_ID3, buff_body);
            Cropbody(ptv_ID13, buff_body);

            if (ctv_ID4 != null)
            {
                ptv_ID14.SegmentVolume = ctv_ID4.Margin(5.0); //PTV Cuello d
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID14);
                Cropbody(ctv_ID4, buff_body);
                Cropbody(ptv_ID14, buff_body);
            }
            //else ss.RemoveStructure(ctv_ID4); ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 != null)
            {
                ptv_ID15.SegmentVolume = ctv_ID5.Margin(5.0); //PTV Cuello i
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID15);
                Cropbody(ctv_ID5, buff_body);
                Cropbody(ptv_ID15, buff_body);
            }
            if (ctv_ID8 != null)
            {
                ptv_ID18.SegmentVolume = ctv_ID8.Margin(5.0); //PTV nivel1
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID18);
                Cropbody(ctv_ID8, buff_body);
                Cropbody(ptv_ID18, buff_body);
            }

            ptv_ID22.SegmentVolume = ptv_ID13.Sub(ptv_ID23);//67-58
            Cropbody(ptv_ID22, buff_body); //int outer-4

            if (ptv_ID21 != null)
            {
                ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID23);//50-67
                ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID22);//50-67-58
                ptv_ID25.SegmentVolume = ptv_ID25.Or(ptv_ID21);
            } 
            Cropbody(ptv_ID21, buff_body);
            Cropbody(ptv_ID14, buff_body);
            Cropbody(ptv_ID15, buff_body);

            prv_brainstem.SegmentVolume = brainstem.Margin(5.0);// PRV 
            prv_sc.SegmentVolume = sc.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 5, 5, 5, 5, 30, 5));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.// PRV 
            prv_sc.SegmentVolume = prv_sc.And(buff_body);//int outer-4

            ptv_ID25.SegmentVolume = ptv_ID25.Or(ptv_ID22);
            ptv_ID25.SegmentVolume = ptv_ID25.Or(ptv_ID23);//ptv total
            Cropbody(ptv_ID25, buff_body);
            Cropbody(ptv_ID23, buff_body);

            auxi.SegmentVolume = (parotidL.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 2))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));//.Margin(30);//anado
            auxi2.SegmentVolume = (parotidR.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));
            auxi3.SegmentVolume = auxi.And(auxi2);//int
            buff_body.SegmentVolume = (body0.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 50, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 25, 0));//.Margin(-7.0);//auter -7
            auxi3.SegmentVolume = auxi3.And(buff_body);//int buff
            auxi.SegmentVolume = ptv_ID25.Margin(10);
            auxi3.SegmentVolume = auxi3.And(buff_body);//int buff
            auxi3.SegmentVolume = auxi3.Sub(auxi);
            buff_body.SegmentVolume = body0.Margin(-7);
            //buff_body.SegmentVolume = (body.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 50, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 10, 0));//
            //auxi3.SegmentVolume = auxi3.Sub(buff_body);// vol - buff reduced 7.5cm hacia abajo
            //buff_body.SegmentVolume = body.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 40, 0, 0, 40, 0, 0));//*/
            mr.SegmentVolume = auxi3.And(buff_body);//region boca hecho/////
            auxi2.SegmentVolume = sc.Margin(40);
            mr.SegmentVolume = mr.Sub(auxi2);

            if (ctv_ID5 != null && ctv_ID4 != null)//INferior
            {
                auxi.SegmentVolume = ptv_ID14.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));//.Margin(50);//anado
                auxi2.SegmentVolume = ptv_ID15.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));//.Margin(50);//anad
                auxi3.SegmentVolume = auxi.And(auxi2);
                auxi.SegmentVolume = ptv_ID14.Margin(5);//anado
                auxi2.SegmentVolume = ptv_ID15.Margin(5);//anad
                auxi3.SegmentVolume = auxi3.Sub(auxi);
                auxi3.SegmentVolume = auxi3.Sub(auxi2);
                auxi.SegmentVolume = ((parotidL.Margin(40)).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));//para quitar el inferior que sube mucho.
                auxi2.SegmentVolume = (parotidR.Margin(40).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 50, 0, 50, 50, 0));//anad
                auxi3.SegmentVolume = auxi3.Sub(auxi);
                auxi3.SegmentVolume = auxi3.Sub(auxi2);
                buff_body.SegmentVolume = body0.Margin(-18);
                auxi3.SegmentVolume = auxi3.And(buff_body);
                inf.SegmentVolume = auxi3;
                //inf.SegmentVolume = (inf.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 0, 50))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 0, 45));
                inf.SegmentVolume = inf.Sub(ptv_ID25.Margin(5));
            }
            af.SetAssignedHU(0.0);//hu artefacto
            dientes.SetAssignedHU(900);//HU dientes dado por el fabi
            ss.RemoveStructure(auxi);
            ss.RemoveStructure(auxi2);
            ss.RemoveStructure(auxi3);
            ss.RemoveStructure(buff_body);
            ss.RemoveStructure(body0);
            if (ctv_ID4 == null) ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 == null) ss.RemoveStructure(ptv_ID15);
            if (ctv_ID6 == null) ss.RemoveStructure(ptv_ID16);
            if (ctv_ID7 == null) ss.RemoveStructure(ptv_ID17);
            if (ctv_ID8 == null) ss.RemoveStructure(ptv_ID18);
        }

        public void St_Cervix_25fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "Cervix";
            //gtv-ctvs
            string[] N_GTV = {      "GTV_Cervix",       "Tumor" };  //TU
            string[] N_CTV3 = {     "CTV_Paramet_L" };          //PARAMETRIO DER
            string[] N_CTV4 = {     "CTV_Paramet_R" };          //PARAMETRIO IZQ
            string[] N_CTV5 = {     "CTV_Rest_Uterus" };        //RESTO DE UTERO
            string[] N_CTV6 = {     "CTV_Rest_Vagina" };        //RESTO DE VAGINA
            string[] N_CTV7 = {     "CTV_LN_Iliac" };           //ILIACOS
            string[] N_CTV8 = {     "CTV_LN_Presacra" };        //PRESACROS
            string[] N_CTV9 = {     "CTV_LN_Paraaort",  "LAO" }; //LAO
            string[] N_CTV10 = {    "CTV_ADP_I" };              //ADP
            string[] N_CTV13 = {    "CTV_ADP_II"};              //ADP PEDIDO POR MURINA
            string[] N_CTV11 = {    "CTV_LN_Pelvic" };          //GANGLIOS PELVICOS
            string[] N_CTV12 = {    "CTV_Parametrium" };        //PARAMETRIOS UNIDOS SOLO SI LOS DOS PARAMETRIOS VAN A LA MISMA DOSIS

            //oars
            string[] N_Colon = {    "Colon",            "colon" };
            string[] N_Bladder = {  "Bladder",          "Vejiga","vejiga"};
            string[] N_Rectum = {   "Rectum",           "Recto" };
            string[] N_Bowel = {    "Bowel",            "Intestino", "intestino","intestinos", "Intestinos" };
            string[] N_Body = {     "Body",             "Outer Contour", "body" };
            string[] N_FJL = {      "FemoralJoint_L"};
            string[] N_FJR = {      "FemoralJoint_R" };
            string[] N_KL = {       "Kidney_L" };
            string[] N_KR = {       "Kidney_R" };
            string[] N_SC = {       "SpinalCord",       "Spinal Cord", "Sc", "sc", "ME" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_GTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_CTV3.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_CTV4.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_CTV5.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_CTV6.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.FirstOrDefault(s => N_CTV7.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID8 = ss.Structures.FirstOrDefault(s => N_CTV8.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID9 = ss.Structures.FirstOrDefault(s => N_CTV9.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID10 = ss.Structures.FirstOrDefault(s => N_CTV10.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID11 = ss.Structures.FirstOrDefault(s => N_CTV11.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID12 = ss.Structures.FirstOrDefault(s => N_CTV12.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID13 = ss.Structures.FirstOrDefault(s => N_CTV13.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bladder = ss.Structures.FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(ctv_ID7) || !HighResol(ctv_ID8) || !HighResol(ctv_ID9) || !HighResol(ctv_ID10) || !HighResol(ctv_ID11) || 
                !HighResol(ctv_ID12)|| !HighResol(ctv_ID13) || !HighResol(colon)|| !HighResol(bladder) || !HighResol(rectum)|| !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_GTV[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_CTV3[0]);//es necesario true
                VerifSt(ctv_ID4, false, N_CTV4[0]);
                VerifSt(ctv_ID5, false, N_CTV5[0]);
                VerifSt(ctv_ID6, false, N_CTV6[0]);
                VerifSt(ctv_ID7, false, N_CTV7[0]);
                VerifSt(ctv_ID8, false, N_CTV8[0]);
                VerifSt(ctv_ID9, false, N_CTV9[0]);
                VerifSt(ctv_ID10, false, N_CTV10[0]);//es necesario true
                VerifSt(ctv_ID11, false, N_CTV11[0]);
                VerifSt(ctv_ID12, false, N_CTV12[0]);
                VerifSt(ctv_ID13, false, N_CTV13[0]);

                //OARS
                VerifSt(colon, false, N_Colon[0]);
                VerifSt(bladder, true, N_Bladder[0]);
                if (bladder == null) return;
                VerifSt(rectum, true, N_Rectum[0]);
                if (rectum == null) return;
                VerifSt(bowel, true, N_Bowel[0]);
                if (bowel == null) return;
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_GTV[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_CTV3[0]);//es necesario true
                VerifStLow(ctv_ID4, false, N_CTV4[0]);
                VerifStLow(ctv_ID5, false, N_CTV5[0]);
                VerifStLow(ctv_ID6, false, N_CTV6[0]);
                VerifStLow(ctv_ID7, false, N_CTV7[0]);
                VerifStLow(ctv_ID8, false, N_CTV8[0]);
                VerifStLow(ctv_ID9, false, N_CTV9[0]);
                VerifStLow(ctv_ID10, false, N_CTV10[0]);//es necesario true
                VerifStLow(ctv_ID11, false, N_CTV11[0]);
                VerifStLow(ctv_ID12, false, N_CTV12[0]);
                VerifStLow(ctv_ID13, false, N_CTV13[0]);

                //OARS
                VerifStLow(colon, false, N_Colon[0]);
                VerifStLow(bladder, true, N_Bladder[0]);
                if (bladder == null) return;
                VerifStLow(rectum, true, N_Rectum[0]);
                if (rectum == null) return;
                VerifStLow(bowel, true, N_Bowel[0]);
                if (bowel == null) return;
            }

            Structure body = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifSt(body, true, N_Bowel[0]);
            if (body == null) return;

            //New Structures 
            const string PTV_ID12 = "PTV_GTV_Cervix";
            const string PTV_ID13 = "PTV_Paramet_L";
            const string PTV_ID14 = "PTV_Paramet_R";
            const string PTV_ID15 = "PTV_Rest_Uterus";
            const string PTV_ID16 = "PTV_Rest_Vagina";
            const string PTV_ID17 = "PTV_LN_Iliac";
            const string PTV_ID18 = "PTV_LN_Presacral";
            const string PTV_ID19 = "PTV_LN_Paraaort";
            const string PTV_ID20 = "PTV_ADP_I";
            const string PTV_ID21 = "zPTV_Low_4300!";
            const string PTV_ID22 = "zPTV_Mid_4800!";
            const string PTV_ID23 = "zPTV_High_5840!";
            const string PTV_ID24 = "zPTV_5840-PRVs!";  //PTV58.4-PRVs
            const string PTV_ID25 = "zPTV_Total!";
            const string PTV_ID26 = "zPTV_RectumPRV05";//PTV58.4 interseccion PRV recto
            const string PTV_ID27 = "zPTV_BladderPRV!"; //PTV58.4 interseccion PRV vejiga
                                                        //PTVs de la variante
            const string PTV_ID28 = "PTV_LN_Pelvic"; //
            const string PTV_ID29 = "PTV_Parametrium";
            const string PTV_ID30 = "PTV_ADP_II";

            const string PRV_Rectum = "Rectum_PRV05";
            const string PRV_Colon = "Colon_PRV05";//
            const string PRV_Bowel = "Bowel_PRV05";//
            const string PRV_Bladder = "Bladder_PRV05";//

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);
            Structure ptv_ID24 = ss.AddStructure("PTV", PTV_ID24);
            Structure ptv_ID25 = ss.AddStructure("PTV", PTV_ID25);
            Structure ptv_ID26 = ss.AddStructure("PTV", PTV_ID26);
            Structure ptv_ID27 = ss.AddStructure("PTV", PTV_ID27);

            Structure ptv_ID28 = ss.AddStructure("PTV", PTV_ID28);//PTV DE LOS GANGLIOS
            Structure ptv_ID29 = ss.AddStructure("PTV", PTV_ID29);//PTV DE LOS PARAMETRIOS
            Structure ptv_ID30 = ss.AddStructure("PTV", PTV_ID30);//ADPII

            Structure prv_rectum = ss.AddStructure("CONTROL", PRV_Rectum);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_Colon);
            Structure prv_intestino = ss.AddStructure("CONTROL", PRV_Bowel);
            Structure prv_bladder = ss.AddStructure("CONTROL", PRV_Bladder);

            Structure auxi = ss.AddStructure("CONTROL", "Auxi");//auxiliar para mochar

            DialogResult result = System.Windows.Forms.MessageBox.Show("OPtions:" + "\n" + "1.-If Yes, Parametrium left(izq) is inside 58.4Gy" + "\n" +
    "2.-If No, Parametrium right(der) is inside 58.4Gy" + "\n" + "3.-If Cancel, both(ambos) of them are inside 58.4Gy", SCRIPT_NAME, MessageBoxButtons.YesNoCancel);
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(ptv_ID25); St.Add(ptv_ID26); St.Add(ptv_ID27); St.Add(ptv_ID28); St.Add(ptv_ID29); St.Add(ptv_ID30);///////
                St.Add(prv_rectum); St.Add(prv_colon); St.Add(prv_intestino); St.Add(prv_bladder); St.Add(auxi);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            ptv_ID12.SegmentVolume = ctv_ID2.Margin(9.0); //PTV Tumor cuello
            if (ctv_ID3 != null) ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV param i
            //else ss.RemoveStructure(ctv_ID3); ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 != null) ptv_ID14.SegmentVolume = ctv_ID4.Margin(6.0); //PTV param d
            //else ss.RemoveStructure(ctv_ID4); ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 != null) ptv_ID15.SegmentVolume = ctv_ID5.Margin(9.0); //PTV Utero
            if (ctv_ID6 != null)
            {
                ptv_ID16.SegmentVolume = ctv_ID6.Margin(9.0); //PTV Vagina
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID16); //ptv param d utero + vagina 48
            }
            if (ctv_ID7 != null)
            {
                ptv_ID17.SegmentVolume = ctv_ID7.Margin(6.0); //PTV gang ilia
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID17); //ptv param d utero + vagina + g iliaco48
            }
            //else ss.RemoveStructure(ctv_ID7); ss.RemoveStructure(ptv_ID17);
            if (ctv_ID8 != null)
            {
                ptv_ID18.SegmentVolume = ctv_ID8.Margin(6.0); //PTV gang presa
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID18); //ptv param d utero + vagina + g iliaco+presac48
                
            }

            //else ss.RemoveStructure(ctv_ID8); ss.RemoveStructure(ptv_ID18);
            if (ctv_ID9 != null) ptv_ID19.SegmentVolume = ctv_ID9.Margin(6.0); //PTV LAO
            //else ss.RemoveStructure(ctv_ID9); ss.RemoveStructure(ptv_ID19);
            if (ctv_ID10 != null) ptv_ID20.SegmentVolume = ctv_ID10.Margin(6.0); //PTV ADP
            else ss.RemoveStructure(ctv_ID10); ss.RemoveStructure(ptv_ID20);

            if (ctv_ID11 != null)
            {
                ptv_ID28.SegmentVolume = ctv_ID11.Margin(6.0); //PTV ganglios
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID28); //ptv param d utero + vagina + g iliaco+presac48+ganglios
            }
            //else ss.RemoveStructure(ctv_ID11); ss.RemoveStructure(ptv_ID28);
            if (ctv_ID12 != null) ptv_ID29.SegmentVolume = ctv_ID12.Margin(6.0); //PTV parametrios
            //else ss.RemoveStructure(ctv_ID12); ss.RemoveStructure(ptv_ID29);
            if (ctv_ID13 != null) ptv_ID30.SegmentVolume = ctv_ID13.Margin(6.0);//PTV adpII
            //PRV
            if (colon != null) prv_colon.SegmentVolume = colon.Margin(5.0);// PRV 
            prv_bladder.SegmentVolume = bladder.Margin(5.0);// PRV 
            prv_rectum.SegmentVolume = rectum.Margin(5.0);// PRV 
            prv_intestino.SegmentVolume = bowel.Margin(5.0);// PRV 

            if (result == DialogResult.Yes)
            {
                ptv_ID23.SegmentVolume = ptv_ID12.Or(ptv_ID13);//PTV tumor+param I 58.4
                auxi.SegmentVolume = ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 15, 0, 0, 30, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
                ptv_ID22.SegmentVolume = ptv_ID14.Or(ptv_ID15); //ptv param d utero 48
            }
            else if (result == DialogResult.No)
            {
                ptv_ID23.SegmentVolume = ptv_ID12.Or(ptv_ID14);//PTV tumor+param D 58.4
                auxi.SegmentVolume = ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 30, 0, 0, 15, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
                ptv_ID22.SegmentVolume = ptv_ID13.Or(ptv_ID15); //ptv param i utero 48
            }
            else
            {
                ptv_ID23.SegmentVolume = ptv_ID12.Or(ptv_ID13);//PTV tumor+param I 58.4
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID14);//PTV tumor+param D 58.4
                ptv_ID23.SegmentVolume = ptv_ID12.Or(ptv_ID29);//PTV tumor+param Ambos si hay 58.4
                auxi.SegmentVolume = ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 26, 0, 0, 26, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
                ptv_ID22.SegmentVolume = ptv_ID15.Or(ptv_ID13); //ptv param i utero 48
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14); //ptv param i+d utero 48
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID29); //ptv param i+d+ambos utero 48
            }


            //Struture Auxiliar for creation of lateral crop in PRV Bladder. esto es solo para parametrio izq

            auxi.SegmentVolume = auxi.And(prv_bladder);// intersection with bladder            
            auxi.SegmentVolume = auxi.AsymmetricMargin(new AxisAlignedMargins(0, 3, 50, 3, 3, 3, 3)); ;//mochado+3mm+5cm hacia arriba para cortar
            ptv_ID27.SegmentVolume = auxi.And(ptv_ID23);//PTV58.4*PRV Bladder mochado para que corte lo necesario
            ptv_ID27.SegmentVolume = ptv_ID27.And(prv_bladder);//PTV58.4*PRV Bladder mochado para que corte lo necesario
            ////////////////////////////////////////////PTV58.4-Prvs
            ptv_ID24.SegmentVolume = ptv_ID23.Sub(auxi);//PTV58.4-PRVs! - PRV Vejiga+3mm
            auxi.SegmentVolume = prv_rectum.Margin(3.0);// ahora el auxi es la extension del prv recto
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(auxi); //PTV58.4-PRVs! - PRV Rectum+3mm
            auxi.SegmentVolume = prv_intestino.Margin(3.0);// ahora el auxi es la extension del prv intestino
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(auxi); //PTV58.4-PRVs! - PRV Rectum+3mm
            auxi.SegmentVolume = prv_colon.Margin(3.0);// ahora el auxi es la extension del prv colon
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(auxi); //PTV58.4-PRVs! - PRV Rectum+3mm
                                                         //////////////////////////////////////////////////////

            

            ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23); //48-58.4

            ptv_ID21.SegmentVolume = ptv_ID19.Sub(ptv_ID22); //43-48
            ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID23); //43-58.4

            //PTV total
            ptv_ID25.SegmentVolume = ptv_ID23.Or(ptv_ID22);//58.4+48
            ptv_ID25.SegmentVolume = ptv_ID25.Or(ptv_ID21);//58.4+43
            //PTV58.4*PRV REC
            ptv_ID26.SegmentVolume = ptv_ID23.And(prv_rectum);

            ss.RemoveStructure(auxi);
            if (ctv_ID3 == null) ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 == null) ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 == null) ss.RemoveStructure(ptv_ID15);
            if (ctv_ID6 == null) ss.RemoveStructure(ptv_ID16);
            if (ctv_ID7 == null) ss.RemoveStructure(ptv_ID17);
            if (ctv_ID8 == null) ss.RemoveStructure(ptv_ID18);
            if (ctv_ID9 == null) ss.RemoveStructure(ptv_ID19);

            if (ctv_ID11 == null) ss.RemoveStructure(ptv_ID28);
            if (ctv_ID12 == null) ss.RemoveStructure(ptv_ID29);
            if (ctv_ID13 == null) ss.RemoveStructure(ptv_ID30);

        }

        public void St_HDR_15fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "HDR_15Fx";
            string[] N_Prostate = { "CTV_Prostate",     "CTV_Prostata", "1-Prostata" };
            string[] N_LN = {       "CTV_LN_Obturator", "ganglio", "ganglios", "obturador" };
            string[] N_VS = {       "CTV_SeminalVes",   "Vesiculas", "Vesiculas Sem", "VS+1cm" };
            string[] N_SIB = {      "GTV_SIB",          "_SIB", "nodulo" };
            string[] N_GP = {       "CTV_LN_Pelvic",    "Pelviano", "Pelvico", "ganglios pelvicos", "RegGanglionares" };
            string[] N_Urethra = {  "Urethra",          "Uretra", "uretra" };
            string[] N_Trigone = {  "Trigone",          "trigono", "Trigono" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino" };
            string[] N_Body = {     "Body",             "Outer Contour", "body" };
            //CAMBIAR NOMBRE
            string[] N_HJL = {      "FemoralJoint_L",   "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR = {      "FemoralJoint_R",   "Hip Joint, Right", "Hip Joint Right",  "CFD" };
            string[] N_Penile = {   "PenileBulb",       "Penile Bulb", "Pene B", "penile bulb", "B Pene", "Bulbo" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(uretra) || !HighResol(trigono) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifSt(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifSt(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifSt(uretra, true, N_Urethra[0]);//es necesario true
                //if (uretra == null) return;
                VerifSt(trigono, true, N_Trigone[0]);//es necesario true
                //if (trigono == null) return;
                VerifSt(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifSt(colon, false, N_Colon[0]);//es necesario true
                VerifSt(bowel, false, N_Bowel[0]);//es necesario true
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifStLow(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifStLow(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifStLow(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifStLow(uretra, false, N_Urethra[0]);//es necesario true
                //if (uretra == null) return;
                VerifStLow(trigono, false, N_Trigone[0]);//es necesario true
                //if (trigono == null) return;
                VerifStLow(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifStLow(colon, false, N_Colon[0]);//es necesario true
                VerifStLow(bowel, false, N_Bowel[0]);//es necesario true
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre
            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure penile = ss.Structures.FirstOrDefault(s => N_Penile.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjl,false, N_HJL[0]);//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjr, false, N_HJR[0]);
            VerifStLow(penile,false, N_Penile[0]);
            //============================
            // GENERATE 5mm expansion of PTV
            //============================
            //New Structures 
            const string PTV_ID12 = "PTV_Prostate";
            const string PTV_ID13 = "PTV_LN_Obturator";
            const string PTV_ID14 = "PTV_SeminalVes";//
            const string PTV_ID15 = "PTV_SIB";//
            const string PTV_ID16 = "PTV_LN_Pelvic";//
                                                     //const string PTV_ID15 = "PTV_Trigone!";//
                                                     //const string PTV_ID16 = "PTV_RectumPRV05!";
                                                     //const string PTV_ID18 = "PTVLn-PTVpros!";
            const string PTV_ID19 = "PTV_Total!";
            const string PTV_ID20 = "PTV_High_4320";
            const string PTV_ID21 = "PTV_Mid_3900";
            const string Rect_ant = "Rectum_A";
            const string Rect_post = "Rectum_P";
            const string PRV_Sigma = "Colon_PRV05!";//
            const string PRV_Intestino = "Bowel_PRV05!";//

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);//prost
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);//gg
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);//semv
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);//sib
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);//prost
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);//total
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);// 43200
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);//3900

            Structure rect_ant = ss.AddStructure("CONTROL", Rect_ant);
            Structure rect_post = ss.AddStructure("CONTROL", Rect_post);
            Structure prv_sigma = ss.AddStructure("CONTROL", PRV_Sigma);
            Structure prv_intestino = ss.AddStructure("CONTROL", PRV_Intestino);
            Structure uretra2 = ss.AddStructure("PTV", "Buffer_U");
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID19); St.Add(ptv_ID20); St.Add(ptv_ID21); St.Add(rect_ant); St.Add(rect_post); St.Add(prv_sigma);
                St.Add(prv_intestino); St.Add(uretra2);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }

            ptv_ID12.SegmentVolume = ctv_ID2.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5));// PTV Prostate asymmetry

            /*if (uretra!= null || !uretra.IsEmpty)
            {
                uretra.SegmentVolume = uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 0, 10, 0, 0, 10));//expande urethra 1 cm sup and inf   z1 pies z2 cab pero lo hace mal porque es chico
                //uretra2.SegmentVolume = uretra2.Sub(uretra);
                //uretra2.SegmentVolume = uretra2.Sub(uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 7, 0, 0, 7, 0)));
                //uretra2.SegmentVolume = uretra2.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 4, 0));
                //uretra.SegmentVolume = uretra.Or(uretra2);
            }*/
                

            ptv_ID21.SegmentVolume = ptv_ID12;//39=prost
            if (ctv_ID4 != null)   /// Evite mistake in structure empty
            {
                ptv_ID14.SegmentVolume = ctv_ID4.AsymmetricMargin(new AxisAlignedMargins(0, 9, 12, 9, 9, 5, 9));// PTV seminal
                ptv_ID20.SegmentVolume = ptv_ID14;
            }
            // expand PTV
            if (ctv_ID3 != null)   /// Evite mistake in structure empty
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV reg gang
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID13); //ptv_ID21 39 prost+gg
                if (ctv_ID4 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID14); //PTV39-43
            }
            if (ctv_ID6 != null)   /// Evite mistake in structure empty
            {
                ptv_ID16.SegmentVolume = ctv_ID6.Margin(6.0); //PTV reg gang
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID16); //ptv_ID21 39 prost+gg
                if (ctv_ID6 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID14); //PTV39-43
            }
            if (colon != null)
            {
                prv_sigma.SegmentVolume = colon.Margin(5.0);// PRV Sigma
            }
            if (bowel != null)
            {
                prv_intestino.SegmentVolume = bowel.Margin(5.0);// PRV Sigma
            }
            //Structure aux                 = ss.AddStructure("CONTROL", "auxilary");//axilary
            if (ctv_ID5 != null) ptv_ID15.SegmentVolume = ctv_ID5.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5));//ptv sib

            rect_post.SegmentVolume = rectum.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 17, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            rect_ant.SegmentVolume = rectum.Sub(rect_post);
            ptv_ID19.SegmentVolume = ptv_ID20.Or(ptv_ID21);///PTV_total
            ss.RemoveStructure(uretra2);
            //Remove strutures null     
            if (ctv_ID3 == null)//ln obturator
            {
                ss.RemoveStructure(ctv_ID3);
                ss.RemoveStructure(ptv_ID13);
            }
            if (ctv_ID6 == null)
            {
                ss.RemoveStructure(ctv_ID6);
                ss.RemoveStructure(ptv_ID16);
            }
            if (ctv_ID5 == null)
            {
                ss.RemoveStructure(ctv_ID5);
                ss.RemoveStructure(ptv_ID15);
            }
            if (ctv_ID4 == null)
            {
                ss.RemoveStructure(ptv_ID14);
                ss.RemoveStructure(ptv_ID20);
            }
            if (bowel == null)
            {
                ss.RemoveStructure(bowel);
                ss.RemoveStructure(prv_intestino);
            }
            if (colon == null)
            {
                ss.RemoveStructure(colon);
                ss.RemoveStructure(prv_sigma);
            }
        }

        public void St_Lecho_20fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "ProstateBed20Fx";
            string[] N_Prostate = { "CTV_ProstateBed",  "CTV_Prostata", "1-Prostata" };
            string[] N_LN = {       "CTV_LN_Obturator", "ganglio", "ganglios", "obturador" };
            string[] N_VS = {       "CTV_SeminalVes",   "VSem", "Vesiculas", "Vesiculas Sem", "VS+1cm" };
            string[] N_SIB = {      "GTV_SIB",          "_SIB", "nodulo" };
            string[] N_GP = {       "CTV_LN_Pelvic",    "Pelviano", "Pelvico", "ganglios pelvicos", "RegGanglionares" };
            string[] N_Urethra = {  "Urethra",          "Uretra", "uretra" };
            string[] N_Trigone = {  "Trigone",          "trigono", "Trigono" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino","INTESTINO" };
            string[] N_Body = {     "Body",             "Outer Contour", "body" };
            //CAMBIAR NOMBRE
            string[] N_HJL = {      "FemoralJoint_L",   "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR = {      "FemoralJoint_R",   "Hip Joint, Right", "Hip Joint Right",  "CFD" };
            string[] N_Penile = {   "PenileBulb",       "Penile Bulb", "Pene B", "penile bulb", "B Pene", "Bulbo" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(uretra) || !HighResol(trigono) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifSt(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifSt(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifSt(uretra, true, N_Urethra[0]);//es necesario true
                if (uretra == null) return;
                VerifSt(trigono, true, N_Trigone[0]);//es necesario true
                if (trigono == null) return;
                VerifSt(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifSt(colon, false, N_Colon[0]);//es necesario true
                VerifSt(bowel, false, N_Bowel[0]);//es necesario true
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifStLow(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifStLow(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifStLow(ctv_ID6, false, N_GP[0]);//es necesario true
                VerifStLow(uretra, true, N_Urethra[0]);//es necesario true
                if (uretra == null) return;
                VerifStLow(trigono, true, N_Trigone[0]);//es necesario true
                if (trigono == null) return;
                VerifStLow(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifStLow(colon, false, N_Colon[0]);//es necesario true
                VerifStLow(bowel, false, N_Bowel[0]);//es necesario true
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre
            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure penile = ss.Structures.FirstOrDefault(s => N_Penile.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjl, false, N_HJL[0]);//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjr, false, N_HJR[0]);
            VerifStLow(penile, false, N_Penile[0]);

            //comienza las estrucuras
            //New Structures 
            const string PTV_ID12 = "PTV_Prostate";
            const string PTV_ID13 = "PTV_LN_Obturator";
            const string PTV_ID14 = "zPTV_Urethra!";
            const string PTV_ID15 = "zPTV_Trigone!";
            const string PTV_ID16 = "zPTV_RectumPRV05";
            const string PTV_ID17 = "zPTV-PRVs!";
            const string PTV_ID18 = "PTV_SeminalVes";//
            const string PTV_ID19 = "zPTV_Total!";
            const string PTV_ID20 = "zPTV_High_5840!";
            const string PTV_ID21 = "zPTV_Low_4400!";
            const string PTV_ID22 = "zPTV_Mid_4920!";
            const string PTV_ID23 = "PTV_SIB";
            const string PTV_ID24 = "PTV_LN_Pelvic";
            const string PRV_Rectum = "Rectum_PRV05";
            const string Rect_ant = "Rectum_A";
            const string Rect_post = "Rectum_P";
            const string PRV_colon = "Colon_PRV05";//
            const string PRV_bowel = "Bowel_PRV05";//
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);//ptv20 arriba
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);
            Structure ptv_ID22 = ss.AddStructure("PTV", PTV_ID22);
            Structure ptv_ID23 = ss.AddStructure("PTV", PTV_ID23);
            Structure ptv_ID24 = ss.AddStructure("PTV", PTV_ID24);
            Structure prv_rectum = ss.AddStructure("CONTROL", PRV_Rectum);
            Structure rect_ant = ss.AddStructure("CONTROL", Rect_ant);
            Structure rect_post = ss.AddStructure("CONTROL", Rect_post);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_colon);
            Structure prv_bowel = ss.AddStructure("CONTROL", PRV_bowel);
            //Structure uretra2 = ss.AddStructure("PTV", "Buffer_U");
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(prv_rectum); St.Add(rect_ant); St.Add(rect_post); St.Add(prv_colon); St.Add(prv_bowel);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            ptv_ID12.SegmentVolume = ctv_ID2.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5));// PTV Prostate asymmetry
            /*uretra2.SegmentVolume = uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 0, 10, 0, 0, 10));//expande urethra 1 cm sup and inf   z1 pies z2 cab pero lo hace mal porque es chico
            uretra2.SegmentVolume = uretra2.Sub(uretra);
            uretra2.SegmentVolume = uretra2.Sub(uretra.AsymmetricMargin(new AxisAlignedMargins(0, 0, 7, 0, 0, 7, 0)));
            uretra2.SegmentVolume = uretra2.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 0, 0, 0, 4, 0));//quite esto porque dibujaba la uretra mal
            uretra.SegmentVolume = uretra.Or(uretra2);*///era para que la uretra se salga del ptv pero lo hace mal
            ptv_ID20.SegmentVolume = ptv_ID12;///PTV4000 o 3625
            // expand PTV
            if (ctv_ID4 != null)
            {
                ptv_ID18.SegmentVolume = ctv_ID4.AsymmetricMargin(new AxisAlignedMargins(0, 9, 9, 9, 9, 5, 9));// ves sem 9&5
                ptv_ID22.SegmentVolume = ptv_ID18.Sub(ptv_ID12); //PTV2750-3625
            }
            if (ctv_ID3 != null)   /// Evite mistake in structure empty el orden es importante 3 debe estar desp 4
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV reg gang   obturator
                ptv_ID21.SegmentVolume = ptv_ID13.Sub(ptv_ID12); //PTV2500-3625
                if (ctv_ID4 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID18); //PTV2500-27500
            }
            if (ctv_ID6 != null)   /// Evite mistake in structure empty el orden es importante 3 debe estar desp 4
            {
                ptv_ID24.SegmentVolume = ctv_ID6.Margin(6.0); //PTV reg gang   pelvic
                ptv_ID21.SegmentVolume = ptv_ID24.Sub(ptv_ID12); //PTV2500-3625
                if (ctv_ID4 != null) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID18); //PTV2500-27500
            }
            if (ctv_ID5 != null)
            {
                ptv_ID23.SegmentVolume = ctv_ID5.AsymmetricMargin(new AxisAlignedMargins(0, 5, 5, 5, 5, 3, 5)); //PTV sib
            }
            if (colon != null)
            {
                prv_colon.SegmentVolume = colon.Margin(5.0);// PRV colon
            }
            if (bowel != null)
            {
                prv_bowel.SegmentVolume = bowel.Margin(5.0);// PRV colon
            }
            //Structure aux                 = ss.AddStructure("CONTROL", "auxilary");//axilary
            prv_rectum.SegmentVolume = rectum.Margin(5.0);// PRV Rectum
            rect_post.SegmentVolume = rectum.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 17, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            rect_ant.SegmentVolume = rectum.Sub(rect_post);

            ptv_ID14.SegmentVolume = ptv_ID12.And(uretra);//PTV*U
            ptv_ID15.SegmentVolume = ptv_ID12.And(trigono);//PTV*T
            ptv_ID16.SegmentVolume = ptv_ID12.And(prv_rectum);//PTV*PRVV re
            ptv_ID17.SegmentVolume = uretra.Or(trigono);//U+T
            ptv_ID17.SegmentVolume = ptv_ID17.Or(prv_rectum);//U+T+PrvRe
            ptv_ID17.SegmentVolume = ptv_ID12.Sub(ptv_ID17);//PTV pros-(U+T+PrvRe)
            ptv_ID19.SegmentVolume = ptv_ID12.Or(ptv_ID21);///PTV_total           
            ptv_ID19.SegmentVolume = ptv_ID19.Or(ptv_ID18);///PTV_total

            //ss.RemoveStructure(uretra2);
            //Remove strutures null   
            if (ctv_ID6 == null && ctv_ID3 == null) ss.RemoveStructure(ptv_ID21);
            if (ctv_ID3 == null)
            {
                ss.RemoveStructure(ctv_ID3);
                ss.RemoveStructure(ptv_ID13);
                //ss.RemoveStructure(ptv_ID18);
            }
            if (ctv_ID6 == null)
            {
                ss.RemoveStructure(ctv_ID6);
                ss.RemoveStructure(ptv_ID24);
                //ss.RemoveStructure(ptv_ID18);
            }
            if (ctv_ID4 == null)
            {
                ss.RemoveStructure(ctv_ID4);
                ss.RemoveStructure(ptv_ID18);
                ss.RemoveStructure(ptv_ID22);
            }
            if (ctv_ID5 == null)
            {
                ss.RemoveStructure(ctv_ID5);
                ss.RemoveStructure(ptv_ID23);
            }
            if (bowel == null)
            {
                ss.RemoveStructure(bowel);
                ss.RemoveStructure(prv_bowel);
            }
            if (colon == null)
            {
                ss.RemoveStructure(colon);
                ss.RemoveStructure(prv_colon);
            }

        }

        public void St_Bladder_20fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "Bladder20Fx";
            string[] N_Bladder = { "CTV_Bladder",       "CTV_Vejiga", "Vejiga" };
            string[] N_LN = {       "CTV_LN_Pelvic",    "CTV_Ganglio", "Pelvicos" };
            string[] N_SIB = {      "GTV_SIB",          "_SIB", "nodulo" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino", "intestino" };
            string[] N_Body = {     "Body",             "Outer Contour", "body" };
            //CAMBIAR NOMBRE
            string[] N_HJL = {      "FemoralJoint_L",   "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR = {      "FemoralJoint_R",   "Hip Joint, Right", "Hip Joint Right",  "CFD" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Bladder[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_SIB[0]);//es necesario true
                VerifSt(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifSt(colon, false, N_Colon[0]);//es necesario true
                VerifSt(bowel, true, N_Bowel[0]);//es necesario true
                if (bowel == null) return;
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Bladder[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifStLow(ctv_ID4, false, N_SIB[0]);//es necesario true
                VerifStLow(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifStLow(colon, false, N_Colon[0]);//es necesario true
                VerifStLow(bowel, true, N_Bowel[0]);//es necesario true
                if (bowel == null) return;
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre
            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjl, false, N_HJL[0]);//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjr, false, N_HJR[0]);
            
            //comienza las estrucuras
            //New Structures 
            const string PTV_ID12 = "PTV_Bladder";
            const string PTV_ID13 = "PTV_LN_Pelvic";
            const string PTV_ID14 = "PTV_SIB";
            const string PTV_ID15 = "zPTV_High_5600!";
            const string PTV_ID16 = "zPTV_Low_4500!";
            const string PTV_ID17 = "zPTV_High_6600!";
            const string PTV_ID18 = "zPTV56-BowelPRV!";
            const string PTV_ID19 = "zPTV66-BowelPRV!";
            const string PTV_ID20 = "zPTV_BowelPRV05!";
            const string PTV_ID21 = "zPTV_Total!";
            
            const string PRV_Rectum = "Rectum_PRV05";
            const string PRV_colon = "Colon_PRV05";//
            const string PRV_bowel = "Bowel_PRV05";//

            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);//sib
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);//5600
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);//4500
            Structure ptv_ID17 = ss.AddStructure("PTV", PTV_ID17);//6600
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);//56-int
            Structure ptv_ID19 = ss.AddStructure("PTV", PTV_ID19);//66-int
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);//intersec int
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);//total

            Structure prv_rectum = ss.AddStructure("CONTROL", PRV_Rectum);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_colon);
            Structure prv_bowel = ss.AddStructure("CONTROL", PRV_bowel);

            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(prv_rectum);  St.Add(prv_colon); St.Add(prv_bowel);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }

            prv_rectum.SegmentVolume = rectum.Margin(5.0);
            prv_bowel.SegmentVolume = bowel.Margin(5.0);
            if (colon != null) prv_colon.SegmentVolume = colon.Margin(5.0);
            else ss.RemoveStructure(prv_colon);

            ptv_ID12.SegmentVolume = ctv_ID2.Margin(10.0);// PTV Bladder 
            ptv_ID15.SegmentVolume = ptv_ID12;// PTV Bladder 
            ptv_ID21.SegmentVolume = ptv_ID15;//total=5600
            if (ctv_ID3 != null)
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0);//gg
                ptv_ID16.SegmentVolume = ptv_ID13.Sub(ptv_ID12);//45-56
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID16);//total=5600+4500
            }
            else
            {
                ss.RemoveStructure(ptv_ID13);
                ss.RemoveStructure(ptv_ID16);
            }

            if (ctv_ID4 != null)
            {
                ptv_ID14.SegmentVolume = ctv_ID4.Margin(10.0);//sib
                ptv_ID17.SegmentVolume = ptv_ID14;
                ptv_ID15.SegmentVolume = ptv_ID15.Sub(ptv_ID14);//56-65
                ptv_ID19.SegmentVolume = ptv_ID17.Sub(prv_bowel);
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID17);//total=5600+4500+6500
                if (ctv_ID3!=null || !ctv_ID3.IsEmpty) ptv_ID16.SegmentVolume = ptv_ID16.Sub(ptv_ID14);//45-56-65
            }
            else
            {
                ss.RemoveStructure(ptv_ID14);
                ss.RemoveStructure(ptv_ID17);
                ss.RemoveStructure(ptv_ID19);
            }
            ptv_ID18.SegmentVolume = ptv_ID15.Sub(prv_bowel);
            ptv_ID20.SegmentVolume= ptv_ID15.And(prv_bowel);
        }

        public void St_Endometrium_20fx(ScriptContext context)//gonzalez hilda //cipolletti  susana elisab
        {
            const string SCRIPT_NAME0 = "Endometrium20Fx";
            string[] N_Bladder = {      "GTV_SurgicalBed",  "LQ" };
            string[] N_LN = {           "CTV_LN_Pelvic",    "CTV_Ganglio", "Pelvicos" };
            string[] N_RV = {           "CTV_RestVagina",   "RV" };//rest vagina
            string[] N_Rectum = {       "Rectum",           "recto",  "rectum" };
            string[] N_Colon = {        "Colon",            "colon",   "sigma", "COLON" };
            string[] N_Bowel = {        "Bowel",            "bowels",  "intestinos", "Intestino", "intestino", "INTESTINO" };
            string[] N_Body = {         "Body",             "Outer Contour", "body" };
            //CAMBIAR NOMBRE
            string[] N_HJL = {          "FemoralJoint_L",   "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR = {          "FemoralJoint_R",   "Hip Joint, Right", "Hip Joint Right",  "CFD" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.FirstOrDefault(s => N_RV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Bladder[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_RV[0]);//es necesario true
                VerifSt(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifSt(colon, false, N_Colon[0]);//es necesario true
                VerifSt(bowel, true, N_Bowel[0]);//es necesario true
                if (bowel == null) return;
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Bladder[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifStLow(ctv_ID4, false, N_RV[0]);//es necesario true
                VerifStLow(rectum, true, N_Rectum[0]);//es necesario true
                if (rectum == null) return;
                VerifStLow(colon, false, N_Colon[0]);//es necesario true
                VerifStLow(bowel, true, N_Bowel[0]);//es necesario true
                if (bowel == null) return;
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre
            Structure hjl = ss.Structures.FirstOrDefault(s => N_HJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_HJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjl, false, N_HJL[0]);//s = structura s.id su id names es el array de string para ver
            VerifStLow(hjr, false, N_HJR[0]);

            //comienza las estrucuras
            //New Structures 
            const string PTV_ID12 = "PTV_SurgicalBed";
            const string PTV_ID13 = "PTV_LN_Pelvic";
            const string PTV_ID14 = "PTV_RestVagina";
            const string PTV_ID15 = "zPTV_High_4800!";
            const string PTV_ID16 = "zPTV_Low_4500!";
            const string PTV_ID18 = "zPTV48-BowelPRV!";//pude estar vacio
            const string PTV_ID20 = "zPTV_BowelPRV05!";
            const string PTV_ID21 = "zPTV_Total!";

            const string PRV_Rectum = "Rectum_PRV05";
            const string PRV_colon = "Colon_PRV05";//
            const string PRV_bowel = "Bowel_PRV05";//

            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);//sib
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);//4800
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);//4500
            Structure ptv_ID18 = ss.AddStructure("PTV", PTV_ID18);//48-int
            Structure ptv_ID20 = ss.AddStructure("PTV", PTV_ID20);//intersec int
            Structure ptv_ID21 = ss.AddStructure("PTV", PTV_ID21);//total

            Structure prv_rectum = ss.AddStructure("CONTROL", PRV_Rectum);
            Structure prv_colon = ss.AddStructure("CONTROL", PRV_colon);
            Structure prv_bowel = ss.AddStructure("CONTROL", PRV_bowel);

            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16);  St.Add(ptv_ID18); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(prv_rectum); St.Add(prv_colon); St.Add(prv_bowel);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }

            prv_rectum.SegmentVolume = rectum.Margin(5.0);
            prv_bowel.SegmentVolume = bowel.Margin(5.0);
            if (colon != null) prv_colon.SegmentVolume = colon.Margin(5.0);
            else ss.RemoveStructure(prv_colon);
            ptv_ID12.SegmentVolume = ctv_ID2.Margin(9.0);// PTV lecho
            ptv_ID15.SegmentVolume = ptv_ID12;// PTV 48 
            ptv_ID21.SegmentVolume = ptv_ID15;//total=4800
            if (ctv_ID3 != null)
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0);//gg
                ptv_ID16.SegmentVolume = ptv_ID13.Sub(ptv_ID12);//45-48
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID16);//total=5600+4500
            }
            else
            {
                ss.RemoveStructure(ptv_ID13);  
            }

            if (ctv_ID4 != null)
            {
                ptv_ID14.SegmentVolume = ctv_ID4.Margin(9.0);//sib
                if (ctv_ID3 == null) ptv_ID16.SegmentVolume = ptv_ID14;
                ptv_ID16.SegmentVolume = ptv_ID16.Or(ptv_ID14);//45 gg+rv
                ptv_ID16.SegmentVolume = ptv_ID16.Sub(ptv_ID15);//45-48
                ptv_ID21.SegmentVolume = ptv_ID21.Or(ptv_ID16);//total=4800+4500
                if (ctv_ID3 != null || !ctv_ID3.IsEmpty) ptv_ID16.SegmentVolume = ptv_ID16.Sub(ptv_ID14);//45-56-65
            }
            else
            {
                ss.RemoveStructure(ptv_ID14);
                ss.RemoveStructure(ptv_ID16);
            }
            ptv_ID18.SegmentVolume = ptv_ID15.Sub(prv_bowel);
            ptv_ID20.SegmentVolume = ptv_ID15.And(prv_bowel);
        }

        public void St_Higado_3fx(ScriptContext context)//CanAddStructure n34741
        {
            const string SCRIPT_NAME0 = "Liver_3Fx";
            string[] N_Liver = { "GTV_Liver", "ITV", "GTV MT hepatic" };
            string[] N_Stomach = { "Stomach", "Estomago", "estomago" };
            string[] N_Esophagus = { "Esophagus", "Esofago", "esofago" };
            string[] N_Bowel = { "Bowel", "bowels", "intestinos", "Intestino", "intestino", "INTESTINO" };
            string[] N_Body = { "Body", "Outer Contour", "body" };
            string[] N_Duodenum = { "Duodenum", "Duodeno", "duodeno" };//hip joint left

            string[] N_Lungs = { "Lungs", "Pulmones", "pulmones" };
            string[] N_Lung_L = { "Lung_L", "Pulmon Izq", "Lung L" };//Lung R
            string[] N_Lung_R = { "Lung_R", "Pulmon Der", "Lung R" };//Lung R
            string[] N_Kidney_L = { "Kidney_L", "RD" };
            string[] N_Kidney_R = { "Kidney_R", "RI" };//

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.FirstOrDefault(s => N_Liver.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure stomach = ss.Structures.FirstOrDefault(s => N_Stomach.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure esophagus = ss.Structures.FirstOrDefault(s => N_Esophagus.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure duodenum = ss.Structures.FirstOrDefault(s => N_Duodenum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver


            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(stomach) || !HighResol(esophagus) || !HighResol(bowel) || !HighResol(duodenum))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Liver[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(stomach, false, N_Stomach[0]);//es necesario true/false
                VerifSt(esophagus, false, N_Esophagus[0]);//es necesario true
                VerifSt(bowel, false, N_Bowel[0]);//es necesario true  
                VerifSt(duodenum, false, N_Duodenum[0]);//es necesario true               
            }
            else
            {
                Low = true;
                VerifStLow(ctv_ID2, true, N_Liver[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifStLow(stomach, false, N_Stomach[0]);//es necesario true/false
                VerifStLow(esophagus, false, N_Esophagus[0]);//es necesario true
                VerifStLow(bowel, false, N_Bowel[0]);//es necesario true
                VerifStLow(duodenum, false, N_Duodenum[0]);//es necesario true
            }
            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver////el body no deja convertirr a high entonces copiar la estrucutura enn body2
            VerifSt(body, true, N_Body[0]);//es necesario true
            if (body == null) return;

            //solo cambia nombre
            Structure lungs = ss.Structures.FirstOrDefault(s => N_Lungs.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure lungl = ss.Structures.FirstOrDefault(s => N_Lung_L.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure lungr = ss.Structures.FirstOrDefault(s => N_Lung_R.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure kidneyl = ss.Structures.FirstOrDefault(s => N_Kidney_R.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure kidneyr = ss.Structures.FirstOrDefault(s => N_Kidney_L.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifStLow(lungs, false, N_Lungs[0]);
            VerifStLow(lungl, false, N_Lung_L[0]);
            VerifStLow(lungr, false, N_Lung_R[0]);
            VerifStLow(kidneyl, false, N_Kidney_L[0]);
            VerifStLow(kidneyr, false, N_Kidney_R[0]);

            //comienza las estrucuras
            //New Structures 
            const string PTV_ID12 = "PTV_Liver";
            const string PTV_ID13 = "zPTV_High_4800!";
            const string PTV_ID14 = "zPTV-PRVs!";
            const string PTV_ID15 = "zPTV_BowelPRV05!";
            const string PTV_ID16 = "zPTV_DuodePRV05!";
            const string PRV_esophagus = "Esophagus_PRV05";
            const string PRV_duode = "Duodenum_PRV05";//
            const string PRV_bowel = "Bowel_PRV05";//
            const string PRV_stomach = "Stomach_PRV05";//

            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = ss.AddStructure("PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = ss.AddStructure("PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = ss.AddStructure("PTV", PTV_ID14);//sib
            Structure ptv_ID15 = ss.AddStructure("PTV", PTV_ID15);//4800
            Structure ptv_ID16 = ss.AddStructure("PTV", PTV_ID16);//4500

            Structure prv_esop = ss.AddStructure("CONTROL", PRV_esophagus);
            Structure prv_duodenum = ss.AddStructure("CONTROL", PRV_duode);
            Structure prv_bowel = ss.AddStructure("CONTROL", PRV_bowel);
            Structure prv_stomach = ss.AddStructure("CONTROL", PRV_stomach);
            Structure prv_total = ss.AddStructure("CONTROL", "zPRV_total!");

            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(prv_esop); St.Add(prv_duodenum); St.Add(prv_bowel); St.Add(prv_stomach);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            DialogResult result = System.Windows.Forms.MessageBox.Show("Tiene Compresor o es con Expiracion?" + "\n" + "If Yes, the volume has Compressor(Compresor)." + "\n" + "If No, the volume is in Expiration(Expiracion/Gatting)." + "\n" + "If Cancel, stop Script", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes) ptv_ID12.SegmentVolume = ctv_ID2.Margin(5);
            else if (result == DialogResult.No) ptv_ID12.SegmentVolume = ctv_ID2.Margin(3);
            else return;
            ptv_ID13.SegmentVolume = ptv_ID12;
            if (esophagus != null)
            {
                prv_esop.SegmentVolume = esophagus.Margin(4);
                prv_total.SegmentVolume = prv_total.Or(prv_esop);
            }
            else
            {
                ss.RemoveStructure(esophagus);
                ss.RemoveStructure(prv_esop);
            }
            if (duodenum != null)
            {
                prv_duodenum.SegmentVolume = duodenum.Margin(4);
                prv_total.SegmentVolume = prv_total.Or(prv_duodenum);
                ptv_ID16.SegmentVolume = prv_duodenum.And(ptv_ID12);
            }
            else
            {
                ss.RemoveStructure(duodenum);
                ss.RemoveStructure(prv_duodenum);
                ss.RemoveStructure(ptv_ID16);
            }
            if (bowel != null)
            {
                prv_bowel.SegmentVolume = bowel.Margin(4);
                prv_total.SegmentVolume = prv_total.Or(prv_bowel);
                ptv_ID15.SegmentVolume = prv_bowel.And(ptv_ID12);
            }
            else
            {
                ss.RemoveStructure(bowel);
                ss.RemoveStructure(prv_bowel);
                ss.RemoveStructure(ptv_ID15);
            }
            if (stomach != null) 
            {
                prv_stomach.SegmentVolume = stomach.Margin(4);
                prv_total.SegmentVolume = prv_total.Or(prv_stomach);
            }
            else
            {
                ss.RemoveStructure(stomach);
                ss.RemoveStructure(prv_stomach);
            }
            if (prv_total != null) ptv_ID14.SegmentVolume = ptv_ID12.Sub(prv_total);
        }
    }
}
