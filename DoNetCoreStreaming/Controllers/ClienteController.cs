using DoNetCoreStreaming.Enums;
using DoNetCoreStreaming.Models;
using DoNetCoreStreaming.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace DoNetCoreStreaming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClienteController : ControllerBase
    {
        private static ConcurrentBag<StreamWriter> _clientes;

        static ClienteController()
        {
            _clientes = new ConcurrentBag<StreamWriter>();
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Streaming")]
        public IActionResult Stream()
        {
            return new InserirResultFluxo(NoFluxoDisponivel, "text/event-stream", HttpContext.RequestAborted);
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            //Fazer o Insert
            EnviarEvento(cliente, EventoEnum.Insert);

            return Ok();
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(Cliente cliente)
        {
            //Fazer o Update
            EnviarEvento(cliente, EventoEnum.Update);

            return Ok();
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>20/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(Cliente cliente)
        {
            EnviarEvento(cliente, EventoEnum.Delete);

            return Ok();
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        private static async Task EnviarEvento(object dados, EventoEnum evento)
        {
            foreach (var cliente in _clientes)
            {
                string jsonEvento = string.Format("{0}\n", JsonConvert.SerializeObject(new { dados, evento }));
                await cliente.WriteAsync(jsonEvento);
                await cliente.FlushAsync();
            }
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pedidoAbortado"></param>
        private void NoFluxoDisponivel(Stream stream, CancellationToken pedidoAbortado)
        {
            var wait = pedidoAbortado.WaitHandle;
            var client = new StreamWriter(stream);
            _clientes.Add(client);

            wait.WaitOne();

            StreamWriter ignore;
            _clientes.TryTake(out ignore);
        }
    }
}