using System;
using System.Runtime.Serialization;

namespace WiSoLibrary
{
	[Serializable]
	internal class NotEnoughAnswersGivenException : Exception
	{
		public NotEnoughAnswersGivenException()
		{
		}

		public NotEnoughAnswersGivenException(string message) : base(message)
		{
		}

		public NotEnoughAnswersGivenException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NotEnoughAnswersGivenException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}