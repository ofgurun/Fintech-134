using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteraktifKredi.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // Test için session'a kullanıcı bilgisi ekle (gerçek uygulamada login'den gelir)
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                HttpContext.Session.SetString("CustomerId", "12345");
                HttpContext.Session.SetString("UserName", "Ahmet Yılmaz");
            }
        }
    }
}

