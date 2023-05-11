using System;

namespace ExemploEventosDelegados.Exceptions
{
	[Serializable]
	public class ExceptionInputInvalido :Exception
	{
		public ExceptionInputInvalido()
		{
		}

        public ExceptionInputInvalido (string message): base (message)
		{
		}

        public ExceptionInputInvalido(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

