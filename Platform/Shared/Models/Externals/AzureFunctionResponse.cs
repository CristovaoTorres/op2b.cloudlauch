using System;
namespace Platform.Shared.Models.Externals
{
    public class AzureFunctionVirtualMachineStateResponse
    {
        public string PowerState { get; set; }
        public string Name { get; set; }
    }

    public class AzureFunctionResponse
    {
       public string OperationId { get; set; }
       public string Status { get; set; }
       public DateTime StartTime { get; set; }
       public DateTime EndTime { get; set; }
       public string Error { get; set; }
       public string Name { get; set; }

      public string DisplayStatus { get; set; }

        /// <summary>
        /// Estado da maquina virtual, opcoes validas:
        ///
        /// VM deallocated
        /// VM running
        /// </summary>
        public string PowerState { get; set; } 
    }



  
}
