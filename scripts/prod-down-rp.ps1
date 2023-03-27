$CURRENTDIR = Split-Path -parent $MyInvocation.MyCommand.Definition
$TARGETDIR = Resolve-Path -Path "$CURRENTDIR/../src"
Set-Location $TARGETDIR
docker compose -f docker-compose.yml -f docker-compose.prod.yml --profile backend --profile frontend --profile reverseproxy down --remove-orphans