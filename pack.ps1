dotnet build -c Release 
dotnet pack .\SingleApi\ -c Release -o ..\_publish
dotnet pack .\SingleApi.Client\ -c Release -o ..\_publish
