using E_FirmaElect_Demo.Models;
using SIGPLUSLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace E_FirmaElect_Demo.Controllers
{
    public class FirmaElectronicaController : Controller
    {
        // GET: FirmaElectronica
        public ActionResult Index()
        {
            FirmaElectronicaModel oFirmaElectronicaModel = new FirmaElectronicaModel();
            oFirmaElectronicaModel.oPersona = new Entidades.Persona_BE();
            oFirmaElectronicaModel.oPersona.DNI = "46258239";
            return View(oFirmaElectronicaModel);
        }
        [HttpPost]
        public JsonResult Index(FirmaElectronicaModel oFirmaElectronicaModel)
        {

            String Resultado = "";
            String vNuevoSigString = "";
            String pRutaArchivo = string.Concat(System.Web.Hosting.HostingEnvironment.MapPath("~/DocumentosGenerados/"), "Firma_Electrina" + oFirmaElectronicaModel.oPersona.DNI + ".pdf");


            SIGPLUSLib.SigPlus oSigPlus = new SIGPLUSLib.SigPlus();
            oSigPlus.InitSigPlus();
            oSigPlus.SigCompressionMode = 0;
            oSigPlus.SigString = oFirmaElectronicaModel.oFirmaElectronica.SIGSTRING;

            Obtener_Pdf_CC(pRutaArchivo,oFirmaElectronicaModel,oFirmaElectronicaModel.oFirmaElectronica.SIGSTRING);

            oSigPlus.EncryptionMode = 0;
            oSigPlus.AutoKeyData = pRutaArchivo;
            oSigPlus.AutoKeyFinish();
            oSigPlus.EncryptionMode = 2;
            oSigPlus.SigCompressionMode = 4;

            vNuevoSigString = oSigPlus.SigString;
            oFirmaElectronicaModel.oFirmaElectronica.SIGSTRING_ENCRIPTADA = vNuevoSigString;
            oFirmaElectronicaModel.Resultado = "Ok";

            return Json(oFirmaElectronicaModel, JsonRequestBehavior.AllowGet);
        }
        #region FIRMA DIGITAL
        public Boolean Obtener_Pdf_CC(String pRutaArchivo, FirmaElectronicaModel oFirmaElectronicaModel, String pImg64)
        //Fin E.Z. 13/05/2016
        {
            Boolean b_Resultado = false;
            iTextSharp.text.Document oDocument = null;
            iTextSharp.text.pdf.PdfWriter oPdfWriter = null;
            iTextSharp.text.pdf.PdfContentByte oPdfContentByte = null;
            iTextSharp.text.Chunk oChunk = null;
            iTextSharp.text.HeaderFooter oFooter = null;
            try
            {

                using (FileStream fs = new FileStream(pRutaArchivo, FileMode.Create, FileAccess.Write))
                    {
                        // Crear PDF
                        oDocument = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30, 30, 15, 25);
                        oPdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(oDocument, fs);

                        oDocument.Open();
                        oPdfContentByte = oPdfWriter.DirectContent;
                        oPdfContentByte.Stroke();
                        oPdfContentByte.SetLineWidth(0.2f);

                        oDocument.Add(Obtener_Logos(null));
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Titulo("098923"));
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Cliente(oFirmaElectronicaModel));
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Cuenta(oFirmaElectronicaModel));
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Solicitud(oFirmaElectronicaModel, "ARamirez"));
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Blanco());
                        oDocument.Add(Obtener_Pie(oFirmaElectronicaModel));
                        #region "Firma"
                        //Firma 1
                        //oPdfContentByte.Rectangle(59f, 40.5f, 300f, 72.5f);
                        oPdfContentByte.BeginText();
                        oPdfContentByte.SetFontAndSize(iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED), (float)8);
                        oPdfContentByte.ShowTextAligned(iTextSharp.text.Element.ALIGN_BASELINE, "Asesor de Servicio al Cliente Finantienda", 59f, 122.5f, 0);
                        oPdfContentByte.EndText();
                        oPdfContentByte.Stroke();
                        oPdfContentByte.BeginText();
                        oPdfContentByte.SetFontAndSize(iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED), (float)8);
                        oPdfContentByte.ShowTextAligned(iTextSharp.text.Element.ALIGN_BASELINE, "Cesar Mariñoas Asmat", 59f, 102.5f, 0);
                        oPdfContentByte.EndText();
                        oPdfContentByte.Stroke();
                         
                        oPdfContentByte.Rectangle(385.5f, 40.5f, 150f, 72.5f);
                        oPdfContentByte.BeginText();
                        oPdfContentByte.SetFontAndSize(iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.WINANSI, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED), (float)8);
                        oPdfContentByte.ShowTextAligned(iTextSharp.text.Element.ALIGN_BASELINE, "Firma del Cliente Titular", 430.5f, 122.5f, 0);
                        oPdfContentByte.EndText();
                        oPdfContentByte.Stroke();
                        
                        if (!string.IsNullOrEmpty(pImg64))
                        {
                            iTextSharp.text.Image _imagen = iTextSharp.text.Image.GetInstance(ConvertStringBase64ToImage(oFirmaElectronicaModel.oFirmaElectronica.SIGSTRING_64));
                            _imagen.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            _imagen.BorderColor = iTextSharp.text.Color.WHITE;
                            _imagen.SetAbsolutePosition(385.5f, 55.8f);
                            _imagen.ScaleToFit(150f, 81.5f);

                            oDocument.Add(_imagen);
                        }
                        oDocument.Close();
                        b_Resultado = true;
                    }
                        #endregion
                
            }
            catch (Exception ex) { b_Resultado = false; }
            //
            return b_Resultado;
        }
        private iTextSharp.text.Table Obtener_Logos(System.Web.UI.Page pPage)
        {
            iTextSharp.text.Table tbLogos = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Image oFinancieraUno = null;
            iTextSharp.text.Image oOechsle = null;
            // Tabla
            tbLogos = new iTextSharp.text.Table(3);
            tbLogos.Width = 90;
            tbLogos.Cellpadding = 1;
            tbLogos.Cellspacing = 0;
            tbLogos.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //
            if (pPage == null)
            {
                oFinancieraUno = iTextSharp.text.Image.GetInstance(string.Concat(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagen/"), "Logo_ESSolutions.jpg"));
            }
            else
            {
                oFinancieraUno = iTextSharp.text.Image.GetInstance(string.Concat(pPage.Server.MapPath("~/Imagen/"), "Logo_ESSolutions.jpg"));
            }
            oFinancieraUno.ScaleAbsolute(300, 90);
            tdCell = new iTextSharp.text.Cell();
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.Add(oFinancieraUno);
            tbLogos.AddCell(tdCell);
            //
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbLogos.AddCell(tdCell);
            //
            if (pPage == null)
            {
                oOechsle = iTextSharp.text.Image.GetInstance(string.Concat(System.Web.Hosting.HostingEnvironment.MapPath("~/Imagen/"), "Logo_ESSolutions.jpg"));
            }
            else
            {
                oOechsle = iTextSharp.text.Image.GetInstance(string.Concat(pPage.Server.MapPath("~/Imagen/"), "Logo_ESSolutions.jpg"));
            }
            oOechsle.ScaleAbsolute(65, 45);
            tdCell = new iTextSharp.text.Cell();
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            tdCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.Add(oOechsle);
            tbLogos.AddCell(tdCell);
            //
            return tbLogos;
        }

        private iTextSharp.text.Table Obtener_Titulo(String pNro_Solicitud)
        {
            iTextSharp.text.Table tbTitulo = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            // Tabla
            tbTitulo = new iTextSharp.text.Table(1);
            tbTitulo.Width = 90;
            tbTitulo.Cellpadding = 1;
            tbTitulo.Cellspacing = 0;
            tbTitulo.Border = iTextSharp.text.Rectangle.NO_BORDER;
            // Texto
            oChunk = new iTextSharp.text.Chunk("SOLICITUD DE ATENCIÓN MÚLTIPLE", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            // Celda > Título
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Header = false;
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbTitulo.AddCell(tdCell);
            // Texto
            oChunk = new iTextSharp.text.Chunk(String.Format("Nro.: {0}", pNro_Solicitud), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            // Celda > Nro
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Header = false;
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbTitulo.AddCell(tdCell);
            //
            return tbTitulo;
        }

        private iTextSharp.text.Table Obtener_Blanco()
        {
            iTextSharp.text.Table tbBlanco = null;
            iTextSharp.text.Cell tdCell = null;
            //
            tbBlanco = new iTextSharp.text.Table(1);
            tbBlanco.Width = 90;
            tbBlanco.Cellpadding = 3;
            tbBlanco.Cellspacing = 0;
            tbBlanco.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell = new iTextSharp.text.Cell();
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.BorderColor = iTextSharp.text.Color.BLACK;
            tbBlanco.AddCell(tdCell);
            //
            return tbBlanco;
        }

        private iTextSharp.text.Table Obtener_Cliente(FirmaElectronicaModel oFirmaElectronicaModel)
        {
            iTextSharp.text.Table tbCliente = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            // Tabla
            tbCliente = new iTextSharp.text.Table(4, 4);
            tbCliente.Widths = new float[4] { 20, 25, 20, 25 };
            tbCliente.Width = 90;
            tbCliente.Cellpadding = 1;
            tbCliente.Cellspacing = 0;
            tbCliente.Border = iTextSharp.text.Rectangle.NO_BORDER;
            // Sub-Título
            oChunk = new iTextSharp.text.Chunk(" Datos del Cliente", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(255, 255, 255)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Header = true;
            tdCell.Colspan = 4;
            tdCell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Colspan = 4;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            // Documento : Tipo y Nro.
            oChunk = new iTextSharp.text.Chunk("Tipo Documento:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("DNI", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Nro. Documento:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk(oFirmaElectronicaModel.oPersona.DNI, iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            // Nombres y Apellidos
            oChunk = new iTextSharp.text.Chunk("Nombres:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk(String.Format("{0} {1}", oFirmaElectronicaModel.oPersona.PRI_NOMBRE.ToUpper(), (String.IsNullOrEmpty(oFirmaElectronicaModel.oPersona.SEG_NOMBRE) ? String.Empty : oFirmaElectronicaModel.oPersona.SEG_NOMBRE).ToUpper()), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Apellidos:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk(String.Format("{0} {1}", oFirmaElectronicaModel.oPersona.APE_PATERNO.ToUpper(), (String.IsNullOrEmpty(oFirmaElectronicaModel.oPersona.APE_MATERNO) ? String.Empty : oFirmaElectronicaModel.oPersona.APE_MATERNO).ToUpper()), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCliente.AddCell(tdCell);
            //
            return tbCliente;
        }

        private iTextSharp.text.Table Obtener_Cuenta(FirmaElectronicaModel oFirmaElectronicaModel)
        {
            iTextSharp.text.Table tbCuenta = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            // Tabla
            tbCuenta = new iTextSharp.text.Table(4);
            tbCuenta.Widths = new float[4] { 20, 25, 20, 25 };
            tbCuenta.Width = 90;
            tbCuenta.Cellpadding = 1;
            tbCuenta.Cellspacing = 0;
            tbCuenta.Border = iTextSharp.text.Rectangle.NO_BORDER;
            // Sub-Título
            oChunk = new iTextSharp.text.Chunk(" Datos de la Cuenta", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(255, 255, 255)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Header = true;
            tdCell.Colspan = 4;
            tdCell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Colspan = 4;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            // Producto y Cuenta
            oChunk = new iTextSharp.text.Chunk("Producto:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("VISA", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Nro. Cuenta:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("544656157671165", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbCuenta.AddCell(tdCell);
            //
            return tbCuenta;
        }

        private iTextSharp.text.Table Obtener_Solicitud(FirmaElectronicaModel oFirmaElectronicaModel , String pUsuario)
        {
            Boolean boFlag_Titular = false, boFlag_Adicional = false;
            iTextSharp.text.Table tbSolicitud = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            // Tabla
            tbSolicitud = new iTextSharp.text.Table(4);
            tbSolicitud.Widths = new float[4] { 20, 25, 20, 25 };
            tbSolicitud.Width = 90;
            tbSolicitud.Cellpadding = 1;
            tbSolicitud.Cellspacing = 0;
            tbSolicitud.Border = iTextSharp.text.Rectangle.NO_BORDER;

            #region "Solicitud > Superior"
            // Sub-Título
            oChunk = new iTextSharp.text.Chunk(" Datos de la Solicitud", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(255, 255, 255)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Header = true;
            tdCell.Colspan = 4;
            tdCell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_LEFT;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Colspan = 4;
            tdCell.BackgroundColor = new iTextSharp.text.Color(66, 139, 202);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            // Fecha y Agencia
            oChunk = new iTextSharp.text.Chunk("Fecha de Solicitud:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk(DateTime.Now.ToShortDateString(), iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Agencia:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Principal", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            // Tipo de Requerimiento
            oChunk = new iTextSharp.text.Chunk("Tipo de atención:", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Contratos", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Colspan = 3;
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbSolicitud.AddCell(tdCell);
            #endregion
             
            return tbSolicitud;
        }

        private iTextSharp.text.Table Obtener_Pie(FirmaElectronicaModel oFirmaElectronicaModel)
        {
            String[] strArreglo = null;
            iTextSharp.text.Table tbPie = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            //
            tbPie = new iTextSharp.text.Table(1);
            tbPie.Width = 90;
            tbPie.Cellpadding = 0;
            tbPie.Cellspacing = 0;
            tbPie.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbPie.BorderWidthTop = (float)1;
            // Footer
            
                        oChunk = new iTextSharp.text.Chunk("Be careful! In Oracle, TIMESTAMP means a datatype, similar to but distinct from DATE. You'll avoid confusion if you don't use the word  to mean anything else.", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(0, 0, 0)));
                        tdCell = new iTextSharp.text.Cell(oChunk);
                        tdCell.HorizontalAlignment = iTextSharp.text.Rectangle.ALIGN_JUSTIFIED;
                        tdCell.VerticalAlignment = iTextSharp.text.Rectangle.ALIGN_BASELINE;
                        tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        tbPie.AddCell(tdCell);
             
            //
            return tbPie;
        }

        private iTextSharp.text.Table Obtener_Firma()
        {
            iTextSharp.text.Table tbFirma = null;
            iTextSharp.text.Cell tdCell = null;
            iTextSharp.text.Chunk oChunk = null;
            // Tabla
            tbFirma = new iTextSharp.text.Table(3);
            tbFirma.Widths = new float[3] { 25, 30, 25 };
            tbFirma.Width = 80; ;
            tbFirma.Cellpadding = 1;
            tbFirma.Cellspacing = 0;
            tbFirma.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            tbFirma.Border = 1;
            tbFirma.BorderWidth = (float)1;
            tbFirma.BorderWidthBottom = (float)1;
            tbFirma.BorderWidthLeft = (float)1;
            tbFirma.BorderWidthRight = (float)1;
            //
            oChunk = new iTextSharp.text.Chunk("A", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 10, iTextSharp.text.Color.WHITE));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.BorderWidthLeft = (float)1;
            tdCell.BorderWidthRight = (float)1;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("B", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 10, iTextSharp.text.Color.WHITE));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.BorderWidthLeft = (float)1;
            tdCell.BorderWidthRight = (float)1;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("V°B° Autorizador", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Firma y sello del Asesor de Servicio", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.BorderWidthLeft = (float)1;
            tdCell.BorderWidthRight = (float)1;
            tbFirma.AddCell(tdCell);
            oChunk = new iTextSharp.text.Chunk("Firma del cliente", iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(0, 0, 0)));
            tdCell = new iTextSharp.text.Cell(oChunk);
            tdCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tdCell.BorderWidthLeft = (float)1;
            tdCell.BorderWidthRight = (float)1;
            tbFirma.AddCell(tdCell);
            tdCell = new iTextSharp.text.Cell(String.Empty);
            tdCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tbFirma.AddCell(tdCell);
            //
            return tbFirma;
        }

        public static Stream ConvertStringBase64ToImage(String s_Firma)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(s_Firma);
                Image im_Firma = null;
                var sst_Firma = new MemoryStream();
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    im_Firma = Image.FromStream(ms);
                }
                im_Firma.Save(sst_Firma, ImageFormat.Png);
                sst_Firma.Position = 0;

                return sst_Firma;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        public FileResult Obtener_Archivo_Validado(String pSigString,String pNum_DNI)
        {
            String s_Nombre ="Archivo_PDF_Verificado.pdf";
            String pRutaArchivo = string.Concat(System.Web.Hosting.HostingEnvironment.MapPath("~/DocumentosGenerados/"), "Firma_Electrina" + pNum_DNI + ".pdf");

            byte[] archivo = ObtenerArchivo_ValTopaz(pSigString, pRutaArchivo);
            if (archivo == null)
            {
                s_Nombre = "NO SE CUMPLE LAS VALIDACIONES PARA MOSTRAR EL ARCHIVO";
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = s_Nombre,
                Inline = false,
                CreationDate = DateTime.Now
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(archivo, MediaTypeNames.Application.Octet);
        }

        public byte[] ObtenerArchivo_ValTopaz(String pSigString , String pRutaArchivo )//INI JJCS 20160930
        {
            try
            {
                
                Stream strArchivo = new MemoryStream();

                SigPlus oSigPlus = new SigPlus();
                oSigPlus.InitSigPlus();
                oSigPlus.SigCompressionMode = 4;
                oSigPlus.EncryptionMode = 2;
                oSigPlus.AutoKeyData = pRutaArchivo;
                oSigPlus.AutoKeyFinish();

                oSigPlus.SigString = pSigString;

                if (oSigPlus.NumberOfTabletPoints() <= 0)
                {
                    return null;
                }
                if (System.IO.File.Exists(pRutaArchivo))
                {
                    return System.IO.File.ReadAllBytes(pRutaArchivo);
                }
                //}
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}