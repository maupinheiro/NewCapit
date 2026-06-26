using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewCapit.dist.Models;

namespace NewCapit.dist.ServicesCte
{


    public class ImpostoService
    {
        public decimal Calcular(decimal baseCalculo, decimal aliquota)
        {
            return baseCalculo * aliquota / 100;
        }
        public List<ImpostoCte> CalcularTodos(
            decimal baseCalculo,
            decimal icmsAliq,
            decimal ibsAliq,
            decimal cbsAliq)
        {
            return new List<ImpostoCte>
            {
                new ImpostoCte
                {
                    Imposto = "ICMS",
                    BaseCalculo = baseCalculo,
                    Aliquota = icmsAliq,
                    Valor = baseCalculo * icmsAliq / 100
                },

                new ImpostoCte
                {
                    Imposto = "IBS",
                    BaseCalculo = baseCalculo,
                    Aliquota = ibsAliq,
                    Valor = baseCalculo * ibsAliq / 100
                },

                new ImpostoCte
                {
                    Imposto = "CBS",
                    BaseCalculo = baseCalculo,
                    Aliquota = cbsAliq,
                    Valor = baseCalculo * cbsAliq / 100
                }
            };
        }
    }
}