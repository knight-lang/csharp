using System;
using System.IO;

namespace Knight
{
	/// <summary>
	/// A Helper class for running Knight code.
	/// </summary>
	public class Kn
	{
		/// <summary>
		/// Parses a value from the stream, returning <see langword="null"/> if nothing could be parsed.
		/// </summary>
		internal static IValue Parse(Stream stream) {
			while (!stream.IsEmpty()) {
				// strip comments.
				if (stream.TakeWhileIfStartsWith('#', c => c != '\n') != null)
					continue;

				// strip whitespace.
				if (stream.TakeWhile(c => char.IsWhiteSpace(c) || "(){}[]:".Contains(c)) != null)
					continue;

				// if we neither had comments or whitespace, break out.
				break;
			}

			// nothing parsed.
			if (stream.IsEmpty())
				return null;

			return Number.Parse(stream) ??
				Boolean.Parse(stream) ?? 
				Text.Parse(stream) ??
				Null.Parse(stream) ??
				Identifier.Parse(stream) ??
				(IValue) Function.Parse(stream); // cast is needed so typechecking passes.
		}

		/// <summary>
		/// Returns the result of running <paramref name="stream"/> as Knight code.
		/// </summary>
		/// <exception cref="ParseException">Thrown if there was a problem parsing <paramref name="stream"/>. </exception>
		/// <exception cref="RuntimeException">Thrown if there was a problem when running the parsed code. </exception>
		public static IValue Run(string stream) => Run(new Stream(stream));

		/// <summary>
		/// Returns the result of running <paramref name="stream"/> as Knight code.
		/// </summary>
		/// <exception cref="ParseException">Thrown if there was a problem parsing <paramref name="stream"/> </exception>
		/// <exception cref="RuntimeException">Thrown if there was a problem when running the parsed code. </exception>
		internal static IValue Run(Stream stream) {
			if (stream.IsEmpty())
				throw new ParseException("nothing to parse.");

			IValue value = Parse(stream);
			
			if (value == null)
				throw new ParseException($"Unknown token start '{stream.Take()}'.");

			return value.Run();
		}

		/// <summary>
		/// The entrypoint for the Knight executable.
		/// </summary>
		/// <param name="args">
		/// The arguments to the executiable---should either be <c>"-e", "&lt;expression&gt;"</c> or <c>"-f", "&lt;filename&gt;"</c>.
		/// </param>
		public static int Main(string[] args) {
			if (args.Length != 2 || args[0] != "-e" && args[0] != "-f") {
				Console.Error.WriteLine("usage: {0} (-e 'program' | -f file)", Environment.GetCommandLineArgs()[0]);
				return 1;
			}

			try {
				Run(args[0] == "-e" ? args[1] : File.ReadAllText(args[1]));
				return 0;
			} catch (KnightException err) {
				Console.Error.WriteLine("Invalid program: {0}", err.Message);
				return 1;
			} catch (Exception err) {
				Console.Error.WriteLine("Unexpected exception encountered: {0}", err.Message);
				return 1;
			}
		}
	}
}
