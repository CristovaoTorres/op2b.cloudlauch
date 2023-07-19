using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Platform.Shared.Models
{

    /// <summary>
    /// Informacoes do usuario que chama/executa uma controller http.
    /// </summary>
    public class UserContextInfo
    {
        [JsonIgnore]
        public System.Security.Principal.IIdentity User { get; set; }
        public UserContextInfo()
        {
        }
        public UserContextInfo(System.Security.Principal.IIdentity user) : this(user, string.Empty)
        {
        }
        public UserContextInfo(System.Security.Principal.IIdentity user, string ipAddress)
        {
            User = user;

            IdUser = GetClaimValue<int>(ClaimsConstants.CLAIM_ID_USER, 0);
            if (IdUser == 0)
            {
                IdUser = GetClaimValue<int>(ClaimsConstants.CLAIM_NAME_IDENTIFIER, 0);
            }
           
            SetContext(IdUser,
                        GetClaimValue<string>(ClaimsConstants.CLAIM_FULL_NAME, string.Empty),
                        null,
                        ipAddress);

        }

  


        public void SetContext(int idUser, string fullName, Dictionary<string, string> headers = null, string ipAddress = null, Guid? appSessionId = null)
        {
            this.IdUser = idUser;
            this.FullName = fullName;
            this.Headers = headers;
            this.IpAddress = ipAddress;
        }


        private T GetClaimValue<T>(string claimType, T defaultValue = default(T))
        {
            var identity = User as ClaimsIdentity;

            var claims = identity.Claims.Where(c => c.Type == claimType);

            if (claims == null || claims.Count() == 0)
            {
                return defaultValue;
            }

            object obj = claims.Select(x => x.Value).FirstOrDefault();

            if ((object)obj == null || obj == DBNull.Value)
            {
                return defaultValue;
            }


            if (defaultValue is Guid)
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }


        /// <summary>
        /// Id do usuario chamador.
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Nome completo do usuario
        /// </summary>
        public string FullName { get; set; }


        /// <summary>
        /// Headers HTTP da requisicao do usuario.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Endereco de Ip do chamador.
        /// </summary>
        public string IpAddress { get; set; }

    


    }
}
