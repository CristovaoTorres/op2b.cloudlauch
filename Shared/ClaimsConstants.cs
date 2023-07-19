using System;
namespace Platform.Shared
{
    public struct ClaimsConstants
    {
        public const string CLAIM_NAME_IDENTIFIER = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        public const string CLAIM_ID_USER = "idUser";
        public const string CLAIM_FULL_NAME = "fullname";
        /// <summary>
        /// Manter semore com esse valor.
        /// Referencia: JwtRegisteredClaimNames.UniqueName
        /// </summary>
        public const string CLAIM_USERNAME = "unique_name";
        public const string ID_USER = "idUser";



        public const string PERMISSION_CLAIM_CAN_START_VM = "can-start-vm";
        public const string PERMISSION_CLAIM_CAN_STOP_VM = "can-stop-vm";
        public const string PERMISSION_CLAIM_IS_ADMIN = "isAdmin";




    }
}
