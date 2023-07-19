using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Client.Models
{

    /// <summary>
    /// Representa uma resposta agregada a ser apresentada na UI, decorrente de um processamento em camadas internas do App.
    /// </summary>
    public class AggregateResponse
    {
        public AggregateResponse()
        {
          
        }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }

    /// <summary>
    /// Representa uma resposta agregada a ser apresentada na UI, decorrente da analise das respostas de uma ou mais chamadas à camadas internas do App.
    /// </summary>
    public class AggregateResponse<TResponse>: AggregateResponse
    {
        public AggregateResponse()
        {
            Responses = new List<TResponse>();
        }

        /// <summary>
        /// Respostas recebidas camadas internas do app
        /// </summary>
        public List<TResponse> Responses { get; set; }
    }
}
