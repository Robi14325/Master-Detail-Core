using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DetailCrud.Models;

namespace DetailCrud.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly DbApplicant _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApplicantsController(DbApplicant context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {

            return View(_context.Applicants.Include(a => a.ApplicantExes).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.ApplicantExes.Add(new ApplicantEx()
            {
                CompanyName = "",
                Designation = "",
                YearofEx = 0
            });
            return View(applicant);



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string btn, Applicant applicant)
        {
            if (btn == "ADD")
            {
                applicant.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Create")
            {

                if (applicant.Picture != null)
                {

                    var rootPath = this._webHostEnvironment.ContentRootPath;
                    var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", applicant.Picture.FileName);
                    using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                    {
                        applicant.Picture.CopyToAsync(fileStream);
                    }
                    applicant.Picpath = "~/Pictures/" + applicant.Picture.FileName;

                    _context.Applicants.Add(applicant);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Provide Profile Picture");
                    return View(applicant);
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                ModelState.AddModelError("", message);
            }

            return View(applicant);
        }
        public IActionResult Edit(int id)
        {

            return View(_context.Applicants.Include(f => f.ApplicantExes).Where(f => f.Id.Equals(id)).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult Edit(Applicant applicant, string btn)
        {
            if (btn == "ADD")
            {
                applicant.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Update")
            {
                var oldapplicant = _context.Applicants.Find(applicant.Id);
                if (applicant.Picture != null)
                {

                    var rootPath = this._webHostEnvironment.ContentRootPath;
                    var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", applicant.Picture.FileName);
                    using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                    {
                        applicant.Picture.CopyToAsync(fileStream);
                    }
                    applicant.Picpath = "~/Pictures/" + applicant.Picture.FileName;

                }
                else
                {
                    oldapplicant.Picpath = oldapplicant.Picpath;
                }
                oldapplicant.Name = applicant.Name;
                oldapplicant.Date = applicant.Date;
                //oldapplicant.TotalEx = applicant.TotalEx;
                _context.ApplicantsEx.RemoveRange(_context.ApplicantsEx.Where(s => s.ApplicantID == applicant.Id));
                _context.SaveChanges();
                oldapplicant.ApplicantExes = applicant.ApplicantExes;
                _context.Entry(oldapplicant).State = EntityState.Modified;
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
                //}
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
                    ModelState.AddModelError("", message);
                }
            }
            return View(applicant);
        }
        public ActionResult Details(int id)
        {

            Applicant employee = _context.Applicants.Include(a => a.ApplicantExes).FirstOrDefault(a => a.Id == id);



            return View(employee);
        }
        public ActionResult Delete(int id)
        {
            _context.Applicants.Remove(_context.Applicants.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
