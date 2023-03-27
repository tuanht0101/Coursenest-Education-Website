$CURRENTDIR = Split-Path -parent $MyInvocation.MyCommand.Definition
$TARGETDIR = Resolve-Path -Path "$CURRENTDIR/../src"
Set-Location $TARGETDIR
docker compose --profile backend build
docker compose --profile backend up -d