using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChallengePad.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengePad.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        readonly IChallengePadDb Db;

        public FilesController(IChallengePadDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            Response.Headers.Add("Content-Disposition", $"attachment; filename={await Db.GetFileName(id, HttpContext.RequestAborted)}");
            return File(await System.IO.File.ReadAllBytesAsync($"./Uploads/{id}", HttpContext.RequestAborted), "application/octet-stream");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ICollection<IFormFile> files, [FromForm] long id, [FromForm] bool isObjectiveFile)
        {
            if (HttpContext.User.Identity.Name == null)
                throw new Exception("User without name claim");
            await Db.AddFiles(files, id, isObjectiveFile, HttpContext.User.Identity.Name, HttpContext.RequestAborted);
            return NoContent();
        }
    }
}
