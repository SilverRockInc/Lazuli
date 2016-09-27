using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilverRock.AzureTools.Models;

namespace SilverRock.Lazuli.UnitTests
{
	[TestClass]
	public class ProgramTestFixture
	{
		[TestMethod]
		public void IsJsonWhenValidJsonStartsWithWhitespace()
		{
			DoTestIsJson(WHITESPACE + VALID_JSON, true);
		}

		[TestMethod]
		public void IsJsonWhenValidJsonStartsWithoutWhitespace()
		{
			DoTestIsJson(VALID_JSON, true);
		}

		[TestMethod]
		public void IsJsonWhenInvalidJsonStartsWithWhitespace()
		{
			DoTestIsJson(WHITESPACE + INVALID_JSON, true);
		}

		[TestMethod]
		public void IsJsonWhenInvalidJsonStartsWithoutWhitespace()
		{
			DoTestIsJson(INVALID_JSON, true);
		}

		[TestMethod]
		public void IsJsonWhenTextStartsWithWhitespace()
		{
			DoTestIsJson(WHITESPACE + NOT_JSON, false);
		}

		[TestMethod]
		public void IsJsonWhenTextStartsWithoutWhitespace()
		{
			DoTestIsJson(NOT_JSON, false);
		}

		[TestMethod]
		public void IsJsonWhenOnlyWhitespace()
		{
			DoTestIsJson(WHITESPACE, false);
		}

		[TestMethod]
		public void IsJsonWhenNull()
		{
			DoTestIsJson(null, false);
		}

		private void DoTestIsJson(string text, bool expected)
		{
			// Arrange

			// Act
			bool result = Program.IsJson(text);

			// Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void GetScriptWhenTextIsValidYamlScript()
		{
			DoGetScriptTest(VALID_YAML_SCRIPT);
		}

		[TestMethod]
		public void GetScriptWhenTextIsValidJsonScript()
		{
			DoGetScriptTest(VALID_JSON_SCRIPT);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void GetScriptWhenTextIsInvalidYaml()
		{
			DoGetScriptTest(INVALID_YAML);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void GetScriptWhenTextIsInvalidJson()
		{
			DoGetScriptTest(INVALID_JSON);
		}

		private void DoGetScriptTest(string script)
		{
			// Arrange

			// Act
			Script result = Program.GetScript(script);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.DeployEnvironments);
			Assert.AreEqual(1, result.DeployEnvironments.Count);
			Assert.IsNotNull(result.DeployEnvironments[0].Topics);
			Assert.IsNotNull(result.DeployEnvironments[0].Topics.Create);
		}

		const string VALID_JSON = "{\"obj\":{\"prop\":\"value\"}}";
		const string INVALID_JSON = "{INVALID";
		const string NOT_JSON = "NOT_JSON";
		const string INVALID_YAML = "%%%%%%";
		const string VALID_JSON_SCRIPT = "{\"deployEnvironments\":[{\"name\":\"prod\",\"topics\":{\"create\":[{\"namespace\":{\"endpoint\":\"test-endpoint\",\"accessKeyName\":\"test-accessKeyName\",\"accessKey\":\"test-accessKey\"},\"path\":\"test-path\",\"subscriptions\":[{\"name\":\"test-sub1\"},{\"name\":\"test-sub2\"}]}]}}]}";
		const string VALID_YAML_SCRIPT = @"
deployEnvironments:
- name: prod
  topics:
    create:
    - namespace:
        endpoint: test-endpoint
        accessKeyName: test-accessKeyName
        accessKey: test-accessKey
      path: test-path
      subscriptions:
      - name: test-sub1
      - name: test-sub2
";

		const string WHITESPACE = " \t\r\n";
	}
}
