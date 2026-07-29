using System;
using System.Collections.Generic;

namespace Domain
{
    public class ResultadoLancamentoDTO
    {

        public int Gravados { get; set; }

        public List<ControleFaltasDTO> DatasDuplicadas { get; set; }

        public ResultadoLancamentoDTO()
        {
            DatasDuplicadas = new List<ControleFaltasDTO>();
        }
    }
}
