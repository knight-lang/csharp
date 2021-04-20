using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Knight
{
	/// <summary>
	/// A class that represents a function call within Knight.
	/// </summary>
	public class Function : NonIdempotent
	{
		private static  Dictionary<char, (FunctionBody, int)> FUNCTIONS = new Dictionary<char, (FunctionBody, int)>();

		/// <summary>
		/// The signature that all Knight functions must adhere to.
		/// </summary>
		public delegate IValue FunctionBody(params IValue[] args);

		private FunctionBody _function;
		private IValue[] _args;
		private char _name;

		// note that this is private because we don't want people associating random `name`s with unrelated `function`s.
		private Function(FunctionBody function, char name, IValue[] args) {
			_function = function;
			_args = args;
			_name = name;
		}

		/// <summary>
		/// Executes this class's <c>function</c> with its <c>_args</c>.
		/// </summary>
		public override IValue Run() => _function(_args);

		/// <inheritdoc/>
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


		/// <summary>
		/// Registers a new function with the given <paramref name="arity"/> and <paramref name="body"/>, identified by <paramref name="name"/>.
		/// </summary>
		public static void Register(char name, int arity, FunctionBody body) => FUNCTIONS[name] = (body, arity);

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

		/// <summary>
		/// Reads a line from stdin.
		/// </summary>
		private static IValue Prompt(params IValue[] args) => new Text(Console.ReadLine());

		/// <summary>
		/// Returns a random <c>long</c>.
		/// </summary>
		private static IValue Random(params IValue[] args) => new Number(RANDOM.Next());


		/// <summary>
		/// Evaluates the first parameter as Knight code.
		/// </summary>
		private static IValue Eval(params IValue[] args) => Knight.Kn.Run(args[0].ToString());

		/// <summary>
		/// Simply returns the first argument, unevaluated.
		/// </summary>
		private static IValue Block(params IValue[] args) => args[0];

		/// <summary>
		/// <c>Run</c>s the first argument twice.
		/// </summary>
		private static IValue Call(params IValue[] args) => args[0].Run().Run();

#if ! EMBEDDED
		/// <summary>
		/// Runs the first argument as a shell command and returns the stdout of the command.
		/// </summary>
		private static IValue System(params IValue[] args) {
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
		}
#endif

		/// <summary>
		/// Stops the program execution with the given status code.
		/// </summary>
		private static IValue Quit(params IValue[] args) {
			Environment.Exit((int) args[0].ToLong());
			return null;
		}

		/// <summary>
		/// Returns the logical negation of the first argument.
		/// </summary>
		private static IValue Not(params IValue[] args) => new Boolean(!args[0].ToBool());

		/// <summary>
		/// Converts the first argument to a string, then returns its length.
		/// </summary>
		private static IValue Length(params IValue[] args) => new Number(args[0].ToString().Length);

		/// <summary>
		/// Dumps the first argument to stdout, then returns it.
		/// </summary>
		private static IValue Dump(params IValue[] args) {
			var val = args[0].Run();
			val.Dump();
			Console.WriteLine();
			return val;	
		}

		/// <summary>
		/// Prints the first argument to stdout with a trailing newline. If the line ends with `\\`, it's removed and no newline is printed.
		/// </summary>
		/// <returns>
		/// <c>Null</c>.
		/// </returns>
		private static IValue Output(params IValue[] args) {
			var val = args[0].ToString();

			if (val != "" && val[val.Length - 1] == '\\') {
				Console.Write(val.Remove(val.Length - 1));
			} else {
				Console.WriteLine(val);
			}

			return new Null();
		}

		/// <summary>
		/// Adds the first and second arguments together.
		/// </summary>
		private static IValue Add(params IValue[] args) => args[0].Run().Add(args[1].Run());

		/// <summary>
		/// Subtracts the second argument from the first.
		/// </summary>
		private static IValue Sub(params IValue[] args) => args[0].Run().Sub(args[1].Run());

		/// <summary>
		/// Multiplies the first and second arguments together.
		/// </summary>
		private static IValue Mul(params IValue[] args) => args[0].Run().Mul(args[1].Run());

		/// <summary>
		/// Divides the first argument by the second.
		/// </summary>
		private static IValue Div(params IValue[] args) => args[0].Run().Div(args[1].Run());

		/// <summary>
		/// Modulos the first argument by the second.
		/// </summary>
		private static IValue Mod(params IValue[] args) => args[0].Run().Mod(args[1].Run());

		/// <summary>
		/// Exponentiates the first argument by the second.
		/// </summary>
		private static IValue Pow(params IValue[] args) => args[0].Run().Pow(args[1].Run());

		/// <summary>
		/// Returns whether the first and second arguments are equal.
		/// </summary>
		private static IValue Eql(params IValue[] args) => new Boolean(args[0].Run().Equals(args[1].Run()));

		/// <summary>
		/// Checks to see if the first argument is less than the second.
		/// </summary>
		private static IValue Lth(params IValue[] args) => new Boolean(((IComparable<IValue>) args[0].Run()).CompareTo(args[1].Run()) < 0);

		/// <summary>
		/// Checks whether the first argument is greater than the second.
		/// </summary>
		private static IValue Gth(params IValue[] args) => new Boolean(((IComparable<IValue>) args[0].Run()).CompareTo(args[1].Run()) > 0);

		/// <summary>
		/// Returns the first argument if it's falsey, otherwise executes and returns the second.
		/// </summary>
		private static IValue And(params IValue[] args) {
			var lhs = args[0].Run();
				
			return lhs.ToBool() ? args[1].Run() : lhs;
		}

		/// <summary>
		/// Returns the first argument if it's true, otherwise executes and returns the second.
		/// </summary>
		private static IValue Or(params IValue[] args) {
			var lhs = args[0].Run();
				
			return lhs.ToBool() ? lhs : args[1].Run();
		}

		/// <summary>
		/// Executes the first argument, then executes and returns the second.
		/// </summary>
		private static IValue Then(params IValue[] args) {
			args[0].Run();
			return args[1].Run();
		}

		/// <summary>
		/// Assigns the second argument to the first. The first should be a <c>Variable</c>.
		/// </summary>
		private static IValue Assign(params IValue[] args) {
			var rhs = args[1].Run();

			((Variable) args[0]).Assign(rhs);

			return rhs;
		}

		/// <summary>
		/// Runs the second argument whilst the first is truth
		/// </summary>
		private static IValue While(params IValue[] args) {
			while (args[0].ToBool())
				args[1].Run();

			return new Null();
		}

		/// <summary>
		/// Executes the second argument if the first is truthy, otherwise executes the third.
		/// </summary>
		private static IValue If(params IValue[] args) => args[0].ToBool() ? args[1].Run() : args[2].Run();

		/// <summary>
		/// Returns a substring of the first argument, starting at the second with the length of the third.
		/// </summary>
		private static IValue Get(params IValue[] args) {
			var str = args[0].ToString();
			var start = (int) args[1].ToLong();
			var length = (int) args[2].ToLong();

			return new Text(str.Length <= start ? "" : str.Substring(start, length));
		}

		/// <summary>
		/// Returns the first argument, with the range [argument 2, argument 2+argument 3) replaced by the fourth argument.
		/// </summary>
		private static IValue Substitute(params IValue[] args) {
			var str = args[0].ToString();
			var start = (int) args[1].ToLong();
			var length = (int) args[2].ToLong();
			var repl = args[3].ToString();

			return new Text(str.Substring(0, start) + repl + str.Substring(start + length));
		}

		// registers all the functions for this class.
		// note that we use named functions so we can profile better.
		static Function() {
			Register('P', 0, Prompt);
			Register('R', 0, Random);

			Register('E', 1, Eval);
			Register('B', 1, Block);
			Register('C', 1, Call);
			Register('`', 1, System);
			Register('Q', 1, Quit);
			Register('!', 1, Not);
			Register('L', 1, Length);
			Register('D', 1, Dump);
			Register('O', 1, Output);

			Register('+', 2, Add);
			Register('-', 2, Sub);
			Register('*', 2, Mul);
			Register('/', 2, Div);
			Register('%', 2, Mod);
			Register('^', 2, Pow);
			Register('?', 2, Eql);
			Register('<', 2, Lth);
			Register('>', 2, Gth);
			Register('&', 2, And);
			Register('|', 2, Or);
			Register(';', 2, Then);
			Register('=', 2, Assign);
			Register('W', 2, While);

			Register('I', 3, If);
			Register('G', 3, Get);

			Register('S', 4, Substitute);
		}
	}
}
