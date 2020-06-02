<#
.SYNOPSIS
    Script to updating project version.
.DESCRIPTION
    Script will update version for all csharp projects.
.PARAMETER mode
    Specify a value for the version
.EXAMPLE
    UpdateVersion.ps1 "1.2.3.4"
#>

[cmdletbinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$version
)

$projectFiles = Get-ChildItem -Path $PSScriptRoot/../*.csproj -Recurse -Force

foreach ($file in $projectFiles) {
    Write-Host "Found project file:" $file.Name

    $xml = [xml](Get-Content $file)
    [bool]$updated = $false

    $xml.GetElementsByTagName("PackageVersion") | ForEach-Object{
        Write-Host "Updating PackageVersion to:" $version
        $_."#text" = $version

        $updated = $true
    }

    if ($updated) {
        Write-Host "Project file saved"
        $xml.Save($file.FullName)
    } else {
        Write-Host "Version property not found in the project file"
    }
}

# Apply the version to the assembly property files
$files = Get-ChildItem "$PSScriptRoot/../" -recurse -include "*Properties*","My Project" |
	Where-Object{ $_.PSIsContainer } |
    ForEach-Object { Get-ChildItem -Path $_.FullName -Recurse -include AssemblyInfo.* }

if($files) {
	Write-Host "Will apply verion '$version' to $($files.count) files"

    # Regular expression pattern to find the version in the build number
    # and then apply it to the assemblies
    $VersionRegex = "\d+\.\d+\.\d+\.\d+"

	foreach ($file in $files) {
		$filecontent = Get-Content($file)
		#attrib $file -r
		$filecontent -replace $VersionRegex, $version | Out-File $file -Encoding utf8
		Write-Host "$($file.FullName) - version applied"
	}
}
else
{
	Write-Host "No AssemblyInfo files found"
}