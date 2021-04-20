using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Knight
{
	public class Function : NonIdempotent
	{
		private static  Dictionary<char, (FunctionBody, int)> FUNCTIONS = new Dictionary<char, (FunctionBody, int)>();
		public delegate IValue FunctionBody(params IValue[] args);

		private FunctionBody _function;
		private IValue[] _args;
		private char _name;

		private Function(FunctionBody function, char name, IValue[] args) {
			_function = function;
			_args = args;
			_name = name;
		}

		public override IValue Run() => _function(_args);
		public override void Dump() {
			Console.Write("Function(");

			var first = true;

			foreach (var arg in _args) {
				if (first) {
					first = false;
				} else {
					Console.Write(", ");
				}

				arg.Dump();
			}

			Console.Write(")");
		}

		public static void Register(char name, int arity, FunctionBody body) => FUNCTIONS[name] = (body, arity);

		/// <summary>
		/// Attempts to parse a <c>Function</c> from the given <paramref name="stream"/>, returnning <see langword="null"/> if nothing could be parsed.
		/// </summary>
		/// <param name="stream">
		/// The stream from which to parse.
		/// </param>
		/// <returns>
		/// The parsed <c>Function</c>, or <see langword="null"/> if the <paramref name="stream"> didn't start with a function character.
		/// </returns>
		internal static Function Parse(Stream stream) {
			(FunctionBody, int) func = (null, 0);
			char name;

			if (!stream.StartsWith(c => FUNCTIONS.TryGetValue(c, out func)))
				return null;

			if (char.IsUpper(name = stream.Take()))
				stream.StripKeyword();

			var args = new IValue[func.Item2];

			for (int i = 0; i < func.Item2; ++i) {
				if ((args[i] = Kn.Parse(stream)) == null)
					throw new ParseException($"Unable to parse variable '{i}' for function '{name}'.");
			}

			return new Function(func.Item1, name, args);
		}

		private static readonly Random RANDOM = new Random();

		private static IValue Prompt(params IValue[] args) => new Text(Console.ReadLine());
		static Function() {
			/// <summary>/
			Register('P', 0, Prompt);
			Register('R', 0, args => new Number(RANDOM.Next()));

			Register('E', 1, args => Knight.Kn.Run(args[0].ToString()));
			Register('B', 1, args => args[0]);
			Register('C', 1, args => args[0].Run().Run());
			Register('`', 1, args => {
				Process proc = new Process();
				// Redirect the output stream of the child process.
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.FileName = "/bin/sh";
				proc.StartInfo.ArgumentList.Add("-c");
				proc.StartInfo.ArgumentList.Add(args[0].ToString());
				proc.Start();
				string output = proc.StandardOutput.ReadToEnd();
				proc.WaitForExit();
				return new Text(output);
			});
			Register('Q', 1, args => { Environment.Exit((int) args[0].ToLong()); return null; });
			Register('!', 1, args => new Boolean(!args[0].ToBool()));
			Register('L', 1, args => new Number(args[0].ToString().Length));
			Register('D', 1, args => {
				var val = args[0].Run();
				val.Dump();
				Console.WriteLine();
				return val;
			 });
			Register('O', 1, args => {
				var val = args[0].ToString();

				if (val[val.Length - 1] == '\\') {
					Console.Write(val.Remove(val.Length - 1));
				} else {
					Console.WriteLine(val);
				}

				return new Null();
			});

			Register('+', 2, args => args[0].Run().Add(args[1].Run()));
			Register('-', 2, args => args[0].Run().Sub(args[1].Run()));
			Register('*', 2, args => args[0].Run().Mul(args[1].Run()));
			Register('/', 2, args => args[0].Run().Div(args[1].Run()));
			Register('%', 2, args => args[0].Run().Mod(args[1].Run()));
			Register('^', 2, args => args[0].Run().Pow(args[1].Run()));
			Register('?', 2, args => new Boolean(args[0].Run().Equals(args[1].Run())));
			Register('<', 2, args => new Boolean(((IComparable<IValue>) args[0].Run()).CompareTo(args[1].Run()) < 0));
			Register('>', 2, args => new Boolean(((IComparable<IValue>) args[0].Run()).CompareTo(args[1].Run()) > 0));
			Register('&', 2, args => {
				var lhs = args[0].Run();
				
				return lhs.ToBool() ? args[1].Run() : lhs;
			});
			Register('|', 2, args => {
				var lhs = args[0].Run();
				
				return lhs.ToBool() ? lhs : args[1].Run();
			});
			Register(';', 2, args => {
				args[0].Run();
				return args[1].Run();
			});
			Register('=', 2, args => {
				var rhs = args[1].Run();

				((Identifier) args[0]).Assign(rhs);

				return rhs;
			});
			Register('W', 2, args => {
				while (args[0].ToBool()) {
					args[1].Run();
				}
				return new Null();
			});

			Register('I', 3, args => args[0].ToBool() ? args[1].Run() : args[2].Run());
			Register('G', 3, args => {
				var str = args[0].ToString();
				var start = (int) args[1].ToLong();
				var length = (int) args[2].ToLong();

				return new Text(str.Length <= start ? "" : str.Substring(start, length));
			});

			Register('S', 4, args => {
				var str = args[0].ToString();
				var start = (int) args[1].ToLong();
				var length = (int) args[2].ToLong();
				var repl = args[3].ToString();

				return new Text(str.Substring(0, start) + repl + str.Substring(start + length));
			});
			
		}
	}
}
