using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace NewCapit
{
    public class Veiculos2
    {

       

        public int linha { get; set; }
        
        public int nr_idveiculo { get; set; }
        
        public string ds_placa { get; set; }
        
        public int fl_bloqueio { get; set; }
       
        public string ds_cidade { get; set; }
        
        public string dt_posicao { get; set; }
       
        public int nr_dist_referencia { get; set; }
        
        public int nr_gps { get; set; }
        
        public int fl_ignicao { get; set; }
        
        public int nr_jamming { get; set; }
        
        public string ds_lat { get; set; }
       
        public string ds_long { get; set; }
       
        public int nr_odometro { get; set; }
        
        public string nr_pontoreferencia { get; set; }
       
        public string ds_rua { get; set; }
        
        public int nr_tensao { get; set; }
       
        public int nr_satelite { get; set; }
        
        public string ds_uf { get; set; }
        
        public int nr_velocidade { get; set; }
        public int rp { get; set; }
    }

}