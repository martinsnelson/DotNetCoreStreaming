using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DoNetCoreStreaming.Results
{
    public class InserirResultFluxo : IActionResult
    {
        private readonly Action<Stream, CancellationToken> _noFluxoDisponivel;
        private readonly string _tipoConteudo;
        private readonly CancellationToken _pedidoAbortado;

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="noFluxoDisponivel"></param>
        /// <param name="tipoConteudo"></param>
        /// <param name="pedidoAbortado"></param>
        public InserirResultFluxo(Action<Stream, CancellationToken> noFluxoDisponivel, string tipoConteudo, CancellationToken pedidoAbortado)
        {
            _noFluxoDisponivel = noFluxoDisponivel;
            _tipoConteudo = tipoConteudo;
            _pedidoAbortado = pedidoAbortado;
        }

        /// <Author>Nelson Martins</Author>
        /// <Date>19/02/2019</Date>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            var fluxo = context.HttpContext.Response.Body;

            context.HttpContext.Response.GetTypedHeaders().ContentType = new MediaTypeHeaderValue(_tipoConteudo);
            _noFluxoDisponivel(fluxo, _pedidoAbortado);

            return Task.CompletedTask;
        }
    }
}
