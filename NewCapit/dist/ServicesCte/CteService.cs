using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewCapit.dist.InterfacesCte;
using NewCapit.dist.Models;
using NewCapit.dist.ServicesCte;

namespace NewCapit.dist.Services
{
    public class CteService
    {
        private readonly ICfopService _cfopService;
        private readonly IFreteService _freteService;
        private readonly IImpostoService _impostoService;

        public CteService(
            ICfopService cfopService,
            IFreteService freteService,
            IImpostoService impostoService)
        {
            _cfopService = cfopService;
            _freteService = freteService;
            _impostoService = impostoService;
        }

        public ResultadoCte CalcularCte(
            string ufOrigem,
            string ufDestino,
            string tipoCte,
            List<ComponenteFrete> componentes,
            decimal aliqICMS,
            decimal aliqIBS,
            decimal aliqCBS)
        {
            var resultado = new ResultadoCte();

            resultado.CFOP = _cfopService.ObterCFOP(ufOrigem, ufDestino, tipoCte);

            decimal totalComponentes = _freteService.CalcularComponentes(componentes);
            resultado.TotalComponentes = totalComponentes;

            decimal baseCalculo = totalComponentes;

            resultado.ICMS = _impostoService.Calcular(baseCalculo, aliqICMS);
            resultado.IBS = _impostoService.Calcular(baseCalculo, aliqIBS);
            resultado.CBS = _impostoService.Calcular(baseCalculo, aliqCBS);

            resultado.TotalGeral =
                totalComponentes +
                resultado.ICMS +
                resultado.IBS +
                resultado.CBS;

            return resultado;
        }
    }
}