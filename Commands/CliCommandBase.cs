using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frctl.Commands
{
	public abstract class CliCommandBase
	{
		internal string CommandName;

	    private CliCommandBase FindSubCommand(string name)
		{
			IEnumerable<Type> kek = this.GetType().Assembly.GetTypes().Where(type => type.IsSubclassOf(this.GetType()));
			return null;
		}

		public string Route(string[] arguments)
		{
			if(arguments.Length == 0)//> 0
			{
				CliCommandBase cmd = FindSubCommand(""); //FindSubCommand(arguments[0]);
				if (cmd != null)
				{
					return cmd.Route(arguments.Skip(1).ToArray());
				}
			}

			return Exec(arguments);
		}

		public virtual string Exec(string[] arguments)
		{
			string res = "Im " + CommandName + "-command! these were my arguments: ";
			foreach (string s in arguments)
			{
				res += "[" + s + "] ";
			}

			return res;
		}

		public virtual string GetName()
		{
			return "Root";
		}
	}

	public struct CommandData
	{
		string name;
		object parameters;
		object options;
		Type instance;
		CommandData[] subcommands;
	}
}