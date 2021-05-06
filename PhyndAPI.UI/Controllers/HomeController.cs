using PhyndAPI.DATA.Models;
using PhyndAPI.UI.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;

namespace PhyndAPI.UI.Controllers
{
    public class HomeController : DefaultController
    {
        public ActionResult Index()
        {
            ViewBag.Result = "";
            ViewData["Comments"] = null;
            return View();
        }

        public ActionResult TestAPI_NoAuth(string npiList = "", int maxComments = 10, int pageNum = 1, int pageSize = 25)
        {
            var client = new RestClient("https://localhost:44389/api/Get");
            var request = new RestRequest(Method.GET);
            request.AddParameter("Comments", maxComments);
            request.AddParameter("Page", pageNum);
            request.AddParameter("PageSize", pageSize);
            List<string> npis = new List<string>(npiList.Split(','));
            foreach (string npi in npis)
            {
                request.AddParameter("NPI", npi);
            }
            request.Timeout = int.MaxValue;
            IRestResponse response = client.Execute(request);

            if (response.Content.Contains("Authorization has been denied for this request."))
            {
                ViewBag.Result = "Authorization has been denied for this request.";
                ViewData["Comments"] = null;
                return View("Index");
            }
            else
            {
                ViewBag.Result = "";
                IEnumerable<PGComment> comments = new JsonDeserializer().Deserialize<IEnumerable<PGComment>>(response);
                ViewData["Comments"] = comments;
                return View("Index");
            }
        }

        public ActionResult TestAPI_WithAuth(string npiList = "", int maxComments = 10, int pageNum = 1, int pageSize = 25)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var client = new RestClient("https://dev-6ghwg5kl.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            var clientID = ConfigurationManager.AppSettings["API_ClientID"];
            var clientSecret = ConfigurationManager.AppSettings["API_ClientSecret"];
            var apiIdentifier = ConfigurationManager.AppSettings["API_Identifier"];
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={clientID}&client_secret={clientSecret}&audience={apiIdentifier}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            TokenResponse token = new JsonDeserializer().Deserialize<TokenResponse>(response);

            client = new RestClient("https://localhost:44389/api/Get");
            request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {token.access_token}");
            request.AddParameter("Comments", maxComments);
            request.AddParameter("Page", pageNum);
            request.AddParameter("PageSize", pageSize);
            List<string> npis = new List<string>(npiList.Split(','));
            foreach(string npi in npis)
            {
                request.AddParameter("NPI", npi);
            }
            request.Timeout = int.MaxValue;
            response = client.Execute(request);

            if (response.Content.Contains("Authorization has been denied for this request."))
            {
                ViewBag.Result = "Authorization has been denied for this request.";
                ViewData["Comments"] = null;
                return View("Index");
            }
            else
            {
                ViewBag.Result = "";
                IEnumerable<PGComment> comments = new JsonDeserializer().Deserialize<IEnumerable<PGComment>>(response);
                ViewData["Comments"] = comments;
                return View("Index");
            }
        }
    }
}