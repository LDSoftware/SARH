using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Configuration;
using ISOSA.SARH.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SARH.WebUI.Models.Configuration;

namespace SARH.WebUI.Controllers
{
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [ActionName("Descuentos")]
        public IActionResult Discounts([FromServices]IRepository<EmployeeDiscount> discountRepo)
        {
            ScheduleDiscountModel model = new ScheduleDiscountModel();

            var allDiscounts = discountRepo.GetAll().ToList();

            if (allDiscounts.Any()) 
            {
                model.SchedulesDiscounts = allDiscounts.Select(u => new ScheduleDiscountModelItem()
                {
                    Id = u.Id,
                    DiscountName = string.IsNullOrEmpty(u.CombineDiscount) ? $"{u.Description}" : GetDiscountName(u.Id, u.CombineDiscount, allDiscounts),
                    Descripcion = string.IsNullOrEmpty(u.CombineDiscount) ? $"{DiscountType(u.DiscountType.ToString())} de {u.RangeInitial} a {u.RangeEnd}" : GetDiscountCombine(u.Id, u.CombineDiscount, allDiscounts),
                    Discount = string.IsNullOrEmpty(u.CombineDiscount) ? $"{u.Discount}%" : GetDiscountPorcentaje(u.Id, u.CombineDiscount, allDiscounts),
                    TipoDescuento = string.IsNullOrEmpty(u.CombineDiscount) ? "Simple" : "Mixto",
                    Habilitado = u.Enabled == true ? "Si" : "No"
                }).ToList();
            }

            List<SelectListItem> catalog = new List<SelectListItem>();
            catalog.Add(new SelectListItem() { Value = "1", Text = "Retardo entrada" });
            catalog.Add(new SelectListItem() { Value = "2", Text = "Salida anticipada" });
            catalog.Add(new SelectListItem() { Value = "3", Text = "Sin Registro de entrada" });
            catalog.Add(new SelectListItem() { Value = "7", Text = "Sin Registro de salida" });
            catalog.Add(new SelectListItem() { Value = "8", Text = "Sin Registro de salida comida" });
            catalog.Add(new SelectListItem() { Value = "9", Text = "Sin Registro de entrada comida" });
            catalog.Add(new SelectListItem() { Value = "4", Text = "Salida anticipada horario de comida" });
            catalog.Add(new SelectListItem() { Value = "5", Text = "Retardo horario de comida" });

            ViewBag.Catalog = catalog;

            return View(model);
        }

        [HttpPost]
        public JsonResult SaveDiscount(ScheduleDiscountInput model, [FromServices]IRepository<EmployeeDiscount> discountRepo)
        {
            string combo = string.Empty;
            if (!string.IsNullOrEmpty(model.Combinacion))
            {
                var vars = model.Combinacion.Split('|');
                vars.ToList().ForEach(x =>
                {
                    var lines = x.Split('-');
                    if (lines.Length > 1) 
                    {
                        combo += lines[1] + "|";
                    }
                });
            }

            discountRepo.Create(new EmployeeDiscount() 
            {
                DiscountType = int.Parse(model.Tipo),
                Discount = model.Porcentaje,
                Days = model.Dias,
                CombineDiscount = combo,
                RangeInitial = model.RangeInitial,
                RangeEnd = model.RangeEnd,
                Description = model.Description
            });


            return Json("Ok");
        }

        [HttpPost]
        public JsonResult DisabledDiscount(int id, [FromServices]IRepository<EmployeeDiscount> discountRepo) 
        {

            var row = discountRepo.GetElement(id);
            if (row != null && row.Enabled) 
            {
                row.Enabled = false;
                discountRepo.Update(row);
            }

            return Json("ok");
        }


        [HttpPost]
        public JsonResult EnabledDiscount(int id, [FromServices]IRepository<EmployeeDiscount> discountRepo)
        {

            var row = discountRepo.GetElement(id);
            if (row != null)
            {
                row.Enabled = true;
                discountRepo.Update(row);
            }

            return Json("ok");
        }


        [HttpPost]
        public JsonResult ViewDiscounts(int id, [FromServices]IRepository<EmployeeDiscount> discountRepo) 
        {
            var discounts = discountRepo.GetAll().ToList();
            var row = discountRepo.GetElement(id);
            List<ScheduleDiscountModelItem> assigneddiscounts = new List<ScheduleDiscountModelItem>();

            assigneddiscounts.Add(new ScheduleDiscountModelItem() 
            {
                Descripcion = $"{DiscountType(row.DiscountType.ToString())} de {row.RangeInitial} a {row.RangeEnd}",
                Discount = $"{row.Discount}%",
            });



            if (!string.IsNullOrEmpty(row.CombineDiscount)) 
            {
                foreach (var item in row.CombineDiscount.Split('|').ToList())
                {
                    if (!string.IsNullOrEmpty(item)) 
                    {
                        var element = discounts.Where(f => f.Id.Equals(int.Parse(item))).FirstOrDefault();
                        assigneddiscounts.Add(new ScheduleDiscountModelItem()
                        {
                            Descripcion = $"{DiscountType(element.DiscountType.ToString())} de {element.RangeInitial} a {element.RangeEnd}",
                            Discount = $"{element.Discount}%",
                        });
                    }
                }
            }

            return Json(assigneddiscounts);
        }

        [HttpPost]
        public JsonResult GetEditDiscount(int id, [FromServices]IRepository<EmployeeDiscount> discountRepo) 
        {

            var row = discountRepo.GetElement(id);
            var response = new
            {
                id = id,
                discounttype = $"{DiscountType(row.DiscountType.ToString())}",
                rangeini = row.RangeInitial,
                rangefin = row.RangeEnd,
                dias = row.Days,
                descripcion = row.Description,
                discount = row.Discount,
                enabled = row.Enabled
            };

            return Json(response);
        }


        [HttpPost]
        public JsonResult SaveDiscountEdit(int id, ScheduleDiscountInput model, [FromServices]IRepository<EmployeeDiscount> discountRepo)
        {
            var row = discountRepo.GetElement(id);

            row.Discount = model.Porcentaje;
            row.Days = model.Dias;
            row.RangeInitial = model.RangeInitial;
            row.RangeEnd = model.RangeEnd;
            row.Description = model.Description;
            discountRepo.Update(row);

            return Json("Ok");
        }



        private string DiscountType(string id) 
        {
            string response = string.Empty;
            switch (id)
            {
                case "1":
                    response = "Retardo entrada";
                    break;
                case "2":
                    response = "Salida anticipada";
                    break;
                case "3":
                    response = "Sin Registro de entrada";
                    break;
                case "4":
                    response = "Salida anticipada horario de comida";
                    break;
                case "5":
                    response = "Retardo horario de comida";
                    break;
                case "6":
                    response = "Sin registro de comida";
                    break;
                case "7":
                    response = "Sin Registro de salida";
                    break;
                case "8":
                    response = "Sin Registro de salida comida";
                    break;
                case "9":
                    response = "Sin Registro de entrada comida";
                    break;

                default:
                    break;
            }

            return response;
        }

        private string GetDiscountCombine(int id, string parameters, List<EmployeeDiscount> discountAll)
        {
            string response = string.Empty;
            var row = discountAll.Where(d => d.Id.Equals(id)).FirstOrDefault();
            response = $"{DiscountType(row.DiscountType.ToString())} de {row.RangeInitial} a {row.RangeEnd},";

            foreach (var item in parameters.Split('|').ToList())
            {
                if (!string.IsNullOrEmpty(item)) 
                {
                    var element = discountAll.Where(f => f.Id.Equals(int.Parse(item))).FirstOrDefault();
                    response += $"{DiscountType(element.DiscountType.ToString())} de {element.RangeInitial} a {element.RangeEnd},";
                }
            }

            response = response.Remove(response.Length - 1);
            return response;
        }

        private string GetDiscountName(int id, string parameters, List<EmployeeDiscount> discountAll)
        {
            string response = string.Empty;
            var row = discountAll.Where(d => d.Id.Equals(id)).FirstOrDefault();
            response = $"{row.Description} +";

            foreach (var item in parameters.Split('|').ToList())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var element = discountAll.Where(f => f.Id.Equals(int.Parse(item))).FirstOrDefault();
                    response += $"{element.Description} +";
                }
            }

            response = response.Remove(response.Length - 1);
            return response;
        }

        private string GetDiscountPorcentaje(int id, string parameters, List<EmployeeDiscount> discountAll)
        {
            int response =0;
            var row = discountAll.Where(d => d.Id.Equals(id)).FirstOrDefault();
            response = int.Parse(row.Discount);

            foreach (var item in parameters.Split('|').ToList())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var element = discountAll.Where(f => f.Id.Equals(int.Parse(item))).FirstOrDefault();
                    response += int.Parse(element.Discount);
                }
            }

            return $"{response}%";
        }

    }
}