using AgvantageTransferLogViewer.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;


namespace AgvantageTransferLogViewer.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferLogController : ControllerBase
    {
        private readonly AgvantageTransferContext _context;

        public TransferLogController(AgvantageTransferContext context)
        {
            _context = context;
        }


       
        [HttpGet("Get")]
        public IActionResult Get([FromQuery] DataSourceLoadOptions loadOptions)
        {
            var query = _context.AgvantageTransferLogs
                                .AsNoTracking()
                                .OrderByDescending(x => x.TaskTime);

            var result = DataSourceLoader.Load(query, loadOptions);
            return Ok(result);
        }
    }

}
