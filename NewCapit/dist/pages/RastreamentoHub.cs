using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.Configuration;

namespace NewCapit.dist.pages
{
    public class RastreamentoHub : Hub
    {
        // Método chamado pelo dispositivo / simulador para enviar telemetria
        SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conexao"].ToString());
        public void EnviarTelemetria(dynamic data)
        {
            // data deve conter: NumeroCarga, Latitude, Longitude, Velocidade, RPM, Temperatura, NivelCombustivel, FrenagemBrusca
            string numero = data.NumeroCarga;
            double lat = (double)data.Latitude;
            double lng = (double)data.Longitude;
            decimal vel = (decimal)data.Velocidade;
            int rpm = (int)data.RPM;
            decimal temp = (decimal)data.Temperatura;
            decimal nivel = (decimal)data.NivelCombustivel;
            bool frenagem = (bool)data.FrenagemBrusca;

            // 1) Persistir no banco (opcional, mas recomendado)
            SaveTelemetria(numero, lat, lng, vel, rpm, temp, nivel, frenagem);

            // 2) Broadcast para todos clientes conectados que estão assistindo essa carga
            Clients.Group(numero).atualizarTelemetria(new
            {
                NumeroCarga = numero,
                Latitude = lat,
                Longitude = lng,
                Velocidade = vel,
                RPM = rpm,
                Temperatura = temp,
                NivelCombustivel = nivel,
                FrenagemBrusca = frenagem,
                Timestamp = DateTime.UtcNow
            });

            // Se condição de alerta detectada no servidor, salvar evento e notificar
            if (frenagem)
            {
                SaveEvento(numero, "FRENAGEM", "Frenagem brusca detectada.");
                Clients.Group(numero).alerta(new { tipo = "FRENAGEM", msg = "Frenagem brusca detectada" });
            }
        }

        // Quando cliente "assiste" uma carga, adicionar ao grupo
        public Task JoinGrupo(string numeroCarga)
        {
            return Groups.Add(Context.ConnectionId, numeroCarga);
        }

        public Task LeaveGrupo(string numeroCarga)
        {
            return Groups.Remove(Context.ConnectionId, numeroCarga);
        }

        void SaveTelemetria(string numero, double lat, double lng, decimal vel, int rpm, decimal temp, decimal nivel, bool frenagem)
        {
            // Inserir no banco - ajuste connection string
            using (var cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                string sql = @"INSERT INTO tbTelemetria(NumeroCarga, Latitude, Longitude, Velocidade, RPM, Temperatura, NivelCombustivel, FrenagemBrusca)
                           VALUES (@num,@lat,@lng,@vel,@rpm,@temp,@nivel,@fren)";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@num", numero);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lng", lng);
                    cmd.Parameters.AddWithValue("@vel", vel);
                    cmd.Parameters.AddWithValue("@rpm", rpm);
                    cmd.Parameters.AddWithValue("@temp", temp);
                    cmd.Parameters.AddWithValue("@nivel", nivel);
                    cmd.Parameters.AddWithValue("@fren", frenagem);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        void SaveEvento(string numero, string tipo, string descricao)
        {
            using (var cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString))
            {
                string sql = "INSERT INTO tbEventos(NumeroCarga, Tipo, Descricao) VALUES(@num,@tipo,@desc)";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@num", numero);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@desc", descricao);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}