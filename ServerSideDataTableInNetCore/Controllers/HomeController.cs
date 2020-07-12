using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSideDataTableInNetCore.Data;
using ServerSideDataTableInNetCore.Data.POCO;
using ServerSideDataTableInNetCore.Extensions;
using ServerSideDataTableInNetCore.Models;

namespace ServerSideDataTableInNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            await SeedData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = await _context.Employees.ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.FirstSurname != null && r.FirstSurname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.SecondSurname != null && r.SecondSurname.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Street != null && r.Street.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Phone != null && r.Phone.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.ZipCode != null && r.ZipCode.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Country != null && r.Country.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Notes != null && r.Notes.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Employees.CountAsync();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToList()
            });
        }

        public async Task SeedData()
        {
            if (!_context.Employees.Any())
            {
                for (var i = 0; i < 1000; i++)
                {
                    await _context.Employees.AddAsync(new Employee
                    {
                        Name = $"TestName{i}",
                        FirstSurname = $"TestFirstSurname{i}",
                        SecondSurname = $"TestSecondSurname{i}",
                        Street = $"TestStreet{i}",
                        Phone = $"TestPhone{i}",
                        ZipCode = $"TestZipCode{i}",
                        Country = $"TesCountry{i}",
                        Notes = $"TestNotes{i}"
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
