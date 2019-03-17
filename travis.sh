curl -o preview-sdk.tgz https://download.visualstudio.microsoft.com/download/pr/35c9c95a-535e-4f00-ace0-4e1686e33c6e/b9787e68747a7e8a2cf8cc530f4b2f88/dotnet-sdk-3.0.100-preview3-010431-linux-x64.tar.gz
mkdir -p $HOME/dotnet && tar zxf preview-sdk.tgz -C $HOME/dotnet
export DOTNET_ROOT=$HOME/dotnet 
export PATH=$HOME/dotnet:$PATH

dotnet --info
dotnet restore
dotnet build
dotnet test test/StartupModules.Tests
