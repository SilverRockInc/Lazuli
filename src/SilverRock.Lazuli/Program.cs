using NDesk.Options;
using Newtonsoft.Json;
using SilverRock.AzureTools;
using SilverRock.AzureTools.Models;
using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace SilverRock.Lazuli
{
	class Program
	{
		static void Main(string[] args)
		{
			OptionSet options = new OptionSet();
			options
				.Add<string>("e:|environment:", "Environment to be configured", e => _environment = e)
				.Add<string>("c:|config:", "File path to the config (json or yaml)", c => _config = c)
				.Add<string>("s:|silent", "No console output", s => _silent = true)
				.Add("f|force", "Forces re-creation of existing entities.  (MAY CAUSE DATA LOSS!)", f => _force = true)
				.Add("?|h|help", h => Help(options))
				.Parse(args);

			if (string.IsNullOrWhiteSpace(_environment))
				Console.WriteLine("Missing required environment.  See -? for details.");
			else if (string.IsNullOrWhiteSpace(_config))
				Console.WriteLine("Missing required configuration.  See -? for details.");
			else
			{
				ScriptRunner runner = new ScriptRunner();
				Script script = null;

				try
				{
					using (FileStream stream = new FileStream(_config, FileMode.Open))
					using (StreamReader reader = new StreamReader(stream))
						script = GetScript(reader.ReadToEnd());
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return;
				}

				if (!_silent)
					runner.Message += (sender, e) => Console.Write(e.Message);

				try
				{
					runner.Run(script, _environment, _force);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return;
				}
			}
		}

		internal static Script GetScript(string text)
		{
			if (!IsJson(text))
				text = YamlToJson(text);

			try
			{
				return JsonConvert.DeserializeObject<Script>(text,new JsonSerializerSettings { CheckAdditionalContent = false });
			}
			catch (Exception ex)
			{
				throw new FormatException("Cannot parse JSON as a valid script", ex);
			}

		}

		internal static bool IsJson(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return false;

			foreach (char c in s)
			{
				if (char.IsWhiteSpace(c))
					continue;
				else
					return c == '{';
			}

			return false;
		}

		internal static string YamlToJson(string s)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				new JsonSerializer().Serialize(new StringWriter(sb), new Deserializer().Deserialize(new StringReader(s)));
				return sb.ToString();
			}
			catch (Exception ex)
			{
				throw new FormatException("Cannot convert YAML to JSON", ex);
			}
		}

		static void Help(OptionSet options)
		{
			StringBuilder sb = new StringBuilder();

			using (TextWriter writer = new StringWriter(sb))
			{
				options.WriteOptionDescriptions(writer);
			}

			Console.WriteLine(sb.ToString());
		}

		static string _environment;
		static string _config;
		static bool _force;
		static bool _silent;
	}
}
