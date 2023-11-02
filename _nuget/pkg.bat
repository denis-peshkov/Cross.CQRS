dotnet build  --configuration release ..\Cross.CQRS.sln
nuget.exe pack config.nuspec -Symbols -SymbolPackageFormat snupkg
