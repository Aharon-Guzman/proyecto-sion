using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Exceptions
{
    public class SionException : Exception
    {
        public SionException(string message) : base(message){}
        public SionException(string message, Exception innerException) : base(message, innerException){}
    }
    public class EntidadNoEncontradaException : SionException
    {
        public EntidadNoEncontradaException(string entidad, int id) :
        base($"{entidad} con ID {id} no fue encontrado"){}
    }
    public class OperacionNoPermitidaException : SionException 
    
    {
        public OperacionNoPermitidaException(string mensaje) : base(mensaje) { }

    }
    public class ArchivoInvalidoException : SionException
    {
        public ArchivoInvalidoException(string mensaje) : base(mensaje) { }
    }

}
