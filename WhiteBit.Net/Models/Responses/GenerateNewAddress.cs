using System;
namespace WhiteBit.Net.Models.Responses
{
	public class GenerateNewAddress
	{

		public AccountModel Account { get; set; }
		public class AccountModel
		{
			public string Address { get; set; }
			public string Memo { get; set; }
		}
	}
}

