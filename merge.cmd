nuget install ILMerge -Version 2.14.1208 -OutputDirectory tools

.\tools\ILMerge.2.14.1208\tools\ilmerge.exe ^
	.\src\SilverRock.Lazuli\bin\Release\SilverRock.Lazuli.exe ^
	.\src\SilverRock.Lazuli\bin\Release\Microsoft.ServiceBus.dll ^
	.\src\SilverRock.Lazuli\bin\Release\NDesk.Options.dll ^
	.\src\SilverRock.Lazuli\bin\Release\Newtonsoft.Json.dll ^
	.\src\SilverRock.Lazuli\bin\Release\SilverRock.AzureTools.dll ^
	/out:lazuli.exe