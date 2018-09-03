using Spire.Pdf;


using Newtonsoft.Json;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Queries;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.GeoLocation;
using Path = YAMBOLY.PORTALADMINISTRADOR.HELPER.Path;
using Spire.Xls.Converter;
using Spire.Xls;
using System.Reflection;
using iTextSharp.text.pdf;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation
{
    public class MapLogic
    {
        public MapViewModel GetMapViewModel(DataContext dataConext)
        {
            var model = new MapViewModel();
            model.ShapeList = GetShapeList(ref model, dataConext);

            #region JsonList
            model.CanalJList = new CanalLogic().GetJList(dataConext);
            model.DepartamentoJList = new DepartamentoLogic().GetJList(dataConext);
            model.DistritoJList = new DistritoLogic().GetJList(dataConext, string.Empty);
            model.GiroJList = new GiroLogic().GetJList(dataConext);
            model.JefeVentasJList = new JefeVentasLogic().GetJList(dataConext, null);
            model.ProvinciaJList = new ProvinciaLogic().GetJList(dataConext, string.Empty);
            model.RegionJList = new RegionLogic().GetJList(dataConext);
            model.ZonaJList = new ZonaLogic().GetJList(dataConext);
            model.RutaJList = new RutaLogic().GetJList(dataConext);
            model.SupervisorJList = new SupervisorLogic().GetJList(dataConext, null);
            model.TipoClienteJList = new List<JsonEntityTwoString>();//TODO:
            model.VendedorJList = new VendedorLogic().GetJList(dataConext);
            model.FrecuenciaVisitaJList = new UserDefinedValuesLogic().GetJList(dataConext, "CRD1", 7);
            #endregion

            return model;
        }

        private List<TreeViewNode> GetShapeList(ref MapViewModel model, DataContext dataContext)
        {
            List<TreeViewNode> treeViewNode = new List<TreeViewNode>();
            var zoneList = new ZonaLogic().GetList(dataContext);
            var routeList = new RutaLogic().GetList(dataContext);
            var clientList = new AddressLogic().GetList(dataContext);
            foreach (var zone in zoneList)
            {
                var zoneNode = new TreeViewNode();
                zoneNode.Id = zone.Id;
                zoneNode.text = zone.Name;
                zoneNode.ShapeType = ShapeType.Zone;
                zoneNode.GeoOptions = zone.GeoOptions;

                foreach (var route in routeList.Where(x => x.ZoneId == zone.Id))
                {
                    var routeNode = new TreeViewNode();
                    routeNode.Id = route.Id;
                    routeNode.text = route.Name;
                    routeNode.ShapeType = ShapeType.Route;
                    routeNode.GeoOptions = route.GeoOptions;
                    routeNode.ParentId = zoneNode.Id;
                    foreach (var client in clientList.Where(x => x.RutaId == route.Id && x.ZonaId == zone.Id))
                    {
                        var clientNode = new TreeViewNode();
                        clientNode.Id = client.CodigoInterno;
                        clientNode.text = client.Ruc + " - " + client.RazonSocial;
                        clientNode.ShapeType = ShapeType.Address;
                        clientNode.GeoOptions = client.GeoOptions;
                        clientNode.ParentId = routeNode.Id;
                        routeNode.nodes.Add(clientNode);
                    }
                    zoneNode.nodes.Add(routeNode);
                }
                treeViewNode.Add(zoneNode);
            };
            return treeViewNode;
        }

        public void AddUpdateMap(DataContext dataContext, MapViewModel model)
        {
            List<TreeViewNode> postedObjects = JsonConvert.DeserializeObject<List<TreeViewNode>>(model.PostedShapeList[0]);//TODO:

            var queries = new List<string>();
            if (postedObjects != null)
                foreach (var zone in postedObjects?.ToList())
                {
                    queries.Add(new ZonaLogic().GetQuery(dataContext, zone));
                    foreach (var route in zone.nodes)
                    {
                        queries.Add(new RutaLogic().GetUpdateQuery(dataContext, route));
                        foreach (var client in route.nodes)
                        {
                            queries.Add(new AddressLogic().GetQuery(dataContext, client));
                        }
                    }
                }
            var finalQuery = string.Join(" ", queries);
            WebHelper.GetJsonPostResponse(Queries.GetUrlPath(), finalQuery);
        }

        public string GetCoordinatesArray(GeoOptions geoOptions, ShapeType polygonType)
        {
            var rootObject = new RootObject();
            double?[] coordinates = new double?[2];
            switch (polygonType)
            {
                case ShapeType.Zone:
                case ShapeType.Route:
                    if (geoOptions.paths != null)
                    {
                        foreach (var item in geoOptions.paths)
                        {
                            coordinates[0] = item.lat;
                            coordinates[1] = item.lng;
                            rootObject.coords.Add(coordinates.ToList());
                        }
                        return JsonConvert.SerializeObject(rootObject);
                    }

                    break;
                case ShapeType.Address:
                    if (geoOptions.coords != null)
                    {
                        coordinates[0] = geoOptions.coords.lat;
                        coordinates[1] = geoOptions.coords.lng;
                        rootObject.coords.Add(coordinates.ToList());
                        return JsonConvert.SerializeObject(rootObject);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return string.Empty;
        }

        public GeoOptions GetGeoOptionsFromCoordinates(string coordinates, ShapeType shapeType)
        {
            if (string.IsNullOrEmpty(coordinates)) return null;
            RootObject obj = JsonConvert.DeserializeObject<RootObject>(coordinates);
            var geoOptions = new GeoOptions();

            switch (shapeType)
            {
                case ShapeType.Zone:
                case ShapeType.Route:
                    foreach (var item in obj.coords)
                    {
                        geoOptions.paths.Add(new Path() { lat = item[0], lng = item[1] });
                    }
                    break;
                case ShapeType.Address:
                    foreach (var item in obj.coords)
                    {
                        geoOptions.coords = new Path() { lat = item[0], lng = item[1] };
                    }
                    break;
                default:
                    throw new System.Exception("Error");//TODO:
            }
            return geoOptions;
        }

        public dynamic GetShapeInfo(DataContext dataContext, string id, ShapeType shapeType)
        {
            dynamic shapeInfo;
            switch (shapeType)
            {
                case ShapeType.Zone:
                    shapeInfo = new ZonaLogic().Get(dataContext, id);
                    break;
                case ShapeType.Route:
                    shapeInfo = new RutaLogic().Get(dataContext, id);
                    break;
                case ShapeType.Address:
                    shapeInfo = new AddressLogic().Get(dataContext, id);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return shapeInfo;
        }

        public List<string> GetFilteredClientList(DataContext dataContext, MapViewModel model)
        {
            var list = new AddressLogic().GetList(dataContext).AsQueryable();

            var addressList = list.Where(x => string.IsNullOrEmpty(model.Region) | x.Region == model.Region
                                        && string.IsNullOrEmpty(model.Departamento) | x.Departamento == model.Departamento
                                        && string.IsNullOrEmpty(model.Provincia) | x.Provincia == model.Provincia
                                        && string.IsNullOrEmpty(model.Distrito) | x.Distrito == model.Distrito
                                        && string.IsNullOrEmpty(model.Zona) | x.ZonaId == model.Zona
                                        && string.IsNullOrEmpty(model.Ruta) | x.RutaId == model.Ruta
                                        && string.IsNullOrEmpty(model.Canal) | x.Canal == model.Canal
                                        && string.IsNullOrEmpty(model.Giro) | x.Giro == model.Giro
                                        && x.ConActivos == model.ConActivos
                                        && string.IsNullOrEmpty(model.FrecuenciaVisita) | x.FrecuenciaVisita == model.FrecuenciaVisita
                                        //TODO: TIPO CLIENTE (Clientes Yamboly, competencia, leads)
                                        //&& x.TipoCliente == model.TipoCliente
                                        && string.IsNullOrEmpty(model.Vendedor) | x.Vendedor == model.Vendedor
                                        && string.IsNullOrEmpty(model.Supervisor) | x.Supervisor == model.Supervisor
                                        && string.IsNullOrEmpty(model.JefeVentas) | x.JefeDeVentas == model.JefeVentas
                                        && !model.DiaDeVisitaLunes | x.DiaDeVisitaLunes == model.DiaDeVisitaLunes
                                        && !model.DiaDeVisitaMartes | x.DiaDeVisitaMartes == model.DiaDeVisitaMartes
                                        && !model.DiaDeVisitaMiercoles | x.DiaDeVisitaMiercoles == model.DiaDeVisitaMiercoles
                                        && !model.DiaDeVisitaJueves | x.DiaDeVisitaJueves == model.DiaDeVisitaJueves
                                        && !model.DiaDeVisitaViernes | x.DiaDeVisitaViernes == model.DiaDeVisitaViernes
                                        && !model.DiaDeVisitaSabado | x.DiaDeVisitaSabado == model.DiaDeVisitaSabado
                                        && !model.DiaDeVisitaDomingo | x.DiaDeVisitaDomingo == model.DiaDeVisitaDomingo
                                        )
                                    .Select(x => x.CodigoInterno).ToList();

            //-------------------------------------------------------------Agrega Filtro de ventas//-------------------------------------------------------------
            var clientsInSalesRange = GetFilteredClientListBySales(dataContext, model.VentasMontoMinimo, model.VentasMontoMaximo, model.VentasFechaInicio, model.VentasFechaFin);
            var finalAddressList = new List<string>();
            foreach (var address in addressList)
            {
                if (clientsInSalesRange.Contains(address.Split('-')[0].ToSafeString().Trim()) && !finalAddressList.Contains(address))
                    finalAddressList.Add(address);
            }
            //-------------------------------------------------------------Fin filtro de ventas-------------------------------------------------------------

            return finalAddressList;
        }

        private List<string> GetFilteredClientListBySales(DataContext dataContext, decimal? montoInicial, decimal? montoFinal, DateTime? fechaInicial, DateTime? fechaFinal)
        {

            var _salesInDateRange = new VentasLogic().GetList(dataContext).Where(x => fechaInicial <= x.DocDate && x.DocDate <= fechaFinal
                                                                                               && montoInicial <= x.DocTotal && x.DocTotal <= montoFinal);


            var salesInDateRange = new VentasLogic().GetList(dataContext).Where(x => (fechaInicial ?? DateTime.MinValue) <= x.DocDate && x.DocDate <= (fechaFinal ?? DateTime.MaxValue)
                                                                                   && (montoInicial ?? decimal.MinValue) <= x.DocTotal && x.DocTotal <= (montoFinal ?? decimal.MaxValue))
                                                                         .Select(x => x.CardCode).Distinct().ToList();
            return salesInDateRange;
        }


        public dynamic GetReport(DataContext dataContext, MapViewModel model)
        {
            const string excelTemplateFileName = "reportTemplate.xlsx";
            string serverPath = HostingEnvironment.MapPath("~/App_Data/");

            var key = DateTime.Now.ToString("yyyy-MM-dd hhmmss") + Guid.NewGuid();
            var newExcelDocumentPath = serverPath + "ReporteDirecciones_" + key + ".xslx";
            var newPdfDocumentPath = serverPath + "ReporteDirecciones_" + key + ".pdf";
            var newPdfDocumentEditedPath = serverPath + "ReporteDirecciones_" + key + "v2" + ".pdf";

            var fileInfo = new FileInfo(serverPath + excelTemplateFileName);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets.First();
                int columnIndex = 0;
                int rowIndex = 0;

                #region GeneralList 
                var regionList = new RegionLogic().GetJList(dataContext);
                var departamentoList = new DepartamentoLogic().GetJList(dataContext);
                var zonaList = new ZonaLogic().GetJList(dataContext);
                var rutaList = new RutaLogic().GetJList(dataContext);
                var canalList = new CanalLogic().GetJList(dataContext);
                var giroList = new GiroLogic().GetJList(dataContext);
                var vendedorList = new VendedorLogic().GetJList(dataContext);
                var supervisorList = new SupervisorLogic().GetJList(dataContext, null);
                var jefeVentasList = new JefeVentasLogic().GetJList(dataContext, null);
                var frecuenciaVisitaList = new UserDefinedValuesLogic().GetJList(dataContext, "CRD1", 7);
                #endregion

                #region Filters
                rowIndex = 3;
                columnIndex = 1;
                var filterCount = 0;

                for (var i = 0; i < model.GetType().GetProperties().Count(); i++)
                {
                    var propertyInfo = model.GetType().GetProperties()[i];
                    var validPropertyTypes = new List<Type> { typeof(string), typeof(int), typeof(bool) };
                    if (validPropertyTypes.Contains(propertyInfo.PropertyType))
                    {
                        var propertyName = propertyInfo.Name;
                        var propertyValue = propertyInfo.GetValue(model, null);

                        if (propertyName == model.GetMemberName(x => model.Region))
                            propertyValue = regionList.FirstOrDefault(x => x.id == model.Region.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Departamento))
                            propertyValue = departamentoList.FirstOrDefault(x => x.id == model.Departamento.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Zona))
                            propertyValue = zonaList.FirstOrDefault(x => x.id == model.Zona.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Ruta))
                            propertyValue = rutaList.FirstOrDefault(x => x.id == model.Ruta.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Canal))
                            propertyValue = canalList.FirstOrDefault(x => x.id == model.Canal.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Giro))
                            propertyValue = giroList.FirstOrDefault(x => x.id == model.Giro.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Vendedor))
                            propertyValue = vendedorList.FirstOrDefault(x => x.id == model.Vendedor.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.JefeVentas))
                            propertyValue = jefeVentasList.FirstOrDefault(x => x.id == model.JefeVentas.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.Supervisor))
                            propertyValue = supervisorList.FirstOrDefault(x => x.id == model.Supervisor.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.FrecuenciaVisita))
                            propertyValue = frecuenciaVisitaList.FirstOrDefault(x => x.id == model.FrecuenciaVisita.ToSafeString())?.text;

                        else if (propertyName == model.GetMemberName(x => model.DiaDeVisitaLunes)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaMartes)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaMiercoles)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaJueves)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaViernes)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaSabado)
                                || propertyName == model.GetMemberName(x => model.DiaDeVisitaDomingo)
                            )
                            propertyValue = (bool)propertyValue ? "Si" : string.Empty;

                        worksheet.Cells[rowIndex, columnIndex].Value = propertyName;
                        worksheet.Cells[rowIndex, columnIndex + 1].Value = propertyValue;

                        rowIndex += 2;
                        filterCount++;

                        if (filterCount % 4 == 0)
                        {
                            rowIndex = 3;
                            columnIndex += 3;
                        }
                    }
                }

                #endregion

                #region TableHeader
                columnIndex = 1;
                rowIndex = 11;

                worksheet.Cells[rowIndex, columnIndex].Value = "Código"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Tipo dirección"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Nombre"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Departamento"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Provincia"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Distrito"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Nombre y N° Calle"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Canal"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Giro"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Tipo cliente"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Zona"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Ruta"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Vendedor"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Supervisor"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Jefe de ventas"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita LU"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita MA"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita MI"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita JU"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita VI"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita SA"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Visita DO"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Frecuencia de visita"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Código activo"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Latitud"; columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = "Longitud";
                rowIndex++;

                #endregion

                #region TableBody
                rowIndex = 12;
                columnIndex = 1;
                List<TreeViewNode> postedObjects = JsonConvert.DeserializeObject<List<TreeViewNode>>(model.PostedShapeList[0]);//TODO:
                List<string> visibleMarkersCodes = JsonConvert.DeserializeObject<List<string>>(model.VisibleMarkers[0].ToSafeString());
                var addressList = new AddressLogic().GetList(dataContext);

                foreach (var code in visibleMarkersCodes)
                {
                    var item = addressList.Where(x => x.CodigoInterno == code).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = AddressLogic.GetCardCodeAndAddressFromCode(code)[0]; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = AddressLogic.GetCardCodeAndAddressFromCode(code)[1]; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.RazonSocial; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = departamentoList.FirstOrDefault(x => x.id == item.Departamento)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.Provincia; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.Distrito; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.Direccion; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = canalList.FirstOrDefault(x => x.id == item.Canal)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = giroList.FirstOrDefault(x => x.id == item.Giro)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.TipoCliente; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = zonaList.FirstOrDefault(x => x.id == item.ZonaId)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = rutaList.FirstOrDefault(x => x.id == item.RutaId)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = vendedorList.FirstOrDefault(x => x.id == item.Vendedor)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.Supervisor; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.JefeDeVentas; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaLunes ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaMartes ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaMiercoles ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaJueves ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaViernes ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaSabado ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.DiaDeVisitaDomingo ? "Si" : "No"; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = frecuenciaVisitaList.FirstOrDefault(x => x.id == item.FrecuenciaVisita)?.text.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.CodigoActivo; columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.GeoOptions?.coords?.lat.ToSafeString(); columnIndex++;
                        worksheet.Cells[rowIndex, columnIndex].Value = item.GeoOptions?.coords?.lng.ToSafeString(); columnIndex++;

                        rowIndex++;
                        columnIndex = 1;
                    }

                }

                #endregion

                package.SaveAs(new FileInfo(newExcelDocumentPath));

                switch (model.ReportType)
                {
                    case ReportType.Excel:
                        var memStream = new MemoryStream(package.GetAsByteArray());
                        return memStream;
                    case ReportType.Pdf:
                        CreateAndSavePdfFile(dataContext, model, newExcelDocumentPath, newPdfDocumentPath, newPdfDocumentEditedPath);
                        return newPdfDocumentEditedPath;
                    default:
                        throw new InvalidDataException(); //TODO:
                }
            }
        }

        public void CreateAndSavePdfFile(DataContext dataContext, MapViewModel model, string newExcelDocumentPath, string newPdfDocumentPath, string newPdfDocumentEditedPath)
        {
            WritePdf(newExcelDocumentPath, newPdfDocumentPath);
            var contentToReplace = "Evaluation Warning : The document was created with Spire.PDF for .NET.";
            VerySimpleReplaceText(newPdfDocumentPath, newPdfDocumentEditedPath, contentToReplace, string.Empty);
        }

        public static void WritePdf(string excelPath, string pdfPath)
        {
            // load Excel file
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(excelPath);

            // Set PDF template
            Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
            pdfDocument.PageSettings.Orientation = PdfPageOrientation.Landscape;
            //pdfDocument.PageSettings.Width = 970;
            //pdfDocument.PageSettings.Height = 850;


            //Convert Excel to PDF using the template above
            PdfConverter pdfConverter = new PdfConverter(workbook);
            PdfConverterSettings settings = new PdfConverterSettings();
            settings.TemplateDocument = pdfDocument;
            settings.FitSheetToOnePage = FitToPageType.ScaleWithSameFactor;
            pdfDocument = pdfConverter.Convert(settings);

            // Save and preview PDF
            pdfDocument.SaveToFile(pdfPath);
        }

        private static void VerySimpleReplaceText(string OrigFile, string ResultFile, string origText, string replaceText)
        {
            using (PdfReader reader = new PdfReader(OrigFile))
            {

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    byte[] contentBytes = reader.GetPageContent(i);
                    string contentString = PdfEncodings.ConvertToString(contentBytes, PdfObject.TEXT_PDFDOCENCODING);
                    contentString = contentString.Replace(origText, replaceText).Replace(origText, replaceText);
                    reader.SetPageContent(i, PdfEncodings.ConvertToBytes(contentString, PdfObject.TEXT_PDFDOCENCODING));
                }
                new PdfStamper(reader, new FileStream(ResultFile, FileMode.Create, FileAccess.Write)).Close();
            }
        }
    }
}
