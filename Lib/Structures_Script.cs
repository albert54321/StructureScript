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
// Lic. en F�sica  Universidad Mayor de San Simon
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
        public string SCRIPT_NAME { get; set; } ="Script";//el nombre que muestra en la applicacion////////////////////ojo el default name puede estar mal ya que lod defino despues
        public int number { get; set; }
        public string Key { get; } = "PHYSICS_ALBERTO_ALARCON";
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
        public void Start(int template, ScriptContext context, bool appr)
        {//ejecuta el scrit seleccionado en User script.c   LIGADO CON VALOR DE TEMPLATE
            if (template == 0 && appr) St_Prostate(context);
            else if (template == 1 && appr) St_Breast_ChestWall(context);
            //else if (template == 2 && appr) St_Breast_ChestWall_RA(context);
            else if (template == 2 && appr) St_Rectum20fx(context);
            else if (template == 3 && appr) St_CYC_25fx(context);
            else if (template == 4 && appr) St_Cervix_20fx(context);
            else if (template == 5 && appr) St_HDR_15fx(context);//
            else if (template == 6 && appr) St_Lecho_20fx(context);
            else if (template == 7 && appr) St_Bladder_20fx(context);//
            else if (template == 8 && appr) St_Endometrium_20fx(context);//endometrium
            else if (template == 9 && appr) St_Higado_3fx(context);//endometrium
            else System.Windows.MessageBox.Show("The Script does not approved for clinical use ");
        }// Aqui hay que implementar el nuevo script de estructuras y correlacionar los numeros sino no se ejecuta

        public void VerifSt( Structure st1, bool need,string name,bool inicio = true)//cambia el nombre y convierte en alta resolucion
        {
            if (st1 == null || st1.IsEmpty)
            {
                if (inicio) 
                {
                    System.Windows.MessageBox.Show(string.Format("'{0}' not found!", name), "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    if (need == true)
                    {
                        System.Windows.MessageBox.Show("Script doesnt execute");
                    }
                }
                
            }
            else {
                if(name != "Body" && name != "Outer Contour" && st1.CanConvertToHighResolution()) st1.ConvertToHighResolution();
                ChangeName( name,st: st1);             
            }
        }

        public void VerifStLow(Structure st1, bool need, string name,bool inicio=true)//cambia el nombre y NO convierte en alta resolucion
        {
            if (st1 == null || st1.IsEmpty)
            {
                if (inicio)
                {
                    System.Windows.MessageBox.Show(string.Format("'{0}' not found!", name), "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    if (need == true) System.Windows.MessageBox.Show("Script doesnt execute");
                }
            }
            else ChangeName(name, st: st1);
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

        public void Cropbody(Structure st1, Structure body1)
        {
            if (st1 != null && body1 != null && !st1.IsEmpty && !body1.IsEmpty)   /// Esto es para que 
            {
                st1.SegmentVolume = body1.And(st1);
            }
        }//crop 0.5cm del body

        //en desuso por la funcion add_structure:
        public bool IsResim(StructureSet ss)
        {
            bool resim = false;
            if (ss.Structures.Any(x => x.Id.Contains("PTV"))) resim = true;
            return resim;
        }

        private void FindCouch(StructureSet ss, ScriptContext context)
        {
            if (!ss.Structures.Any(x => x.Id.Contains("Couch")))
            {
                System.Windows.MessageBox.Show("Cant find structure Couch, add the Couch in the StructureSet.(No se encontro  la camilla, a�ada y vuelva a comenzar el script)");
            };
            if (string.IsNullOrEmpty(context.Image.Series.ImagingDeviceId))
            {
                System.Windows.MessageBox.Show("Add in Imaging device: Somaton SPIRIT, forget it. (A�ada en series -> Dispositivo de Imagenes : Somaton SPIRIT, olvido colocarlo)");
            }
        }

        public DialogResult question()//pregunta para paraar en mitad antes de la optimizacion
        {
            DialogResult desicion = System.Windows.Forms.MessageBox.Show("Do you Continue? Desea continuar?", "Warning", MessageBoxButtons.YesNo);
            return desicion;
        }

        private Structure Add_structure(StructureSet structure_set, string dicom_type, string name)
        {
            if (name.Length > 16) throw new Exception($"El Id/Name de la structura {name} es mayor que 16 caracteres corrija esto.");
            Structure structure = structure_set.AddStructure(dicom_type, "_");
            if (structure_set.Structures.Any(x => x.Id == name))
            {
                if (structure_set.Structures.FirstOrDefault(x => x.Id == name).IsEmpty)
                {
                    structure_set.RemoveStructure(structure);
                    structure = structure_set.Structures.FirstOrDefault(x => x.Id == name);
                }
                else
                {
                    try
                    {
                        if (name.Length == 16)
                        {
                            structure_set.RemoveStructure(structure);
                            structure = structure_set.AddStructure(dicom_type, name.Remove(name.Length - 1) + ".");
                        }
                        else
                        {
                            structure_set.RemoveStructure(structure);//remover la estructura sino no lo puede aumentar en la siguiente sentencia
                            structure = structure_set.AddStructure(dicom_type, name + "."); ;
                        }
                    }
                    catch (Exception)
                    {
                        if (name.Length == 16) structure.Id = name.Remove(name.Length - 1)+ "_";
                        else structure.Id = name + "_";
                    }
                }
                return structure;
            }
            else
            {
                try
                {
                    structure.Id = name;
                }
                catch (Exception)
                {
                    structure_set.RemoveStructure(structure);
                    structure = structure_set.AddStructure(dicom_type, name);
                }
                return structure;
            }
        }

        private void ChangeName(string name, Structure st=null, StructureSet ss=null,Image img=null)
        {
            if (img != null)
            {
                try
                {
                    img.Id = name;
                }
                catch (Exception)
                {
                    if (name.Length < 16) img.Id = name + ".";
                    else img.Id = name.Remove(name.Length - 1) + ".";
                }
            }
            if (ss != null)
            {
                try
                {
                    ss.Id = name;
                }
                catch (Exception)
                {
                    if (name.Length < 16) ss.Id = name + ".";
                    else ss.Id = name.Remove(name.Length - 1) + ".";
                }
            }
            if (st != null)
            {
                try
                {
                    st.Id = name;
                }
                catch (Exception)
                {
                    if (name.Length < 16) st.Id = name + ".";
                    else st.Id = name.Remove(name.Length - 1) + ".";
                }
            }   
        }

        public void St_Prostate(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_SBRT_Prostate";
            string[] N_Prostate = { "CTV_Prostate",     "CTV_Prostata", "1-Prostata" };
            string[] N_LN = {       "CTV_LN_Obturator", "ganglio","ganglios","obturador"};
            string[] N_VS = {       "CTV_SeminalVes",   "VSem", "Vesiculas", "Vesiculas Sem", "VS+1cm" };
            string[] N_SIB = {      "CTV_SIB",          "_SIB",  "nodulo", "GTV_SIB" };
            string[] N_GP = {       "CTV_LN_Pelvic",    "Pelviano", "Pelvico", "ganglios pelvicos", "RegGanglionares" };
            string[] N_Urethra = {  "Urethra",          "Uretra", "uretra"};
            string[] N_Trigone = {  "Trigone",          "trigono", "Trigono" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino" };
            string[] N_Body = {     "Body",             "Outer Contour", "body","BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

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
            string PTV_ID12 = "PTV_Prostate";
            string PTV_ID13 = "PTV_LN_Obturator";
            string PTV_ID14 = "PTV_Urethra!";
            string PTV_ID15 = "PTV_Trigone!";
            string PTV_ID16 = "PTV_RectumPRV05!";
            string PTV_ID17 = "PTV-PRVs!";
            string PTV_ID18 = "PTV_SeminalVes";//
            string PTV_ID19 = "PTV_Total!";
            string PTV_ID20 = "PTV_High_3625";
            string PTV_ID20_ = "PTV_High_4000";
            string PTV_ID21 = "PTV_Low_2500";
            string PTV_ID22 = "PTV_Mid_2750";
            string PTV_ID23 = "PTV_SIB";
            string PTV_ID24 = "PTV_LN_Pelvic";
            string PRV_Rectum = "Rectum_PRV05";
            string Rect_ant = "Rectum_A";
            string Rect_post = "Rectum_P";
            string PRV_colon = "Colon_PRV05";//
            string PRV_bowel = "Bowel_PRV05";//
            
            /*if (IsResim(ss) )
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("Se ha detectado PTVs, el plan es resimilacion?", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if(result0==DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13.Remove(PTV_ID13.Length - 1);
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16.Remove(PTV_ID16.Length - 1);
                    PTV_ID17 += ".";
                    PTV_ID18 += ".";
                    PTV_ID19 += ".";
                    PTV_ID20 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID24 += ".";
                    PRV_Rectum += ".";
                    Rect_ant += ".";
                    Rect_post += ".";
                    PRV_colon += ".";
                    PRV_bowel += ".";
                }
            }*/
            FindCouch(ss, context);
            DialogResult result = System.Windows.Forms.MessageBox.Show("Dose prescription is 36.25Gy?"+ "\n" +"If Yes, dose prescription is 36.25Gy" + "\n" +"If No, dose prescription is 40Gy" + "\n" + "If Cancel, End aplication", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Cancel) return;
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);
            if (result == DialogResult.No) ChangeName(PTV_ID20_, ptv_ID20);
            
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures
            
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);//ptv20 arriba
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);
            Structure ptv_ID24 = Add_structure(ss, "PTV", PTV_ID24);
            Structure prv_rectum = Add_structure(ss, "CONTROL", PRV_Rectum);
            Structure rect_ant = Add_structure(ss, "CONTROL", Rect_ant);
            Structure rect_post = Add_structure(ss, "CONTROL", Rect_post);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_colon);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_bowel);
            Structure auxi = Add_structure(ss, "CONTROL", "Buffer");
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(prv_rectum); St.Add(rect_ant); St.Add(rect_post); St.Add(prv_colon); St.Add(prv_bowel); St.Add(auxi);
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
            if (ctv_ID3 != null && ctv_ID6 != null && !ctv_ID3.IsEmpty && !ctv_ID6.IsEmpty)
            {
                ptv_ID21.SegmentVolume = ptv_ID13.Or(ptv_ID24);
                ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID12); //PTV2500-3625
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
            //Structure aux                 = Add_structure(ss, "CONTROL", "auxilary");//axilary
            prv_rectum.SegmentVolume = rectum.Margin(5.0);// PRV Rectum
            rect_post.SegmentVolume = rectum.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 17, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            rect_ant.SegmentVolume = rectum.Sub(rect_post);

            //ss.RemoveStructure(auxi);

            ptv_ID14.SegmentVolume = ptv_ID12.And(uretra);//PTV*U
            ptv_ID15.SegmentVolume = ptv_ID12.And(trigono);//PTV*T
            ptv_ID16.SegmentVolume = ptv_ID12.And(prv_rectum);//PTV*PRVV re
            ptv_ID17.SegmentVolume = uretra.Or(trigono);//U+T
            ptv_ID17.SegmentVolume = ptv_ID17.Or(prv_rectum);//U+T+PrvRe
            ptv_ID17.SegmentVolume = ptv_ID12.Sub(ptv_ID17);//PTV pros-(U+T+PrvRe)
            ptv_ID19.SegmentVolume = ptv_ID12.Or(ptv_ID21);///PTV_total           
            ptv_ID19.SegmentVolume = ptv_ID19.Or(ptv_ID18);///PTV_total

            auxi.SegmentVolume = ptv_ID19.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 50, 0, 2, 50, 50, 2));//esto es para que el recto_A yP solo quede en el ptv y no sa todo el recto
            auxi.SegmentVolume = auxi.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 30, 0, 0, 30, 30, 0));
            rect_post.SegmentVolume = rect_post.And(auxi);
            rect_ant.SegmentVolume = rect_ant.And(auxi);
            
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
            else
            {
                ptv_ID23.SegmentVolume = ptv_ID23.Sub(uretra.Margin(2));//ptv sib-u
                ptv_ID23.SegmentVolume = ptv_ID23.Sub(prv_rectum.Margin(2)); //ptv sib-prv rec
                ptv_ID23.SegmentVolume = ptv_ID23.Sub(trigono.Margin(2));//ptv sib-trigon
                ptv_ID20.SegmentVolume = ptv_ID20.Sub(ptv_ID23);//36.25-sib
                ptv_ID17.SegmentVolume = ptv_ID17.Sub(ptv_ID23);//ptv-prvs-sib
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
            ss.RemoveStructure(auxi);//tieneq estar ahi porqu lo uso en ctv5
        }
        
        public void BreastStructureCreation(StructureSet ss,DialogResult result, DialogResult result2, DialogResult result3,bool inicio, ScriptContext context) 
        {
            const string SCRIPT_NAME0 = "Script_Breast_ChestWall";
            string[] N_Breast = { "zCTV_Mama", "1-Mama", "CTV_Breast", "zCTV_Mama_I", "zCTV_Mama_D" };
            string[] N_LNI = { "CTV_GL_Axila_1", "2-Ax I", "CTV_LN_Ax_L1", "Axila I" };
            string[] N_LNII = { "CTV_GL_Axila_2", "3-Ax II", "CTV_LN_Ax_L2", "Axila II" };
            string[] N_LNIII = { "CTV_GL_Axila_3", "4-Ax III", "CTV_LN_Ax_L3", "Axila III" };
            string[] N_Rotter = { "CTV_GL_Rotter", "5-Rotter", "CTV_LN_Rotter", "7 Rotter" };
            string[] N_Sclav = { "CTV_GL_Supra", "6-Supra", "CTV_LN_Sclav", "6-SUPRA" };
            string[] N_IMN = { "CTV_GL_CMI", "7-CMI", "CTV_LN_IMN" };
            string[] N_Dist = { "CTV_Mama_Dist", "8-MDISTAL", "CTV_Breast_Dist", "Distal", "10- CTV distal", "zCTV_Mama_Dist" };
            string[] N_Prox = { "CTV_Mama_Prox", "9-MPROX", "CTV_Breast_Prox", "Proximal", "9 CTV proximal", "zCTV_Mama_Prox" };
            string[] N_SIB = { "GTV_SIB", "10-SIB", "8 SIB" };
            string[] N_Chest = { "CTV_Pared", "1-Pared", "CTV_Chestwall", "Pared", "zCTV_Pared" };
            //bad names
            string[] N_Body = { "Body", "Outer Contour", "body", "BODY" };
            string[] N_SC = { "MedulaEspinal", "SpinalCord", "Spinal Cord", "Spinal, Cord", "medula" };
            string[] N_LL = { "Pulmon_I", "Lung_L", "Lung Left", "Lung, Left" };
            string[] N_LR = { "Pulmon_D", "Lung_R", "Lung Right", "Lung, Right" };
            string[] N_Es = { "Esofago", "Esophagus" };
            string[] N_BR = { "Mama_Der", "Breast_R", "MDer", "MD" };
            string[] N_BL = { "Mama_I", "Breast_L", "MIzq", };
            string[] N_Tr = { "Traquea", "Trachea", "traquea" };//aumentar corazon
            string[] N_Cor = { "Corazon", "Heart", "corazon" };//aumentar corazon
            string[] N_Intes = { "Intestino", "Bowel", "intestino" };//aumentar corazon
            string[] N_Cardiaca = { "Reg_Cardiaca_Izq" };//aumentar corazon
            string[] N_CoroI = { "Reg_A_CoronariaI", "Reg_CoronariaI", "Reg_Coronaria_I" };//aumentar corazon
            string[] N_VentI = { "Reg_Ventriculo_I", "Reg_VentriculoI" };//aumentar corazon
            
            Structure ctv_ID11 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Chest.Any(x => s.Id.Contains(x)));
            if (result == DialogResult.No || result == DialogResult.Cancel) //esto no lo convierto en alta resolucion para que haga la superficie, despues si
            {
                if (ctv_ID11 == null)
                {
                    System.Windows.MessageBox.Show(string.Format("'{0}' not found!", N_Chest[0]), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;//es la unica por el momento para terminar la aplicacion
                }
                else VerifStLow(ctv_ID11, true, N_Chest[0],inicio);
            }
            //Crea las estructuras necesarias
            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LNI.Any(x => s.Id.Contains(x)));
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LNII.Any(x => s.Id.Contains(x)));
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LNIII.Any(x => s.Id.Contains(x)));
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Rotter.Any(x => s.Id.Contains(x)));
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Sclav.Any(x => s.Id.Contains(x)));
            Structure ctv_ID7 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_IMN.Any(x => s.Id.Contains(x)));
            Structure ctv_ID8 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Dist.Any(x => s.Id.Contains(x)));
            Structure ctv_ID9 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Prox.Any(x => s.Id.Contains(x)));
            Structure ctv_ID10 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(ctv_ID7) || !HighResol(ctv_ID8) || !HighResol(ctv_ID9) || !HighResol(ctv_ID10) || !HighResol(ctv_ID11))
            {
                Low = false;
                //VerifSt(ctv_ID1, true, N_Breast[0]);
                VerifSt(ctv_ID2, false, N_LNI[0], inicio);
                VerifSt(ctv_ID3, false, N_LNII[0], inicio);
                VerifSt(ctv_ID4, false, N_LNIII[0], inicio);
                VerifSt(ctv_ID5, false, N_Rotter[0], inicio);
                VerifSt(ctv_ID6, false, N_Sclav[0], inicio);
                VerifSt(ctv_ID7, false, N_IMN[0], inicio);
                if (result == DialogResult.Yes) VerifSt(ctv_ID8, true, N_Dist[0], inicio);
                if (result == DialogResult.Yes && ctv_ID8 == null) return;
                if (result == DialogResult.Yes) VerifSt(ctv_ID9, true, N_Prox[0], inicio);
                if (result == DialogResult.Yes && ctv_ID9 == null) return;
                if (result == DialogResult.Yes) VerifSt(ctv_ID10, true, N_SIB[0], inicio);
                if (result == DialogResult.Yes && ctv_ID10 == null) return;
                //if (result == DialogResult.No || result==DialogResult.Cancel) VerifSt(ctv_ID11, true, N_Chest[0]);
                //if (result == DialogResult.No && ctv_ID11 == null) return;
            }
            else
            {
                Low = true;
                //VerifStLow(ctv_ID1, true, N_Breast[0]);
                VerifStLow(ctv_ID2, false, N_LNI[0], inicio);
                VerifStLow(ctv_ID3, false, N_LNII[0], inicio);
                VerifStLow(ctv_ID4, false, N_LNIII[0], inicio);
                VerifStLow(ctv_ID5, false, N_Rotter[0], inicio);
                VerifStLow(ctv_ID6, false, N_Sclav[0], inicio);
                VerifStLow(ctv_ID7, false, N_IMN[0], inicio);
                if (result == DialogResult.Yes) VerifStLow(ctv_ID8, true, N_Dist[0], inicio);
                if (result == DialogResult.Yes && ctv_ID8 == null) return;
                if (result == DialogResult.Yes) VerifStLow(ctv_ID9, true, N_Prox[0], inicio);
                if (result == DialogResult.Yes && ctv_ID9 == null) return;
                if (result == DialogResult.Yes) VerifStLow(ctv_ID10, true, N_SIB[0], inicio);
                if (result == DialogResult.Yes && ctv_ID10 == null) return;
                //if (result == DialogResult.No || result==DialogResult.Cancel) VerifSt(ctv_ID11, true, N_Chest[0]);
                //if (result == DialogResult.No && ctv_ID11 == null) return;
                if (result == DialogResult.No || result == DialogResult.Cancel) //esto no lo convierto en alta resolucion para que haga la superficie, despues si
                {
                    VerifStLow(ctv_ID11, true, N_Chest[0], inicio);
                    if (ctv_ID11 == null) return;
                }
            }
            Structure ctv_ID1 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Breast.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //if (result == DialogResult.Yes) VerifSt(ctv_ID1, true, N_Breast[0]);//es necesario true
            if (result == DialogResult.Yes && ctv_ID1 == null)// esto no lo convierto en alta resolucion para que haga la superficie, despues si
            {
                System.Windows.MessageBox.Show(string.Format("'{0}' not found!", N_Breast[0]), SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;//es la unica por el momento para terminar la aplicacion
            }
            else if (result == DialogResult.Yes && ctv_ID1 != null)
            { 
                VerifStLow(ctv_ID1, true, N_Breast[0], inicio);
                //revisar que el SIB no  sea mas del 10% de la mama.
                if (inicio) System.Windows.MessageBox.Show("The percentage of " + ctv_ID10.Id + " Volume with respect to the " + ctv_ID1.Id + " is:\n (V_SIB / V_Mama) * 100 = " + ((100*ctv_ID10.Volume) / ctv_ID1.Volume).ToString("0.00") + "% \n\n Tolerance <=10%", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            //if (result == DialogResult.Cancel && ctv_ID11 == null) return;
            Structure body0 = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));
            VerifSt(body0, true, N_Body[0], inicio);
            //solo cambia nombre
            if (inicio)
            {
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
                VerifStLow(cor, false, N_Cor[0]);
                Structure vi = ss.Structures.FirstOrDefault(s => N_VentI.Any(x => s.Id.Contains(x)));
                VerifStLow(vi, false, N_VentI[0]);
                Structure artc = ss.Structures.FirstOrDefault(s => N_CoroI.Any(x => s.Id.Contains(x)));
                VerifStLow(artc, false, N_CoroI[0]);
                Structure reg_card = ss.Structures.FirstOrDefault(s => N_Cardiaca.Any(x => s.Id.Contains(x)));
                VerifStLow(reg_card, false, N_Cardiaca[0]);
            }
            /*else if (result3 == DialogResult.No && result == DialogResult.Yes) esto era para colocar -05 pero no fue viable
            {
                ctv_ID8.Id  = NDIST;//coloca los nombres -5
                ctv_ID9.Id  = NPROX;
                ctv_ID10.Id = NSIB;
            }*/
            //New Structures Breast 16fx
            string PTV_ID12 = "PTV_GL_Axila_1";//"PTV_LN_Ax_L1";     //"PTV_Ax I"
            string PTV_ID13 = "PTV_GL_Axila_2";//"PTV_LN_Ax_L2";     //"PTV_Ax II"
            string PTV_ID14 = "PTV_GL_Axila_3";//"PTV_LN_Ax_L3";     //"PTV_Ax III"
            string PTV_ID15 = "PTV_GL_Rotter";//"PTV_LN_Rotter";    //"PTV_Rotter"
            string PTV_ID16 = "PTV_GL_Supra";//"PTV_LN_Supra";//"PTV_LN_Sclav";     //"PTV_Supra"
            string PTV_ID17 = "PTV_GL_CMI";//"PTV_LN_CMI"; //"PTV_LN_IMN";       //"PTV_CMI"
            string PTV_ID18 = "PTV_Mama_Dist";//"PTV_Breast_Dist";  //"PTV_MDISTAL"
            string PTV_ID19 = "PTV_Mama_Prox";//"PTV_Breast_Prox";  //"PTV_MPROX"
            string PTV_ID_20 = "PTV_GTV_SIB";//"PTV_GTV_SIB";     //"PTV_SIB"
            string PTV_ID20 = "zPTV_High_5200!";  //"PTV_52Gy"
            string PTV_ID21 = "zPTV_Low_4000!";   //PTV_40Gy
            string PTV_ID22 = "zPTV_Gang_4300!";   //PTV_41Gy
            string PTV_ID23 = "zPTV_Prox_4300!";   //PTV_43.2Gy
            string PTV_ID24 = "zPTV_Total!";      //PTV_Total

            string PTV_ID28 = "zPTV_Mid_4300!";      //PTV_gg+prox mid 16fx
            string PTV_ID28_ = "zPTV_Mid_4600!";      //PTV_gg+prox mid 20fx
            string Ring = "zAnillo";//"zRing";                //Anillo
            string Surface = "zSuperficie";//"zSurface";          //Superficie
            //20fx Breast
            string PTV_ID20_ = "zPTV_High_5600!"; //PTV_56.4Gy//tengo un problema con los IDS por eso le quito el signo de admiracion
            string PTV_ID21_ = "zPTV_Low_4300!";  //PTV_43Gy
            string PTV_ID22_ = "zPTV_Gang_4600!";  //PTV_46Gy
            string PTV_ID23_ = "zPTV_Prox_4600!";  //PTV_45.4Gy
            //Chest wall 16fx
            string PTV_ID25 = "zPTV_High_4400!";  //PTV_44Gy
            //string PTV_ID22__ = "zPTV_Low_4100!";   //PTV_41Gy
            //Chest wall 20fx
            string PTV_ID25_ = "zPTV_High_4700!"; //PTV_47Gy//problema con el id
            //const string PTV_ID26_ = "zPTV_Mid_4600!";  //PTV_46Gy
            string PTV_ID27 = "PTV_Pared";//"PTV_Chestwall";  //PTV pared
            string SURCO ="zSurco";
            /*if (IsResim(ss))
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID17 += ".";
                    PTV_ID18 += ".";
                    PTV_ID19 += ".";
                    PTV_ID20 += ".";
                    PTV_ID_20 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID24 += ".";
                    PTV_ID25 += ".";
                    PTV_ID27 += ".";
                    Ring += ".";
                    Surface += ".";
                    PTV_ID20_ += ".";
                    PTV_ID21_ += ".";
                    PTV_ID22_ += ".";
                    PTV_ID23_ += ".";
                    PTV_ID25_ += ".";
                    PTV_ID28 += ".";
                    PTV_ID28_ += ".";
                }
            }*/
            if (result2 == DialogResult.No)
            {
                PTV_ID20 = PTV_ID20_;  //"PTV_52Gy"
                PTV_ID21 = PTV_ID21_;   //PTV_40Gy
                PTV_ID22 = PTV_ID22_;   //PTV_41Gy
                PTV_ID23 = PTV_ID23_;   //PTV_41Gy
                PTV_ID25 = PTV_ID25_;   //PTV_41Gy
                PTV_ID28 = PTV_ID28_;   //PTV_43Gy
            }
            if (inicio) 
            {
                FindCouch(ss, context);
                DialogResult result_1 = System.Windows.Forms.MessageBox.Show("Do you continue? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result_1 == DialogResult.No)
                {
                    throw new Exception("Se detiene el programa");
                }
            }            
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);
            Structure ptv_ID_20 = Add_structure(ss, "PTV", PTV_ID_20);
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);
            Structure ptv_ID24 = Add_structure(ss, "PTV", PTV_ID24);
            Structure ptv_ID25 = Add_structure(ss, "PTV", PTV_ID25);
            Structure ptv_ID27 = Add_structure(ss, "PTV", PTV_ID27);
            Structure ptv_ID28 = Add_structure(ss, "PTV", PTV_ID28);
            Structure ring = Add_structure(ss, "CONTROL", Ring);
            Structure surface = Add_structure(ss, "AVOIDANCE", Surface);
            Structure buffered_outer = Add_structure(ss, "CONTROL", "outer-5");
            Structure ctv_ID_1 = Add_structure(ss, "CTV", "Breast");//Mama MD o MI
            Structure body = Add_structure(ss, "CONTROL", "High_Body");
            Structure zsurco = Add_structure(ss, "CONTROL", SURCO);
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
                    VerifStLow(ctv_ID1, true, N_Breast[0]);
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
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(surface); St.Add(ctv_ID_1); St.Add(body); St.Add(ptv_ID28); //St.Add(buffered_outer); St.Add(ring); 
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
                if (result2 == DialogResult.Yes) ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23);//41-43.2 46-45.4///
                else if (result2 == DialogResult.No) ptv_ID23.SegmentVolume = ptv_ID23.Sub(ptv_ID22);//45.4-46///
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID20);//41-52 46-56.4
            }
            else if (result == DialogResult.No && !ptv_ID22.IsEmpty)
            {
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID25);//41-44  46-47
            }
            else if (result == DialogResult.Cancel && !ptv_ID22.IsEmpty)
            {
                ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID25);//41-44
            }
            //PTV Total
            if (result == DialogResult.Yes) //no se puede unir dos elementos vacios en alta resolucion??
            {
                ptv_ID24.SegmentVolume = ptv_ID20.Or(ptv_ID21);
                ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID22);
                ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID23);
            }
            else if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                ptv_ID24.SegmentVolume = ptv_ID22.Or(ptv_ID25);//pared
            }
            Structure buffered_ring = Add_structure(ss, "AVOIDANCE", "b_ring");//luego se quita
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
                    Cropbody(ctv_ID11, buffered_outer);//me retrae la pared 
                    Cropbody(ptv_ID25, buffered_outer);
                    Cropbody(ptv_ID27, buffered_outer);
                    ptv_ID24.SegmentVolume = ptv_ID22.Or(ptv_ID25);//pared
                }
            }
            ptv_ID28.SegmentVolume = ptv_ID22;
            if (ptv_ID23 != null && ptv_ID22 != null) ptv_ID28.SegmentVolume = ptv_ID22.Or(ptv_ID23);
            if (ptv_ID23 != null && ptv_ID22 == null) ptv_ID28.SegmentVolume = ptv_ID23;
            if (ptv_ID23.IsEmpty && ptv_ID22.IsEmpty) ss.RemoveStructure(ptv_ID28);

            //Remove Auxilary Structure      
            ss.RemoveStructure(buffered_ring);
            ss.RemoveStructure(ctv_ID_1);
            ss.RemoveStructure(buffered_outer);
            ss.RemoveStructure(body);
            if (result3 == DialogResult.No)
            {
                ss.RemoveStructure(ring);
                ss.RemoveStructure(surface);
                ss.RemoveStructure(zsurco);
            }
            /*
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
                
            }*/
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
            //else if (ptv_ID22 != null && result2 == DialogResult.Yes) ptv_ID22.Id = PTV_ID22__;
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
            else if (!ctv_ID9.IsEmpty && ptv_ID22.IsEmpty)
            {
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
            if (result == DialogResult.Yes && result3 == DialogResult.No) ChangeName(ctv_ID1.Id.Remove(0, 1), ctv_ID1);//le quita la primera letra
        }

        private StructureSet CT_Resim(ScriptContext context, string[] CT_name)// encuentra la CT mas reciente
        {
            StructureSet CT = null;
            if (context.Image.Series.Images.Any(x => CT_name.Any(s => x.Id.Contains(s))))
            {
                CT = context.Patient.StructureSets.Where(s => CT_name.Any(x => s.Id.Contains(x))).OrderByDescending(x => x.HistoryDateTime).First();
            }
            else System.Windows.MessageBox.Show($"No se encontro la CT: {CT_name[0]} revise el nombre del conjunto de estructuras" );


            if (context.Patient.StructureSets.Where(s => CT_name.Any(x => s.Id.Contains(x))).Count() > 1) System.Windows.MessageBox.Show($"Se detectaron {context.Patient.StructureSets.Where(s => CT_name.Any(x => s.Id.Contains(x))).Count()} CT con el nombre: {CT_name[0]}, se trabajara con la CT de fecha {CT.HistoryDateTime}");
            return CT;
        }

        public void St_Breast_ChestWall(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_Breast_ChestWall";
            /*//names for original
            string NPROX    = "CTV_Mama_Prox-05";
            string NDIST    = "CTV_Mama_Dist-05";
            string NSIB     = "GTV_SIB-05";*/
            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            string[] CT_name0 = { "CT_MODIFICADA", "modif", "MODIF", "CT_Modificad", "CT MODIFICAD", "MODIFICAD","modificad" };//para cambiar el nombre despues esto sirva en el script de planing
            string[] CT_name1 = { "CT_ORIGINAL", "origin", "ORIGI", "CT_Original", "CT ORIGINAL", "ORIGINAL","original" };
            //Image CT_ImModif = context.Image.Series.Images.FirstOrDefault(x => CT_name0.Any(s=>x.Id.Contains(s)));//esto obtiene las CTs
            //Image CT_ImOrig = context.Image.Series.Images.FirstOrDefault(x => CT_name0.Any(s => x.Id.Contains(s))); en desuso por lo que sigue
            //StructureSet CT_modificada = CT_Resim(context,CT_name0);En desuso por lo que sigue
            //StructureSet CT_Original = CT_Resim(context, CT_name1); 

            //para ver si es resimulacion
            StructureSet CT_modificada = context.StructureSet;
            ChangeName(CT_name0[0], ss: CT_modificada, img: context.Image);//CAMBIO EL NOMBRE DE LA CT y ss
            StructureSet CT_Original = CT_modificada.Copy();//
            ChangeName(CT_name1[0], ss: CT_Original, img: CT_Original.Image);
            
  
            //context.Patient.StructureSets;
            if (CT_modificada == null || CT_Original == null) 
            { 
                System.Windows.MessageBox.Show("Los nombres del Set de Structuras son incorrectos recuerde que deben ser: CT_MODIFICADA y CT_ORIGINAL, corrija e intente de nuevo");
                return;
            }        

            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            //bool resol=ss.Structures. Any(x => x.IsHighResolution); //encuentra un elemeto de alta resolucion el problema esta que te encuentra de todo
            
            DialogResult result = System.Windows.Forms.MessageBox.Show("Breast or Chest wall or Prosthesis?" + "\n" + "If Yes, the volume is Breast(Mama)." + "\n" + "If No, the volume is Chest wall(Pared)." + "\n" + "If Cancel, the volume is chestwall with expander(Expansor).", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            
            DialogResult result2 = System.Windows.Forms.MessageBox.Show("Fraction: 16Fx or 20Fx?" + "\n" + "If Yes, the volume is 16Fx." + "\n" + "If No, the volume is 20Fx." + "\n" + "If Cancel, Stop Script", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            if (result2 == DialogResult.Cancel) return;
            //le tengo que pasar el structure set y luego los result 12 y para el result 3 debeser yes si estoy en la modificada y no si estoy en  la original
            BreastStructureCreation(CT_modificada, result, result2, DialogResult.Yes,true,context);//aca hace lo de la mama modificada
            BreastStructureCreation(CT_Original, result, result2, DialogResult.No,false,context);//aca hace lo de la mama modificada
        }

        public void St_Rectum20fx(ScriptContext context /*, System.Windows.Window window, ScriptEnvironment environment*/)
        {
            const string SCRIPT_NAME0 = "Script_Rectum20Fx";
            //ctv
            string[] N_SIB      = { "GTV_SIB",              "_SIB", "SIB" };
            string[] N_Lat      = { "CTV_GL_Lateral",       "Laterales", "laterales" };
            string[] N_Meso     = { "CTV_Mesorecto",        "Mesorecto", "mesorecto", "meso" };
            string[] N_Inf      = { "CTV_Recto_Inf",        "Inferior", "inferior" };
            string[] N_Pre      = { "CTV_GL_PreSacro",      "Presacro", "presacro" };
            string[] N_Ing      = { "CTV_GL_Inguinal",      "Inguinal" };
            //oar
            string[] N_Colon    = { "Colon",                "colon","sigma" };
            string[] N_Bladder  = { "Vejiga",               "Bladder",          "vejiga" };
            string[] N_Bowel    = { "Intestino",            "Bowel",            "intestino","intestinos" };
            string[] N_Body     = { "Body",                 "Outer Contour", "BODY" };
            //cambiar nombre
            string[] N_Prostate = { "Prostata",             "Prostate",      "prostata" };
            string[] N_Penile   = { "BulboPeniano",         "PenileBulb",     "Penile Bulb", "Pene B", "penile bulb", "B Pene", "Bulbo", "bulbo peneano","bulbo" };
            string[] N_GM       = { "GluteoMayor",          "Gluteus_Maximus",    "Gluteo Mayor", "gluteos", "Gluteo mayor" };
            string[] N_GS       = { "GluteoPiel",           "Gluteal_Skin",       "Piel Glutea", "pielG", "Piel glutea","piel", "piel glutea" };
            string[] N_HJL      = { "CabezaFemoral_I",     "FemoralJoint_L",    "Hip Joint, Left", "Hip Joint Left",  "CFI" };//hip joint left
            string[] N_HJR      = { "CabezaFemoral_D",      "FemoralJoint_R",    "Hip Joint, Right", "Hip Joint Right",  "CFD" };
            string[] N_SC       = { "MedulaEspinal",        "SpinalCord",         "Spinal Cord", "Spinal, Cord" };
            string[] N_Vagina   = { "Vagina",               "vagina"};
            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver// es para que no se confunda con el PTV_SIB 
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Lat.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Meso.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Inf.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Pre.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Ing.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OAR
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bladder = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver        
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(ctv_ID7) || !HighResol(colon) || !HighResol(bladder) || !HighResol(bowel) )
            {
                Low = false;
                VerifSt(ctv_ID2, false, N_SIB[0]);//es necesario true
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
                VerifStLow(ctv_ID2, false, N_SIB[0]);//es necesario true                
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

            Structure vagina = ss.Structures.FirstOrDefault(s => N_Vagina.Any(x => s.Id.Contains(x)));
            VerifStLow(vagina, false, N_Vagina[0]);

            string PTV_ID12 = "PTV_SIB";//no hay ninguno mas de 15
            string PTV_ID13 = "PTV_GL_Lateral";
            string PTV_ID14 = "PTV_Mesorecto";
            string PTV_ID15 = "PTV_Recto_Inf";
            string PTV_ID16 = "PTV_GL_Presacro";
            string PTV_ID17 = "PTV_GL_Inguinal";
            string PTV_ID21 = "zPTV_Low_4600!";
            string PTV_ID22 = "zPTV_Mid_4800!";
            string PTV_ID22_ = "zPTV_Low_4800!";
            string PTV_ID23 = "zPTV_High_5200!";
            string PTV_ID24 = "zPTV_High_5400!";
            string PTV_ID25 = "zPTV_High_5900!";
            string PTV_ID26 = "zPTV_Low_4900!";
            string PTV_ID27 = "zPTV_Total!";
            string PRV_Colon = "Colon_PRV05";//
            string PRV_Bowel = "Intestino_PRV05";//
            FindCouch(ss, context);//se fija si no te olvidaste la camila o el CT
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))//resimulacion??
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16.Remove(PTV_ID16.Length - 1);
                    PTV_ID17 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID24 += ".";
                    PTV_ID25 += ".";
                    PTV_ID27 += ".";
                    PRV_Colon += ".";
                    PRV_Bowel += ".";
                }
            }*/

            DialogResult result = System.Windows.Forms.MessageBox.Show("OPtions:" + "\n" + "1.-If Yes, SIB of 52Gy" + "\n" +
    "2.-If No, SIB of 54Gy" + "\n" + "3.-If Cancel, SIB of 59Gy", SCRIPT_NAME0, MessageBoxButtons.YesNoCancel);
            //============================
            // GENERATE  expansion of PTV
            //============================
            Structure buffered_outer = Add_structure(ss, "PTV", "Aux");
            buffered_outer.SegmentVolume = body.Margin(-5.0);

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);//ptv sib todos
            Structure ptv_ID27 = Add_structure(ss, "PTV", PTV_ID27);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_Colon);
            Structure prv_intestino = Add_structure(ss, "CONTROL", PRV_Bowel);
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID27); St.Add(prv_colon); St.Add(prv_intestino);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }

            if (ctv_ID2 != null)
            {
                ptv_ID12.SegmentVolume = ctv_ID2.Margin(9.0); //PTV sib
                ptv_ID23.SegmentVolume = ptv_ID12;//ptv 52gy
                Cropbody(ptv_ID12, buffered_outer);
                Cropbody(ptv_ID23, buffered_outer);
            }
            if (ctv_ID3 != null)
            {
                ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV latera
                Cropbody(ptv_ID13, buffered_outer);
            }
            //else ss.RemoveStructure(ctv_ID3); ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 != null)
            {
                ptv_ID14.SegmentVolume = ctv_ID4.Margin(9.0); //PTV mesorecto
                Cropbody(ptv_ID14, buffered_outer);
            }
            //else ss.RemoveStructure(ctv_ID4); ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 != null)
            {
                ptv_ID15.SegmentVolume = ctv_ID5.Margin(9.0); //PTV inf
                Cropbody(ptv_ID15, buffered_outer);
            }
            if (ctv_ID6 != null)
            {
                ptv_ID16.SegmentVolume = ctv_ID6.Margin(6.0); //PTV presacral
                Cropbody(ptv_ID16, buffered_outer);
            }
            if (ctv_ID7 != null)
            {
                ptv_ID17.SegmentVolume = ctv_ID7.Margin(6.0); //PTV inguinal
                Cropbody(ptv_ID17, buffered_outer);
            }

            if (!ptv_ID13.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID13);                //union ptv 48   22=48
            if (!ptv_ID14.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14);
            if (!ptv_ID15.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID15);
            if (!ptv_ID16.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID16);
            ptv_ID21.SegmentVolume = ptv_ID17;//ptv46gy
            if (!ptv_ID22.IsEmpty && ptv_ID23!=null && !ptv_ID23.IsEmpty) ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23);//48-52
            if (!ptv_ID21.IsEmpty) ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID22);//46-48

            //PTV total
            ptv_ID27.SegmentVolume = ptv_ID27.Or(ptv_ID23);// ptv total = sib
            if (!ptv_ID22.IsEmpty) ptv_ID27.SegmentVolume = ptv_ID27.Or(ptv_ID22);//58.4+48
            if (!ptv_ID21.IsEmpty) ptv_ID27.SegmentVolume = ptv_ID27.Or(ptv_ID21);//58.4+43
            Cropbody(ptv_ID22, buffered_outer);
            Cropbody(ptv_ID21, buffered_outer);
            Cropbody(ptv_ID27, buffered_outer);
            if (colon != null) prv_colon.SegmentVolume = colon.Margin(5.0);// PRV 
            if (bowel != null) prv_intestino.SegmentVolume = bowel.Margin(5.0);// PRV 

            if (result == DialogResult.No)
            {
                ChangeName( PTV_ID24, ptv_ID23);
            }
            else if (result == DialogResult.Cancel)
            {
                ChangeName( PTV_ID25, ptv_ID23);
                if (ptv_ID22!=null) ChangeName( PTV_ID26, ptv_ID22);

            }
            if (ctv_ID2 == null)
            {
                ss.RemoveStructure(ptv_ID23);
                ss.RemoveStructure(ptv_ID12);
            }
            if (ctv_ID3 == null) ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 == null) ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 == null) ss.RemoveStructure(ptv_ID15);
            if (ctv_ID6 == null) ss.RemoveStructure(ptv_ID16);
            if (ctv_ID7 == null)
            {
                ss.RemoveStructure(ptv_ID17);
                ss.RemoveStructure(ptv_ID21);
                if (result != DialogResult.Cancel) ChangeName(PTV_ID22_, ptv_ID22);
            }
            if (ctv_ID3 == null && ctv_ID4 == null && ctv_ID5 == null && ctv_ID6 == null) ss.RemoveStructure(ptv_ID22);

            ss.RemoveStructure(buffered_outer);
        }

        public void St_CYC_25fx(ScriptContext context)
        {
            const string SCRIPT_NAME0 = "CYC";
            //gtv-ctvs
            string[] N_GTV      = {"GTV_SIB",           "SIB", "sib base leng", "1 GTV supra y gl" };
            string[] N_CTV      = {"CTV_Peritumoral",    "CTV_Tumor", "CTV_Peritumor", "5- CTV peritumor" };
            string[] N_NL       = {"CTV_GL_Cuello_I",   "CTV_LN_Neck_L",    "cuello izq", "Cuello izq", "cuello izq", "3 Cuello izquier" };
            string[] N_NR       = {"CTV_GL_Cuello_D",   "CTV_LN_Neck_R",    "cuello der", "Cuello d", "cuello derech", "4 Cuello derecho" };
            string[] N_ADPR     = {"GTV_ADP_D",         "GTV_ADP_R",        "adp der", "ADP D", "sib adp izq" };
            string[] N_ADPL     = {"GTV_ADP_I",         "GTV_ADP_L",        "adp izq", "ADP I" };
            string[] N_N1       = {"CTV_GL_Nivel_IA",   "CTV_LN_NI_A",      "Nivel Ia" };//50gy=cuello
            //oars
            string[] N_Brainst  = {"TroncoCerebral",   "Brainstem",     "tronco" };
            string[] N_Parotid_L= {"Parotida_I",       "Parotid_L",   "parotida izq", "parotid i", "Parotid Gland, L", "Parotida Izq" };
            string[] N_Parotid_R= {"Parotida_D",       "Parotid_R",   "parotida der", "parotid d", "Parotid Gland, R", "Parotida D" };
            string[] N_Body     = {"Body",             "Outer Contour", "body", "BODY" };
            string[] N_SC       = {"MedulaEspinal",    "SpinalCord",   "Spinal Cord", "Sc", "sc" , "Spinal Cord, Nec" };
            string[] N_OpticNR  = {"NervioOptico_D",   "OpticNrv_R",        "NOD" };//falta
            string[] N_OpticNL  = {"NervioOptico_I",   "OpticNrv_L",        "NOI" };
            string[] N_Coclea   = {"Coclea",           "cochlea", "Cochlea" };

            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.
            DialogResult result = System.Windows.Forms.MessageBox.Show("Unio los huecos de aire del contorno externo? Recuerde que el script retrae con respecto a esta estructura" , SCRIPT_NAME0, MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;// mensaje para no perder los CTVs

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_GTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_NL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_NR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_ADPR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_ADPL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID8 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_N1.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OAR CON PRV
            Structure brainstem = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Brainst.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure sc = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_SC.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OARS
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

            Structure body0 = Add_structure(ss, "CONTROL", "Body0");
            body0.SegmentVolume = body;

            string Dientes = "zDientes";//no hay ninguno mas de 15
            string Inf = "zReg_Inferior";
            string MR = "zReg_Boca";
            string Af = "zArtefacto";
            string Lips = "zLabios";
            string PTV_ID12 = "PTV_SIB";
            string PTV_ID13 = "PTV_Peritumoral";
            string PTV_ID14 = "PTV_GL_Cuello_I";
            string PTV_ID15 = "PTV_GL_Cuello_D";
            string PTV_ID16 = "PTV_ADP_D";
            string PTV_ID17 = "PTV_ADP_I";
            string PTV_ID18 = "PTV_GL_Nivel_IA";
            string PTV_ID21 = "zPTV_Low_5000!";
            string PTV_ID22 = "zPTV_Mid_5800!";
            string PTV_ID23 = "zPTV_High_6700!";
            string PTV_ID25 = "zPTV_Total!";
            string PRV_Brainstem = "Tronco_PRV05";
            string PRV_SC = "MedulaEsp_PRV05";//
            FindCouch(ss, context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12.Remove(PTV_ID12.Length - 1);
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID17 += ".";
                    PTV_ID18 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID25 += ".";
                    Inf.Remove(Inf.Length - 1);
                    PRV_Brainstem += ".";
                    PRV_SC += ".";
                    Dientes += ".";
                    MR += ".";
                    Af += ".";
                    Lips += ".";
                }
            }*/

            //============================
            // GENERATE  expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);//n1
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);
            Structure ptv_ID25 = Add_structure(ss, "PTV", PTV_ID25);

            Structure prv_brainstem = Add_structure(ss, "CONTROL", PRV_Brainstem);
            Structure prv_sc = Add_structure(ss, "CONTROL", PRV_SC);
            Structure dientes = Add_structure(ss, "CONTROL", Dientes);
            Structure mr = Add_structure(ss, "CONTROL", MR);
            Structure af = Add_structure(ss, "CONTROL", Af);
            Structure inf = Add_structure(ss, "CONTROL", Inf);
            Structure lips = Add_structure(ss, "CONTROL", Lips);

            Structure auxi = Add_structure(ss, "CONTROL", "Auxi");//auxiliar 
            Structure auxi2 = Add_structure(ss, "CONTROL", "auxi2");//auxiliar 
            Structure auxi3 = Add_structure(ss, "CONTROL", "auxi3");//auxiliar 
            Structure buff_body = Add_structure(ss, "CONTROL", "buff_body");//auxiliar
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

        public void St_Cervix_20fx(ScriptContext context)//es 20fx
        {
            const string SCRIPT_NAME0 = "Cervix";
            //gtv-ctvs
            string[] N_GTV      = { "GTV_Cervix",       "Tumor","GTV" };  //TU
            string[] N_CTV3     = { "CTV_Paramet_I" };          //PARAMETRIO DER
            string[] N_CTV4     = { "CTV_Paramet_D" };          //PARAMETRIO IZQ
            string[] N_CTV5     = { "CTV_Resto_Utero", "RU" };        //RESTO DE UTERO
            string[] N_CTV6     = { "CTV_Resto_Vagina","RestoVagina" };        //RESTO DE VAGINA
            string[] N_CTV7     = { "CTV_GL_Iliacos" };           //ILIACOS
            string[] N_CTV8     = { "CTV_Gl_Presacro" };        //PRESACROS
            string[] N_CTV9     = { "CTV_GL_LumbAort",  "LAO", "RP" }; //LAO
            string[] N_CTV10    = { "CTV_ADP_I" };              //ADP este se anade al de 58.4
            string[] N_CTV13    = { "CTV_ADP_II"};              //ADP PEDIDO POR MURINA solo se crea u PTV no se anade a ninguno de dosis
            string[] N_CTV11    = { "CTV_GL_Pelvicos", "RG" };          //GANGLIOS PELVICOS
            string[] N_CTV12    = { "CTV_Parametrios", "PARAMETRIOS" };        //PARAMETRIOS UNIDOS SOLO SI LOS DOS PARAMETRIOS VAN A LA MISMA DOSIS

            //oars
            string[] N_Colon    = { "Colon",        "colon", "COLON" };
            string[] N_Bladder  = { "Vejiga",        "Bladder",          "Vejiga","vejiga"};
            string[] N_Rectum   = { "Recto",          "Rectum",           "Recto" };
            string[] N_Bowel    = { "Intestino",       "Bowel",            "intestino","intestinos", "Intestinos", "INTESTINO" };
            string[] N_Body     = { "Body",             "Outer Contour", "body", "BODY" };
            string[] N_FJL      = { "CabezaFemoral_I", "FemoralJoint_L", "Hip Joint, Left" };
            string[] N_FJR      = { "CabezaFemoral_D", "FemoralJoint_R", "Hip Joint, Right" };
            string[] N_KL       = { "Ri�on_I","Kidney_L", "Kidney, Left" };
            string[] N_KR       = { "Ri�on_D", "Kidney_R" , "Kidney, Right" };
            string[] N_SC       = { "MedulaEspinal","SpinalCord",       "Spinal Cord", "Sc", "sc" };
            string[] N_LIVER    = { "Higado","Liver"  };
            string[] GASTRO     = { "Reg_Gastrointes","Gastrointest_Reg" };
            if (context.Patient == null || context.StructureSet == null)
            {
                System.Windows.MessageBox.Show("Please load a patient, 3D image, and structure set before running this script.", SCRIPT_NAME0, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            StructureSet ss = context.StructureSet;
            context.Patient.BeginModifications();   // enable writing with this script.

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_GTV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV3.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV4.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV5.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV6.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID7 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV7.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID8 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV8.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID9 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV9.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID10 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV10.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID11 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV11.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID12 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV12.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID13 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_CTV13.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OAR CON PRV
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bladder = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //SOLO NOMBRES
            Structure hjl = ss.Structures.FirstOrDefault(s => N_FJL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure hjr = ss.Structures.FirstOrDefault(s => N_FJR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure nkl = ss.Structures.FirstOrDefault(s => N_KL.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure nkr = ss.Structures.FirstOrDefault(s => N_KR.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure nsc = ss.Structures.FirstOrDefault(s => N_SC.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure nliver = ss.Structures.FirstOrDefault(s => N_LIVER.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure gastro = ss.Structures.FirstOrDefault(s => GASTRO.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
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

            Structure body = ss.Structures.FirstOrDefault(s => N_Body.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            VerifSt(body, true, N_Body[0]);
            if (body == null) return;

            //Solo cambia nombre
            VerifSt(hjl, false, N_FJL[0]);//es necesario true
            VerifSt(hjr, false, N_FJR[0]);//es necesario true
            VerifSt(nkl, false, N_KL[0]);//es necesario true
            VerifSt(nkr, false, N_KR[0]);//es necesario true
            VerifSt(nsc, false, N_SC[0]);//es necesario true
            VerifSt(nliver, false, N_LIVER[0]);//es necesario true
            VerifSt(gastro, false, GASTRO[0]);

            //New Structures 
            string PTV_ID12 = "PTV_GTV_Cervix";//no hay ninguno de mas de 15/solo1
            string PTV_ID13 = "PTV_Paramet_I";
            string PTV_ID14 = "PTV_Paramet_D";
            string PTV_ID15 = "PTV_Resto_Utero";
            string PTV_ID16 = "PTV_Resto_Vagina";
            string PTV_ID17 = "PTV_GL_Iliacos";
            string PTV_ID18 = "PTV_GL_Presacro";
            string PTV_ID19 = "PTV_GL_LumbAort";
            string PTV_ID20 = "PTV_ADP_I";
            string PTV_ID21 = "zPTV_Low_4300!";
            string PTV_ID22 = "zPTV_Mid_4800!";
            string PTV_ID31 = "zPTV48-BowelPRV";//me falta colocar la funcion de esto me parece bueno que este
            string PTV_ID23 = "zPTV_High_5840!";
            string PTV_ID24 = "zPTV5840-PRVs!";  //PTV58.4-PRVs
            string PTV_ID25 = "zPTV_Total!";
            string PTV_ID26 = "zPTV58_Recto!";//PTV58.4 interseccion PRV recto
            string PTV_ID27 = "zPTV58_Vejiga!"; //PTV58.4 interseccion PRV vejiga
                                                        //PTVs de la variante
            string PTV_ID28 = "PTV_GL_Pelvicos"; //
            string PTV_ID29 = "PTV_Parametrios";
            string PTV_ID30 = "PTV_ADP_II";

            string PRV_Rectum = "Recto_PRV05";
            string PRV_Colon = "Colon_PRV05";//
            string PRV_Bowel = "Intestino_PRV05";//
            string PRV_Bladder = "Vejiga_PRV05";//
            FindCouch(ss, context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID17 += ".";
                    PTV_ID18.Remove(PTV_ID18.Length - 1);
                    PTV_ID19 += ".";
                    PTV_ID20 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID25 += ".";
                    PTV_ID26.Remove(PTV_ID26.Length - 1);
                    PTV_ID27.Remove(PTV_ID27.Length - 1);
                    PTV_ID28 += ".";
                    PTV_ID29 += ".";
                    PTV_ID30 += ".";
                    PRV_Rectum += ".";
                    PRV_Colon += ".";
                    PRV_Bowel += ".";
                    PRV_Bladder += ".";
                }
            }*/
            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);
            Structure ptv_ID24 = Add_structure(ss, "PTV", PTV_ID24);
            Structure ptv_ID25 = Add_structure(ss, "PTV", PTV_ID25);
            Structure ptv_ID26 = Add_structure(ss, "PTV", PTV_ID26);
            Structure ptv_ID27 = Add_structure(ss, "PTV", PTV_ID27);

            Structure ptv_ID28 = Add_structure(ss, "PTV", PTV_ID28);//PTV DE LOS GANGLIOS
            Structure ptv_ID29 = Add_structure(ss, "PTV", PTV_ID29);//PTV DE LOS PARAMETRIOS
            Structure ptv_ID30 = Add_structure(ss, "PTV", PTV_ID30);//ADPII
            Structure ptv_ID31 = Add_structure(ss, "PTV", PTV_ID31);//ptv48-PRVbowel

            Structure prv_rectum = Add_structure(ss, "CONTROL", PRV_Rectum);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_Colon);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_Bowel);
            Structure prv_bladder = Add_structure(ss, "CONTROL", PRV_Bladder);

            Structure auxi = Add_structure(ss, "CONTROL", "Auxi");//auxiliar para mochar

            DialogResult result = System.Windows.Forms.MessageBox.Show("Options:" + "\n" + "1.-If Yes, Parametrium left(izq) is inside 58.4Gy" + "\n" +
    "2.-If No, Parametrium right(der) is inside 58.4Gy" + "\n" + "3.-If Cancel, both(ambos) of them are inside 58.4Gy", SCRIPT_NAME, MessageBoxButtons.YesNoCancel);
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13); St.Add(ptv_ID14); St.Add(ptv_ID15); St.Add(ptv_ID16); St.Add(ptv_ID17); St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
                St.Add(ptv_ID21); St.Add(ptv_ID22); St.Add(ptv_ID23); St.Add(ptv_ID24); St.Add(ptv_ID25); St.Add(ptv_ID26); St.Add(ptv_ID27); St.Add(ptv_ID28); St.Add(ptv_ID29); St.Add(ptv_ID30); St.Add(ptv_ID31);///////
                St.Add(prv_rectum); St.Add(prv_colon); St.Add(prv_bowel); St.Add(prv_bladder); St.Add(auxi); //St.Add(rectum); St.Add(bladder); St.Add(bowel);
                foreach (Structure x in St) x.ConvertToHighResolution();
            }
            ptv_ID12.SegmentVolume = ctv_ID2.Margin(9.0); //PTV Tumor cuello
            if (ctv_ID3 != null) ptv_ID13.SegmentVolume = ctv_ID3.Margin(6.0); //PTV param i
            //else ss.RemoveStructure(ctv_ID3); ss.RemoveStructure(ptv_ID13);
            if (ctv_ID4 != null) ptv_ID14.SegmentVolume = ctv_ID4.Margin(6.0); //PTV param d
            //else ss.RemoveStructure(ctv_ID4); ss.RemoveStructure(ptv_ID14);
            if (ctv_ID5 != null)
            {
                ptv_ID15.SegmentVolume = ctv_ID5.Margin(9.0); //PTV Utero
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID15); //ptv utero 
            }
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
            if (ctv_ID10 != null)
            {
                ptv_ID20.SegmentVolume = ctv_ID10.Margin(6.0); //PTV ADP
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID20);//anade al de 58.4
                ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID20);
            }
            else 
            { 
                ss.RemoveStructure(ctv_ID10); 
                ss.RemoveStructure(ptv_ID20); 
            }

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
            prv_bowel.SegmentVolume = bowel.Margin(5.0);// PRV 

            if (result == DialogResult.Yes)
            {
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID12);//PTV tumor+param I 58.4
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID13);//PTV tumor+param I 58.4
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14); //ptv param d utero 48
                auxi.SegmentVolume = ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 14, 0, 0, 28, 0, 0)).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 0, 50, 0, 0, 0, 0)); ;// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            }
            else if (result == DialogResult.No)
            {
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID12);//PTV tumor+param D 58.4                
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID14);//PTV tumor+param D 58.4                
                ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID13); //ptv param i utero 48
                auxi.SegmentVolume = ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 28, 0, 0, 14, 0, 0)).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 0, 50, 0, 0, 0, 0)); ;// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            }
            else
            {
                ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID12);
                if (ptv_ID13 != null)
                {
                    ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID13);//PTV tumor+param I 58.4
                    ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID13); //ptv param i utero 48
                }
                if (ptv_ID14 != null)
                {
                    ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID14);//PTV tumor+param D 58.4
                    ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID14); //ptv param i+d utero 48
                }
                if (ptv_ID29 != null)
                {
                    ptv_ID23.SegmentVolume = ptv_ID23.Or(ptv_ID29);//PTV tumor+param Ambos si hay 58.4
                    ptv_ID22.SegmentVolume = ptv_ID22.Or(ptv_ID29); //ptv param i+d+ambos utero 48
                }
                auxi.SegmentVolume = (ptv_ID23.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 25, 0, 0, 25, 0, 0))).AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Outer, 0, 50, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            }

            //Struture Auxiliar for creation of lateral crop in PRV Bladder. esto es solo para parametrio izq

            ptv_ID27.SegmentVolume = auxi.And(bladder);// intersection with bladder
            ptv_ID27.SegmentVolume = ptv_ID27.And(ptv_ID23);//lo intersecto para que nos e vaya por arriba
            ptv_ID24.SegmentVolume = ptv_ID24.Or(ptv_ID23); //PTV58.4-PRVs! - PRV Rectum+3mm
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(ptv_ID27.Margin(6)); //PTV58.4-PRVs! - PRV Rectum+3mm
            /*//ptv_ID27.SegmentVolume = auxi;
            auxi.SegmentVolume = auxi.AsymmetricMargin(new AxisAlignedMargins(0, 3, 50, 3, 3, 3, 3)); ;//mochado+3mm+5cm hacia arriba para cortar
            ptv_ID27.SegmentVolume = auxi.And(ptv_ID23);//PTV58.4*Bladder mochado para que corte lo necesario modificacdo por carola
            ptv_ID27.SegmentVolume = ptv_ID27.And(bladder);//PTV58.4*PRV Bladder mochado para que corte lo necesario
            ////////////////////////////////////////////PTV58.4-Prvs
            auxi.SegmentVolume = auxi.Margin(3);
            ptv_ID24.SegmentVolume = ptv_ID23.Sub(auxi);//PTV58.4-PRVs! - PRV Vejiga+3mm*/
            //auxi.SegmentVolume = prv_rectum;// ahora el auxi es la extension del prv recto
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(prv_rectum); //PTV58.4-PRVs! - PRV Rectum+3mm
            //auxi.SegmentVolume = prv_intestino.Margin(1.0);// ahora el auxi es la extension del prv intestino
            ptv_ID24.SegmentVolume = ptv_ID24.Sub(prv_bowel.Margin(3.0)); //PTV58.4-PRVs! - PRV Rectum+3mm
            //auxi.SegmentVolume = prv_colon;// ahora el auxi es la extension del prv colon
            if (colon!=null)ptv_ID24.SegmentVolume = ptv_ID24.Sub(prv_colon); //PTV58.4-PRVs! - PRV Rectum+3mm
                                                         //////////////////////////////////////////////////////
            ptv_ID22.SegmentVolume = ptv_ID22.Sub(ptv_ID23); //48-58.4
            ptv_ID21.SegmentVolume = ptv_ID19.Sub(ptv_ID22); //43-48
            ptv_ID21.SegmentVolume = ptv_ID21.Sub(ptv_ID23); //43-58.4

            //PTV total
            ptv_ID25.SegmentVolume = ptv_ID23.Or(ptv_ID22);//58.4+48
            ptv_ID25.SegmentVolume = ptv_ID25.Or(ptv_ID21);//58.4+43
            //PTV58.4*RECto dado por carola
            ptv_ID26.SegmentVolume = ptv_ID23.And(rectum);
            if (ptv_ID22 != null) ptv_ID31.SegmentVolume = ptv_ID22.Sub(prv_bowel);
            else ss.RemoveStructure(ptv_ID22);

            ss.RemoveStructure(auxi);
            if (gastro==null)ss.RemoveStructure(gastro);
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
            string[] N_Body = {     "Body",             "Outer Contour", "body", "BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

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
            string PTV_ID12 = "PTV_Prostate";
            string PTV_ID13 = "PTV_LN_Obturator";
            string PTV_ID14 = "PTV_SeminalVes";//
            string PTV_ID15 = "PTV_SIB";//
            string PTV_ID16 = "PTV_LN_Pelvic";//
                                                     //const string PTV_ID15 = "PTV_Trigone!";//
                                                     //const string PTV_ID16 = "PTV_RectumPRV05!";
                                                     //const string PTV_ID18 = "PTVLn-PTVpros!";
            string PTV_ID19 = "PTV_Total!";
            string PTV_ID20 = "PTV_High_4320";
            string PTV_ID21 = "PTV_Mid_3900";
            string Rect_ant = "Rectum_A";
            string Rect_post = "Rectum_P";
            string PRV_Sigma = "Colon_PRV05!";//
            string PRV_Intestino = "Bowel_PRV05!";//
            FindCouch(ss, context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13.Remove(PTV_ID13.Length - 1);
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID19 += ".";
                    PTV_ID20 += ".";
                    PTV_ID21 += ".";
                    Rect_ant += ".";
                    Rect_post += ".";
                    PRV_Sigma += ".";
                    PRV_Intestino += ".";
                }
            }*/

            // create the empty "ptv+5mm" structure ans auxilary structures
            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);//prost
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);//gg
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);//semv
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);//sib
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);//prost
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);//total
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);// 43200
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);//3900

            Structure rect_ant = Add_structure(ss, "CONTROL", Rect_ant);
            Structure rect_post = Add_structure(ss, "CONTROL", Rect_post);
            Structure prv_sigma = Add_structure(ss, "CONTROL", PRV_Sigma);
            Structure prv_intestino = Add_structure(ss, "CONTROL", PRV_Intestino);
            Structure uretra2 = Add_structure(ss, "PTV", "Buffer_U");
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
            //Structure aux                 = Add_structure(ss, "CONTROL", "auxilary");//axilary
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
            string[] N_Prostate = { "CTV_ProstateBed",  "CTV_Prostata", "1-Prostata","Lecho" };
            string[] N_LN = {       "CTV_LN_Obturator", "ganglio", "ganglios", "obturador" };
            string[] N_VS = {       "CTV_SeminalVes",   "VSem", "Vesiculas", "Vesiculas Sem", "VS+1cm" };
            string[] N_SIB = {      "GTV_SIB",          "_SIB", "nodulo" };
            string[] N_GP = {       "CTV_LN_Pelvic",    "Pelviano", "Pelvico", "ganglios pelvicos", "RegGanglionares" };
            //string[] N_Urethra = {  "Urethra",          "Uretra", "uretra" };
            //string[] N_Trigone = {  "Trigone",          "trigono", "Trigono" };
            string[] N_Rectum = {   "Rectum",           "recto", "rectum" };
            string[] N_Colon = {    "Colon",            "colon", "sigma" };
            string[] N_Bowel = {    "Bowel",            "bowels", "intestinos", "Intestino","INTESTINO" };
            string[] N_Body = {     "Body",             "Outer Contour", "body", "BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Prostate.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_VS.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID5 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID6 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_GP.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //Structure uretra = ss.Structures.FirstOrDefault(s => N_Urethra.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //Structure trigono = ss.Structures.FirstOrDefault(s => N_Trigone.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //OAR CON PRV
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

            bool Low = false; //determina si las estructuras son baja resolucion, por defecto es alta
            if (!HighResol(ctv_ID2) || !HighResol(ctv_ID3) || !HighResol(ctv_ID4) || !HighResol(ctv_ID5) || !HighResol(ctv_ID6) || !HighResol(rectum) || !HighResol(colon) || !HighResol(bowel))
            {
                Low = false;
                VerifSt(ctv_ID2, true, N_Prostate[0]);//es necesario true
                if (ctv_ID2 == null) return;
                VerifSt(ctv_ID3, false, N_LN[0]);//es necesario true/false
                VerifSt(ctv_ID4, false, N_VS[0]);//es necesario true
                VerifSt(ctv_ID5, false, N_SIB[0]);//es necesario true
                VerifSt(ctv_ID6, false, N_GP[0]);//es necesario true
                //VerifSt(uretra, true, N_Urethra[0]);//es necesario true
                //VerifSt(trigono, true, N_Trigone[0]);//es necesario true
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
                //VerifStLow(uretra, true, N_Urethra[0]);//es necesario true
                //VerifStLow(trigono, true, N_Trigone[0]);//es necesario true
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
            string PTV_ID12 = "PTV_Prostate";
            string PTV_ID13 = "PTV_LN_Obturator";
//            string PTV_ID14 = "zPTV_Urethra!";
//            string PTV_ID15 = "zPTV_Trigone!";
            string PTV_ID16 = "zPTV_RectumPRV05";
//            string PTV_ID17 = "zPTV-PRVs!";
            string PTV_ID18 = "PTV_SeminalVes";//
            string PTV_ID19 = "zPTV_Total!";
            string PTV_ID20 = "zPTV_High_5700!";
            string PTV_ID21 = "zPTV_Low_4400!";
            string PTV_ID22 = "zPTV_Mid_5100!";
            string PTV_ID23 = "PTV_SIB";
            string PTV_ID24 = "PTV_LN_Pelvic";
            string PRV_Rectum = "Rectum_PRV05";
            string Rect_ant = "Rectum_A";
            string Rect_post = "Rectum_P";
            string PRV_colon = "Colon_PRV05";//
            string PRV_bowel = "Bowel_PRV05";//
            FindCouch(ss,context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar

            /*if (IsResim(ss))//RE SIMULAICON
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13.Remove(PTV_ID13.Length - 1);
                    //PTV_ID14 += ".";
                    //PTV_ID15 += ".";
                    PTV_ID16.Remove(PTV_ID16.Length - 1);
                    //PTV_ID17 += ".";
                    PTV_ID18 += ".";
                    PTV_ID19 += ".";
                    PTV_ID20 += ".";
                    PTV_ID21 += ".";
                    PTV_ID22 += ".";
                    PTV_ID23 += ".";
                    PTV_ID24 += ".";
                    PRV_Rectum += ".";
                    Rect_ant += ".";
                    Rect_post += ".";
                    PRV_colon += ".";
                    PRV_bowel += ".";
                }
            }*/
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);
            //Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);
            //Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);
            //Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);//ptv20 arriba
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);
            Structure ptv_ID22 = Add_structure(ss, "PTV", PTV_ID22);
            Structure ptv_ID23 = Add_structure(ss, "PTV", PTV_ID23);
            Structure ptv_ID24 = Add_structure(ss, "PTV", PTV_ID24);
            Structure prv_rectum = Add_structure(ss, "CONTROL", PRV_Rectum);
            Structure rect_ant = Add_structure(ss, "CONTROL", Rect_ant);
            Structure rect_post = Add_structure(ss, "CONTROL", Rect_post);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_colon);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_bowel);
            //Structure uretra2 = Add_structure(ss, "PTV", "Buffer_U");
            if (!Low)
            {
                List<Structure> St = new List<Structure>();//convierto todos a alta resolucion
                St.Add(ptv_ID12); St.Add(ptv_ID13);  St.Add(ptv_ID16);  St.Add(ptv_ID18); St.Add(ptv_ID19); St.Add(ptv_ID20);
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
            //Structure aux                 = Add_structure(ss, "CONTROL", "auxilary");//axilary
            prv_rectum.SegmentVolume = rectum.Margin(5.0);// PRV Rectum
            rect_post.SegmentVolume = rectum.AsymmetricMargin(new AxisAlignedMargins(StructureMarginGeometry.Inner, 0, 17, 0, 0, 0, 0));// Enumeradores Enum: StructureMarginGeometry.Inner se llama con la clase y el identificador esto devuelve un valor de la lista.
            rect_ant.SegmentVolume = rectum.Sub(rect_post);

            //ptv_ID14.SegmentVolume = ptv_ID12.And(uretra);//PTV*U
            //ptv_ID15.SegmentVolume = ptv_ID12.And(trigono);//PTV*T
            ptv_ID16.SegmentVolume = ptv_ID12.And(prv_rectum);//PTV*PRVV re
            //ptv_ID17.SegmentVolume = uretra.Or(trigono);//U+T
            //ptv_ID17.SegmentVolume = ptv_ID17.Or(prv_rectum);//U+T+PrvRe
            //ptv_ID17.SegmentVolume = ptv_ID12.Sub(ptv_ID17);//PTV pros-(U+T+PrvRe)
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
            string[] N_Body = {     "Body",             "Outer Contour", "body", "BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_SIB.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

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
            FindCouch(ss,context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            //comienza las estrucuras
            //New Structures 
            string PTV_ID12 = "PTV_Bladder";
            string PTV_ID13 = "PTV_LN_Pelvic";
            string PTV_ID14 = "PTV_SIB";
            string PTV_ID15 = "zPTV_High_5600!";
            string PTV_ID16 = "zPTV_Low_4500!";
            string PTV_ID17 = "zPTV_High_6600!";
            string PTV_ID18 = "zPTV56-BowelPRV!";
            string PTV_ID19 = "zPTV66-BowelPRV!";
            string PTV_ID20 = "zPTV_BowelPRV05!";
            string PTV_ID21 = "zPTV_Total!";
            
            string PRV_Rectum = "Rectum_PRV05";
            string PRV_colon = "Colon_PRV05";//
            string PRV_bowel = "Bowel_PRV05";//

            /*if (IsResim(ss))//RE SIMULAICON
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID17 += ".";
                    PTV_ID18.Remove(PTV_ID18.Length - 1);
                    PTV_ID19.Remove(PTV_ID19.Length - 1);
                    PTV_ID20.Remove(PTV_ID20.Length - 1);
                    PTV_ID21 += ".";
                    PRV_Rectum += ".";
                    PRV_colon += ".";
                    PRV_bowel += ".";                    
                }
            }*/
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);//sib
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);//5600
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);//4500
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);//6600
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);//56-int
            Structure ptv_ID19 = Add_structure(ss, "PTV", PTV_ID19);//66-int
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);//intersec int
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);//total

            Structure prv_rectum = Add_structure(ss, "CONTROL", PRV_Rectum);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_colon);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_bowel);

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
            string[] N_Body = {         "Body",             "Outer Contour", "body", "BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Bladder.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID3 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_LN.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure ctv_ID4 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_RV.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            //oars con prv
            Structure rectum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Rectum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure colon = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Colon.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver

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
            string PTV_ID12 = "PTV_SurgicalBed";
            string PTV_ID13 = "PTV_LN_Pelvic";
            string PTV_ID14 = "PTV_RestVagina";
            string PTV_ID15 = "zPTV_High_4800!";
            string PTV_ID16 = "zPTV_Low_4500!";
            string PTV_ID17 = "zPTV_Mid_4600!";
            string PTV_ID18 = "zPTV48-BowelPRV!";//pude estar vacio
            string PTV_ID20 = "zPTV_BowelPRV05!";
            string PTV_ID21 = "zPTV_Total!";

            string PRV_Rectum = "Rectum_PRV05";
            string PRV_colon = "Colon_PRV05";//
            string PRV_bowel = "Bowel_PRV05";//
            FindCouch(ss, context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))//RE SIMULAICON
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    PTV_ID15 += ".";
                    PTV_ID16 += ".";
                    PTV_ID18.Remove(PTV_ID18.Length - 1);
                    PTV_ID20.Remove(PTV_ID20.Length - 1);
                    PTV_ID21 += ".";
                    PRV_Rectum += ".";
                    PRV_colon += ".";
                    PRV_bowel += ".";
                }
            }*/
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);//sib
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);//4800
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);//4500
            Structure ptv_ID17 = Add_structure(ss, "PTV", PTV_ID17);//4600
            Structure ptv_ID18 = Add_structure(ss, "PTV", PTV_ID18);//48-int
            Structure ptv_ID20 = Add_structure(ss, "PTV", PTV_ID20);//intersec int
            Structure ptv_ID21 = Add_structure(ss, "PTV", PTV_ID21);//total

            Structure prv_rectum = Add_structure(ss, "CONTROL", PRV_Rectum);
            Structure prv_colon = Add_structure(ss, "CONTROL", PRV_colon);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_bowel);

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
                ptv_ID17.SegmentVolume = ptv_ID14.Sub(ptv_ID15);//46-48
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
            string[] N_Body = { "Body", "Outer Contour", "body", "BODY" };
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

            Structure ctv_ID2 = ss.Structures.Where(x => !x.Id.Contains("PTV")).FirstOrDefault(s => N_Liver.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure stomach = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Stomach.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure esophagus = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Esophagus.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure bowel = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Bowel.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver
            Structure duodenum = ss.Structures.Where(x => !x.Id.Contains("PRV")).FirstOrDefault(s => N_Duodenum.Any(x => s.Id.Contains(x)));//s = structura s.id su id names es el array de string para ver


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
            string PTV_ID12 = "PTV_Liver";
            string PTV_ID13 = "zPTV_High_4800!";
            string PTV_ID14 = "zPTV-PRVs!";
            string PTV_ID15 = "zPTV_BowelPRV05!";
            string PTV_ID16 = "zPTV_DuodePRV05!";
            string PRV_esophagus = "Esophagus_PRV05";
            string PRV_duode = "Duodenum_PRV05";//
            string PRV_bowel = "Bowel_PRV05";//
            string PRV_stomach = "Stomach_PRV05";//
            FindCouch(ss, context);
            if (question() == DialogResult.No) return;//pregunta si deseamos continuar
            /*if (IsResim(ss))//RE SIMULAICON
            {
                DialogResult result0 = System.Windows.Forms.MessageBox.Show("PTVs detected, is the plan a re-simulation? ", SCRIPT_NAME0, MessageBoxButtons.YesNo);
                if (result0 == DialogResult.Yes)
                {
                    PTV_ID12 += ".";
                    PTV_ID13 += ".";
                    PTV_ID14 += ".";
                    
                    PTV_ID16 += ".";
                    PTV_ID15.Remove(PTV_ID15.Length - 1);
                    PTV_ID16.Remove(PTV_ID16.Length - 1);
                    PRV_esophagus += ".";
                    PRV_duode += ".";
                    PRV_bowel += ".";
                    PRV_stomach += ".";
                }
            }*/
            //============================
            // GENERATE 5mm expansion of PTV
            //============================

            // create the empty "ptv+5mm" structure ans auxilary structures

            Structure ptv_ID12 = Add_structure(ss, "PTV", PTV_ID12);//bladder
            Structure ptv_ID13 = Add_structure(ss, "PTV", PTV_ID13);//ganglios
            Structure ptv_ID14 = Add_structure(ss, "PTV", PTV_ID14);//sib
            Structure ptv_ID15 = Add_structure(ss, "PTV", PTV_ID15);//4800
            Structure ptv_ID16 = Add_structure(ss, "PTV", PTV_ID16);//4500

            Structure prv_esop = Add_structure(ss, "CONTROL", PRV_esophagus);
            Structure prv_duodenum = Add_structure(ss, "CONTROL", PRV_duode);
            Structure prv_bowel = Add_structure(ss, "CONTROL", PRV_bowel);
            Structure prv_stomach = Add_structure(ss, "CONTROL", PRV_stomach);
            Structure prv_total = Add_structure(ss, "CONTROL", "zPRV_total!");

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
