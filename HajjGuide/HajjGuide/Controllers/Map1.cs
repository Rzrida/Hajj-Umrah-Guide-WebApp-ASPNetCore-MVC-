
using Azure.Core;
using HajjGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Transactions;

namespace HajjGuide.Controllers
{
    public class Map1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
