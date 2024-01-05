# ステージ1: ビルド用のイメージ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルのコピー
COPY ["ServiceBusReceiver/ServiceBusReceiver.csproj", "ServiceBusReceiver/"]
# プロジェクトの依存関係の復元
RUN dotnet restore "ServiceBusReceiver/ServiceBusReceiver.csproj"

# ソースのコピー
COPY . .
WORKDIR "/src/ServiceBusReceiver"
# ビルド
RUN dotnet build "ServiceBusReceiver.csproj" -c Release -o /app/build

# ステージ2: 実行用のイメージ
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "ServiceBusReceiver.dll"]
