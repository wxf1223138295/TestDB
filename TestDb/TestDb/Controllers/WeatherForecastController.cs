using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using TestDb.Model;
using TestDb.MyDbContext;

namespace TestDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public AplloDbContext _aplloDbContext;
        public BankDbContext _bankDbContext;

        public WeatherForecastController(AplloDbContext aplloDbContext, BankDbContext bankDbContext)
        {
            _aplloDbContext = aplloDbContext;
            _bankDbContext = bankDbContext;
        }

        [HttpGet]
        public async Task<string> Get()
        {

            //using (var tran= await _aplloDbContext.Database.BeginTransactionAsync())
            //{
            //    try
            //    {
            //        await _aplloDbContext.TestApllo.AddAsync(new TestApllo { Name = "2222" });
            //        await _aplloDbContext.SaveChangesAsync();
            //        await _aplloDbContext.Database.GetDbConnection()
            //            .ExecuteAsync("insert into mqm.testapllo(name) values ('1111')");

            //        await tran.CommitAsync();
            //    }
            //    catch (Exception e)
            //    {
            //        await tran.RollbackAsync();
            //    }
            //}

            //var strategy = _aplloDbContext.Database.CreateExecutionStrategy();

            //await strategy.ExecuteAsync(async () =>
            //{
            //    using (var tran = await _aplloDbContext.Database.BeginTransactionAsync())
            //    {

            //    }
            //});

       

            return "2222";
        }
    }
}
