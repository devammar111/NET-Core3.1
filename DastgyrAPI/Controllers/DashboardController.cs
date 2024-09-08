using System.Net;
using System.Threading.Tasks;
using DastgyrAPI.Interfaces.Services;
using DastgyrAPI.Models.ViewModels.ResponseModels;
using DastgyrAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DastgyrAPI.Controllers
{
    public class Dashboard : BaseController
    {
        private IOrdersService _ordersService;

        public Dashboard(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: api/Dashboard
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DashboardResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var response = await _ordersService.GetDashboardResultByUserId();
            return Ok(response);
        }

    }
}