FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ConnectFour_Doubtfire.csproj .
RUN dotnet restore "ConnectFour_Doubtfire.csproj"
COPY . . 
RUN dotnet publish "ConnectFour_Doubtfire.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final 
WORKDIR /app
COPY --from=build /publish . 

ENTRYPOINT [ "dotnet", "ConnectFour_Doubtfire.csproj" ]