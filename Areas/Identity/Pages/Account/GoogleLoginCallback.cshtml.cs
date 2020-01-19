using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chat.Areas.Identity.Pages.Account
{
    public class GoogleLoginCallbackModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return RedirectToPage();
        }
    }
}
