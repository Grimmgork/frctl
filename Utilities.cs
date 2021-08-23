using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frctl
{
	public static class Utilities
	{
		public static int MapValue(int value, int fromLow, int fromHigh, int toLow, int toHigh)
		{
			return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
		}
	}
}
