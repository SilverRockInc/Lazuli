nuget install ILMerge -Version 2.14.1208 -OutputDirectory tools

.\tools\ILMerge.2.14.1208\tools\ilmerge.exe ^
	.\src\SilverRock.Lazuli\bin\Debug\SilverRock.Lazuli.exe ^
	.\src\SilverRock.Lazuli\bin\Debug\Microsoft.ServiceBus.dll ^
	.\src\SilverRock.Lazuli\bin\Debug\NDesk.Options.dll ^
	.\src\SilverRock.Lazuli\bin\Debug\Newtonsoft.Json.dll ^
	.\src\SilverRock.Lazuli\bin\Debug\SilverRock.AzureTools.dll ^
	/out:lazuli.exe