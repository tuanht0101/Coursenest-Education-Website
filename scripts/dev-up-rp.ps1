$CURRENTDIR = Split-Path -parent $MyInvocation.MyCommand.Definition
$TARGETDIR = Resolve-Path -Path "$CURRENTDIR/../src"
Set-Location $TARGETDIR
docker compose --profile backend --profile frontend --profile reverseproxy build
docker compose --profile backend --profile frontend --profile reverseproxy up -d