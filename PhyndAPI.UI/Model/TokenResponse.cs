using System.Web.Mvc;
using System.Web;
using PhyndAPI.DATA.Models;

namespace PhyndAPI.UI.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }

    }
}