# Download latest release artifacts from github
$repo = "evgenygunko/CopyWords.Parsers"

$releases = "https://api.github.com/repos/$repo/releases"

Write-Host Determining latest release
$tag = (Invoke-WebRequest $releases -UseBasicParsing | ConvertFrom-Json)[0].tag_name
$file = "CopyWords.Parsers.Evgeny.$tag.nupkg"

$downloadUrl = "https://github.com/$repo/releases/download/$tag/$file"
Write-Host "downloadUrl = $downloadUrl"

$targetFolder = "$($env:AGENT_BUILDDIRECTORY)/Nuget/local_nuget"
#$targetFolder = "/Temp/packages"
$targetFile = "$targetFolder/$file"
Write-Host "targetFile = $targetFile"

New-Item -ItemType Directory -Force -Path $targetFolder

Write-Host Dowloading latest release
Invoke-WebRequest $downloadUrl -Out $targetFile