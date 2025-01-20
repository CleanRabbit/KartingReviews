using System;
using System.Collections.Generic;
using System.Linq;

namespace Summers.Wyvern.Common
{
	public class Guarded : Loggable
	{
		/// <summary>
		/// Simplifies the validation of parameters
		/// </summary>
		/// <param name="value">The value to be checked.</param>
		/// <param name="name">The name of the value for logging purposes.</param>
		protected void Guard(object obj, string name)
		{
			if (obj == null)
				LogAndThrow<ArgumentNullException>(name);
		}

		/// <summary>
		/// Simplifies the validation of parameters
		/// </summary>
		/// <param name="objList">The list of values to be checked.</param>
		/// <param name="name">The name of the value for logging purposes.</param>
		protected void Guard<T>(IEnumerable<T> objList, string name)
		{
			if (objList == null)
				LogAndThrow<ArgumentNullException>(name);
			if (objList.Any(o => o == null))
				LogAndThrow<ArgumentException>($"{name} cannot contain null items");
		}

		/// <summary>
		/// Simplifies the validation of parameters
		/// </summary>
		/// <param name="value">The value to be checked.</param>
		/// <param name="name">The name of the value for logging purposes.</param>
		protected void Guard(string value, string name)
		{
			if (string.IsNullOrWhiteSpace(value))
				LogAndThrow<ArgumentNullException>(name);
		}
	}
}
